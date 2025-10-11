using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hr_scaleSlider : BaseSlider
{
    
    public override void OnValueChanged(float value)
    {
        float realValue = (float) Mathf.Round(value * 20f) / 20f;
        GameManager.sdManager.txt2ImageBody.hr_scale = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
