using GeminiLLM;
using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class LLMManager : ManagerBase<LLMManager>
{
    //������ Gemini�ۿ� ������, Dict���·� ���������� ���� : ���߿� �ٸ� LLM �𵨵� �߰��Ǹ� Dict���·� �ٲ� ���� ����
    List<GeminiResponse> genAIResponses = new List<GeminiResponse>();

    CancellationTokenSource _streamingCts;

    /// <summary>
    /// ������ �����մϴ�.
    /// ��Ʈ������ �ƴ� �Ϲ� ��û���Դϴ�.
    /// </summary>
    /// <returns>UŸ������ �� ��ü ��ȯ</returns>
    public async Task<GeminiResponse> GenerateResponse(string targetUrl, GeminiResponse request, HeaderSetting header)
    {
        GeminiResponse response =
            await Communication.PostRequestAsync<GeminiResponse, GeminiResponse>(targetUrl, header, ContentType.Json, request);

        genAIResponses.Add(response);

        return response;
    }

    public async Task StreamingResponse(string targetUrl, GeminiResponse request, HeaderSetting header)
    {
        _streamingCts = new CancellationTokenSource();

        await foreach(string line in Communication.PostAndStreamLinesAsync(targetUrl, header, ContentType.Json, request))
        {
            Debug.Log(line);
        }
    }
}
