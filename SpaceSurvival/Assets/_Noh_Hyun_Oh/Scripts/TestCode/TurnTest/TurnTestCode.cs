using System.Collections.Generic;
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
    /// �� ���� ���� �׽�Ʈ
    /// </summary>
    /// <param name="context"></param>
    protected override void Test1(InputAction.CallbackContext context)
    {
        GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_UNIT_POOL); //��������
        TurnBaseObject tbo = obj?.GetComponent<TurnBaseObject>(); //ã��
        tbo.UnitBattleIndex = turnManager.BattleIndex; //�ε��� �����ϰ�
        tbo.InitUI(); //UI �ʱ�ȭ ��Ű��
        RectTransform rt = obj.GetComponent<RectTransform>(); //�������� ��ġ �ѷ��ֱ����� ��ġ��������������
        rt = rt == null ? obj.AddComponent<RectTransform>() : rt; // ������ �߰��ϰ� 
        GameObject parentObj = Instantiate(unit); // �׽�Ʈ�� ĳ���� �����ϰ�
        parentObj.transform.position = new Vector3(
                                        UnityEngine.Random.Range(-10.0f, 10.0f),
                                        0.0f,
                                        UnityEngine.Random.Range(-10.0f, 0.0f)
                                        );//������ġ�� �Ѹ���
        tbo.TurnAddValue = UnityEngine.Random.Range(0.0f, 0.1f); //������ø��� �����Ǵ� �ൿ�°� ���� ���� -�׽�Ʈ
        tbo.TurnActionValue = UnityEngine.Random.Range(0.0f, 8.0f); // -�׽�Ʈ�� ����
        obj.transform.SetParent(parentObj.transform);//�θ���ġ �ű�� - �θ� Ȱ��ȭ ���°� �ƴҰ�� �����߻�����������
        rt.anchoredPosition3D = new Vector3(0.0f, 2.0f, 0.0f);// UI�⺻��ġ ��¦���� �ű�� 

        turnManager.TurnListAddObject(tbo); //�ϸ���Ʈ�� �߰� 
    }

    /// <summary>
    /// �� ���� ���� �׽�Ʈ
    /// </summary>
    /// <param name="context"></param>
    protected override void Test2(InputAction.CallbackContext context)
    {
        
        TurnBaseObject turnObj = GameObject.FindObjectOfType<TurnBaseObject>();// ���� ã�Ƽ� 
        if (turnObj == null || turnObj.TurnEndAction != null) return; //������ ���ų�  ���� �������� �������̸� ������ �ȵǾߵȴ�.
        GameObject obj = turnObj.transform.parent.gameObject; // �θ� ã�Ƴ���
        turnObj.ResetData(); //�������ʱ�ȭ 
        turnManager.TurnListDeleteObj(turnObj); //����Ʈ ���� ���� 
        GameObject.Destroy(obj); // ���ӿ�����Ʈ ���� 
    }
    /// <summary>
    /// �� ���� 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test3(InputAction.CallbackContext context)
    {
        ITurnBaseData node = turnManager.GetNode(); //������ ĳ���� �ʿ��� �ϿϷ� ��ư ȣ���ؾ��ϴµ� ĳ���� ����������  �׽�Ʈ�ڵ�� ã�ƿ´�.
        if (node == null) 
        {
            Debug.Log("�ָ�ã��?");
            return;
        } 
        Debug.Log($"{node.UnitBattleIndex}��° ���� : �� :{node.TurnActionValue} �Լ�����ϵ��ֳ�? : {node.TurnEndAction}");
        node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //�� ���� �� �ൿ�� ����ġ ���� �����ִ´�.

        node.TurnEndAction(node); //�ϿϷ� �� �˸���.
    }

    /// <summary>
    /// �� ���� ���� �ʱ�ȭ�� ����ȭ�鿡�ִ� ĳ���� ������Ʈ �����
    /// </summary>
    /// <param name="context"></param>
    protected override void Test4(InputAction.CallbackContext context)
    {
        turnManager.ResetBattleData(); //�ϰ������� ������ ������
        Player[] ps = GameObject.FindObjectsOfType<Player>(); //���游�� ������Ʈ ã��
        foreach (Player p in ps)
        {
            GameObject.Destroy(p.gameObject); //������Ʈ ����
        }
    }
    /// <summary>
    /// �� �ʱ�ȭ �Լ� ����
    /// </summary>
    /// <param name="context"></param>
    protected override void Test5(InputAction.CallbackContext context)
    {
        turnManager.ResetBattleData(); //������
        turnManager.InitTurnData();//�ʱⵥ���� ���� 
    }
    /// <summary>
    /// �ϸ���Ʈ�� ���� ��� 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test6(InputAction.CallbackContext context)
    {
        turnManager.ViewTurnList();//�ϸ���Ʈ �� �����͸� ���ĵ� ������� ��� 
    }
    
    /// <summary>
    /// ������ ĳ������ ���¸� �߰�
    /// </summary>
    /// <param name="context"></param>
    protected override void Test7(InputAction.CallbackContext context)
    {
        TurnBaseObject tbo =  (TurnBaseObject)turnManager.RandomGetNode();
        tbo.BattleUI.AddOfStatus(EnumList.StateType.Poison);//�����̻� �߰��غ��� 
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test8(InputAction.CallbackContext context)
    {
        
    }



}
