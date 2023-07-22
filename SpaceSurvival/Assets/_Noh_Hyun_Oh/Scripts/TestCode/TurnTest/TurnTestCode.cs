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
    /// �� �ʱ�ȭ ������ ����(�ܺο��� ����������Ұ͵�  : ���ּ��� , ��ġ����) 
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
    /// �Ͻ��� 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test2(InputAction.CallbackContext context)
    {
        TurnBaseObject[] objs = FindObjectsOfType<TurnBaseObject>();
        GameObject deleteObj = null;
        foreach (TurnBaseObject item in objs)
        {
            turnManager.TurnListDeleteObj(item);
            deleteObj = item.transform.parent.gameObject;//ĳ�� �޾ƿͼ� 
            item.InitValue();// ���������̰���  �ʱ�ȭ
            GameObject.Destroy(deleteObj); //���� �ı� 
        }
    }
    //�Ͻ��� 
    protected override void Test3(InputAction.CallbackContext context)
    {
        LinkedListNode<ITurnBaseData> node = turnManager.GetNode();

        node.Value.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f);

        Debug.Log($"{node.Value.UnitBattleIndex}��° ���� : �� :{node.Value.TurnActionValue}");
        node.Value.TurnEndAction(node.Value);
    }
    //Ȱ�µ� �Ҹ���� ���ĵȰ� ��� 
    protected override void Test4(InputAction.CallbackContext context)
    {
        Debug.Log("�������� ���ĵ� �� ������������ �ൿ�¼Ҹ�Ȼ���");
        turnManager.ViewTurnList();
        Debug.Log("���İ� �� ======================================");
    }
}
