using GeminiLLM;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

//������ Gemini ���� �Ŵ���
//���� �ٸ� LLM�� �߰��Ѵٸ�, LLMManager�� ��ӹ޾� ����
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
    /// System Instruction ����
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
    /// Gemini���� ��Ʈ������ �ƴ� �Ϲ� ������ �����ϰ�, ������ ������Ʈ�� AI�� �亯�� ��ȭ ������ �߰�
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
            Debug.LogError($"Gemini ���� ���� �� ����: {ex.Message}");
            _geminiRequest.Contents.Remove(userContent); //���� �� ���� ������Ʈ �ѹ�
            throw;
        }

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

        Content userContent = AddUserPrompt(prompt);

        await foreach (var line in Communication.PostAndStreamLinesAsync(targetUrl, header, ContentType.Json, _geminiRequest))
        {
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: "))
            {
                // SSE�� �� ���� keep-alive ��ȣ�� ���� �� �����Ƿ� ����
                continue;
            }

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
            catch (Exception ex)
            {
                Debug.LogError($"[StreamingResponse] API ��û ����: {ex.Message}");
                //���� ��, ��� �߰��� userContent�� �����丮���� ���� (�ѹ�)
                _geminiRequest.Contents.Remove(userContent);
                throw; // ������ ������ �ٽ� ����
            }
        }

        //���� ������ ��ȭ ������ �߰�
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
    /// ��ȭ ������ �ʱ�ȭ�մϴ�.
    /// </summary>
    public void ClearHistory()
    {
        _geminiRequest.Contents.Clear();
    }
}
