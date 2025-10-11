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
/// ����� ����ϴ� Ŭ����. ����� URL�� ����� �� �ִ� �޼ҵ带 �����մϴ�.
/// ��� �������� ����� �� Ŭ������ ���� ���.
/// </summary>
public partial class Communication
{
    static readonly HttpClient httpClient = new HttpClient();

    /// <summary>
    /// StableDiffusion Webui�� ����� �غ� �Ǿ�����, �ƴ��� �Ǵ��ϴ� �ܼ��� �޼ҵ��Դϴ�.
    /// </summary>
    public static async Task<bool> ConnectingCheck()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, sDurls.pingAPI);
        HttpResponseMessage response = null;

        try
        {
            response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode; // 200~299�̸� true
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Ping ����] {ex.Message}");
            return false;
        }
        finally
        {
            request.Dispose(); // ��û ��ü�� ����.
            response?.Dispose(); // ���� ��ü�� ����.
        }
    }

    /// <summary>
    /// GET ��û�� �񵿱������� �����ϰ�, ������ �Ľ��ؼ� ��ȯ�մϴ�.
    /// </summary>
    /// <typeparam name="T">��ȯ ���� Ÿ���� ����մϴ�</typeparam>
    public static async Task<T> GetRequestAsync<T>(string targetURL, [CallerMemberName] string caller = "")
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
                request.Headers.Add(sDurls.nameHeader, sDurls.valueHeader);

                response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();

                T Data = JsonConvert.DeserializeObject<T>(result);

                return Data;
            }
            catch (HttpRequestException ex)
            {
                Debug.LogError($"{caller}���� [HTTP ERROR] ��û ����: {ex.Message}");
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Debug.LogError($"{caller}���� [JSON ����ȭ ����] {ex.Message}");
                throw;
            }
            catch (JsonReaderException ex)
            {
                Debug.LogError($"{caller}���� [JSON �Ľ� ����] {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                Debug.LogError($"{caller}���� [TIMEOUT] ��û �ð� �ʰ�: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{caller}���� [��Ÿ ����] {ex.GetType().Name}: {ex.Message}");
                throw;
            }
            finally
            {
                response?.Dispose(); // ���� ��ü�� �����մϴ�.
            }
        }
    }

    /// <summary>
    /// POST ��û�� �񵿱������� ����������, ������ ��ȯ���� �ʽ��ϴ�.
    /// </summary>
    public static async Task PostRequestAsync<U>(string targetURL, U postData, [CallerMemberName] string caller = "")
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
                request.Headers.Add(sDurls.nameHeader, sDurls.valueHeader);
                string requestBody = JsonConvert.SerializeObject(postData);//post�� �����͸� ���ڿ��� ��ȯ

                request.Content = new StringContent(requestBody, Encoding.UTF8, sDurls.valueHeader);

                response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Debug.LogError($"{caller}���� [HTTP ERROR] ��û ����: {ex.Message}");
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Debug.LogError($"{caller}���� [JSON ����ȭ ����] {ex.Message}");
                throw;
            }
            catch (JsonReaderException ex)
            {
                Debug.LogError($"{caller}���� [JSON �Ľ� ����] {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                Debug.LogError($"{caller}���� [TIMEOUT] ��û �ð� �ʰ�: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{caller}���� [��Ÿ ����] {ex.GetType().Name}: {ex.Message}");
                throw;
            }
            finally
            {
                response?.Dispose();
            }
        }
    }
    /// <summary>
    /// POST ��û�� �񵿱������� �����ϰ�, ������ ��ȯ�մϴ�.
    /// </summary>
    public static async Task<T> PostRequestAsync<U,T>(string targetURL, U postData, [CallerMemberName] string caller = "")
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
                request.Headers.Add(sDurls.nameHeader, sDurls.valueHeader);
                string requestBody = JsonConvert.SerializeObject(postData);//post�� �����͸� ���ڿ��� ��ȯ

                request.Content = new StringContent(requestBody, Encoding.UTF8, sDurls.valueHeader);

                response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseText = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(responseText);
            }
            catch (HttpRequestException ex)
            {
                Debug.LogError($"{caller}���� [HTTP ERROR] ��û ����: {ex.Message}");
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Debug.LogError($"{caller}���� [JSON ����ȭ ����] {ex.Message}");
                throw;
            }
            catch (JsonReaderException ex)
            {
                Debug.LogError($"{caller}���� [JSON �Ľ� ����] {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                Debug.LogError($"{caller}���� [TIMEOUT] ��û �ð� �ʰ�: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{caller}���� [��Ÿ ����] {ex.GetType().Name}: {ex.Message}");
                throw;
            }
            finally
            {
                response?.Dispose();
            }
        }
    }

    /// <summary>
    /// ���� ����ɶ� �ڵ����� AysncManager���� �ڵ����� ����Ǵ� �޼ҵ�
    /// </summary>
    public static void HttpDispose()
    {
        httpClient.Dispose();
    }
}
