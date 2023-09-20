using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase_Gyu : MonoBehaviour
{
    /// <summary>
    /// 엔피씨의 관리할 인덱스
    /// </summary>
    public int Npcid;

    /// <summary>
    /// 현재 진행중인 퀘스트
    /// </summary>
    Quest_Gyu currentQuest;
    public Quest_Gyu currentQuestGyu
    {
        get => currentQuest;
        set
        {
            if (currentQuest != value)
            {
                currentQuest = value;

            }
        }
    }

    /// <summary>
    /// 이엔피씨가 가지고있는 퀘스트 종류 스크립터블로 넣어준다.
    /// </summary>
    [SerializeField]
    Quest_Gyu[] questArray;

    /// <summary>
    /// 엔피씨의 현재 대화종류
    /// </summary>
    QuestManager_Gyu.TalkType talkType;
    public QuestManager_Gyu.TalkType TalkType => talkType;

    /// <summary>
    /// 근처에와서 대화가능하도록 실행시키는 델리게이트
    /// </summary>
    public Action<int> onTalkEnableButton;

    /// <summary>
    /// 주변을 벗어나면 신호보낼 델리게이트
    /// </summary>
    public Action onTalkDisableButton;

    public void SettingQuest(int index) 
    {
        currentQuest = new Quest_Gyu(index);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onTalkEnableButton?.Invoke(Npcid);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onTalkDisableButton?.Invoke();
        }
    }
}
