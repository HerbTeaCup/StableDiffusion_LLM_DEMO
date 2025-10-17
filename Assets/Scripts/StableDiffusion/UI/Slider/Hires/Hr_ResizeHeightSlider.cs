using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hr_ResizeHeightSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        int realValue = (int)System.Math.Round(value);
        ManagerResister.GetManager<SDManager>().txt2ImageBody.hr_resize_y = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
