using GeminiLLM;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StreamTest : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] TMPro.TMP_Text outputField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            TestMethod();
        }
    }
    async void TestMethod()
    {
        string input = inputField.text;

        UrlManager urlManager = ManagerResister.GetManager<UrlManager>();

        string url = urlManager.Gemini.GetUrl(GeminiRequestPurpose.StreamGeneratedContent);
        HeaderSetting header = urlManager.Gemini.GetHeader(HeaderPurpose.XGoogleApiKey);
        header.value = APIKeyResister.GeminiKey;

        StringBuilder fullResponse = new();

        GeminiRequest request = new();
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

        await foreach (var line in Communication.PostAndStreamLinesAsync(url, header, ContentType.Json, request))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                // SSE�� �� ���� keep-alive ��ȣ�� ���� �� �����Ƿ� ����
                continue;
            }

            // 1. �Ľ�: "data: " ���λ� ����
            if (line.StartsWith("data: "))
            {
                string jsonText = line.Substring("data: ".Length);

                try
                {
                    // 2. ������ȭ: JSON�� C# ��ü�� ��ȯ
                    var streamResponse = JsonConvert.DeserializeObject<GeminiResponse>(jsonText);

                    // 3. �ؽ�Ʈ ����: ���� �ؽ�Ʈ ����(chunk)�� ������
                    string textChunk = streamResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text;

                    if (textChunk != null)
                    {
                        // (�ɼ� 1) ���� ���� ���(chunk) ������ UI�� ǥ��
                        outputField.text += textChunk;

                        // (�ɼ� 2) ���߿� Ÿ�ڱ� ȿ���� ���� ��ü �ؽ�Ʈ�� ����
                        fullResponse.Append(textChunk);
                    }
                }
                catch (JsonException ex)
                {
                    Debug.LogWarning($"JSON ������ȭ ����: {ex.Message} | ����: {jsonText}");
                }
            }

            Debug.Log("��ü ����: " + fullResponse.ToString());
        }
    }
}
