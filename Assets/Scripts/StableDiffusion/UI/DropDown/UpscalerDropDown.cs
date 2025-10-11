using System.Threading.Tasks;
using static SDsetting;
using static Communication;

public class UpscalerDropDown : BaseDropDown<UpscalerModel>
{
    //DropDown 변경시 인덱스 값 변경
    public override void OnValueChanged(int index)
    {
        GameManager.sdManager.txt2ImageBody.hr_upscaler =
            GameManager.sdManager.upscalerModels[index].name;

        GameManager.sdManager.UpscalerModelIndex = index;
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
        GameManager.sdManager.upscalerModels = data;
        GameManager.sdManager.UpscalerModelIndex = 0;

        GameManager.sdManager.txt2ImageBody.hr_upscaler =
            GameManager.sdManager.upscalerModels[GameManager.sdManager.UpscalerModelIndex].name;
    }
}
