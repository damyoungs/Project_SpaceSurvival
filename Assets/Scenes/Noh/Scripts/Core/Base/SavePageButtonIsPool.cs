using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePageButtonIsPool : ObjectIsPool
{
    /// <summary>
    /// ȭ�鿡 ������ ����¡��ȣ 
    /// </summary>
    int pageIndex = -1;
    public int PageIndex { 
        get => pageIndex;
        set { 
            pageIndex = value;
            text.text = $"{pageIndex}";
        }
    }
    /// <summary>
    /// ó���� Ŭ���� ��������
    /// </summary>
    SaveDataSort proccessClass;

    /// <summary>
    /// ȭ�鿡 ������ �ؽ�Ʈ��ġ ��������
    /// </summary>
    TextMeshProUGUI text;

    /// <summary>
    /// ������Ʈã��
    /// </summary>
    private void Awake()
    {
        isPositionReset = false; //Ȱ��ȭ�� ���������� �����̼� �ʱ�ȭ�������ʴ´�.
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        proccessClass = transform.parent.parent.parent.GetComponent<SaveDataSort>();
    }

    /// <summary>
    /// ������ ��ư Ŭ�� �̺�Ʈ
    /// </summary>
    public void OnPageDownButton()
    {
        if (pageIndex > -1)
        {
            proccessClass.SetPageList(pageIndex-1); //ȭ�鿡�� 1���� �����̱⶧���� �迭ó���ϱ����� -1
        }
    }
}
