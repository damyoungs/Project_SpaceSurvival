using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Data",menuName = "Scriptable Object/QuestDatas/Quest", order = 1)]
public class Gyu_QuestBaseData :ScriptableObject
{
    [Header("퀘스트의 기본정보")]
    /// <summary>
    /// 퀘스트 id
    /// </summary>
    [SerializeField]
    int questId = -1;
    public int QuestId => questId;

    /// <summary>
    /// 퀘스트 타입
    /// </summary>
    [SerializeField]
    QuestType type;
    public QuestType QuestType => type;
    
    /// <summary>
    /// 퀘스트 아이콘
    /// </summary>
    [SerializeField]
    Sprite iconImage;
    public Sprite IconImage => iconImage;
    
    /// <summary>
    /// 퀘스트 제목
    /// </summary>
    [SerializeField]
    string title;
    public string Title => title;

    /// <summary>
    /// 퀘스트 내용
    /// </summary>
    [SerializeField] 
    string description;
    public string Description => description;   
    
    /// <summary>
    /// 퀘스트 목표
    /// </summary>
    [SerializeField] 
    string clearObjectives;
    public string ClearObjectives => clearObjectives;
   
    //--------------------------- 보상관련 변수 
    [Header("퀘스트의 보상 관련 정보")]
    /// <summary>
    /// 퀘스트의 보상 금액
    /// </summary>
    [SerializeField]
    int rewardCoin;
    public int RewardCoin => rewardCoin;

    /// <summary>
    /// 퀘스트의 보상 아이템
    /// </summary> 
    [SerializeField]
    ItemCode[] rewardItem;
    public ItemCode[] RewardItem => rewardItem;

    /// <summary>
    /// 보상아이템의 갯수
    /// </summary>
    [SerializeField]
    int[] itemCount;
    public int[] ItemCount => itemCount;



    //------------------------------- 퀘스트 진행관련 변수 

    [Header("퀘스트의 수행에 관련된 정보")]
    /// <summary>
    /// 퀘스트에 필요한 아이템
    /// </summary> 
    [SerializeField]
    ItemCode[] requestItem;
    public ItemCode[] RequestItem => requestItem;
    
    
    [Header("토벌시 몬스터정보셋팅이 필요함")]
    /// <summary>
    /// 클리어까지 필요한 카운트(갯수)
    /// </summary>
    [SerializeField]
    int[] requiredCount;
    public int[] RequiredCount => requiredCount;

    /// <summary>
    /// 현재 카운트(갯수)
    /// </summary>
    protected int[] currentCount;
    public int[] CurrentCount => currentCount;

    /// <summary>
    /// 퀘스트에대한 상태 정보
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
    /// 퀘스트가 현재 진행중인 퀘스트상황
    /// </summary>
    /// <returns>완료됬거나 퀘스트가 없을땐  true 아니면 false </returns>
    public bool IsSucess() 
    {
        int requestArrayLength = requiredCount.Length;
        for (int i = 0; i < requestArrayLength; i++)
        {
            if (requiredCount[i] > currentCount[i]) //완료 안됬는지 체크해서  
            {
                return false; //클리어 안됬으면 안됬다고 반환
            }
        }
        return true; // 클리어 됬으면 true 로주자
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
