using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 클래스에 등록된 IManager들만이 ManagerResister에 등록됩니다.
/// </summary>
public class ManagerInitializer : MonoBehaviour
{
    [SerializeField] List<ManagerBase> managers;

    private void Awake()
    {
        managers.Sort((a, b) => a.Order.CompareTo(b.Order));

        foreach (var manager in managers)
        {
            ManagerResister.AddManager(manager.GetType(), manager);
        }

        foreach (var manager in managers)
        {
            manager.AfterAllManagerInitialized();
        }
    }
}
