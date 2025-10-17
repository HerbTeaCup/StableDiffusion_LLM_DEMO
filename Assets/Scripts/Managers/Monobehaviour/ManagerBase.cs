using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour, IManager where T : ManagerBase<T>
{
    protected void Awake()
    {
        ManagerResister.AddManager<T>(this as T);
    }

    protected virtual void OnDestroy()
    {
        ManagerResister.RemoveManager<T>();
    }
}
