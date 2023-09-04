using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TurnGaugeOnOff : MonoBehaviour
{
    /// <summary>
    /// ��ġ������ ��ư �θ������Ʈ
    /// </summary>
    RectTransform battleButtonRt;

    /// <summary>
    /// �ϰ����� UI ���� ���ϱ�
    /// </summary>
    float viewHeight = 0.0f;
    /// <summary>
    /// ���̴¼ӵ�
    /// </summary>
    [SerializeField]
    float moveSpeed = 1000.0f;
    /// <summary>
    /// �ð�������
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// �ӽ÷� ��Ƶ� ���� 
    /// </summary>
    Vector2 tempMin = Vector2.zero;
    Vector2 tempMax = Vector2.zero;
    private void Awake()
    {
        RectTransform rt = GetComponent<RectTransform>();
        viewHeight = rt.rect.height;
        
        //��û ������ ã�ƿ��� .. ��ġ�ٲ��� ã�����ְ�.
        Transform battleController = transform.parent.GetComponentInChildren<BattleActionUIController>(true).transform;

        battleButtonRt = battleController.parent.parent.GetComponent<RectTransform>();
        Debug.Log(battleButtonRt.name);
    }
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(UIOpen()); //�ٽ� ����
    }
    private void OnDisable()
    {
        SetTopBottomValue(0);
        //StopAllCoroutines();
        //StartCoroutine(UIClose());//�ٽ� �ݴ´� //��Ȱ��ȭ�� �ڷ�ƾ����ȵ�

    }
    IEnumerator UIOpen()
    {
        while (timeElapsed > viewHeight)
        {
            timeElapsed += Time.deltaTime * moveSpeed; //�ε巴�� ������Ű������ ��ŸŸ�� ������Ű��
            SetTopBottomValue(timeElapsed);
            yield return null;

        }
        SetTopBottomValue(viewHeight);//���� ��Ŀ�����־ ��ġ�°��� �ذ��ϱ����� �߰�

    }
    IEnumerator UIClose()
    {
        while (timeElapsed < 0)
        {
            timeElapsed -= Time.deltaTime * moveSpeed; //�ε巴�� ������Ű������ ��ŸŸ�� ������Ű��
            SetTopBottomValue(timeElapsed);
            yield return null;

        }
        SetTopBottomValue(0);//���� �� �۾��� �� �־ �ذ��ϱ����� �߰�
    }

    /// <summary>
    ///  rectTransform �� top�� bottom ���� 
    ///  �������ڰ����� �����Ѵ�  top �� -���ڰ�  bottom �� ���ڰ� �״�� 
    ///<param name="value">������ ��</param>
    /// </summary>
    private void SetTopBottomValue(float value)
    {
        tempMin = battleButtonRt.offsetMin; //bottom
        tempMax = battleButtonRt.offsetMax; //top
        tempMin.y = value; //bottom ���� ����� �״�� ����
        tempMax.y = value; //top ���� ����� ������ ���������� ��ȯ�Ǿ� ����.
        battleButtonRt.offsetMin = tempMin; //bottom
        battleButtonRt.offsetMax = tempMax; //top
    }
}
