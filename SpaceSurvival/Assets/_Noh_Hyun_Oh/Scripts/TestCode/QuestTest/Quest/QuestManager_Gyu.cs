using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �����̴� �޴����� ĵ������ �̵����Ѽ� �Ѱ����� �����ϵ����Ѵ�.
/// </summary>
public class QuestManager_Gyu : MonoBehaviour
{
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
        Killcount,              //  �������Ʈ
        Gathering,              //  ��������Ʈ
        Comunication,           //  �Ϲݴ�ȭ
        Story                   //  �ó����� ����Ʈ
    }
    /// <summary>
    /// ��ȭŸ�� �����ϱ�
    /// </summary>
    TalkType talkType = TalkType.Comunication;
    TalkType NPCTalkType 
    {
        get => talkType;
        set 
        {
            if (talkType != value)
            {
                talkType = value;
            }
        }
    }

    /// <summary>
    /// ���� ���۽����� ����Ʈ ���Ǿ� �� ã�Ƽ� �����ϸ�ǰ� 
    /// </summary>
    TalkData_Gyu talkData;   // ��� ����


    /// <summary>
    /// ����Ʈâ ����
    /// </summary>
    TextMeshProUGUI titleBox;

    /// <summary>
    /// ����Ʈâ ����
    /// </summary>
    TextMeshProUGUI descriptionBox;

    /// <summary>
    /// ����Ʈâ ���� ����
    /// </summary>
    TextMeshProUGUI clearBox;

    /// <summary>
    /// ����Ʈâ ����
    /// </summary>
    Button questAcceptButton;
    /// <summary>
    /// ����Ʈâ ��� 
    /// </summary>
    Button questCancelButton;
    /// <summary>
    /// ����Ʈâ Ȯ��
    /// </summary>
    Button questSuccessButton;

    /// <summary>
    /// ����Ʈ �ڽ��� Ʈ������
    /// </summary>
    Transform questBoxPanel;

    

    /// <summary>
    /// ���Ǿ� �̹���
    /// </summary>
    RawImage npcImg;

    /// <summary>
    /// ���Ǿ� �̸�
    /// </summary>
    TextMeshProUGUI nameBox;

    /// <summary>
    /// ���Ǿ� ��ȭ���
    /// </summary>
    TextMeshProUGUI talkBox;

    /// <summary>
    /// ����Ʈ Ȯ�� ��ư
    /// </summary>
    Button questConfirm;

    /// <summary>
    /// ��ȭ���� ��ư
    /// </summary>
    Button talkEndButton;

    /// <summary>
    /// NPC ��ȭ ��ư 
    /// </summary>
    Button npcTalkButton;

    /// <summary>
    /// ��ȭâ�� Ʈ������
    /// </summary>
    Transform talkBoxPanel;


    /// <summary>
    /// �������� ����Ʈ ���� 
    /// </summary>
    TextMeshProUGUI myQuestBox;

    /// <summary>
    /// �������� �ؽ�Ʈ�ڽ��� Ʈ������ 
    /// </summary>
    Transform myQuestBoxPanel;



    // �������� ����Ʈ
    List<Quest_Gyu> quests;

    /// <summary>
    /// �ʿ��ִ� NPC ��
    /// </summary>
    NpcBase_Gyu[] array_NPC;

    /// <summary>
    /// �������� â�� �����ִ� NPC �ε���
    /// </summary>
    int currentIndex = -1;

    /// <summary>
    /// ���� ���Ǿ��� �������� ����Ʈ
    /// </summary>
    Quest_Gyu currentQuest;


    /// <summary>
    /// ��ȭ �ڷ�ƾ �ߺ�üũ�� 
    /// </summary>
    bool isTalking = false;

    private void Awake()
    {
        array_NPC = FindObjectsOfType<NpcBase_Gyu>(true);   //�����ִ� ���Ǿ� ã�Ƽ� ��Ƶΰ�
        foreach (NpcBase_Gyu npc in array_NPC)
        {
            npc.onTalkDisableButton = TalkDisableButton;     
            npc.onTalkEnableButton = TalkEnableButton;
        }
        /*
         �׽�Ʈ�����ͻ��� 
         */
        for (int i =0; i< array_NPC.Length;  i++)  
        {
            array_NPC[i].SettingQuest(i);
        }

        talkData = FindAnyObjectByType<TalkData_Gyu>();

        questBoxPanel = transform.GetChild(0);
        titleBox = questBoxPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionBox = questBoxPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        clearBox = questBoxPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
        questAcceptButton = questBoxPanel.GetChild(3).GetComponent<Button>();
        questAcceptButton.onClick.AddListener(() => {
            //����Ʈâ��  ������ư
            AcceptButton();
        });
        questCancelButton = questBoxPanel.GetChild(4).GetComponent<Button>();
        questCancelButton.onClick.AddListener(() => {
            //����Ʈâ��  ��ҹ�ư
            CancelButton();
        });

        questSuccessButton = questBoxPanel.GetChild(5).GetComponent<Button>();
        questSuccessButton.onClick.AddListener(() => {
            //����Ʈâ��  Ȯ�ι�ư
            SucessButton();
        });

        talkBoxPanel = transform.GetChild(1);
        npcImg = talkBoxPanel.GetChild(0).GetChild(0).GetComponent<RawImage>();
        nameBox = talkBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        talkBox = talkBoxPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
        questConfirm = talkBoxPanel.GetChild(3).GetComponent<Button>();
        questConfirm.onClick.AddListener(() => {
            //NPC ���ִ� ����Ʈâ���� ��ư
            NpcQuest();
        });
        talkEndButton = talkBoxPanel.GetChild(4).GetComponent<Button>();
        talkEndButton.onClick.AddListener(() => {
            //��ȭâ �ݱ��ư
            initialize();
        });
        
        myQuestBoxPanel = transform.GetChild(2); // �̰� ����Ű�� ����� . ���������Ȳ�̴� 
        myQuestBox = myQuestBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();

        npcTalkButton = transform.GetChild(3).GetComponent<Button>();
        npcTalkButton.onClick.AddListener(() => {
            //NPC ��ó������ ��ȭ�������� �߰� �װ� ������ �ߵ�
            Talk();
        });
        initialize();
    }

    // ȭ�� �߾� ��ȭ�ϱ� ��ư Ȱ��ȭ�ϱ� 
    private void TalkEnableButton(int npcId)
    {
        currentIndex = npcId;   //���� ��ȭâ�� ������ NPC ���� ����

        npcTalkButton.gameObject.SetActive(true);
    }

    private void TalkDisableButton() 
    {
        initialize();
    } 

    /// <summary>
    /// Npc ��縦 ���
    /// </summary>
    public void Talk()
    {
        if (!isTalking)
        { 
            talkBoxPanel.gameObject.SetActive(true);
            nameBox.text = array_NPC[currentIndex].name;
            currentQuest = array_NPC[currentIndex].currentQuestGyu;
            string[] talkString = talkData.GetTalk(currentIndex);
            StartCoroutine(Typing(talkString));
        }
    }

    // Npc ��ȣ�ۿ� - ����Ʈ ����, ����, �Ϸ�
    public void NpcQuest()
    {
        Debug.Log(questBoxPanel.gameObject.activeSelf);
        if (!questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(true);
            Quest_Gyu currentQuest = array_NPC[currentIndex].currentQuestGyu;
            // ����Ʈ Ŭ����X, ����X
            if (array_NPC[currentIndex].currentQuestGyu.IsSucess == false && array_NPC[currentIndex].currentQuestGyu.processValue < 0.1f)
            {
                ForQuest();
                titleBox.text = array_NPC[currentIndex].currentQuestGyu.Title;
                descriptionBox.text = array_NPC[currentIndex].currentQuestGyu.Description;
                clearBox.text = array_NPC[currentIndex].currentQuestGyu.ClearObjectives;
            }
            // ����Ʈ Ŭ����X, ����O
            else if (array_NPC[currentIndex].currentQuestGyu.IsSucess == false && array_NPC[currentIndex].currentQuestGyu.processValue > 0.1f)
            {
                ProgressQuest();
                titleBox.text = array_NPC[currentIndex].currentQuestGyu.Title;
                descriptionBox.text = array_NPC[currentIndex].currentQuestGyu.Description;
                clearBox.text = array_NPC[currentIndex].currentQuestGyu.ClearObjectives;
            }
            // ����Ʈ Ŭ����O, ����O
            else if (array_NPC[currentIndex].currentQuestGyu.IsSucess == true && array_NPC[currentIndex].currentQuestGyu.processValue > 0.1f)
            {
                SucessQuest();
                titleBox.text = array_NPC[currentIndex].currentQuestGyu.Title;
                descriptionBox.text = array_NPC[currentIndex].currentQuestGyu.Description;
                clearBox.text = array_NPC[currentIndex].currentQuestGyu.ClearObjectives;
            }
        }
        else 
        {
            questBoxPanel.gameObject.SetActive(false);
        }
    }

    // ����Ʈ ���� ��ư
    public void AcceptButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            if (quests.Contains(currentQuest))
            {
                Debug.Log($"{currentQuest}����Ʈ �߰��� ���ִ�.");

            }
            else 
            {
                quests.Add(currentQuest);
            }
        }
    }

    // ����Ʈ ���� ��ư
    public void CancelButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            Debug.Log("����Ʈ ��ҹ�ư");
        }
    }

    // ����Ʈ�� �Ϸ� ��ư(Ŭ����)
    public void SucessButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            Debug.Log("����Ʈ �Ϸ��ư");
        }
        CompleteQuest();

        // ����Ʈ Ŭ���� ���� ����
        //var itemC = (ItemCode)ItemNumber;
        //Debug.Log(itemC);
        //GameManager.SlotManager.AddItem(itemC);

    }

    // ���� �������� ����Ʈ ����
    public void MyQuestButton()
    {
        if (!myQuestBoxPanel.gameObject.activeSelf)
        {
           myQuestBoxPanel.gameObject.SetActive(true);
            if (currentQuest != null)
            {
                myQuestBox.text = array_NPC[currentIndex].currentQuestGyu.Description;
            }
          
        }
        else
        {
            myQuestBoxPanel.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���� ������ ����Ʈ
    /// </summary>
    void ForQuest()
    {
        questAcceptButton.gameObject.SetActive(true);
        questCancelButton.gameObject.SetActive(true);
        questSuccessButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// �Ϸ� ������ ����Ʈ
    /// </summary>
    void SucessQuest()
    {
        questAcceptButton.gameObject.SetActive(false);
        questCancelButton.gameObject.SetActive(false);
        questSuccessButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// ����Ʈ�� �������϶�
    /// </summary>
    void ProgressQuest()
    {
        questAcceptButton.gameObject.SetActive(false);
        questCancelButton.gameObject.SetActive(false);
        questSuccessButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����Ʈ �Ϸ� ó�� 
    /// </summary>
    void CompleteQuest()
    {

    }

    // ���.Panel �ʱ�ȭ
    public void initialize()
    {
        talkType = TalkType.Comunication;
        
        titleBox.text = "";
        descriptionBox.text = "";
        clearBox.text = "";
        
        npcImg.texture = null;
        nameBox.text = "";
        talkBox.text = "";

        //currentIndex = -1;
        currentQuest = null;


        myQuestBoxPanel.gameObject.SetActive(false);
        questBoxPanel.gameObject.SetActive(false);
        talkBoxPanel.gameObject.SetActive(false);

        //quests = null;
        //array_NPC = null;
        npcTalkButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ÿ���� ȿ�����ִ� �ؽ�Ʈ ��� �ϱ�
    /// </summary>
    /// <param name="textArray"></param>
    /// <returns></returns>
    IEnumerator Typing(string[] textArray)
    {
        isTalking = true;
        talkBox.text = "";
        foreach (string text in textArray)
        {
            foreach(char letter in text.ToCharArray()) 
            {
                talkBox.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
        }
        isTalking = false;
    }
}
