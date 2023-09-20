using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestClear_Gyu
{
    /// <summary>
    /// 클리어 Npcid
    /// </summary>
    public int ClearNpcid;

    /// <summary>
    /// 퀘스트 타입
    /// </summary>
    public QuestManager_Gyu.QuestType clearType;

    /// <summary>
    /// 클리어까지 필요한 카운트(갯수)
    /// </summary>
    public int requiredCount;

    /// <summary>
    /// 현재 카운트(갯수)
    /// </summary>
    public int currentCount;

    /// <summary>
    /// 퀘스트조건 달성여부
    /// </summary>
    public bool isSucess => requiredCount - 1 < currentCount;


    /// <summary>
    /// 진행사항
    /// </summary>
    public void QuestCounting()
    {
        currentCount++;
    }
}
