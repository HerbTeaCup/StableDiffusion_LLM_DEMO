using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Success ������Ʈ�� �־ ���� ������ �ʱ�ȭ Ŭ������ ���� �־��� �� ����
//�� ���¸� �����ֱⰡ �ȸ¾Ƽ� CanvasGroup�� ���İ��� �����ϰų� ��ȣ�ۿ��� �������Ѽ� �������� �ƿ� �ؾ���
public class AsyncManager : ManagerBase<AsyncManager>
{
    public bool isloading = false; //Ping()�� ���� ������ ����  
    public bool connected = false; //������ �����ߴ���
    
    bool _refreshing = false; //Refresh()�� ���� ������ ����

    public List<IAsyncElement> awakeAsync = new List<IAsyncElement>();
    public List<IAsyncElement> asyncElements = new List<IAsyncElement>();
    public List<IAsyncElementWithPriority> dependentAsyncs = new List<IAsyncElementWithPriority>(); //is�� as�� ó�������� ���� ������

    //Awake�� OnEnable���� ����Ʈ���� ��Ұ� �߰��Ǵ� �ݵ�� start���� �ʱ�ȭ �������  
    private async void Start()
    {
        await Ping();
        if (connected)
            _ = UIUpdate();
    }

    //��ư���θ� �ҷ��� �޼ҵ�  
    //�ڵ�����δ� UIUpdate()�� ����ؾ���  
    public void UIUpdateRetry()
    {
        _ = UIUpdate();
    }

    public async void Refresh()
    {
        if (_refreshing)
        {
            Debug.LogWarning("Refresh is already in progress.");
            return; // �̹� Refresh�� ���� ���̸� �ߺ� ���� ����
        }

        _refreshing = true; // Refresh ����

        await Ping();
        if (connected == true)
        {
            await UIUpdate();
        }

        _refreshing = false; // Refresh �Ϸ�
    }

    /// <summary>
    /// ���ο��� �� �޼ҵ带 ȣ���Ͽ� WebUI���� ������ Ȯ��.
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
    /// UI ������Ʈ�� ���� �ʱ�ȭ �޼����Դϴ�.  
    /// </summary>  
    /// <returns>connected�� �����մϴ�</returns>  
    async Task UIUpdate()
    {
        if (connected == false)
        {
            Debug.LogError("Disconnected from WebUI...");
            return;
        }

        //������� �����ؾ� �ϴ� �͵�  
        await Task.WhenAll(awakeAsync.Select(d => d.Init()));

        //���� ��� ������ asyncElements���� �ٷ� ���� ����  
        //�Ϻη� await�� ������� �ʾƼ� �ӵ� ����� �븲  
        _ = Task.WhenAll(asyncElements.Select(d => d.Init()));

        //�������̰ų� ������ �ʿ��� ���  
        var ordered = dependentAsyncs.OrderBy(d => d.Priority).ToList();
        foreach (var d in ordered)
            await d.Init();
    }

    void OnApplicationQuit()
    {
        Communication.HttpDispose();
    }
}
