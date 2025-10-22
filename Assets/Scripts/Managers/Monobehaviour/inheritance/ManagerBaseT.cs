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
    /// ManagerResister�� �ڽ��� ����մϴ�.
    /// ManagerInitializer���� ȣ��˴ϴ�.
    /// </summary>
    public override void ManagerSubScribe()
    {
        ManagerResister.AddManager<T>((T)this);
    }

    //�������� ISP����������, ISP�� ��Ű�� IManager�� �ɰ����ϴµ�, ������ ���������Ƿ� �׳� ��.
    public override void AfterAllManagerInitialized()
    {
        // Override in derived classes if needed
    }
}
