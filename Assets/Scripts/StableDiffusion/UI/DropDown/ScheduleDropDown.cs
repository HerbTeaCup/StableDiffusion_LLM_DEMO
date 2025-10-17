using System.Threading.Tasks;
using static SDsetting;
using static Communication;

public class ScheduleDropDown : BaseDropDown<SDscheduler>, IAsyncElementWithPriority
{
    public int Priority => 2;

    protected override void OnEnable()
    {
        ManagerResister.GetManager<AsyncManager>().dependentAsyncs.Add(this);
    }

    /// <summary>
    /// this method is run when user select model
    /// </summary>
    public override void OnValueChanged(int index)
    {
        ManagerResister.GetManager<SDManager>().SchedulerModelIndex = index;

        //automatic일 경우 sampler에 맞게 scheduler를 변경
        if (ManagerResister.GetManager<SDManager>().schedulerModels[ManagerResister.GetManager<SDManager>().SchedulerModelIndex].label == "Automatic"
            || ManagerResister.GetManager<SDManager>().schedulerModels[ManagerResister.GetManager<SDManager>().SchedulerModelIndex].name == "automatic")
        {
            ManagerResister.GetManager<SDManager>().txt2ImageBody.scheduler =
                ManagerResister.GetManager<SDManager>().samplerModels[ManagerResister.GetManager<SDManager>().SamplerModelIndex].options.scheduler;
        }
        else //아니라면 지정한 스케쥴러로
        {
            ManagerResister.GetManager<SDManager>().txt2ImageBody.scheduler =
                ManagerResister.GetManager<SDManager>().schedulerModels[ManagerResister.GetManager<SDManager>().SchedulerModelIndex].name;
        }
    }

    protected override string GetAPIUrl()
    {
        return sDurls.schedulerAPI;
    }
    
    protected override string GetDisplayName(SDscheduler data)
    {
        return data.name;
    }

    protected override void OnDataApplied(SDscheduler[] data)
    {
        ManagerResister.GetManager<SDManager>().schedulerModels = data;
        ManagerResister.GetManager<SDManager>().SchedulerModelIndex = 0;

        ManagerResister.GetManager<SDManager>().txt2ImageBody.scheduler =
            ManagerResister.GetManager<SDManager>().schedulerModels[ManagerResister.GetManager<SDManager>().SchedulerModelIndex].name;
    }
}
