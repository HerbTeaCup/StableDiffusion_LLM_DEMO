using System.Threading.Tasks;
using static SDsetting;
using static Communication;

public class ScheduleDropDown : BaseDropDown<SDscheduler>, IAsyncElementWithPriority
{
    public int Priority => 2;

    protected override void OnEnable()
    {
        GameManager.asyncManager.dependentAsyncs.Add(this);
    }

    /// <summary>
    /// this method is run when user select model
    /// </summary>
    public override void OnValueChanged(int index)
    {
        GameManager.sdManager.SchedulerModelIndex = index;

        //automatic�� ��� sampler�� �°� scheduler�� ����
        if (GameManager.sdManager.schedulerModels[GameManager.sdManager.SchedulerModelIndex].label == "Automatic"
            || GameManager.sdManager.schedulerModels[GameManager.sdManager.SchedulerModelIndex].name == "automatic")
        {
            GameManager.sdManager.txt2ImageBody.scheduler =
                GameManager.sdManager.samplerModels[GameManager.sdManager.SamplerModelIndex].options.scheduler;
        }
        else //�ƴ϶�� ������ �����췯��
        {
            GameManager.sdManager.txt2ImageBody.scheduler =
                GameManager.sdManager.schedulerModels[GameManager.sdManager.SchedulerModelIndex].name;
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
        GameManager.sdManager.schedulerModels = data;
        GameManager.sdManager.SchedulerModelIndex = 0;

        GameManager.sdManager.txt2ImageBody.scheduler =
            GameManager.sdManager.schedulerModels[GameManager.sdManager.SchedulerModelIndex].name;
    }
}
