using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propmt : MonoBehaviour
{
    TMPro.TMP_InputField inputField;

    enum PromptType
    {
        prompt,
        negative
    }

    [SerializeField] PromptType promptType;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();

        switch (promptType)
        {
            case PromptType.prompt:
                ManagerResister.GetManager<SDManager>().txt2ImageBody.prompt = inputField.text;
                break;
            case PromptType.negative:
                ManagerResister.GetManager<SDManager>().txt2ImageBody.negative_prompt = inputField.text;
                break;
            default:
                Debug.LogWarning("PromptType is not set");
                break;
            
        }
    }
}
