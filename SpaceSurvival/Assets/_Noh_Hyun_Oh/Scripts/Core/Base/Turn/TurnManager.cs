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
    int battleUnitIndex = -1;
    public int BattleIndex => battleUnitIndex;
    /// <summary>
    /// ȭ�鿡 �����൵ ������ UI ��ġ
    /// </summary>
    Transform turnGaugeUI;



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
    //UICamera[] battleInfoCamArray;

    private void Start()
    {
        turnGaugeUI = WindowList.Instance.TurnGaugeUI; //�ϰ����� ��ġ ã��


        uiParent = WindowList.Instance.transform.GetChild(0); //Battle UI�ֻ�� ��������
    }


    /// <summary>
    /// ��Ʋ ���ϰ�� ������ �ʱ�ȭ�� �̷�����ڿ� ȣ���� �Ǿߵȴ� .
    /// ĳ���� �����Ͱ� ���λ����̵Ȼ����϶� ���ڰ����ι����� �����Ѵ�.
    /// </summary>
    public void InitTurnData() {
        turnIndex = 0; //�ϰ� �ʱ�ȭ
        
        ITurnBaseData[] turnListTemp = GameObject.FindObjectsOfType<TurnBaseObject>(); //�����ִ� ���ֵ� �ܾ���� 
        
        if (turnListTemp == null || turnListTemp.Length == 0)
        {
            Debug.LogWarning("��Ʋ�� �����̾��� �Ӹ�");
            return;
        }

        Array.Sort(turnListTemp, SortComparison); //����

        turnObjectList = new LinkedList<ITurnBaseData>(turnListTemp);//��ũ�� ����Ʈ �ʱ�ȭ
         
      
        TurnStart();//�Ͻ���
    }

    

    /// <summary>
    /// �Ͻ����� ������Ʈ�� �����ͼ� �����Լ��� ȣ���Ѵ�.
    /// </summary>
    private void TurnStart() {
        turnIndex++; //�Ͻ��۸��� ī��Ʈ ��Ų��.

        ITurnBaseData currentTurn = GetCurrentTurnObject(); //ó�� �������� ã�ƿͼ� 


        if (turnStartValue < currentTurn.TurnActionValue) //������ �Ҽ��ִ� ���� ������ ������
        {
            currentTurn.TurnEndAction = TurnEnd; //������� ��ȣ�� ���� ��������Ʈ �����ϰ�
            /*
             * ���������϶� Ŭ���̺�Ʈ�� ���� �ϰų� �Ҷ� ����߰� ��ġ
             */


            currentTurn.TurnRemove = TurnListDeleteObj; //�������� ������ ������ ������ �����Լ��� �����Ų��.

            currentTurn.TurnStartAction();  //�Ͻ����� �˸���
        }
        else  //�ƴϸ� ���� �����ؼ� �ൿ�°��� ������Ų��.
        {
            TurnEnd(currentTurn);
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
        turnEndObj.TurnRemove = null;
        turnEndObj.TurnEndAction = null; // �����ᰡ �������� ��������Ʈ�� �ʱ�ȭ�Ѵ�

        SetTurnValue();// ������ø��� ����Ʈ�� ���ֵ��� �ൿ�� ���� �߰����ִ� ���
        
        // TurnSorting(turnEndObj); // ���� ����� ������Ʈ�� ���ı�� ���� -- �����Ḷ�� �ൿ���������� ������ �ش��Լ��� ����Ǵ��ǹ̰��ִ�.

        //�߰��Ǵ� �ൿ�� ���� ���δٸ��ٴ� �����Ͽ� ��ü������ ��õ� 
        SortComponent<ITurnBaseData>.BubbleSortLinkedList(turnObjectList , isAscending); //���̺����� �������� ��ü ������
        
        
        /*
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
    /// �ϰ����� ������Ʈ�� ��������
    /// �����ฮ��Ʈ ���� ��������� �͵� ����
    /// </summary>
    /// <param name="deleteTurnObject">����Ʈ���� ���� ��</param>
    public void TurnListDeleteObj(ITurnBaseData deleteTurnObject)
    {
        if (deleteTurnObject.CharcterList.Count < 1) //�Ͽ�����Ʈ�� ������������ ���°�� 
        {
            deleteTurnObject.ResetData();//�Ͽ�����Ʈ�� �ʱ�ȭ ��Ű�� 
            turnObjectList.Remove(deleteTurnObject);//����Ʈ���� ����
        }
        else 
        {
            Debug.Log($"���� ������ {deleteTurnObject.CharcterList.Count}�� �����ֽ��ϴ� ");
        }
        
    }

    /// <summary>
    /// �ϰ����� ������Ʈ�� �߰��ɰ�� 
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
        turnObjectList.Clear();//����Ʈ ���� 

        //UI ����
        for (int i = 1; i < uiParent.childCount - 1; i++)
        {
            for (int j = 0; j < uiParent.GetChild(i).childCount; j++)
            {
                uiParent.GetChild(i).GetChild(j).gameObject.SetActive(false); //�׼ǹ�ư ���� 
            }
        }
        turnGaugeUI.gameObject.SetActive(false); //�ϰ����� ����

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
    public ITurnBaseData GetPlayerCharcter() 
    {
        return player;
    }
    ITurnBaseData player;
    [SerializeField]
    int initSize = 1;
    /// <summary>
    /// �׽�Ʈ ������ ����¿�
    /// ���: �ʿ� �÷��̾� , ���Ǿ� , ����  �� ���� ��������ϴ� ������ �������Ŀ� �� ����Ʈ�� �ϰ������� ���̽��� ���� 
    ///       ���� 1~3�� �� UI�� ī�޶� �߰����ش�. 
    ///       
    /// </summary>
    public void TestInit() 
    {
        if (IsViewGauge) 
        {
            turnGaugeUI.gameObject.SetActive(true);
        }
        for (int i = 1; i < uiParent.childCount - 1; i++)
        {

            for (int j = 0; j < uiParent.GetChild(i).childCount; j++)
            {
                uiParent.GetChild(i).GetChild(j).gameObject.SetActive(true);
            }
        }
        ITurnBaseData tbo;
        for (int i = 0; i < initSize; i++)
        {
            GameObject obj;
            battleUnitIndex = i;// �ϴ� üũ�� ���� �Է�
            if (i == 0)
            {
                obj= Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_PLAYER_POOL); //
                tbo = obj.GetComponent<PlayerTurnObject>(); //ã��new PlayerTurnObject();
                obj.name = $"PL_Charcter  _{i}";
                player = tbo;
            }
            else 
            {
                obj= Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL); //��������
                tbo = obj.GetComponent<EnemyTurnObject>();
                obj.name = $"ENEMY_Charcter _{i}";
            }
            Debug.Log(tbo);
            InitTurnObject(i,obj, tbo);
        }

    }
    private void InitTurnObject(int index ,GameObject obj, ITurnBaseData tbo) 
    {
       
        tbo.TurnActionValue = UnityEngine.Random.Range(0.0f, 8.0f); // -�׽�Ʈ�� ����
        obj.transform.position = new Vector3(
                                        UnityEngine.Random.Range(-10.0f, 10.0f),
                                        0.0f,
                                        UnityEngine.Random.Range(-10.0f, 0.0f)
                                        );//������ġ�� �Ѹ���
    }
   

}
