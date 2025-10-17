using System.Threading.Tasks;
using static SDsetting;
using static Communication;

public class ScheduleDropDown : BaseDropDown<SDscheduler>, IAsyncElementWithPriority
{
    public int Priority => 2;

    protected override void OnEnable()
    {
        AsyncManager.Instance.dependentAsyncs.Add(this);
    }

    /// <summary>
    /// this method is run when user select model
    /// </summary>
    public override void OnValueChanged(int index)
    {
        SDManager.Instance.SchedulerModelIndex = index;

        //automatic�� ��� sampler�� �°� scheduler�� ����
        if (SDManager.Instance.schedulerModels[SDManager.Instance.SchedulerModelIndex].label == "Automatic"
            || SDManager.Instance.schedulerModels[SDManager.Instance.SchedulerModelIndex].name == "automatic")
        {
            SDManager.Instance.txt2ImageBody.scheduler =
                SDManager.Instance.samplerModels[SDManager.Instance.SamplerModelIndex].options.scheduler;
        }
        else //�ƴ϶�� ������ �����췯��
        {
            SDManager.Instance.txt2ImageBody.scheduler =
                SDManager.Instance.schedulerModels[SDManager.Instance.SchedulerModelIndex].name;
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
        SDManager.Instance.schedulerModels = data;
        SDManager.Instance.SchedulerModelIndex = 0;

        SDManager.Instance.txt2ImageBody.scheduler =
            SDManager.Instance.schedulerModels[SDManager.Instance.SchedulerModelIndex].name;
    }
}
