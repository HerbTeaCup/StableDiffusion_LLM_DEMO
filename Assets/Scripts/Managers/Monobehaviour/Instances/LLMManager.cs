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
    /// Gemini���� ��Ʈ������ �ƴ� �Ϲ� ������ �����ϰ�, ������ ������Ʈ�� AI�� �亯�� ��ȭ ������ �߰�
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

        //response��ü�� ��Ȱ������ ������, �Ź� Response�� ù��° �ĺ��� ��û�� �߰�
        _geminiRequest.Contents.Add(response.Candidates[0].Content);

        return response;
    }

    /// <summary>
    /// ��Ʈ���� ������� AI ������ �޾� �ݹ����� ûũ ���� ����
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
                // SSE�� �� ���� keep-alive ��ȣ�� ���� �� �����Ƿ� ����
                continue;
            }

            if (line.StartsWith("data: "))
            {
                string jsonText = line.Substring("data: ".Length);

                try
                {
                    var streamResponse = JsonConvert.DeserializeObject<GeminiResponse>(jsonText);
                    finalResponse = streamResponse; //������ ��Ÿ������ ����

                    //�ؽ�Ʈ ����: ���� �ؽ�Ʈ ����(chunk)�� ������
                    string textChunk = streamResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text;

                    if (textChunk != null)
                    {
                        //UI��, �α״� ûũ ������ �ѱ��
                        callback.Invoke(textChunk);

                        //���� ���� ������
                        fullResponseBuilder.Append(textChunk);
                    }
                }
                catch (JsonException ex)
                {
                    Debug.LogWarning($"JSON ������ȭ ����: {ex.Message} | ����: {jsonText}");
                }
            }
        }

        //���� ������ ��ȭ ������ �߰�
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
    /// ��ȭ ������ �ʱ�ȭ�մϴ�.
    /// </summary>
    public void ClearHistory()
    {
        _geminiRequest.Contents.Clear();
    }
}
