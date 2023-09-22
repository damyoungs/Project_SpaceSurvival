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
/// ����Ʈ Ÿ��
/// </summary>
public enum QuestType
{
    Killcount,              //  �������Ʈ
    Gathering,              //  ��������Ʈ
    Story                   //  �ó����� ����Ʈ
}

/// <summary>
/// ��ȭ Ÿ��
/// </summary>
public enum TalkType
{
    KillCount = 0,              //  �������Ʈ
    Gathering,              //  ��������Ʈ
    Comunication,           //  �Ϲݴ�ȭ
    Story,                   //  �ó����� ����Ʈ
}

/// <summary>
/// �����̴� �޴����� ĵ������ �̵����Ѽ� �Ѱ����� �����ϵ����Ѵ�.
/// </summary>
public class Gyu_QuestManager : MonoBehaviour
{
    /// <summary>
    /// �÷��̾� ������ 
    /// </summary>
    PlayerQuest_Gyu player;
    public PlayerQuest_Gyu Player => player;
     
    /// <summary>
    /// UI Action ��������� �������� 
    /// </summary>
    Gyu_UI_QuestManager questUIManager;

    /// <summary>
    /// ���Ǿ� ��ȭ��� �����ϴ� Ŭ���� 
    /// </summary>
    TalkData_Gyu talkData;   
    public TalkData_Gyu TalkData => talkData;

    /// <summary>
    /// �������� ����Ʈ 
    /// ����ڰ� �������� ����Ʈ ������ ��Ƶ� ����  ĳ�����ʿ� �ϼ��Ǹ� �۷绩�д�.
    /// </summary>
    List<Gyu_QuestBaseData> quests;

    /// <summary>
    /// ���õ� ����Ʈ 
    /// </summary>
    Gyu_QuestBaseData selectQuest;
    public Gyu_QuestBaseData SelectQuest => selectQuest;

    /// <summary>
    /// �ʿ��ִ� NPC ��
    /// </summary>
    [SerializeField]
    NpcBase_Gyu[] array_NPC;
    public NpcBase_Gyu[] Array_NPC => array_NPC;

    /// <summary>
    /// �������� â�� �����ִ� NPC �ε���
    /// </summary>
    int currentNpcIndex = -1;
    public int CurrentNpcIndex => currentNpcIndex; 

    [Header("���ӻ��� ����Ʈ �迭")]
   
    /// <summary>
    /// ���� ����Ʈ ��Ƶ� �迭 
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] mainStoryQuestArray;
    public Gyu_QuestBaseData[] MainStoryQuestArray => mainStoryQuestArray;

    /// <summary>
    /// ��� ����Ʈ ��Ƶ� �迭 
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] killcountQuestArray;
    public Gyu_QuestBaseData[] KillcountQuestArray => killcountQuestArray;
    
    /// <summary>
    /// ���� ����Ʈ ��Ƶ� �迭
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] gatheringQuestArray;
    public Gyu_QuestBaseData[] GatheringQuestArray => gatheringQuestArray;

    private void Awake()
    {
        player = FindObjectOfType<PlayerQuest_Gyu>();
        talkData = FindAnyObjectByType<TalkData_Gyu>();
        questUIManager = GetComponent<Gyu_UI_QuestManager>();   //��ɺи��� ���� ��ũ��Ʈ�� ���λ���.
        
        questUIManager.onAcceptQuest = () => { 
            array_NPC[currentNpcIndex].CurrentQuest.Quest_state = Quest_State.Quest_Start;
            
            // ����Ʈ �� ������ ����Ǵ���? Ȯ���ʿ� quests �� ���� �����ϸ� npc���� �����Ǿߵ� .. Ŭ������ �Ǿߵȴ�. �ȵǸ� �ε��������� ...
            quests.Add(array_NPC[currentNpcIndex].CurrentQuest);    
        }; 

        questUIManager.onSucessQuest = () => {
            array_NPC[currentNpcIndex].CurrentQuest.Quest_state = Quest_State.Quest_Complete;
            //����ó�� �ʿ���
        }; 

        // ���丮�� �ҽ� ���Ǿ� ��ġ�� � �ĺ������ΰ� �������� �����Ű�°� �������ҰŰ���.
        // �ʱ�ȭ �ϴ°��� ���⸻�� �ٸ������� ���� �ؾߵɰŰ��� .. ���丮 �� ������Ų�ڿ� �����ִ��ϸ� �ɰŰ����ѵ�.. 
        array_NPC = FindObjectsOfType<NpcBase_Gyu>(true);   //�����ִ� ���Ǿ� ã�Ƽ� ��Ƶΰ� ( ã�� ������ �ٲ���������� �ٸ������ ã�ƺ���.)
        for (int i = 0; i < array_NPC.Length; i++)
        {
            //��ġ�� ����� �����Ű�� �ɰŰ��⵵�ѵ�.. �ϴ� ������غ���..
            array_NPC[i].InitData(i, mainStoryQuestArray, killcountQuestArray, gatheringQuestArray); //npc �� �ʱ�ȭ ��Ų��.
            array_NPC[i].onTalkDisableButton = questUIManager.TalkDisableButton;
            array_NPC[i].onTalkEnableButton = (npcId)=> { 
                currentNpcIndex = npcId;
                questUIManager.TalkEnableButton(); 
            };
        }

        questUIManager.onTalkClick = () => array_NPC[currentNpcIndex];

        questUIManager.getTalkDataArray = () => 
        {
            // 0���� ����� �������� ù��° ��ȭ����̴� ������ ������ �ۼ��ؾ��Ѵ�.
            return talkData.GetTalk(array_NPC[currentNpcIndex].TalkType, 0);  
        };

    }




}
