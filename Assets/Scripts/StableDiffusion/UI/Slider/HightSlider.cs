using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HightSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        int realValue = (int)System.Math.Round(value);
        ManagerResister.GetManager<SDManager>().txt2ImageBody.height = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
