using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SDsetting;
using System.Threading.Tasks;

public class ModelDropDown : BaseDropDown<SDmodel>
{

    public void ButtonPressed()
    {
        _ = Init();
    }

    /// <summary>
    /// this method is run when user select model
    /// </summary>
    public override async void OnValueChanged(int index)
    {
        SDManager.Instance.CheckpointIndex = index;

        dropdown.interactable = false;

        SDManager.Instance.config.sd_model_checkpoint = SDManager.Instance.checkpoints[index].model_name;//�����ϰ�

        await Communication.PostRequestAsync<Config>(Communication.sDurls.optionAPI, SDManager.Instance.config);//WebUI Config�� ����(�ϰ���)

        Debug.Log($"Select Model: {SDManager.Instance.config.sd_model_checkpoint}");

        dropdown.interactable = true;
    }

    protected override string GetAPIUrl()
    {
        return Communication.sDurls.sd_modelsAPI;
    }

    protected override string GetDisplayName(SDmodel data)
    {
        return data.model_name;
    }

    protected override void OnDataApplied(SDmodel[] data)
    {
        dropdown.onValueChanged.RemoveAllListeners();

        SDManager.Instance.checkpoints = data;

        for (int i = 0; i < SDManager.Instance.checkpoints.Length; i++)
        {
            if (SDManager.Instance.checkpoints[i].model_name == SDManager.Instance.config.sd_model_checkpoint ||
                SDManager.Instance.checkpoints[i].title == SDManager.Instance.config.sd_model_checkpoint)
            {
                dropdown.value = i;
                SDManager.Instance.CheckpointIndex = i;
                break;
            }
        }

        //dropdonw.value = i���� �ڵ����� OnValueChanged�� ȣ���.
        //���� �������� ȣ���ϸ� ������ ����ǹǷ� �ּ��� �޾Ƶ�.

        dropdown.onValueChanged.AddListener(OnValueChanged);
    }
}
