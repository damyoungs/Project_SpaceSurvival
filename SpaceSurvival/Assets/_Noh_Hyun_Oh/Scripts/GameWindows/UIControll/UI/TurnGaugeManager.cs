using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TurnGaugeManager : MonoBehaviour
{
    public Action addGauge;
    public Action removeGauge;
    public Action refreshGauge;
    public Action<LinkedList<ITurnBaseData>> initGauge;

    private void Awake()
    {
        addGauge += UITurnUnitAdd; 
        removeGauge += UITurnUnitDelete;
        refreshGauge += UITurnUnitUpdate;
        initGauge = UITurnUnitInit;
    }

    private void InitTurnGaugeList(LinkedList<ITurnBaseData> turnList)
    {
        GaugeUnit gaugeUnit;
        GameObject obj;
        LinkedListNode<ITurnBaseData> tempNode = turnList.First;
        //초기화 코드
        for (int i = 0; i < turnList.Count; i++)
        {
            obj = MultipleObjectsFactory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL);
            gaugeUnit = obj.GetComponent<GaugeUnit>();
            gaugeUnit.Unit = tempNode.Value;
            gaugeUnit.ProgressValue = tempNode.Value.TurnActionValue;
            if (tempNode != null) tempNode = tempNode.Next;
        }
    }
    private void ResetTurnGaugeList()
    {
        Transform tf = transform.GetChild(0);
        int unitLength = tf.childCount;
        GaugeUnit gu;
        for (int i = 0; i < unitLength; i++)
        {
            if (tf.GetChild(i).gameObject.activeSelf) 
            {
                gu = tf.GetChild(i).GetComponent<GaugeUnit>();
                gu.ProgressValue = gu.Unit.TurnActionValue;
            }
        }
    }

    private void UITurnUnitInit(LinkedList<ITurnBaseData> list) 
    {
        InitTurnGaugeList(list);
    }
    public void UITurnUnitAdd() 
    {
        
    }
    public void UITurnUnitDelete() 
    {
    
    }
    private void UITurnUnitUpdate() 
    {
        ResetTurnGaugeList();
    }
    public void OnInvisible() 
    {
        Transform tf = transform.GetChild(0);
        GaugeUnit gu;
        for (int i = 0; i < tf.childCount; i++)
        {
            gu = tf.GetChild(i).GetComponent<GaugeUnit>();
            gu.Unit = null;
            gu.ProgressValue = 0.0f;
            tf.GetChild(i).gameObject.SetActive(false);
        }
    }
    
}
