using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour를 위한 제네릭 싱글톤 기본 클래스입니다
/// </summary>
public abstract class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;

    public static T Instance
    {
        get
        {
            // 인스턴스가 아직 없고, 앱이 종료 중이 아니라면
            if (_instance == null && !isAppQuitting)
            {
                // 씬에서 먼저 찾아봅니다.
                _instance = FindObjectOfType<T>();

                // 씬에도 없다면 동적으로 생성합니다.
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    // 앱이 종료될 때 유령 객체(Ghost Object)가 생기는 것을 방지
    private static bool isAppQuitting = false;
    protected virtual void OnApplicationQuit()
    {
        isAppQuitting = true;
    }

    /// <summary>
    /// 싱글톤 인스턴스를 설정하고 중복을 제거하는 Awake 로직
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            // 이 인스턴스를 싱글톤으로 설정
            _instance = this as T;

            // 씬 전환 시 파괴되지 않도록 설정 (선택 사항이지만 매니저에겐 필수)
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            // 이미 다른 인스턴스가 존재하면, 자신을 파괴
            Debug.LogWarning($"중복된 싱글톤 [{typeof(T).Name}]이 생성되어 파괴됩니다.");
            Destroy(this.gameObject);
        }
    }
}
