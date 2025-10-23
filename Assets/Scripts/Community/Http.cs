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
/// ����� ����ϴ� Ŭ����. ����� URL�� ����� �� �ִ� �޼ҵ带 �����մϴ�.
/// ��� �������� ����� �� Ŭ������ ���� ���.
/// </summary>
public static class Communication
{
    static readonly HttpClient httpClient = new();

    static UrlManager _urlManager => ManagerResister.GetManager<UrlManager>();

    /// <summary>
    /// StableDiffusion Webui�� ����� �غ� �Ǿ�����, �ƴ��� �Ǵ��ϴ� �ܼ��� �޼ҵ��Դϴ�.
    /// </summary>
    public static async Task<bool> ConnectingCheck()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Ping));
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
            request.Dispose(); // ��û ��ü�� �����մϴ�.
            response?.Dispose(); // ���� ��ü�� �����մϴ�.
        }
    }

    /// <summary>
    /// POST ��û�� �񵿱������� ����������, ������ ��ȯ���� �ʽ��ϴ�.
    /// </summary>
    public static async Task PostRequestAsync<U>(string targetURL, HeaderSetting header, ContentType content, U postData, [CallerMemberName] string caller = "")
    {
        HttpRequestMessage request = new(HttpMethod.Post, targetURL);
        HttpResponseMessage response = null;
        try
        {
            request.Headers.Add(header.name, header.value);
            string requestBody = JsonConvert.SerializeObject(postData);//post�� �����͸� ���ڿ��� ��ȯ
            string contentValue = ContentSetting.GetContentValue(content);

            request.Content = new StringContent(requestBody, Encoding.UTF8, contentValue);

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
            request?.Dispose();
            response?.Dispose();
        }
    }
    /// <summary>
    /// POST ��û�� �񵿱������� �����ϰ�, ������ ��ȯ�մϴ�.
    /// </summary>
    /// <typeparam name="U">�Է� Ÿ��</typeparam>
    /// <typeparam name="T">��ȯ Ÿ��</typeparam>
    /// <returns></returns>
    public static async Task<T> PostRequestAsync<U,T>(string targetURL, HeaderSetting header, ContentType content, U postData, [CallerMemberName] string caller = "")
    {
        HttpRequestMessage request = new(HttpMethod.Post, targetURL);
        HttpResponseMessage response = null;

        try
        {
            request.Headers.Add(header.name, header.value);
            string requestBody = JsonConvert.SerializeObject(postData);//post�� �����͸� ���ڿ��� ��ȯ
            string contentValue = ContentSetting.GetContentValue(content);

            request.Content = new StringContent(requestBody, Encoding.UTF8, contentValue);

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
            request?.Dispose();
            response?.Dispose();
        }
    }

    // [����] �񵿱� ��Ʈ���� CancellationToken�� �����ϴ� ���� �����ϴ�.
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
            string requestBody = JsonConvert.SerializeObject(postData);//post�� �����͸� ���ڿ��� ��ȯ
            string contentValue = ContentSetting.GetContentValue(content);

            request.Content = new StringContent(requestBody, Encoding.UTF8, contentValue);

            response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            responseStream = await response.Content.ReadAsStreamAsync();
            streamReader = new StreamReader(responseStream);
        }
        catch (HttpRequestException ex)
        {
            Debug.LogError($"{caller}���� [HTTP ERROR] ��û ����: {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (JsonSerializationException ex)
        {
            Debug.LogError($"{caller}���� [JSON ����ȭ ����] {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (JsonReaderException ex)
        {
            Debug.LogError($"{caller}���� [JSON �Ľ� ����] {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (TaskCanceledException ex)
        {
            Debug.LogError($"{caller}���� [TIMEOUT] ��û �ð� �ʰ�: {ex.Message}");
            response?.Dispose();
            throw;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{caller}���� [��Ÿ ����] {ex.GetType().Name}: {ex.Message}");
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
    /// ���� ����ɶ� �ڵ����� AysncManager���� �ڵ����� ����Ǵ� �޼ҵ�
    /// </summary>
    public static void HttpDispose()
    {
        httpClient.Dispose();
    }
}
