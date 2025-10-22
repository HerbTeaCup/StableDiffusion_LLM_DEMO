using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� Ŭ������ ��ϵ� IManager�鸸�� ManagerResister�� ��ϵ˴ϴ�.
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
