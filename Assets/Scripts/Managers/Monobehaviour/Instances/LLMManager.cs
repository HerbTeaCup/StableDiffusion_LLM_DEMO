using GeminiLLM;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

//지금은 Gemini 전용 매니저
//향후 다른 LLM을 추가한다면, LLMManager를 상속받아 구현
public class LLMManager : ManagerBase<LLMManager>
{
    readonly GeminiRequest _geminiRequest = new();

    Content AddUserPrompt(string prompt)
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
    /// System Instruction 설정
    /// </summary>
    public SystemInstruction SystemInstructionSetting(string instruction)
    {
        if (_geminiRequest.SystemInstruction == null)
            _geminiRequest.SystemInstruction = new SystemInstruction();

        Part part = new()
        {
            Text = instruction
        };

        _geminiRequest.SystemInstruction.Parts.Add(part);

        return _geminiRequest.SystemInstruction;
    }

    /// <summary>
    /// Gemini에게 스트리밍이 아닌 일반 응답을 생성하고, 유저의 프롬프트와 AI의 답변을 대화 내역에 추가
    /// </summary>
    public async Task<GeminiResponse> GenerateResponse(string prompt)
    {
        UrlManager urlManager = ManagerResister.GetManager<UrlManager>();

        string targetUrl = urlManager.Gemini.GetUrl(GeminiRequestPurpose.GenerateContent);

        HeaderSetting header = urlManager.Gemini.GetHeader(HeaderPurpose.XGoogleApiKey);

        Content userContent = AddUserPrompt(prompt);

        GeminiResponse response;
        try
        {
            response
                = await Communication.PostRequestAsync<GeminiRequest, GeminiResponse>(targetUrl, header, ContentType.Json, _geminiRequest);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Gemini 응답 생성 중 오류: {ex.Message}");
            _geminiRequest.Contents.Remove(userContent); //실패 시 유저 프롬프트 롤백
            throw;
        }

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

        Content userContent = AddUserPrompt(prompt);

        await foreach (var line in Communication.PostAndStreamLinesAsync(targetUrl, header, ContentType.Json, _geminiRequest))
        {
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: "))
            {
                // SSE는 빈 줄을 keep-alive 신호로 보낼 수 있으므로 무시
                continue;
            }

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
            catch (Exception ex)
            {
                Debug.LogError($"[StreamingResponse] API 요청 실패: {ex.Message}");
                //실패 시, 방금 추가한 userContent를 히스토리에서 제거 (롤백)
                _geminiRequest.Contents.Remove(userContent);
                throw; // 오류를 상위로 다시 던짐
            }
        }

        //최종 응답을 대화 내역에 추가
        if (fullResponseBuilder.Length > 0)
        {
            var modelContent = new Content
            {
                Role = "model",
                Parts = new List<Part> { new() { Text = fullResponseBuilder.ToString() } }
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
