using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ManagerRegister
{
    static Dictionary<System.Type, object> _managers = new Dictionary<System.Type, object>();

    public static void AddManager<T>(T manager) where T : IManager
    {
        _managers.Add(manager.GetType(), manager);
    }

    public static T GetManager<T>() where T : IManager
    {
        return (T)_managers[typeof(T)];
    }
}
