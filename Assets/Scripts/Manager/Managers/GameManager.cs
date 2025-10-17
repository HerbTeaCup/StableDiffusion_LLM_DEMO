using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 게임매니저라 지칭했지만, 그냥 매니저 모아놓는 중간 브릿지입니다
/// </summary>
public class GameManager : MonoBehaviour
{
    //Monobehaviour
    static GameManager _instance;
    SDManager _sd;
    AsyncManager _async;

    //Not MonoBehaviour
    FileManager _file = new FileManager();

    public static GameManager instance { get { Init(); return _instance; } }
    public static SDManager sdManager { get { return instance._sd; } }
    public static AsyncManager asyncManager { get { return instance._async; } }
    public static FileManager fileManager { get { return instance._file; } }

    // Start is called before the first frame update
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        Init();
    }
    static void Init()
    {
        if (_instance != null)
        {
            return;
        }

        GameObject temp = GameObject.Find("@GameManager");

        if (temp == null)
        {
            temp = new GameObject("@GameManager");
            DontDestroyOnLoad(temp);
        }

        temp.TryGetComponent<GameManager>(out _instance);
        if (_instance == null) { _instance = temp.AddComponent<GameManager>(); }
        
        temp.TryGetComponent<SDManager>(out instance._sd);
        if (instance._sd == null) { instance._sd = temp.AddComponent<SDManager>(); }

        temp.TryGetComponent<AsyncManager>(out instance._async);
        if (instance._async == null) { instance._async = temp.AddComponent<AsyncManager>(); }
    }
}
