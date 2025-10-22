using GeminiLLM;
using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class LLMManager : ManagerBase<LLMManager>
{
    //지금은 Gemini밖에 없으니, Dict형태로 관리하지는 않음 : 나중에 다른 LLM 모델도 추가되면 Dict형태로 바꿀 수도 있음
    List<GeminiResponse> genAIResponses = new List<GeminiResponse>();

    CancellationTokenSource _streamingCts;

    /// <summary>
    /// 응답을 생성합니다.
    /// 스트리밍이 아닌 일반 요청용입니다.
    /// </summary>
    /// <returns>U타입으로 된 객체 반환</returns>
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
