using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchCountSlider : BaseSlider
{
    public override void OnValueChanged(float value)
    {
        Debug.Log($"�̱��� : {this.name}");
    }
}
