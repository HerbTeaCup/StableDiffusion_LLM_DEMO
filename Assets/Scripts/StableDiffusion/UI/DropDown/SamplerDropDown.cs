using static SDsetting;
using static Communication;
using System.Threading.Tasks;

public class SamplerDropDown : BaseDropDown<SDsampler>, IAsyncElementWithPriority
{
    public int Priority => 1;

    protected override void OnEnable()
    {
        AsyncManager.Instance.dependentAsyncs.Add(this);
    }

    /// <summary>
    /// this method is run when user select model
    /// </summary>
    public override void OnValueChanged(int index)
    {
        SDManager.Instance.txt2ImageBody.sampler_name =
            SDManager.Instance.samplerModels[index].name;

        SDManager.Instance.SamplerModelIndex = index;
    }

    protected override string GetAPIUrl()
    {
        return sDurls.samplerAPI;
    }

    protected override string GetDisplayName(SDsampler data)
    {
        return data.name;
    }

    protected override void OnDataApplied(SDsampler[] data)
    {
        SDManager.Instance.samplerModels = data;
        SDManager.Instance.SamplerModelIndex = 0;

        SDManager.Instance.txt2ImageBody.sampler_name =
            SDManager.Instance.samplerModels[SDManager.Instance.SamplerModelIndex].name;
    }
}
