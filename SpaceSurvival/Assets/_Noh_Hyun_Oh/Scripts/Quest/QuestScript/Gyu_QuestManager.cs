using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����Ʈ Ÿ�� �ش� ���� �ٲٸ� ����Ʈ �ε��� ���� ���� �ٲ�� ��������
/// �ڼ��Ѱ� QuestScriptableGenerate ��ũ��Ʈ ���� �κ�����
/// </summary>
public enum QuestType
{
    Story = 0,                   //  �ó����� ����Ʈ
    Killcount ,              //  �������Ʈ
    Gathering ,              //  ��������Ʈ
}

/// <summary>
/// ��ȭ Ÿ��
/// </summary>
public enum TalkType
{
    Comunication = 0,           //  �Ϲݴ�ȭ
    Story,                      //  �ó����� ����Ʈ
    KillCount,                  //  �������Ʈ
    Gathering,                  //  ��������Ʈ
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
    /// ���õ� ����Ʈ ��Ƶ� ����
    /// </summary>
    Gyu_QuestBaseData selectQuest;
    Gyu_QuestBaseData SelectQuest 
    {
        get => selectQuest;
        set 
        {
            if (selectQuest != value)
            {
                selectQuest = value;
                onChangeQuest?.Invoke(selectQuest);

            }
        }
    }

    /// <summary>
    /// �ʿ��ִ� NPC ��
    /// </summary>
    [SerializeField]
    QuestNPC[] array_NPC;
    public QuestNPC[] Array_NPC => array_NPC;

    /// <summary>
    /// �������� â�� �����ִ� NPC �ε���
    /// </summary>
    int currentNpcIndex = -1;
    public int CurrentNpcIndex => currentNpcIndex;

    /// <summary>
    /// ��ȭ �������� üũ�ϱ�
    /// </summary>
    public bool isTalking = false;

    public Action<Gyu_QuestBaseData> onChangeQuest;

    /// <summary>
    /// ����Ʈ �������ִ°� �̱������� ���߿������Ѵ� ������ �׽�Ʈ�� �̴�� �׽�Ʈ
    /// </summary>
    QuestScriptableGenerate questScriptableGenerate;

    NpcTalkController talkController;

    private void Awake()
    {
        player = FindObjectOfType<PlayerQuest_Gyu>();

        talkController = FindObjectOfType<NpcTalkController>();

        questUIManager = GetComponent<Gyu_UI_QuestManager>();   //��ɺи��� ���� ��ũ��Ʈ�� ���λ���.

       

      
    }

    private void Start()
    {
        questUIManager.onSelectedQuest = (quest) =>
        {
            //����Ʈ ����
            SelectQuest = quest;
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

       

        questScriptableGenerate = DataFactory.Instance.QuestScriptableGenerate;
        InitDataSetting();
        InputSystemController.InputSystem.Player.Action.performed += (_) => {
            if (isTalking)
            {
                isTalking = false;
                talkController.Talk(0);
            }
        };

    }
    public void InitDataSetting()
    {

        // ���丮�� �ҽ� ���Ǿ� ��ġ�� � �ĺ������ΰ� �������� �����Ű�°� �������ҰŰ���.
        // �ʱ�ȭ �ϴ°��� ���⸻�� �ٸ������� ���� �ؾߵɰŰ��� .. ���丮 �� ������Ų�ڿ� �����ִ��ϸ� �ɰŰ����ѵ�.. 
        array_NPC = FindObjectsOfType<QuestNPC>(true);   //�����ִ� ���Ǿ� ã�Ƽ� ��Ƶΰ� ( ã�� ������ �ٲ���������� �ٸ������ ã�ƺ���.)
        for (int i = 0; i < array_NPC.Length; i++)
        {
            //��ġ�� ����� �����Ű�� �ɰŰ��⵵�ѵ�.. �ϴ� �������غ���..
            array_NPC[i].InitData(i); //npc �� �ʱ�ȭ ��Ų��.
            array_NPC[i].onTalkDisableButton += () => 
            {
                isTalking = false;
                talkController.ResetData();
                talkController.openTalkWindow = null;
                talkController.closeTalkWindow = null;
                talkController.onTalkClick = null;
                talkController.getTalkDataArray = null;
                talkController.LogManager.getLogTalkDataArray = null;
            }; 
            array_NPC[i].onTalkEnableButton = (npcId) =>
            {
                Cursor.lockState = CursorLockMode.None;
                talkController.ResetData();
                talkController.openTalkWindow = () => questUIManager.OnQuestNpc();
                talkController.closeTalkWindow = () => questUIManager.initialize();
                currentNpcIndex = npcId;
                talkController.onTalkClick = () => array_NPC[currentNpcIndex];

                talkController.getTalkDataArray = (talkIndex) =>
                {
                    return talkController.TalkData.GetTalk(array_NPC[currentNpcIndex].TalkType, talkIndex);
                };
                talkController.LogManager.getLogTalkDataArray = (talkIndex) => {
                    return talkController.TalkData.GetLog(array_NPC[currentNpcIndex].TalkType, talkIndex);
                };
                isTalking = true;
            };

            array_NPC[i].InitQuestData(questScriptableGenerate.MainStoryQuestArray,
                                    questScriptableGenerate.KillcountQuestArray,
                                    questScriptableGenerate.GatheringQuestArray); //����Ʈ ������ ó��

           
        }
    }
}