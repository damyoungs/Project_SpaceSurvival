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
/// �ӽ÷� ��Ƶ� ���� Ÿ��
/// </summary>
public enum Monster_Type 
{
    Base = 0,
    Size_S ,
    Size_M ,
    Size_L ,
    Boss ,
}

/// <summary>
/// �����̴� �޴����� ĵ������ �̵����Ѽ� �Ѱ����� �����ϵ����Ѵ�.
/// </summary>
public class Gyu_QuestManager : MonoBehaviour
{
    [SerializeField]
    int questListCapacity = 10;
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
    /// ���õ� ����Ʈ ��Ƶ� ����
    /// </summary>
    Gyu_QuestBaseData selectQuest;

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

    /// <summary>
    /// ����Ʈ �������ִ°� �̱������� ���߿������Ѵ� ������ �׽�Ʈ�� �̴�� �׽�Ʈ
    /// </summary>
    QuestScriptableGenerate questScriptableGenerate;

    private void Awake()
    {

        questScriptableGenerate = GetComponentInChildren<QuestScriptableGenerate>();

        player = FindObjectOfType<PlayerQuest_Gyu>();

        talkData = FindAnyObjectByType<TalkData_Gyu>();

        questUIManager = GetComponent<Gyu_UI_QuestManager>();   //��ɺи��� ���� ��ũ��Ʈ�� ���λ���.

        questUIManager.onSelectedQuest = (quest) => 
        {
            //����Ʈ ����
            selectQuest = quest;
        };

        questUIManager.onAcceptQuest = () => 
        {
            //����Ʈ �߰�
            player.AppendQuest(selectQuest);
        };

        questUIManager.onSucessQuest = () =>
        {
            //����Ʈ �Ϸ� 
            player.ClearQuest(selectQuest);
        };
        questUIManager.onCancelQuest = () => 
        {
            //����Ʈ ��� 
            player.CancelQuest(selectQuest);
        };
        
        // ���丮�� �ҽ� ���Ǿ� ��ġ�� � �ĺ������ΰ� �������� �����Ű�°� �������ҰŰ���.
        // �ʱ�ȭ �ϴ°��� ���⸻�� �ٸ������� ���� �ؾߵɰŰ��� .. ���丮 �� ������Ų�ڿ� �����ִ��ϸ� �ɰŰ����ѵ�.. 
        array_NPC = FindObjectsOfType<NpcBase_Gyu>(true);   //�����ִ� ���Ǿ� ã�Ƽ� ��Ƶΰ� ( ã�� ������ �ٲ���������� �ٸ������ ã�ƺ���.)
        for (int i = 0; i < array_NPC.Length; i++)
        {
            //��ġ�� ����� �����Ű�� �ɰŰ��⵵�ѵ�.. �ϴ� ������غ���..
            array_NPC[i].InitData(i); //npc �� �ʱ�ȭ ��Ų��.
            array_NPC[i].onTalkDisableButton = questUIManager.TalkDisableButton;
            array_NPC[i].onTalkEnableButton = (npcId) =>
            {
                currentNpcIndex = npcId;
                questUIManager.TalkEnableButton();
            };

            if (array_NPC[i] is QuestNPC questNPC)  //����Ʈ ���Ǿ��ϰ�� 
            {
                questNPC.InitQuestData(questScriptableGenerate.MainStoryQuestArray,
                                        questScriptableGenerate.KillcountQuestArray,
                                        questScriptableGenerate.GatheringQuestArray); //����Ʈ ������ ó��
                //questNPC.InitQuestData(DataFactory.Instance.QuestScriptableGenerate.MainStoryQuestArray,
                //                      DataFactory.Instance.QuestScriptableGenerate.KillcountQuestArray,
                //                      DataFactory.Instance.QuestScriptableGenerate.GatheringQuestArray); //����Ʈ ������ ó��
            }
            //else if () // �����ϰ�� �ʱ�ȭó��
            //{
            //}
        }

        questUIManager.onTalkClick = () => array_NPC[currentNpcIndex];

        questUIManager.getTalkDataArray = (talkIndex) => 
        {
            return talkData.GetTalk(array_NPC[currentNpcIndex].TalkType, talkIndex);  
        };

    }

}
