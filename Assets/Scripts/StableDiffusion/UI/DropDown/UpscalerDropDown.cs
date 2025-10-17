using System.Threading.Tasks;
using static SDsetting;
using static Communication;

public class UpscalerDropDown : BaseDropDown<UpscalerModel>
{
    //DropDown 변경시 인덱스 값 변경
    public override void OnValueChanged(int index)
    {
        SDManager.Instance.txt2ImageBody.hr_upscaler =
            SDManager.Instance.upscalerModels[index].name;

        SDManager.Instance.UpscalerModelIndex = index;
    }

    protected override string GetAPIUrl()
    {
        return sDurls.upscalerAPI;
    }

    protected override string GetDisplayName(UpscalerModel data)
    {
        return data.name;
    }

    protected override void OnDataApplied(UpscalerModel[] data)
    {
        SDManager.Instance.upscalerModels = data;
        SDManager.Instance.UpscalerModelIndex = 0;

        SDManager.Instance.txt2ImageBody.hr_upscaler =
            SDManager.Instance.upscalerModels[SDManager.Instance.UpscalerModelIndex].name;
    }
}
