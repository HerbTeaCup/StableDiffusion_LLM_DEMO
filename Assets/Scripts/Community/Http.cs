using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;

/// <summary>
/// 통신을 담당하는 클래스. 통신할 URL과 통신할 수 있는 메소드를 제공합니다.
/// 모든 서버와의 통신은 이 클래스를 통해 통신.
/// </summary>
public partial class Communication
{
    static readonly HttpClient httpClient = new HttpClient();

    //TODO:해당 참조 전부 없애고 APIConfigBase의 헤더로 교체
    static HeaderSetting stableDiffusionBasicHeader = new HeaderSetting(HeaderPurpose.Accept, "Accept", "application/json");
    public static HeaderSetting StalbeDiffusionBasicHeader => stableDiffusionBasicHeader; //임시로 사용하는 것.

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
        if (!Uri.IsWellFormedUriString(targetURL, UriKind.Absolute))
        {
            targetURL = targetURL.TrimEnd('/') + "/" + targetURL.TrimStart('/');
        }

        using(HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, targetURL))
        {
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
                response?.Dispose(); // 응답 객체를 해제합니다.
            }
        }
    }

    /// <summary>
    /// POST 요청을 비동기적으로 수행하지만, 응답을 반환하지 않습니다.
    /// </summary>
    public static async Task PostRequestAsync<U>(string targetURL, HeaderSetting header, ContentType content, U postData, [CallerMemberName] string caller = "")
    {
        if (!Uri.IsWellFormedUriString(targetURL, UriKind.Absolute))
        {
            targetURL = targetURL.TrimEnd('/') + "/" + targetURL.TrimStart('/');
        }

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, targetURL))
        {
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
                response?.Dispose();
            }
        }
    }
    /// <summary>
    /// POST 요청을 비동기적으로 수행하고, 응답을 반환합니다.
    /// </summary>
    public static async Task<T> PostRequestAsync<U,T>(string targetURL, HeaderSetting header, ContentType content, U postData, [CallerMemberName] string caller = "")
    {
        if (!Uri.IsWellFormedUriString(targetURL, UriKind.Absolute))
        {
            targetURL = targetURL.TrimEnd('/') + "/" + targetURL.TrimStart('/');
        }

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, targetURL))
        {
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
                response?.Dispose();
            }
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
