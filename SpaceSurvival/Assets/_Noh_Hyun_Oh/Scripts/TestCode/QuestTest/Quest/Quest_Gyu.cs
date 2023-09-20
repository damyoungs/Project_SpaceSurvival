using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Gyu
{
    /// <summary>
    /// 퀘스트 제목
    /// </summary>
    string title;
    public string Title 
    {
        get => title;
        set => title = value;
    }

    /// <summary>
    /// 퀘스트 내용
    /// </summary>
    string description;
    public string Description 
    {
        get => description;
        set => description = value;
    }

    /// <summary>
    /// 퀘스트 목표
    /// </summary>
    string clearObjectives;
    public string ClearObjectives 
    {
        get => clearObjectives; 
        set => clearObjectives = value;

    }

    /// <summary>
    /// 퀘스트 id
    /// </summary>
    int questId;
    public int QuestId => questId;
   
    /// <summary>
    /// 클리어 여부 확인
    /// </summary>
    public bool IsSucess => processValue > 0.99f;

    /// <summary>
    /// 퀘스트 진행 값
    /// </summary>
    public float processValue = 0.00f;
 

    /// <summary>
    /// 퀘스트 클리어 보상
    /// </summary>
    public QuestReward_Gyu questReward;
    
    /// <summary>
    /// 퀘스트 클리어 조건
    /// </summary>
    public QuestClear_Gyu questClear;

    public Quest_Gyu(int index) 
    {
        questId = index;
        questReward = new();
        questClear = new();
    }

}
