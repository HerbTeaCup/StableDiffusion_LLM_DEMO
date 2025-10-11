using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseSlider : MonoBehaviour, ISlider
{
    protected Slider slider;
    protected TMP_InputField inputField;

    protected virtual void Start()
    {
        slider = GetComponent<Slider>();
        inputField = GetComponentInChildren<TMP_InputField>();

        OnValueChanged(slider.value);

        //값 변화 이벤트 중복 방지하고 추가
        if (!IsListenerRegistered(slider.onValueChanged, nameof(OnValueChanged)))
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        // onEndEdit 이벤트에 중복 방지하고 추가
        if (!IsListenerRegistered(inputField.onEndEdit, nameof(OnInputfield)))
        {
            inputField.onEndEdit.AddListener(OnInputfield);
        }

        // onValueChanged 이벤트에 중복 방지하고 추가
        if (!IsListenerRegistered(inputField.onValueChanged, nameof(OnInputfield)))
        {
            inputField.onValueChanged.AddListener(OnInputfield);
        }
    }

    // 추상 메서드
    public abstract void OnValueChanged(float value);

    // InputField 입력 이벤트
    public void OnInputfield(string value)
    {
        if (float.TryParse(value, out float result))
        {
            OnValueChanged(result);
            slider.value = result;
        }
    }

    // 이벤트 리스너가 이미 등록되어 있는지 검사
    private bool IsListenerRegistered<T>(UnityEngine.Events.UnityEvent<T> unityEvent, string methodName)
    {
        for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
        {
            if (unityEvent.GetPersistentMethodName(i) == methodName)
            {
                return true;
            }
        }
        return false;
    }
}
