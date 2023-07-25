using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
/// <summary>
/// �ϰ����� ������ ������ �߰� �� ���� �����ؼ� �������� �޴��� 
/// </summary>
public class TurnGaugeManager : MonoBehaviour
{
    /// <summary>
    /// �ϰ����� ������ ����Ʈ
    /// </summary>
    List<GaugeUnit> gaugeList;

    /// <summary>
    /// �ϰ����� ��ġ
    /// </summary>
    Transform turnGaugeParent;

    private void Awake()
    {
        turnGaugeParent = transform.GetChild(0).GetChild(0);
        gaugeList = new List<GaugeUnit>(); 
    }
    
    /// <summary>
    /// �ϸ޴������� �����Ҽ��ְ� ��������Ʈ�� �Լ����
    /// </summary>
    private void Start()
    {
        WindowList.Instance.TurnManager.addTurnObjectGauge += UITurnUnitAdd; //�� ���� �߰��� ȣ���� �Լ� ���
        WindowList.Instance.TurnManager.removeTurnObjectGauge += UITurnUnitDelete; //�� ���� ������ ȣ���� �Լ� ���
    }
   
    /// <summary>
    /// �� ������ ���� �ϳ� �߰�
    /// </summary>
    /// <param name="addData">�߰��� ���� ������</param>
    private void UITurnUnitAdd(ITurnBaseData addData) 
    {
        GameObject obj = MultipleObjectsFactory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL);// Ǯ���� ������ �����´�.

        GaugeUnit gaugeUnit = obj.GetComponent<GaugeUnit>(); //������ ���۳�Ʈ ã�Ƽ�
        gaugeUnit.ProgressValue = addData.TurnActionValue; //�ʱⰪ ����
        gaugeList.Add(gaugeUnit); // �����븮��Ʈ�� �߰�
        addData.GaugeUnit = gaugeUnit; //�� ���ֿ� ������ ���� ĳ�� 
        obj.transform.parent = turnGaugeParent; //�θ���ġ ����
    }

    /// <summary>
    /// �� ������ ���� �ϳ� ���� 
    /// </summary>
    /// <param name="deleteData">�Ͽ��� ������ ���ֵ�����</param>
    private void UITurnUnitDelete(ITurnBaseData deleteData) 
    {
        GaugeUnit gaugeUnit = deleteData.GaugeUnit;
        gaugeUnit.ProgressValue = -1.0f; //�� ���½�Ű��
        gaugeUnit.gameObject.SetActive(false); // ť�� ������ 
        gaugeUnit.transform.SetParent(gaugeUnit.PoolTransform); // Ǯ�� ������.
        gaugeList.Remove(gaugeUnit);//������ ����Ʈ���� �����
        deleteData.GaugeUnit = null; // ���������� ��������
    }


    /// <summary>
    /// �ϰ����� ����Ʈ �ʱ�ȭ��Ű��
    /// </summary>
    public void ResetTurnGaugeList(LinkedList<ITurnBaseData> turnList) 
    {
        foreach (ITurnBaseData node in turnList)
        {
             UITurnUnitDelete(node); //�ϸ���Ʈ�� �ִ� ���밡���� �����ϱ�
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
