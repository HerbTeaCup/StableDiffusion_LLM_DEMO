using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviour, IManager
{
    public virtual int Order { get; protected set; } = 0;

    public abstract void ManagerSubScribe();

    public abstract void AfterAllManagerInitialized();
}
