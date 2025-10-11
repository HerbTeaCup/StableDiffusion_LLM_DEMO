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
        GameManager.sdManager.CheckpointIndex = index;

        dropdown.interactable = false;

        GameManager.sdManager.config.sd_model_checkpoint = GameManager.sdManager.checkpoints[index].model_name;//�����ϰ�

        await Communication.PostRequestAsync<Config>(Communication.sDurls.optionAPI, GameManager.sdManager.config);//WebUI Config�� ����(�ϰ���)

        Debug.Log($"Select Model: {GameManager.sdManager.config.sd_model_checkpoint}");

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

        GameManager.sdManager.checkpoints = data;

        for (int i = 0; i < GameManager.sdManager.checkpoints.Length; i++)
        {
            if (GameManager.sdManager.checkpoints[i].model_name == GameManager.sdManager.config.sd_model_checkpoint ||
                GameManager.sdManager.checkpoints[i].title == GameManager.sdManager.config.sd_model_checkpoint)
            {
                dropdown.value = i;
                GameManager.sdManager.CheckpointIndex = i;
                break;
            }
        }

        //dropdonw.value = i���� �ڵ����� OnValueChanged�� ȣ���.
        //���� �������� ȣ���ϸ� ������ ����ǹǷ� �ּ��� �޾Ƶ�.

        dropdown.onValueChanged.AddListener(OnValueChanged);
    }
}
