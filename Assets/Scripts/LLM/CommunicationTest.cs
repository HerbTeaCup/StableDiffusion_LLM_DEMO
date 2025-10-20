using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GeminiLLM;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationTest : MonoBehaviour
{
    static HttpClient client = new HttpClient();

    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] TMPro.TMP_Text outputField;

    string api = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
    string key = APIKeyResister.GeminiKey;

    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isRunning == false)
        {
            isRunning = true;
            TestMethod1();
            isRunning = false;
        }
    }

    async void TestMethod1()
    {
        string input = inputField.text;

        Debug.Log("입력중...");

        GeminiRequest request = new GeminiRequest();

        request.Contents = new List<Content>
        {
            new Content
            {
                Role = "user",
                Parts = new List<Part>
                {
                    new Part { Text = input }
                }
            }
        };
        HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, api);
        HttpResponseMessage response = null;

        try
        {
            httpRequest.Headers.Add("x-goog-api-key", $"{key}");
            httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");

            response = await client.SendAsync(httpRequest);

            string responseText = await response.Content.ReadAsStringAsync();

            Debug.Log("완료");

            outputField.text = responseText;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);

        }
        finally
        {
            httpRequest.Dispose();
            response.Dispose();
        }
    }
}
