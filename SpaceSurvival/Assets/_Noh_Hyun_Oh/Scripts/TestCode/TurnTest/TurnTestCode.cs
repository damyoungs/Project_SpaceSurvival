using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnTestCode : TestBase
{
    [SerializeField]
    GameObject unit = null;

    TurnManager turnManager;
    protected override void Awake()
    {
        base.Awake();
        
    }
    private void Start()
    {
        turnManager = TurnManager.Instance;
    }
    /// <summary>
    /// 턴 초기화 데이터 셋팅(외부에서 셋팅해줘야할것들  : 유닛셋팅 , 위치셋팅) 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test1(InputAction.CallbackContext context)
    {
        GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_UNIT_POOL);
        TurnBaseObject tbo = obj?.GetComponent<TurnBaseObject>();

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt =  rt == null ? obj.AddComponent<RectTransform>() : rt;
        GameObject parentObj = Instantiate(unit);
        if (tbo != null)
        {
            parentObj.transform.position = new Vector3(
                                            UnityEngine.Random.Range(-10.0f, 10.0f), 
                                            0.0f, 
                                            UnityEngine.Random.Range(-10.0f, -5.0f)
                                            );
        }
        obj.transform.SetParent(parentObj.transform);
        rt.anchoredPosition3D = new Vector3(0.0f,2.0f,0.0f);
        obj.SetActive(true);
        turnManager.TurnListAddObject(tbo);
    }
    /// <summary>
    /// 턴실행 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test2(InputAction.CallbackContext context)
    {
        TurnBaseObject[] objs = FindObjectsOfType<TurnBaseObject>();
        GameObject deleteObj = null;
        foreach (TurnBaseObject item in objs)
        {
            turnManager.TurnListDeleteObj(item);
            deleteObj = item.transform.parent.gameObject;//캐릭 받아와서 
            item.InitValue();// 추적유아이관련  초기화
            GameObject.Destroy(deleteObj); //유닛 파괴 
        }
    }
    //턴실행 
    protected override void Test3(InputAction.CallbackContext context)
    {
        LinkedListNode<ITurnBaseData> node = turnManager.GetNode();

        node.Value.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f);

        Debug.Log($"{node.Value.UnitBattleIndex}번째 옵젝 : 값 :{node.Value.TurnActionValue}");
        node.Value.TurnEndAction(node.Value);
    }
    //활력도 소모된후 정렬된값 출력 
    protected override void Test4(InputAction.CallbackContext context)
    {
        Debug.Log("턴종료전 정렬된 값 현재턴유닛은 행동력소모된상태");
        turnManager.ViewTurnList();
        Debug.Log("정렬값 끝 ======================================");
    }
}
