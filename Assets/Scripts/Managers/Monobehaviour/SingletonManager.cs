using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour�� ���� ���׸� �̱��� �⺻ Ŭ�����Դϴ�
/// </summary>
public abstract class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;

    public static T Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ����, ���� ���� ���� �ƴ϶��
            if (_instance == null && !isAppQuitting)
            {
                // ������ ���� ã�ƺ��ϴ�.
                _instance = FindObjectOfType<T>();

                // ������ ���ٸ� �������� �����մϴ�.
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    // ���� ����� �� ���� ��ü(Ghost Object)�� ����� ���� ����
    private static bool isAppQuitting = false;
    protected virtual void OnApplicationQuit()
    {
        isAppQuitting = true;
    }

    /// <summary>
    /// �̱��� �ν��Ͻ��� �����ϰ� �ߺ��� �����ϴ� Awake ����
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            // �� �ν��Ͻ��� �̱������� ����
            _instance = this as T;

            // �� ��ȯ �� �ı����� �ʵ��� ���� (���� ���������� �Ŵ������� �ʼ�)
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            // �̹� �ٸ� �ν��Ͻ��� �����ϸ�, �ڽ��� �ı�
            Debug.LogWarning($"�ߺ��� �̱��� [{typeof(T).Name}]�� �����Ǿ� �ı��˴ϴ�.");
            Destroy(this.gameObject);
        }
    }
}
