using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Communication;
using static Ollama.Generate;

public class OllamaGenerate : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] bool test;
    bool isDone = false;

    // Start is called before the first frame update
    void Start()
    {
        if(test)
            _ = GetInfo();
    }
    async Task GetInfo()
    {
        //������ ResponseData.done�� ����ؾ� ������, ������ �ܼ��� �׽�Ʈ�̹Ƿ�.
        if (isDone)
            return;

        RequestData requestData = new RequestData("tinyllama", "Hello, how are you?", false);

        var item = await PostRequestAsync<RequestData, ResponseData>(ollamaUrls.generateAPI, requestData);

        text.text = item.response;

        isDone = true;
    }
}
