using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static SDsetting;
using static Communication;


//제네릭으로 선언해서 어떤 타입으로 파싱할 건지 자식 클래스에서 지정할 수 있게
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
    /// API 호출 URL을 반환합니다.
    /// </summary>
    protected abstract string GetAPIUrl();

    /// <summary>
    /// DropDown에 표시할 데이터의 이름을 반환합니다.
    /// Linq의 Select를 사용하여 반환된 데이터를 설정한 필드를 리스트로 반환합니다.
    /// </summary>
    /// <param name="data">반환할 데이터의 요소</param>
    protected abstract string GetDisplayName(TData data);

    /// <summary>
    /// 어떻게 표시할지, 어떻게 적용할지에 대한 로직을 작성합니다.
    /// 인덱스를 수정하거나 SDManager에 바인딩하는 작업입니다.
    /// </summary>
    /// <param name="data"></param>
    protected abstract void OnDataApplied(TData[] data);

    public virtual async Task Init()
    {
        //설정객체가 모종의 이유로 없다면
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
            //자식클래스에서 정의한 TData타입으로 역직렬화 반환
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

        dropdown.ClearOptions();        //기존의 요소가 있을 수도 있으니 싹 치우기
        dropdown.AddOptions(showList);  //값들 추가

        OnDataApplied(dataList);

        _refreshing = false;
    }

    public abstract void OnValueChanged(int index);
}

