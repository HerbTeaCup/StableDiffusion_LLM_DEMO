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
        ManagerResister.GetManager<SDManager>().CheckpointIndex = index;

        dropdown.interactable = false;

        ManagerResister.GetManager<SDManager>().config.sd_model_checkpoint = ManagerResister.GetManager<SDManager>().checkpoints[index].model_name;//수정하고
        string optionUrl = urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Options);
        HeaderSetting header= urlManager.StableDiffusion.GetHeader(HeaderPurpose.Accept);

        await Communication.PostRequestAsync<Config>(optionUrl, header, ContentType.Json, ManagerResister.GetManager<SDManager>().config);//WebUI Config와 같게(일관성)

        Debug.Log($"Select Model: {ManagerResister.GetManager<SDManager>().config.sd_model_checkpoint}");

        dropdown.interactable = true;
    }

    protected override string GetAPIUrl()
    {
        return urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.SDModels);
    }

    protected override string GetDisplayName(SDmodel data)
    {
        return data.model_name;
    }

    protected override void OnDataApplied(SDmodel[] data)
    {
        dropdown.onValueChanged.RemoveAllListeners();

        ManagerResister.GetManager<SDManager>().checkpoints = data;

        for (int i = 0; i < ManagerResister.GetManager<SDManager>().checkpoints.Length; i++)
        {
            if (ManagerResister.GetManager<SDManager>().checkpoints[i].model_name == ManagerResister.GetManager<SDManager>().config.sd_model_checkpoint ||
                ManagerResister.GetManager<SDManager>().checkpoints[i].title == ManagerResister.GetManager<SDManager>().config.sd_model_checkpoint)
            {
                dropdown.value = i;
                ManagerResister.GetManager<SDManager>().CheckpointIndex = i;
                break;
            }
        }

        //dropdonw.value = i에서 자동으로 OnValueChanged가 호출됨.
        //따라서 수동으로 호출하면 여러번 실행되므로 주석만 달아둠.

        dropdown.onValueChanged.AddListener(OnValueChanged);
    }
}
