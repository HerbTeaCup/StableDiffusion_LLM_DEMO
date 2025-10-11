using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShowInit : MonoBehaviour
{
    //Dictionary�� �ϴ� �͵� ���������,
    //UI�� �Ѱ� ���� �͸��� �������� �ϴ� Ŭ�����̹Ƿ�
    //+ UI�� �ſ� ���� ���̹Ƿ�
    //Dictionary�� ������� �ʰ�, CanvasGroup�� ���� �����ϴ� ������� ������
    [Header("ShowUI")]
    [SerializeField] CanvasGroup _loading;
    [SerializeField] CanvasGroup _success;
    [SerializeField] CanvasGroup _fail;

    private void Update()
    {
        //�ε� ���� ��
        if (GameManager.asyncManager.isloading)
        {
            Show(_loading);
            Hide(_success);
            Hide(_fail);
            return;
        }

        //�ε��� ������ ��
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
