using static SDsetting;
using static Communication;
using System.Threading.Tasks;

public class SamplerDropDown : BaseDropDown<SDsampler>, IAsyncElementWithPriority
{
    public int Priority => 1;

    protected override void OnEnable()
    {
        ManagerResister.GetManager<AsyncManager>().dependentAsyncs.Add(this);
    }

    /// <summary>
    /// this method is run when user select model
    /// </summary>
    public override void OnValueChanged(int index)
    {
        ManagerResister.GetManager<SDManager>().txt2ImageBody.sampler_name =
            ManagerResister.GetManager<SDManager>().samplerModels[index].name;

        ManagerResister.GetManager<SDManager>().SamplerModelIndex = index;
    }

    protected override string GetAPIUrl()
    {
        return urlManager.StableDiffusion.GetUrl(StableDiffusionRequestPurpose.Samplers);
    }

    protected override string GetDisplayName(SDsampler data)
    {
        return data.name;
    }

    protected override void OnDataApplied(SDsampler[] data)
    {
        ManagerResister.GetManager<SDManager>().samplerModels = data;
        ManagerResister.GetManager<SDManager>().SamplerModelIndex = 0;

        ManagerResister.GetManager<SDManager>().txt2ImageBody.sampler_name =
            ManagerResister.GetManager<SDManager>().samplerModels[ManagerResister.GetManager<SDManager>().SamplerModelIndex].name;
    }
}
