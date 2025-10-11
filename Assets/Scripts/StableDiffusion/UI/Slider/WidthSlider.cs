using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidthSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        int realValue = (int)System.Math.Round(value);
        GameManager.sdManager.txt2ImageBody.width = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
