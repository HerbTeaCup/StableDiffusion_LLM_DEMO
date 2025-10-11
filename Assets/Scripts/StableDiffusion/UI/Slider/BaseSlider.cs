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

        //�� ��ȭ �̺�Ʈ �ߺ� �����ϰ� �߰�
        if (!IsListenerRegistered(slider.onValueChanged, nameof(OnValueChanged)))
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        // onEndEdit �̺�Ʈ�� �ߺ� �����ϰ� �߰�
        if (!IsListenerRegistered(inputField.onEndEdit, nameof(OnInputfield)))
        {
            inputField.onEndEdit.AddListener(OnInputfield);
        }

        // onValueChanged �̺�Ʈ�� �ߺ� �����ϰ� �߰�
        if (!IsListenerRegistered(inputField.onValueChanged, nameof(OnInputfield)))
        {
            inputField.onValueChanged.AddListener(OnInputfield);
        }
    }

    // �߻� �޼���
    public abstract void OnValueChanged(float value);

    // InputField �Է� �̺�Ʈ
    public void OnInputfield(string value)
    {
        if (float.TryParse(value, out float result))
        {
            OnValueChanged(result);
            slider.value = result;
        }
    }

    // �̺�Ʈ �����ʰ� �̹� ��ϵǾ� �ִ��� �˻�
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
