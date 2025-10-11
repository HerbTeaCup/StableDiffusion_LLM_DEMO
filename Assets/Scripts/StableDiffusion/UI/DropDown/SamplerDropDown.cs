using static SDsetting;
using static Communication;
using System.Threading.Tasks;

public class SamplerDropDown : BaseDropDown<SDsampler>, IAsyncElementWithPriority
{
    public int Priority => 1;

    protected override void OnEnable()
    {
        GameManager.asyncManager.dependentAsyncs.Add(this);
    }

    /// <summary>
    /// this method is run when user select model
    /// </summary>
    public override void OnValueChanged(int index)
    {
        GameManager.sdManager.txt2ImageBody.sampler_name =
            GameManager.sdManager.samplerModels[index].name;

        GameManager.sdManager.SamplerModelIndex = index;
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
        GameManager.sdManager.samplerModels = data;
        GameManager.sdManager.SamplerModelIndex = 0;

        GameManager.sdManager.txt2ImageBody.sampler_name =
            GameManager.sdManager.samplerModels[GameManager.sdManager.SamplerModelIndex].name;
    }
}
