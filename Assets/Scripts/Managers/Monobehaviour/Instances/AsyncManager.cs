using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Success 오브젝트에 있어서 연결 성공시 초기화 클래스를 따로 둬야할 것 같음
//이 상태면 생명주기가 안맞아서 CanvasGroup의 알파값을 변경하거나 상호작용을 금지시켜서 눈가리고 아웅 해야함
public class AsyncManager : ManagerBase<AsyncManager>
{

    public bool isloading = false; //Ping()이 실행 중인지 여부  
    public bool connected = false; //연결이 성공했는지
    
    bool _refreshing = false; //Refresh()가 실행 중인지 여부

    public List<IAsyncElement> AwakeAsync => _awakeAsync;
    List<IAsyncElement> _awakeAsync = new List<IAsyncElement>();

    public List<IAsyncElement> AsyncElements => _asyncElements;
    List<IAsyncElement> _asyncElements = new List<IAsyncElement>();

    public List<IAsyncElementWithPriority> DependentAsyncs => _dependentAsyncs;
    List<IAsyncElementWithPriority> _dependentAsyncs = new List<IAsyncElementWithPriority>(); //is나 as로 처리할지는 아직 못정함

    //Awake와 OnEnable에서 리스트들의 요소가 추가되니 반드시 start에서 초기화 해줘야함  
    private async void Start()
    {
        StartCoroutine(WaitNull()); //한 프레임 대기 : 다른 매니저들이 Awake에서 등록하거나, OnEnable에서 Elements에 추가할 시간을 줌

        await Ping();
        if (connected)
            UIUpdate();
    }

    IEnumerator WaitNull()
    {
        yield return null;
    }

    //버튼으로만 불러올 메소드  
    //코드상으로는 UIUpdate()를 사용해야함  
    public void UIUpdateRetry()
    {
        UIUpdate();
    }

    public async void Refresh()
    {
        if (_refreshing)
        {
            Debug.LogWarning("Refresh is already in progress.");
            return; // 이미 Refresh가 실행 중이면 중복 실행 방지
        }

        _refreshing = true; // Refresh 시작

        await Ping();
        if (connected == true)
        {
            UIUpdate();
        }

        _refreshing = false; // Refresh 완료
    }

    /// <summary>
    /// 내부에서 이 메소드를 호출하여 WebUI와의 연결을 확인.
    /// </summary>
    /// <returns></returns>
    async Task<bool> Ping()
    {
        isloading = true;
        connected = await Communication.ConnectingCheck();
        isloading = false;

        return connected;
    }

    /// <summary>  
    /// UI 업데이트를 위한 초기화 메서드입니다.  
    /// </summary>  
    /// <returns>connected를 리턴합니다</returns>  
    async void UIUpdate()
    {
        if (connected == false)
        {
            Debug.LogError("Disconnected from WebUI...");
            return;
        }

        //가장먼저 실행해야 하는 것들  
        await Task.WhenAll(AwakeAsync.Select(d => d.Init()));

        //순서 상관 없으면 asyncElements부터 바로 병렬 실행  
        //일부러 await를 사용하지 않아서 속도 향상을 노림  
        _ = Task.WhenAll(AsyncElements.Select(d => d.Init()));

        //의존적이거나 순서가 필요한 경우  
        var ordered = DependentAsyncs.OrderBy(d => d.Priority).ToList();
        foreach (var d in ordered)
            await d.Init();
    }

    void OnApplicationQuit()
    {
        Communication.HttpDispose();
    }
}
