using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Seed : MonoBehaviour
{
    TMPro.TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();

        ManagerResister.GetManager<SDManager>().txt2ImageBody.seed = int.Parse(inputField.text);
    }

    public void OnEndEdit(string input)
    {
        ManagerResister.GetManager<SDManager>().txt2ImageBody.seed = int.Parse(input);
    }
}
