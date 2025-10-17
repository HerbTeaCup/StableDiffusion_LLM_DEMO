using System.Threading.Tasks;
using static SDsetting;
using static Communication;

public class UpscalerDropDown : BaseDropDown<UpscalerModel>
{
    //DropDown 변경시 인덱스 값 변경
    public override void OnValueChanged(int index)
    {
        ManagerResister.GetManager<SDManager>().txt2ImageBody.hr_upscaler =
            ManagerResister.GetManager<SDManager>().upscalerModels[index].name;

        ManagerResister.GetManager<SDManager>().UpscalerModelIndex = index;
    }

    protected override string GetAPIUrl()
    {
        return urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Upscaler);
    }

    protected override string GetDisplayName(UpscalerModel data)
    {
        return data.name;
    }

    protected override void OnDataApplied(UpscalerModel[] data)
    {
        ManagerResister.GetManager<SDManager>().upscalerModels = data;
        ManagerResister.GetManager<SDManager>().UpscalerModelIndex = 0;

        ManagerResister.GetManager<SDManager>().txt2ImageBody.hr_upscaler =
            ManagerResister.GetManager<SDManager>().upscalerModels[ManagerResister.GetManager<SDManager>().UpscalerModelIndex].name;
    }
}
