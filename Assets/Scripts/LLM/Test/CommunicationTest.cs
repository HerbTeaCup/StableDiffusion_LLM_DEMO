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
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] TMPro.TMP_Text outputField;

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

        LLMManager llmManager = ManagerResister.GetManager<LLMManager>();

        var response = await llmManager.GenerateResponse(input);

        outputField.text = response.Candidates[0].Content.Parts[0].Text;
    }
}
