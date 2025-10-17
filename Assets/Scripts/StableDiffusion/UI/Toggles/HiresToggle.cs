using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiresToggle : MonoBehaviour
{
    UnityEngine.UI.Toggle toggle;

    [SerializeField] CanvasGroup targetGroup;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<UnityEngine.UI.Toggle>();

        toggle.isOn = ManagerResister.GetManager<SDManager>().txt2ImageBody.enable_hr;
        Init();
        toggle.onValueChanged.AddListener(OnClick);
    }

    void OnClick(bool input)
    {
        ManagerResister.GetManager<SDManager>().txt2ImageBody.enable_hr = toggle.isOn;

        if (targetGroup == null)
        {
            Debug.LogWarning($"{this.gameObject.name}'s TargetGroup is not assigned.");
            return;
        }

        targetGroup.alpha = toggle.isOn ? 1 : 0.35f;
        targetGroup.interactable = toggle.isOn;
    }

    void Init()
    {
        if (targetGroup == null)
        {
            Debug.LogWarning($"{this.gameObject.name}'s TargetGroup is not assigned.");
            return;
        }

        targetGroup.alpha = toggle.isOn ? 1 : 0.35f;
        targetGroup.interactable = toggle.isOn;
    }
}
