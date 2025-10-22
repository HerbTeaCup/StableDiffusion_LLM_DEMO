using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager 
{
    /// <summary>
    /// 낮은 값이 먼저 초기화 됩니다.
    /// </summary>
    int Order { get; }
    /// <summary>
    /// 매니저가 모두 등록된 후 호출됩니다.
    /// </summary>
    void AfterAllManagerInitialized();
}

public static class ManagerResister
{
    static Dictionary<System.Type, IManager> _managers = new();

    public static void AddManager(System.Type type, IManager manager)
    {
        if(_managers.ContainsKey(type))
        {
            Debug.LogWarning($"Manager of type {type} is already registered.");
            return;
        }
        _managers.Add(type, manager);
    }

    public static void AddManager<T>(T manager) where T : IManager
    {
        if (_managers.ContainsKey(typeof(T)))
        {
            Debug.LogWarning($"Manager of type {typeof(T)} is already registered.");
            return;
        }
        _managers.Add(typeof(T), manager);
    }
    public static T GetManager<T>() where T : IManager
    {
        if (_managers.TryGetValue(typeof(T), out var manager))
        {
            return (T)manager;
        }
        throw new KeyNotFoundException($"Manager of type {typeof(T)} not found.");
    }

    public static void RemoveManager<T>() where T : IManager
    {
        var type = typeof(T);
        if (_managers.ContainsKey(type))
        {
            _managers.Remove(type);
            Debug.Log($"[ManagerResister] Unregistered: {type.Name}");
        }
        else
        {
            Debug.LogWarning($"Attempted to unregister a manager of type {type} that was not registered.");
        }
    }
    public static void ClearAllManagers()
    {
        _managers.Clear();
        Debug.Log("[ManagerResister] All managers cleared.");
    }
}
