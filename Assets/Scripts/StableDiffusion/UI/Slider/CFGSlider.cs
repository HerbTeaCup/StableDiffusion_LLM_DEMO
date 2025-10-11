using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFGSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        float realValue = (float)System.Math.Round(value,1);
        GameManager.sdManager.txt2ImageBody.cfg_scale = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
