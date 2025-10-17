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

        //automatic�� ��� sampler�� �°� scheduler�� ����
        if (ManagerResister.GetManager<SDManager>().schedulerModels[ManagerResister.GetManager<SDManager>().SchedulerModelIndex].label == "Automatic"
            || ManagerResister.GetManager<SDManager>().schedulerModels[ManagerResister.GetManager<SDManager>().SchedulerModelIndex].name == "automatic")
        {
            ManagerResister.GetManager<SDManager>().txt2ImageBody.scheduler =
                ManagerResister.GetManager<SDManager>().samplerModels[ManagerResister.GetManager<SDManager>().SamplerModelIndex].options.scheduler;
        }
        else //�ƴ϶�� ������ �����췯��
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
