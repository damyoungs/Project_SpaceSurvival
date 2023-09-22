using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 타입
/// </summary>
public enum QuestType
{
    Killcount,              //  토벌퀘스트
    Gathering,              //  수집퀘스트
    Story                   //  시나리오 퀘스트
}

/// <summary>
/// 대화 타입
/// </summary>
public enum TalkType
{
    KillCount = 0,              //  토벌퀘스트
    Gathering,              //  수집퀘스트
    Comunication,           //  일반대화
    Story,                   //  시나리오 퀘스트
}

/// <summary>
/// 유아이는 메니저를 캔버스로 이동시켜서 한곳에서 관리하도록한다.
/// </summary>
public class Gyu_QuestManager : MonoBehaviour
{
    /// <summary>
    /// 플레이어 데이터 
    /// </summary>
    PlayerQuest_Gyu player;
    public PlayerQuest_Gyu Player => player;
     
    /// <summary>
    /// UI Action 연결용으로 가져오기 
    /// </summary>
    Gyu_UI_QuestManager questUIManager;

    /// <summary>
    /// 엔피씨 대화목록 관리하는 클래스 
    /// </summary>
    TalkData_Gyu talkData;   
    public TalkData_Gyu TalkData => talkData;

    /// <summary>
    /// 진행중인 퀘스트 
    /// 사용자가 진행중인 퀘스트 사항을 담아둘 변수  캐릭터쪽에 완성되면 글루빼둔다.
    /// </summary>
    List<Gyu_QuestBaseData> quests;

    /// <summary>
    /// 선택된 퀘스트 
    /// </summary>
    Gyu_QuestBaseData selectQuest;
    public Gyu_QuestBaseData SelectQuest => selectQuest;

    /// <summary>
    /// 맵에있는 NPC 들
    /// </summary>
    [SerializeField]
    NpcBase_Gyu[] array_NPC;
    public NpcBase_Gyu[] Array_NPC => array_NPC;

    /// <summary>
    /// 마지막에 창을 열고있던 NPC 인덱스
    /// </summary>
    int currentNpcIndex = -1;
    public int CurrentNpcIndex => currentNpcIndex; 

    [Header("게임상의 퀘스트 배열")]
   
    /// <summary>
    /// 메인 퀘스트 담아둘 배열 
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] mainStoryQuestArray;
    public Gyu_QuestBaseData[] MainStoryQuestArray => mainStoryQuestArray;

    /// <summary>
    /// 토벌 퀘스트 담아둘 배열 
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] killcountQuestArray;
    public Gyu_QuestBaseData[] KillcountQuestArray => killcountQuestArray;
    
    /// <summary>
    /// 수집 퀘스트 담아둘 배열
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] gatheringQuestArray;
    public Gyu_QuestBaseData[] GatheringQuestArray => gatheringQuestArray;

    private void Awake()
    {
        player = FindObjectOfType<PlayerQuest_Gyu>();
        talkData = FindAnyObjectByType<TalkData_Gyu>();
        questUIManager = GetComponent<Gyu_UI_QuestManager>();   //기능분리를 위해 스크립트를 따로뺏다.
        
        questUIManager.onAcceptQuest = () => { 
            array_NPC[currentNpcIndex].CurrentQuest.Quest_state = Quest_State.Quest_Start;
            
            // 리스트 에 담으면 연결되던가? 확인필요 quests 의 값을 수정하면 npc값이 수정되야됨 .. 클래스라서 되야된다. 안되면 인덱스문젠데 ...
            quests.Add(array_NPC[currentNpcIndex].CurrentQuest);    
        }; 

        questUIManager.onSucessQuest = () => {
            array_NPC[currentNpcIndex].CurrentQuest.Quest_state = Quest_State.Quest_Complete;
            //보상처리 필요함
        }; 

        // 팩토리로 할시 엔피씨 위치를 몇개 후보지역두고 랜덤으로 변경시키는게 더간단할거같다.
        // 초기화 하는것은 여기말고 다른곳으로 빼서 해야될거같다 .. 팩토리 로 생성시킨뒤에 껏다켯다하면 될거같긴한데.. 
        array_NPC = FindObjectsOfType<NpcBase_Gyu>(true);   //씬에있는 엔피씨 찾아서 담아두고 ( 찾는 순서가 바뀔수도있으니 다른방법을 찾아보자.)
        for (int i = 0; i < array_NPC.Length; i++)
        {
            //위치와 모양을 변경시키면 될거같기도한데.. 일단 고민좀해보자..
            array_NPC[i].InitData(i, mainStoryQuestArray, killcountQuestArray, gatheringQuestArray); //npc 를 초기화 시킨다.
            array_NPC[i].onTalkDisableButton = questUIManager.TalkDisableButton;
            array_NPC[i].onTalkEnableButton = (npcId)=> { 
                currentNpcIndex = npcId;
                questUIManager.TalkEnableButton(); 
            };
        }

        questUIManager.onTalkClick = () => array_NPC[currentNpcIndex];

        questUIManager.getTalkDataArray = () => 
        {
            // 0값은 저장된 데이터의 첫번째 대화목록이니 관리할 로직을 작성해야한다.
            return talkData.GetTalk(array_NPC[currentNpcIndex].TalkType, 0);  
        };

    }




}
