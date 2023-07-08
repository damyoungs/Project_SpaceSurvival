using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
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
public class TurnManager : ChildComponentSingeton<TurnManager>
{
    /// <summary>
    /// �ϰ����� ��ũ�� ����Ʈ
    /// </summary>
    LinkedList<ITurnBaseData> turnObjectList;


    /// <summary>
    /// ��ü����Ʈ�� ���Ĺ���� ���Ѵ�. ���� ū�ͺ��� ���̽��۵ǾߵǸ� Descending �����ͺ��� ���۵ǾߵǸ� Ascending �� �־��ָ�ȴ�
    /// true = �������� (- ~ +) , false �������� (+ ~ -)
    /// </summary>
    [SerializeField]
    SortComponent<ITurnBaseData>.SortType isAscending = SortComponent<ITurnBaseData>.SortType.Descending;


    /// <summary>
    /// ������� ����� ���ǰ�
    /// </summary>
    private int turnIndex = 0;
    public int TurnIndex => turnIndex;

    /// <summary>
    /// �ð������� ������� �������� ��
    /// </summary>
    //private int maxTurnValue = 0;

    /// <summary>
    /// �� �̵��� ��Ʋ������ üũ�ϴ� ���� �ε� �Ҷ� �������ش�.
    /// </summary>
    bool isBattleMap = false;
    public bool IsBattleMap
    {
        get => isBattleMap;
        set => isBattleMap = value;
    }

  
    /// <summary>
    /// �ʷε����� ȣ��
    /// </summary>
    protected override void Init(Scene _, LoadSceneMode __)
    {
        //Debug.Log("�ε�Ϸ�");
        if(isBattleMap)//��Ʋ ���̸� 
        { 
            InitTurnData(); //�� ���� ���� 
        }
    }


    /// <summary>
    /// ���̵��� �ѹ��� ����Ǿߵǰ� 
    /// ���ʱ�ȭ �� ������Ʈ ��ġ ������
    /// ������ �ٻ����ǰ� ȣ��Ǿߵ�
    /// </summary>
    public void InitTurnData() {
        turnIndex = 0; //�ϰ� �ʱ�ȭ
        //�׽�Ʈ ������ 
        ITurnBaseData[] turnListTemp = TestGeneratorArraData();//�׽�Ʈ ������ ����
        Array.Sort(turnListTemp, SortComparison); //����

        turnObjectList = new LinkedList<ITurnBaseData>(turnListTemp);//�ʱ�ȭ
        //������� �׽�Ʈ �����ͻ��� ���߿� ������ ������ �������� �Ľ� �۾� �ؾ���.
        TurnStart();//�Ͻ���
    }

    /// <summary>
    /// �Ͻ����� ������Ʈ�� �����ͼ� �����Լ��� ȣ���Ѵ�.
    /// </summary>
    private void TurnStart() {
        turnIndex++; //�������� üũ
        ITurnBaseData nowTurnUnit = GetCurrentTurnObject(); //ó�� �������� ã�ƿͼ� 
        nowTurnUnit.TurnEndAction = TurnEnd; //������� ��ȣ�� ���� ��������Ʈ �����ϰ�

        /*
         * ���������϶� Ŭ���̺�Ʈ�� ���� �ϰų� �Ҷ� ����߰� ��ġ
         */
        nowTurnUnit.TurnStartAction();  //�Ͻ����� �˸���
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
        turnEndObj.TurnEndAction = null; // ���̲��̴°��� �����ϱ����� �����ᰡ �������� ���������͸� �ʱ�ȭ�Ѵ�

        SetTurnValue();// ������ø��� ����Ʈ�� ���ֵ��� �ൿ�� ���� �߰����ִ� ���
        
        TurnSorting(turnEndObj); // ���� ����� ������Ʈ�� ���ı�� ����

        //�߰��Ǵ� ���� ���δٸ��ٴ� �����Ͽ� ��ü������ ��õ� 
        SortComponent<ITurnBaseData>.BubbleSortLinkedList(turnObjectList , isAscending); //���̺����� �������� ��ü ������
       
        /*
         *  ���������϶� Ŭ���̺�Ʈ�� ���� �ϰų� �Ҷ� ����߰� ��ġ
         *  ��Ʋ�ʿ��� �¸������� ü������ ��� �߰��ؾߵǴ� ��ġ
         */

        TurnStart();
    }
    

   

    /// <summary>
    /// ���� ������ ������ ������ 
    /// ����Ʈ�� �̹� �����̵Ȼ����̱⶧���� ���ʹ������θ� Ž���Ͽ� ��ġ����ü�Ѵ�.
    /// </summary>
    /// <param name="turnEndData">�ϻ���� �Ϸ�� ��ü</param>
    private void TurnSorting(ITurnBaseData turnEndData)
    {

        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.Find(turnEndData); //���� ��� �����ͼ�

        LinkedListNode<ITurnBaseData> nextNode = checkNode.Next;//���� ������带 �̸������´�.

        for (int i = 1; i < turnObjectList.Count; i++)//����Ʈ ũ�⸸ŭ ������ 
        {
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
    /// ���Ͱ� �׾ ������ų� ���Ǿ��� ������°�� ������ ��Ȳ������ ������������� ����Ʈ����
    /// �ϸ���Ʈ���� ����.
    /// </summary>
    /// <param name="deleteObject">����� ����</param>
    public void TurnListDeleteObj(ITurnBaseData deleteObject)
    {
        turnObjectList.Remove(deleteObject);
    }

    /// <summary>
    /// ���Ͱ� �����ǰų� ���Ǿ��� �߰��� �߰��ɶ� ��� 
    /// �ϸ���Ʈ�� �߰��� ������ ������ 
    /// </summary>
    /// <param name="addObject">���� ���Ӱ� �߰��� ��ü</param>
    public void TurnListAddObject(ITurnBaseData addObject)
    {
        if (addObject == null) return; // �߰��Ұ��̾����� ���� 
        LinkedListNode<ITurnBaseData> checkNode = turnObjectList.First; //ù��° ��� �����ͼ�

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

    /*======================================== �׽�Ʈ�� ==================================================*/

    /// <summary>
    /// �׽�Ʈ ������ ����
    /// </summary>
    /// <returns></returns>
    private TurnBaseObject[] TestGeneratorArraData()
    {
        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            GameObject go = new GameObject();
            go.name = $"{i} ��° ������Ʈ";
            go.AddComponent<TurnBaseObject>();
            tbo[i] = go.GetComponent<TurnBaseObject>();
            //tbo[i] = new TurnBaseObject(); //���۳�Ʈ��ӹ��� Ŭ������ new ������ؼ� ���������ϰ� ���Ƴ��� .
            //�����Ϸ��� ���ӿ�����Ʈ�� ����� AddComponent�� �̿��ؾ��Ѵ�.
            //Debug.Log($"��ü ���̳�? :{tbo[i]}");
            tbo[i].TurnWaitingValue = UnityEngine.Random.Range(0, 2000);

        }
        return tbo;
    }
    /// <summary>
    /// �ϸ���Ʈ�� ��Ȯ�ο�
    /// </summary>
    public void ViewTurnList() {
        foreach(var j in turnObjectList) {
            Debug.Log($"{j} �� : {j.TurnActionValue}");
        }
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
        if (before.TurnActionValue < after.TurnActionValue)
        {
            return isAscending == SortComponent<ITurnBaseData>.SortType.Ascending ? -1 : 1;
        }
        else if (before.TurnActionValue > after.TurnActionValue)
        {
            return isAscending == SortComponent<ITurnBaseData>.SortType.Descending ? 1 : -1;
        }
        else
        {
            return 0;
        }
    }

}
