using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hr_StepsSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        int realValue = (int)System.Math.Round(value);
        SDManager.Instance.txt2ImageBody.hr_second_pass_steps = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
