using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplingStepsSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        int realValue = (int)System.Math.Round(value);
        GameManager.sdManager.txt2ImageBody.steps = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
