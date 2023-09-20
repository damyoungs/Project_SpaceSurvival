using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReward_Gyu
{
    /// <summary>
    /// 퀘스트의 보상 금액
    /// </summary>
    public int ItemNumber;    

    /// <summary>
    /// 퀘스트의 보상 아이템
    /// </summary>
    public ItemCode RewardItem;

    /// <summary>
    /// 보상아이템의 갯수
    /// </summary>
    public int itemValue;
}
