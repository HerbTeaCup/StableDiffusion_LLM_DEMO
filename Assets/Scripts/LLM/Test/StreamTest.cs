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
                // SSE는 빈 줄을 keep-alive 신호로 보낼 수 있으므로 무시
                continue;
            }

            // 1. 파싱: "data: " 접두사 제거
            if (line.StartsWith("data: "))
            {
                string jsonText = line.Substring("data: ".Length);

                try
                {
                    // 2. 역직렬화: JSON을 C# 객체로 변환
                    var streamResponse = JsonConvert.DeserializeObject<GeminiResponse>(jsonText);

                    // 3. 텍스트 추출: 실제 텍스트 조각(chunk)을 가져옴
                    string textChunk = streamResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text;

                    if (textChunk != null)
                    {
                        // (옵션 1) 지금 당장 덩어리(chunk) 단위로 UI에 표시
                        outputField.text += textChunk;

                        // (옵션 2) 나중에 타자기 효과를 위해 전체 텍스트를 모음
                        fullResponse.Append(textChunk);
                    }
                }
                catch (JsonException ex)
                {
                    Debug.LogWarning($"JSON 역직렬화 실패: {ex.Message} | 원본: {jsonText}");
                }
            }

            Debug.Log("전체 응답: " + fullResponse.ToString());
        }
    }
}
