using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShowInit : MonoBehaviour
{
    //Dictionary로 하는 것도 고려했지만,
    //UI를 켜고 끄는 것만을 목적으로 하는 클래스이므로
    //+ UI가 매우 적은 수이므로
    //Dictionary를 사용하지 않고, CanvasGroup을 직접 참조하는 방식으로 구현함
    [Header("ShowUI")]
    [SerializeField] CanvasGroup _loading;
    [SerializeField] CanvasGroup _success;
    [SerializeField] CanvasGroup _fail;

    private void Update()
    {
        //로딩 중일 때
        if (GameManager.asyncManager.isloading)
        {
            Show(_loading);
            Hide(_success);
            Hide(_fail);
            return;
        }

        //로딩이 끝났을 때
        Hide(_loading);
        if (GameManager.asyncManager.connected == false)
        {
            Hide(_success);
            Show(_fail);
        }
        else
        {
            Show(_success);
            Hide(_fail);
        }
    }

    void Show(CanvasGroup group)
    {
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }
    void Hide(CanvasGroup group)
    {
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}
