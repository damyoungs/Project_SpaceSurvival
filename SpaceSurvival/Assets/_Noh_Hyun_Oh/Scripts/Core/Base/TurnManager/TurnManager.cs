using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���Ĺ�� ��������(ASCENDING), ��������(DESCENDING)
/// </summary>
public enum SortType
{
    Ascending = 0, //��������
    Descending     //��������
}

/// <summary>
/// �� ������ �������� Ŭ����
/// 
/// 1.��ũ�� ����ũ���� Find �� �ϱ����ؼ� ��ü�� �����̵Ǿ��ϴµ� 
///     MonoBehaviour �� ��ӹ��� Ŭ������ new Ű����� ���� �̾ȵǰ�  AddComponent �� �������Ѵ�.
///     MonoBehaviour �׽�Ʈ������ ���鶧 new �� �����ߴ��� ���� null �� ���� LinkedList Find �Լ��� ����� ��ġ�� ã�� ���Ͽ���
/// 
/// TurnBaseData ��ӹ��� ��ü���� �ϳ�������� �װ�ü�� ���� �����ǰ� ���������� �װ��� ������ �ٽ� �������� �Ѵ�.
/// 
/// ��ũ�帮��ũ�� �ٽ� �迭 Ȥ�� ����Ʈ�� �ٲٴ¹�
/// List<ISortBase> b =  turnObjectList.ToList<ISortBase>(); ����Ʈ�� �ٲٴ¹��
/// ISortBase[] a =  turnObjectList.ToArray<ISortBase>();    �迭�� �ٲٴ¹�� 
/// 
/// </summary>
/// 


public class TurnManager : ChildComponentSingeton<TurnManager>
{
    /// <summary>
    /// �ϰ����� �������� ����
    /// </summary>
    [SerializeField]
    private bool isViewGauge = false;
    public bool IsViewGauge => isViewGauge;
    /// <summary>
    /// �ϰ����� ��ũ�� ����Ʈ
    /// </summary>
    LinkedList<ITurnBaseData> turnObjectList = null;

    /// <summary>
    /// ��Ʋ ������ ����ũ�� �ε������� �����ϱ����� �ѹ��� �� ����
    /// </summary>
    int battleUnitIndex = 0;
    public int BattleIndex => battleUnitIndex;
    /// <summary>
    /// ȭ�鿡 �����൵ ������ UI ��ġ
    /// </summary>
    Transform turnGaugeUI;


    /// <summary>
    /// �ش��Ͽ��� ������ ������Ʈ�� �� ������ �����ڿ��� ������ ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> removeTurnObjectGauge;

    /// <summary>
    /// �ش��Ͽ��� �߰���  ������Ʈ�� �� ������ �����ڿ��� ������ ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> addTurnObjectGauge;


    /// <summary>
    /// ��ü����Ʈ�� ���Ĺ���� ���Ѵ�. ���� ū�ͺ��� ���̽��۵ǾߵǸ� Descending �����ͺ��� ���۵ǾߵǸ� Ascending �� �־��ָ�ȴ�
    /// true = �������� (- ~ +) , false �������� (+ ~ -)
    /// </summary>
    [SerializeField]
    SortType isAscending = SortType.Descending;


    /// <summary>
    /// ������� ����� ���ǰ�
    /// </summary>
    private int turnIndex = 0;
    public int TurnIndex => turnIndex;

    /// <summary>
    /// �Ͻ����� �ּҰ�
    /// </summary>
    [SerializeField]
    [Range(0.1f,1.0f)]
    private float turnStartValue = 1.0f;

    /// <summary>
    /// Battle UI �ֻ�� ��ġ
    /// </summary>
    Transform uiParent;

    /// <summary>
    /// �ð������� ������� �������� ��
    /// </summary>
    //private int maxTurnValue = 0;

    /// <summary>
    /// �� �̵��� ��Ʋ������ üũ�ϴ� ���� �ε� �Ҷ� �������ش�.
    /// �׽�Ʈ�� ����
    /// </summary>
    bool isBattleMap = false;
    public bool IsBattleMap
    {
        get => isBattleMap;
        set => isBattleMap = value;
    }
    UICamera[] battleInfoCamArray;

    private void Start()
    {
        turnGaugeUI = WindowList.Instance.TurnGaugeUI; //�ϰ����� ��ġ ã��
        uiParent = WindowList.Instance.transform.GetChild(0); //Battle UI�ֻ�� ��������
        battleInfoCamArray = EtcObjects.Instance.GetComponentsInChildren<UICamera>(true); //��Ʋ����â ī�޶� ��������
    }


    /// <summary>
    /// ��Ʋ ���ϰ�� ������ �ʱ�ȭ�� �̷�����ڿ� ȣ���� �Ǿߵȴ� .
    /// ĳ���� �����Ͱ� ���λ����̵Ȼ����϶� ���ڰ����ι����� �����Ѵ�.
    /// </summary>
    public void InitTurnData() {
        turnIndex = 0; //�ϰ� �ʱ�ȭ
        
        //�׽�Ʈ ������ 
        
        TestInit(); //�׽�Ʈ�� �����ͻ���

        ITurnBaseData[] turnListTemp = GameObject.FindObjectsOfType<TurnBaseObject>(); //�����ִ� ���ֵ� �ܾ���� 

        Array.Sort(turnListTemp, SortComparison); //����

        turnObjectList = new LinkedList<ITurnBaseData>(turnListTemp);//��ũ�� ����Ʈ �ʱ�ȭ

        //������� �׽�Ʈ �����ͻ��� ���߿� ������ ������ �������� �Ľ� �۾� �ؾ���.

        //Sprite image = (Sprite)Resources.Load("/Resources/Forest");
         
        if (turnListTemp == null || turnListTemp.Length == 0)
        {
            Debug.Log("��Ʋ�� �����̾��� �Ӹ�");
            return;
        }
        /*
         �Ͻ����� ����߰� ��ġ
         */

        if (isViewGauge)
        {

        }

        TurnStart();//�Ͻ���
    }

    

    /// <summary>
    /// �Ͻ����� ������Ʈ�� �����ͼ� �����Լ��� ȣ���Ѵ�.
    /// </summary>
    private void TurnStart() {
        turnIndex++; //�������� üũ

        ITurnBaseData nowTurnUnit = GetCurrentTurnObject(); //ó�� �������� ã�ƿͼ� 


        if (turnStartValue < nowTurnUnit.TurnActionValue) //������ �Ҽ��ִ� ���� ������ ������
        {
            nowTurnUnit.TurnEndAction = TurnEnd; //������� ��ȣ�� ���� ��������Ʈ �����ϰ�
            /*
             * ���������϶� Ŭ���̺�Ʈ�� ���� �ϰų� �Ҷ� ����߰� ��ġ
             */


            nowTurnUnit.TurnRemove = TurnListDeleteObj; //�������� ������ ������ ������ �����Լ��� �����Ų��.

            nowTurnUnit.TurnStartAction();  //�Ͻ����� �˸���
        }
        else  //�ƴϸ� ���� �����ؼ� �ൿ�°��� ������Ų��.
        {
            TurnEnd(nowTurnUnit);
        }
        
    }

    /// <summary>
    /// ���� ���� �����ؾ��� ������Ʈ��ȯ.
    /// </summary>
    /// <returns>����Ʈ�� ù��° ����� ���� ��ȯ</returns>
    private ITurnBaseData GetCurrentTurnObject()
    {
        return turnObjectList.First.Value;   

    }

    /// <summary>
    /// ������� ������ ����
    /// </summary>
    /// <param name="turnEndObj">�������� ����</param>
    private void TurnEnd(ITurnBaseData turnEndObj)
    {
        turnEndObj.TurnEndAction = null; // ���̲��̴°��� �����ϱ����� �����ᰡ �������� ��������Ʈ�� �ʱ�ȭ�Ѵ�
        turnEndObj.TurnRemove = null; //�ش����̳������� ��������Ʈ ȸ���Ѵ�.
        turnEndObj.BattleUI.TrunActionStateChange(); //�����̻� �������Ű��
        
        SetTurnValue();// ������ø��� ����Ʈ�� ���ֵ��� �ൿ�� ���� �߰����ִ� ���
        
        // TurnSorting(turnEndObj); // ���� ����� ������Ʈ�� ���ı�� ���� -- �����Ḷ�� �ൿ���������� ������ �ش��Լ��� ����Ǵ��ǹ̰��ִ�.

        //�߰��Ǵ� ���� ���δٸ��ٴ� �����Ͽ� ��ü������ ��õ� 
        SortComponent<ITurnBaseData>.BubbleSortLinkedList(turnObjectList , isAscending); //���̺����� �������� ��ü ������
        
        
        /*
         *  ���������϶� Ŭ���̺�Ʈ�� ���� �ϰų� �Ҷ� ����߰� ��ġ
         *  ��Ʋ�ʿ��� �¸������� ü������ ��� �߰��ؾߵǴ� ��ġ
         */


        TurnStart();
    }

    /// <summary>
    /// ���̳����� �ϸ���Ʈ�� ������Ʈ���� �ൿ���� �߰���Ų��.
    /// </summary>
    private void SetTurnValue() {

        foreach (ITurnBaseData node in turnObjectList) //���̳��������� 
        {
            node.TurnActionValue += node.TurnEndActionValue;//����Ʈ�� ����ڵ��� Ȱ������ �߰� ��Ų��.
        }
    }


    /// <summary>
    /// ���߰��� ���Ͱ� �׾ ������ų� ���Ǿ��� ������°�� 
    /// �����ฮ��Ʈ ������ ������������� �ۼ�
    /// </summary>
    /// <param name="deleteObject">����� ����</param>
    public void TurnListDeleteObj(ITurnBaseData deleteObject)
    {
        if (turnObjectList.Contains(deleteObject)) //������ ����Ʈ�� �����Ұ�� ó�� 
        {
            deleteObject.ResetData();//�ʱ�ȭ �Լ� ����
            turnObjectList.Remove(deleteObject);//����Ʈ���� ����
        }
        
    }

    /// <summary>
    /// ���Ͱ� �����ǰų� ���Ǿ��� �߰��� �߰��ɶ� ��� 
    /// �ϸ���Ʈ�� �߰��� ������ ������ 
    /// </summary>
    /// <param name="addObject">���� ���Ӱ� �߰��� ��ü</param>
    public void TurnListAddObject(ITurnBaseData addObject)
    {
        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.First; //ù��° ��� �����ͼ�
        if (addObject == null) return; // �߰��Ұ��̾����� ���� 

        for (int i = 0; i < turnObjectList.Count; i++)//����Ʈ �ѹ��������� ������������
        {
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(addObject, checkNode.Value, isAscending))//���ڷι��� ��尪�� ����Ʈ�� ��尪���񱳸��ؼ� ���ı��������Ѵ�. 
            {
                turnObjectList.AddBefore(checkNode, addObject); // �׾մܿ� �߰����ع�����.
                break;//�߰������� ����������.
            }
            else if (i == turnObjectList.Count - 1) //���������� �񱳰��� ���������ʾ�����  
            {
                turnObjectList.AddLast(addObject); //�Ǹ������� �߰����Ѵ�. 
                break;//�߰������� ����������.
            }
            checkNode = checkNode.Next; //���� �񱳵��� ������ ���� ��带 ã�´�.
        }
    }

    /// <summary>
    /// �ϰ��� ������ �ʱ�ȭ 
    /// </summary>
    public void ResetBattleData() 
    {
        if (turnObjectList == null) return; // �ϸ���Ʈ�� �������� ������ ����������

        foreach (ITurnBaseData node in turnObjectList)  // �ϵ����� ���鼭
        {
            node.ResetData(); //�������� ������ �ʱ�ȭ 
        }

        for (int i = 1; i < uiParent.childCount - 1; i++) //ù��° ������Ʈ�� �������̶� ���� 1���ͽ���
        {
            for (int j = 0; j < uiParent.GetChild(i).childCount; j++) //ã�ƿͼ� 
            {
                uiParent.GetChild(i).GetChild(j).gameObject.SetActive(false);//���̴� UI �ٰ�����
            }
        }

        turnGaugeUI.gameObject.SetActive(false);//������ UI ���߱�
        turnObjectList.Clear();//����Ʈ ���� 
    }

    /// <summary>
    /// ����Ʈ�� Array �� �⺻���ı���� �̿��� ���� 
    /// ���ڰ��� �ڷ����� ��������Ѵ� ��ȯ���� int -1 0 1
    /// </summary>
    /// <param name="before">���ǰ�</param>
    /// <param name="after">���ǰ�</param>
    /// <returns>�� ���</returns>
    private int SortComparison(ITurnBaseData before, ITurnBaseData after)
    {
        if (before.TurnActionValue < after.TurnActionValue)  //�ΰ��� �� 
        {
            return isAscending == SortType.Ascending ? -1 : 1;  //���� �����̸� -1 ���������̸� 1 
        }
        else if (before.TurnActionValue > after.TurnActionValue)
        {
            return isAscending == SortType.Descending ? 1 : -1; //���������̸� 1 ���� �����̸� -1
        }
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// ���� : ���������� ���ĵ� ���¿��� �ϰ� �̻�Ȳ�� �ƴѻ��¿��� ���� �ε����� ���ϼ��ִ�.
    /// ����Ʈ�� �̹� �����̵Ȼ����̱⶧���� ���ʹ������θ� Ž���Ͽ� ��ġ����ü�Ѵ�. 
    /// �ش��Լ��� ������ �ٵ��ִ� ����Ʈ���� �ϳ��� �ε������� �������� �����ε��� �� ������ġ�� ����ִ±���̴� 
    /// </summary>
    /// <param name="turnEndData">�ϻ���� �Ϸ�� ��ü</param>
    private void TurnSorting(ITurnBaseData turnEndData)
    {

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.Find(turnEndData); //���� ��� �����ͼ�

        LinkedListNode<ITurnBaseData> nextNode = checkNode.Next;//���� ������带 �̸������´�.
        for (int i = 0; i < turnObjectList.Count; i++)//����Ʈ ũ�⸸ŭ ������ 
        {
            Debug.Log(nextNode);
            if (nextNode == null)
            {
                //Debug.Log($"{turnEndData.transform.name} �� �ϳ����� ���Ľ�  : �񱳰� :{nextNode} :: ����Ƚ�� : {i} ::  ��ũ�帮��Ʈ ��üũ�� :{turnObjectList.Count}");
                return;
            }
            if (SortComponent<ITurnBaseData>.SortAscDesCheck(checkNode.Value, nextNode.Value, isAscending)) // ���� ���������̳� ���������̳Ŀ����� �޶�����.
            {                                       //��ü�� �ʿ��Ұ�� 
                turnObjectList.Remove(checkNode);   //�ϴ� ��������
                                                    //������� ������ InvalidOperationException: The LinkedList node already belongs to a LinkedList.
                                                    //���ȿ� �̹� �������ִٰ� �߰��Ҽ����ٰ���.
                turnObjectList.AddBefore(nextNode, checkNode); //����� �տ� �߰�
                break;//��ġ�� ������ ������ ���������� .
            }
            //��ü�� �ʿ���°�� 
            nextNode = nextNode.Next;   //������带 ã�´�
        }

    }

    /*======================================== �׽�Ʈ�� ==================================================*/


    /// <summary>
    /// �ϸ���Ʈ�� ��Ȯ�ο�
    /// </summary>
    public void ViewTurnList() {
        foreach(ITurnBaseData j in turnObjectList) {
            Debug.Log($"{j} �� : {j.TurnActionValue}");
        }
        Debug.Log(turnObjectList.Count);
    }
    /// <summary>
    /// ���� �������� ������ ã�ƿ��� 
    /// </summary>
    /// <returns></returns>
    public ITurnBaseData GetNode() {
        ITurnBaseData isTurnNode = null;
        foreach (ITurnBaseData node in turnObjectList) 
        {
            if (node.TurnEndAction != null) //���� ���������̸� endAction �� ��ϵ��ִ� 
            {
                Debug.Log(node.transform.name);
               isTurnNode = node;//�������� ��� ��Ƽ� 
            }
        }
        return isTurnNode;//��ȯ
    }
    /// <summary>
    /// �׽�Ʈ�� ������ ���� �������� 
    /// </summary>
    /// <returns></returns>
    public ITurnBaseData RandomGetNode() 
    {
        ITurnBaseData isRandNode = null;
        int randomIndex = UnityEngine.Random.Range(0,turnObjectList.Count); //����Ʈ�� ������ �ε��� �� �������� 
        LinkedListNode<ITurnBaseData> node = turnObjectList.First;
        for (int i = 0; i < turnObjectList.Count; i++)
        {
            if (randomIndex == i) 
            {
                isRandNode = node.Value;
                break;
            }
            node = node.Next;
        }
        return isRandNode;
    }
    [SerializeField]
    int initSize = 1;
    /// <summary>
    /// �׽�Ʈ ������ ����¿�
    /// </summary>
    public void TestInit() 
    {
        if (IsViewGauge) 
        {
            turnGaugeUI.gameObject.SetActive(true); // ���丮���� ������ Ȱ��ȭ ���������� �����ֱ����� �̸����� 
                                                    // �̸��ȸ����ָ� SetParent�Լ� �Ҷ� ������� OnDisable OnEnable  �Լ��� ȣ��ȴ�.
            
        }
        for (int i = 1; i < uiParent.childCount - 1; i++)
        {

            for (int j = 0; j < uiParent.GetChild(i).childCount; j++)
            {
                uiParent.GetChild(i).GetChild(j).gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < initSize; i++)
        {
            GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_UNIT_POOL); //��������
            TurnBaseObject tbo = obj?.GetComponent<TurnBaseObject>(); //ã��
            battleUnitIndex = i;// �ϴ� üũ�� ���� �Է�
            tbo.UnitBattleIndex = battleUnitIndex; //�ε��� �����ϰ�
            tbo.InitUI(); //UI �ʱ�ȭ ��Ű��
            tbo.TurnAddValue = UnityEngine.Random.Range(0.0f, 0.1f); //������ø��� �����Ǵ� �ൿ�°� ���� ���� -�׽�Ʈ
            tbo.TurnActionValue = UnityEngine.Random.Range(0.0f, 8.0f); // -�׽�Ʈ�� ����

            obj.transform.position = new Vector3(
                                            UnityEngine.Random.Range(-10.0f, 10.0f),
                                            0.0f,
                                            UnityEngine.Random.Range(-10.0f, 0.0f)
                                            );//������ġ�� �Ѹ���
            Transform cameraTarget = obj.transform.GetChild(0).GetChild(obj.transform.GetChild(0).childCount - 1);

            //UI ��ġ ��¦�ø��°��ؾ���
            
            if(battleInfoCamArray != null  && battleInfoCamArray.Length > i ) //�׽�Ʈ�� �ƹ��ų��������� 
            {
                battleInfoCamArray[i].TargetObject = cameraTarget;
                battleInfoCamArray[i].gameObject.SetActive(true);
            }
        }

    }

   

}
