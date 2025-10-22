using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ManagerBase<T> : ManagerBase where T : ManagerBase<T>
{
    protected virtual void OnDestroy()
    {
        ManagerResister.RemoveManager<T>();
    }

    /// <summary>
    /// ManagerResister에 자신을 등록합니다.
    /// ManagerInitializer에서 호출됩니다.
    /// </summary>
    public override void ManagerSubScribe()
    {
        ManagerResister.AddManager<T>((T)this);
    }

    //따지고보면 ISP위반이지만, ISP를 지키면 IManager를 쪼개야하는데, 오히려 복잡해지므로 그냥 둠.
    public override void AfterAllManagerInitialized()
    {
        // Override in derived classes if needed
    }
}
