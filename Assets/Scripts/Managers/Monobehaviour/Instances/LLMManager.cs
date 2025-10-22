using GeminiLLM;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class LLMManager : ManagerBase<LLMManager>
{
    GeminiRequest _geminiRequest = new();
    public GeminiRequest GeminiRequest { get { return _geminiRequest; } }

    Content PromptInput(string prompt)
    {
        Part part = new()
        {
            Text = prompt
        };
        Content content = new()
        {
            Role = "user",
            Parts = new List<Part> { part }
        };

        _geminiRequest.Contents.Add(content);
        return content;
    }

    /// <summary>
    /// Gemini에게 스트리밍이 아닌 일반 응답을 생성하고, 유저의 프롬프트와 AI의 답변을 대화 내역에 추가
    /// </summary>
    public async Task<GeminiResponse> GenerateResponse(string prompt)
    {
        UrlManager urlManager = ManagerResister.GetManager<UrlManager>();

        string targetUrl = urlManager.Gemini.GetUrl(GeminiRequestPurpose.GenerateContent);

        HeaderSetting header = urlManager.Gemini.GetHeader(HeaderPurpose.XGoogleApiKey);
        header.value = APIKeyResister.GeminiKey;

        PromptInput(prompt);

        GeminiResponse response =
            await Communication.PostRequestAsync<GeminiRequest, GeminiResponse>(targetUrl, header, ContentType.Json, _geminiRequest);

        //response객체를 재활용하진 않으니, 매번 Response의 첫번째 후보를 요청에 추가
        _geminiRequest.Contents.Add(response.Candidates[0].Content);

        return response;
    }

    /// <summary>
    /// 스트리밍 방식으로 AI 응답을 받아 콜백으로 청크 단위 전송
    /// </summary>
    public async Task<GeminiResponse> StreamingResponse(string prompt, Action<string> callback)
    {
        UrlManager urlManager = ManagerResister.GetManager<UrlManager>();

        GeminiResponse finalResponse = null;
        StringBuilder fullResponseBuilder = new();

        string targetUrl = urlManager.Gemini.GetUrl(GeminiRequestPurpose.StreamGeneratedContent);

        HeaderSetting header = urlManager.Gemini.GetHeader(HeaderPurpose.XGoogleApiKey);
        header.value = APIKeyResister.GeminiKey;

        PromptInput(prompt);

        await foreach (var line in Communication.PostAndStreamLinesAsync(targetUrl, header, ContentType.Json, _geminiRequest))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                // SSE는 빈 줄을 keep-alive 신호로 보낼 수 있으므로 무시
                continue;
            }

            if (line.StartsWith("data: "))
            {
                string jsonText = line.Substring("data: ".Length);

                try
                {
                    var streamResponse = JsonConvert.DeserializeObject<GeminiResponse>(jsonText);
                    finalResponse = streamResponse; //마지막 메타데이터 저장

                    //텍스트 추출: 실제 텍스트 조각(chunk)을 가져옴
                    string textChunk = streamResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text;

                    if (textChunk != null)
                    {
                        //UI던, 로그던 청크 단위로 넘기기
                        callback.Invoke(textChunk);

                        //최종 응답 조립용
                        fullResponseBuilder.Append(textChunk);
                    }
                }
                catch (JsonException ex)
                {
                    Debug.LogWarning($"JSON 역직렬화 실패: {ex.Message} | 원본: {jsonText}");
                }
            }
        }

        //최종 응답을 대화 내역에 추가
        if (fullResponseBuilder.Length > 0)
        {
            var modelContent = new Content
            {
                Role = "model",
                Parts = new List<Part> { new Part { Text = fullResponseBuilder.ToString() } }
            };
            _geminiRequest.Contents.Add(modelContent);
        }

        return finalResponse;
    }

    /// <summary>
    /// 대화 내역을 초기화합니다.
    /// </summary>
    public void ClearHistory()
    {
        _geminiRequest.Contents.Clear();
    }
}
