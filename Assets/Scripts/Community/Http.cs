using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 통신을 담당하는 클래스. 통신할 URL과 통신할 수 있는 메소드를 제공합니다.
/// 모든 서버와의 통신은 이 클래스를 통해 통신.
/// </summary>
public static class Communication
{
    static readonly HttpClient httpClient = new();

    static UrlManager _urlManager => ManagerResister.GetManager<UrlManager>();

    /// <summary>
    /// StableDiffusion Webui가 통신할 준비가 되었는지, 아닌지 판단하는 단순한 메소드입니다.
    /// </summary>
    public static async Task<bool> ConnectingCheck()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Ping));
        HttpResponseMessage response = null;

        try
        {
            response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode; // 200~299이면 true
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Ping 실패] {ex.Message}");
            return false;
        }
        finally
        {
            request.Dispose(); // 요청 객체를 해제.
            response?.Dispose(); // 응답 객체를 해제.
        }
    }

    /// <summary>
    /// GET 요청을 비동기적으로 수행하고, 응답을 파싱해서 반환합니다.
    /// </summary>
    /// <typeparam name="T">반환 받을 타입을 명시합니다</typeparam>
    public static async Task<T> GetRequestAsync<T>(string targetURL, HeaderSetting header, [CallerMemberName] string caller = "")
    {
        HttpRequestMessage request = new(HttpMethod.Get, targetURL);
        HttpResponseMessage response = null;
        try
        {
            request.Headers.Add(header.name, header.value);

            response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            T Data = JsonConvert.DeserializeObject<T>(result);

            return Data;
        }
        catch (HttpRequestException ex)
        {
            Debug.LogError($"{caller}에서 [HTTP ERROR] 요청 실패: {ex.Message}");
            throw;
        }
        catch (JsonSerializationException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 직렬화 실패] {ex.Message}");
            throw;
        }
        catch (JsonReaderException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 파싱 실패] {ex.Message}");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            Debug.LogError($"{caller}에서 [TIMEOUT] 요청 시간 초과: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{caller}에서 [기타 예외] {ex.GetType().Name}: {ex.Message}");
            throw;
        }
        finally
        {
            request.Dispose(); // 요청 객체를 해제합니다.
            response?.Dispose(); // 응답 객체를 해제합니다.
        }
    }

    /// <summary>
    /// POST 요청을 비동기적으로 수행하지만, 응답을 반환하지 않습니다.
    /// </summary>
    public static async Task PostRequestAsync<U>(string targetURL, HeaderSetting header, ContentType content, U postData, [CallerMemberName] string caller = "")
    {
        HttpRequestMessage request = new(HttpMethod.Post, targetURL);
        HttpResponseMessage response = null;
        try
        {
            request.Headers.Add(header.name, header.value);
            string requestBody = JsonConvert.SerializeObject(postData);//post할 데이터를 문자열로 변환
            string contentValue = ContentSetting.GetContentValue(content);

            request.Content = new StringContent(requestBody, Encoding.UTF8, contentValue);

            response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Debug.LogError($"{caller}에서 [HTTP ERROR] 요청 실패: {ex.Message}");
            throw;
        }
        catch (JsonSerializationException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 직렬화 실패] {ex.Message}");
            throw;
        }
        catch (JsonReaderException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 파싱 실패] {ex.Message}");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            Debug.LogError($"{caller}에서 [TIMEOUT] 요청 시간 초과: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{caller}에서 [기타 예외] {ex.GetType().Name}: {ex.Message}");
            throw;
        }
        finally
        {
            request?.Dispose();
            response?.Dispose();
        }
    }
    /// <summary>
    /// POST 요청을 비동기적으로 수행하고, 응답을 반환합니다.
    /// </summary>
    /// <typeparam name="U">입력 타입</typeparam>
    /// <typeparam name="T">반환 타입</typeparam>
    /// <returns></returns>
    public static async Task<T> PostRequestAsync<U,T>(string targetURL, HeaderSetting header, ContentType content, U postData, [CallerMemberName] string caller = "")
    {
        HttpRequestMessage request = new(HttpMethod.Post, targetURL);
        HttpResponseMessage response = null;

        try
        {
            request.Headers.Add(header.name, header.value);
            string requestBody = JsonConvert.SerializeObject(postData);//post할 데이터를 문자열로 변환
            string contentValue = ContentSetting.GetContentValue(content);

            request.Content = new StringContent(requestBody, Encoding.UTF8, contentValue);

            response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseText = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseText);
        }
        catch (HttpRequestException ex)
        {
            Debug.LogError($"{caller}에서 [HTTP ERROR] 요청 실패: {ex.Message}");
            throw;
        }
        catch (JsonSerializationException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 직렬화 실패] {ex.Message}");
            throw;
        }
        catch (JsonReaderException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 파싱 실패] {ex.Message}");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            Debug.LogError($"{caller}에서 [TIMEOUT] 요청 시간 초과: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{caller}에서 [기타 예외] {ex.GetType().Name}: {ex.Message}");
            throw;
        }
        finally
        {
            request?.Dispose();
            response?.Dispose();
        }
    }

    // [권장] 비동기 스트림은 CancellationToken을 지원하는 것이 좋습니다.
    public static async IAsyncEnumerable<string> PostAndStreamLinesAsync<U>(string targetURL, HeaderSetting header, ContentType content, U postData,
    [CallerMemberName] string caller = "",
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, targetURL);
        HttpResponseMessage response = null;

        Stream responseStream = null;
        StreamReader streamReader = null;

        try
        {
            request.Headers.Add(header.name, header.value);
            string requestBody = JsonConvert.SerializeObject(postData);//post할 데이터를 문자열로 변환
            string contentValue = ContentSetting.GetContentValue(content);

            request.Content = new StringContent(requestBody, Encoding.UTF8, contentValue);

            response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            responseStream = await response.Content.ReadAsStreamAsync();
            streamReader = new StreamReader(responseStream);
        }
        catch (HttpRequestException ex)
        {
            Debug.LogError($"{caller}에서 [HTTP ERROR] 요청 실패: {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (JsonSerializationException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 직렬화 실패] {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (JsonReaderException ex)
        {
            Debug.LogError($"{caller}에서 [JSON 파싱 실패] {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (TaskCanceledException ex)
        {
            Debug.LogError($"{caller}에서 [TIMEOUT] 요청 시간 초과: {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{caller}에서 [기타 예외] {ex.GetType().Name}: {ex.Message}");
            response?.Dispose();
            throw;
        }

        try
        {
            while (!streamReader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var line = await streamReader.ReadLineAsync();
                if (line != null)
                {
                    yield return line;
                }
            }
        }
        finally
        {
            response?.Dispose();
            streamReader?.Dispose();
            responseStream?.Dispose();
        }
    }

    /// <summary>
    /// 앱이 종료될때 자동으로 AysncManager에서 자동으로 실행되는 메소드
    /// </summary>
    public static void HttpDispose()
    {
        httpClient.Dispose();
    }
}
