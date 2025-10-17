using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DenoisingSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        float realValue = (float)System.Math.Round(value, 2);
        ManagerResister.GetManager<SDManager>().txt2ImageBody.denoising_strength = realValue;

        if (!inputField.isFocused)
        {
            inputField.text = $"{realValue}";
        }
    }
}
