using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static SDsetting;
using static Communication;


//���׸����� �����ؼ� � Ÿ������ �Ľ��� ���� �ڽ� Ŭ�������� ������ �� �ְ�
public abstract class BaseDropDown<TData> : MonoBehaviour, IDropDown
{
    protected virtual void OnEnable()
    {
        GameManager.asyncManager.asyncElements.Add(this);
    }

    protected TMP_Dropdown dropdown;
    protected bool _refreshing = false;
    [SerializeField] protected byte subStringLength = 24;

    /// <summary>
    /// API ȣ�� URL�� ��ȯ�մϴ�.
    /// </summary>
    protected abstract string GetAPIUrl();

    /// <summary>
    /// DropDown�� ǥ���� �������� �̸��� ��ȯ�մϴ�.
    /// Linq�� Select�� ����Ͽ� ��ȯ�� �����͸� ������ �ʵ带 ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="data">��ȯ�� �������� ���</param>
    protected abstract string GetDisplayName(TData data);

    /// <summary>
    /// ��� ǥ������, ��� ���������� ���� ������ �ۼ��մϴ�.
    /// �ε����� �����ϰų� SDManager�� ���ε��ϴ� �۾��Դϴ�.
    /// </summary>
    /// <param name="data"></param>
    protected abstract void OnDataApplied(TData[] data);

    public virtual async Task Init()
    {
        //������ü�� ������ ������ ���ٸ�
        if (GameManager.sdManager.config == null)
        {
            GameManager.sdManager.config = await GetRequestAsync<Config>(sDurls.optionAPI);
        }

        if (_refreshing) return;
        _refreshing = true;

        dropdown = GetComponent<TMP_Dropdown>();

        TData[] dataList = null;

        try
        {
            //�ڽ�Ŭ�������� ������ TDataŸ������ ������ȭ ��ȯ
            dataList = await GetRequestAsync<TData[]>(GetAPIUrl());
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
            return;
        }

        if (dataList == null || dataList.Length == 0)
        {
            Debug.LogWarning($"[{GetType().Name}] No data fetched.");
            _refreshing = false;
            return;
        }

        List<string> showList = dataList.Select(GetDisplayName).ToList();
        for (int i = 0; i < showList.Count; i++)
        {
            if (showList[i].Length >= subStringLength)
                showList[i] = showList[i].Substring(0, subStringLength - 1) + "...";
        }

        dropdown.ClearOptions();        //������ ��Ұ� ���� ���� ������ �� ġ���
        dropdown.AddOptions(showList);  //���� �߰�

        OnDataApplied(dataList);

        _refreshing = false;
    }

    public abstract void OnValueChanged(int index);
}

