using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Data",menuName = "Scriptable Object/QuestDatas/Quest", order = 1)]
public class Gyu_QuestBaseData :ScriptableObject
{
    [Header("����Ʈ�� �⺻����")]
    /// <summary>
    /// ����Ʈ id
    /// </summary>
    [SerializeField]
    int questId = -1;
    public int QuestId => questId;

    /// <summary>
    /// ����Ʈ Ÿ��
    /// </summary>
    [SerializeField]
    QuestType type;
    public QuestType QuestType => type;
    
    /// <summary>
    /// ����Ʈ ������
    /// </summary>
    [SerializeField]
    Sprite iconImage;
    public Sprite IconImage => iconImage;
    
    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    [SerializeField]
    string title;
    public string Title => title;

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    [SerializeField] 
    string description;
    public string Description => description;   
    
    /// <summary>
    /// ����Ʈ ��ǥ
    /// </summary>
    [SerializeField] 
    string clearObjectives;
    public string ClearObjectives => clearObjectives;
   
    //--------------------------- ������� ���� 
    [Header("����Ʈ�� ���� ���� ����")]
    /// <summary>
    /// ����Ʈ�� ���� �ݾ�
    /// </summary>
    [SerializeField]
    int rewardCoin;
    public int RewardCoin => rewardCoin;

    /// <summary>
    /// ����Ʈ�� ���� ������
    /// </summary> 
    [SerializeField]
    ItemCode[] rewardItem;
    public ItemCode[] RewardItem => rewardItem;

    /// <summary>
    /// ����������� ����
    /// </summary>
    [SerializeField]
    int[] itemCount;
    public int[] ItemCount => itemCount;



    //------------------------------- ����Ʈ ������� ���� 

    [Header("����Ʈ�� ���࿡ ���õ� ����")]
    /// <summary>
    /// ����Ʈ�� �ʿ��� ������
    /// </summary> 
    [SerializeField]
    ItemCode[] requestItem;
    public ItemCode[] RequestItem => requestItem;
    
    
    [Header("����� �������������� �ʿ���")]
    /// <summary>
    /// Ŭ������� �ʿ��� ī��Ʈ(����)
    /// </summary>
    [SerializeField]
    int[] requiredCount;
    public int[] RequiredCount => requiredCount;

    /// <summary>
    /// ���� ī��Ʈ(����)
    /// </summary>
    protected int[] currentCount;
    public int[] CurrentCount => currentCount;

    /// <summary>
    /// ����Ʈ������ ���� ����
    /// </summary>
    protected Quest_State quest_State = Quest_State.None;
    public virtual Quest_State Quest_state
    {
        get => quest_State;
        set 
        {
            quest_State = value;
        }
    }

    /// <summary>
    /// ����Ʈ�� ���� �������� ����Ʈ��Ȳ
    /// </summary>
    /// <returns>�Ϸ��ų� ����Ʈ�� ������  true �ƴϸ� false </returns>
    public bool IsSucess() 
    {
        int requestArrayLength = requiredCount.Length;
        for (int i = 0; i < requestArrayLength; i++)
        {
            if (requiredCount[i] > currentCount[i]) //�Ϸ� �ȉ���� üũ�ؼ�  
            {
                return false; //Ŭ���� �ȉ����� �ȉ�ٰ� ��ȯ
            }
        }
        return true; // Ŭ���� ������ true ������
    }
    public void ResetData() 
    {
        int length = currentCount.Length;
        for (int i = 0; i < length; i++)
        {
            currentCount[i] = 0;
        }
        quest_State = Quest_State.None;
    }
}
