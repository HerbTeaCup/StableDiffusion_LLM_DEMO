using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hr_CfgScaleSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        float realValue = (float)System.Math.Round(value,1);
        SDManager.Instance.txt2ImageBody.hr_cfg = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
