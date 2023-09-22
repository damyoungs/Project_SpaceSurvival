using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gyu_UI_QuestManager : MonoBehaviour
{
   
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
    public Button QuestAcceptButton => questAcceptButton;
    /// <summary>
    /// ����Ʈâ ��� 
    /// </summary>
    Button questCancelButton;
    public Button QuestCancelButton => questCancelButton;
    /// <summary>
    /// ����Ʈâ Ȯ��
    /// </summary>
    Button questSuccessButton;
    public Button QuestSuccessButton => questSuccessButton;

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
    public Button QuestConfirm => questConfirm;
    /// <summary>
    /// ��ȭ���� ��ư
    /// </summary>
    Button talkEndButton;
    public Button TalkEndButton => talkEndButton;

    /// <summary>
    /// NPC ��ȭ ��ư 
    /// </summary>
    Button npcTalkButton;
    public Button NpcTalkButton => npcTalkButton;

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


    /// <summary>
    /// ����Ʈ ����Ʈ ������ â�� Ʈ������
    /// </summary>
    Transform questListPanel;

    /// <summary>
    /// ����Ʈ ����Ʈ �� ���� ������ �� Ʈ������
    /// </summary>
    Transform questListContentPanel;


    Quest_UI_Colum[] quest_UI_Array;


    /// <summary>
    /// ��ȭ �ڷ�ƾ �ߺ�üũ�� 
    /// </summary>
    bool isTalking = false;

    //----------------- ����Ʈ �޴���(���� �����Ͱ����ϴ�) �� ������ Action��
    /// <summary>
    /// questData ��������� �������� 
    /// ��ɺи��ϱ����� ���λ��� �߰���
    /// </summary>
    Gyu_QuestManager questManager;

    /// <summary>
    /// Func�� �����Ϳ���Ÿ�̹� ���߱⿡�� �������� �⺻������ null üũ�� �ؾߵǰ� �Լ�ȣ��� �о���°��̶�
    /// �������ۿ�����.
    /// </summary>
    public Func<NpcBase_Gyu>   onTalkClick;

    /// <summary>
    /// ��ȭ ������ �����´�.
    /// </summary>
    public Func<string[]> getTalkDataArray;
    
    /// <summary>
    /// ����Ʈ �����̉�ٰ� �˷��ִ� ��������Ʈ
    /// </summary>
    public Action   onAcceptQuest;
    
    /// <summary>
    /// ����Ʈ �Ϸᰡ ��ٰ� �˷��ִ� ��������Ʈ 
    /// </summary>
    public Action   onSucessQuest;


    private void Awake()
    {
        questManager = GetComponent<Gyu_QuestManager>(); 


        questBoxPanel = transform.GetChild(0);
        titleBox = questBoxPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionBox = questBoxPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        clearBox = questBoxPanel.GetChild(2).GetComponent<TextMeshProUGUI>();

        questAcceptButton = questBoxPanel.GetChild(3).GetComponent<Button>();
        questAcceptButton.onClick.AddListener(AcceptButton);
        
        questCancelButton = questBoxPanel.GetChild(4).GetComponent<Button>();
        questCancelButton.onClick.AddListener(CancelButton);

        questSuccessButton = questBoxPanel.GetChild(5).GetComponent<Button>();
        questSuccessButton.onClick.AddListener(SucessButton);


        talkBoxPanel = transform.GetChild(1);

        npcImg = talkBoxPanel.GetChild(0).GetChild(0).GetComponent<RawImage>();

        nameBox = talkBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        talkBox = talkBoxPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
        
        questConfirm = talkBoxPanel.GetChild(3).GetComponent<Button>();
        questConfirm.onClick.AddListener(QuestListPanelToggle);

        talkEndButton = talkBoxPanel.GetChild(4).GetComponent<Button>();
        talkEndButton.onClick.AddListener(initialize);


        myQuestBoxPanel = transform.GetChild(2); 
        myQuestBox = myQuestBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();


        npcTalkButton = transform.GetChild(3).GetComponent<Button>();
        npcTalkButton.onClick.AddListener(Talk);

        questListPanel = transform.GetChild(4);
        questListContentPanel = questListPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;
        quest_UI_Array = questListContentPanel.GetComponentsInChildren<Quest_UI_Colum>(true);

        initialize();

       
        InputSystemController.InputSystem.Options.Enable(); //���߿� �����

        InputSystemController.InputSystem.Options.Quest.performed += (_) => {
            //������������ ����Ʈ�� ��� �������� ���ѵڿ� �������� ���� �߰��ؾߵ� 

            MyQuestButton(questManager.Array_NPC[questManager.CurrentNpcIndex].CurrentQuest);

        };

    }

    // ȭ�� �߾� ��ȭ�ϱ� ��ư Ȱ��ȭ�ϱ� 
    public void TalkEnableButton()
    {
        npcTalkButton.gameObject.SetActive(true);
    }

    public void TalkDisableButton()
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
            NpcBase_Gyu npc =  onTalkClick?.Invoke();
            npcImg.texture = npc.GetTexture;
            if (npc != null)
            {
                npcTalkButton.gameObject.SetActive(false);
                talkBoxPanel.gameObject.SetActive(true);
                nameBox.text = npc.name;
                string[] talkString = getTalkDataArray?.Invoke();
                if (talkString != null &&  talkString.Length > 0) 
                {
                    StartCoroutine(Typing(talkString));
                }
            }
        }
    }

    public void QuestListPanelToggle()
    {
        QuestListDataReset();
        NpcBase_Gyu currentNpc = questManager.Array_NPC[questManager.CurrentNpcIndex]; //���� ��ȭ���� ���Ǿ������� ��������
        QuestListWindowOpenAndDataSetting(currentNpc.OwnQuestList);
    }

    private void QuestListWindowOpenAndDataSetting(List<Gyu_QuestBaseData> questDataList) 
    {
        int questIndexCount = 0;
        int checkCount = quest_UI_Array.Length-1;   //ui���� �̻� ������ ���������ʰ� üũ�� ������Ƶΰ�
        foreach (Gyu_QuestBaseData quest_UI in questDataList)   //����Ʈ ������ �ٱܾ�ͼ� 
        {
            quest_UI_Array[questIndexCount].SetData(quest_UI);          // ������ �����ϰ� 
            quest_UI_Array[questIndexCount].onClick = questItemClick;   // �׼� �����ϰ� 
            //UI�� �������� ���������ϰ��־ üũ�ϴ� ���� �߰� 
            questIndexCount++;
            if (questIndexCount > checkCount) break;
        }
    }

    private void QuestListDataReset() 
    {
        foreach (Quest_UI_Colum quest_UI in quest_UI_Array)
        {
            quest_UI.ResetData();
        }
    }

    /// <summary>
    /// ������ Ŭ�������� ������ ���� 
    /// </summary>
    private void questItemClick(Gyu_QuestBaseData questData) 
    {
        NpcQuest(questData);
    }
    // Npc ��ȣ�ۿ� - ����Ʈ ����, ����, �Ϸ�
    public void NpcQuest(Gyu_QuestBaseData questData)
    {
        questBoxPanel.gameObject.SetActive(true);
            
        if (questData.Quest_state == Quest_State.None)  //����Ʈ ���۾ȉ����� 
        {
            ForQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
        }
        else if (questData.IsSucess())    //����Ʈ �Ϸ� ���� ����
        {
            SucessQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
        }
        else if (questData.Quest_state == Quest_State.Quest_Start) // ����Ʈ �������λ��� 
        {
            ProgressQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
        }
        // ����Ʈ �Ϸ��Ҽ��ִ� ����
    }

    // ����Ʈ ���� ��ư
    public void AcceptButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onAcceptQuest?.Invoke();
            Debug.Log("����Ʈ ���� ��ư Ŭ�� ");
        }
    }

    // ����Ʈ ���� ��ư
    public void CancelButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            Debug.Log("����Ʈ ��ҹ�ư");
            //������ ��������? �ٽ� ����Ʈ�ִ� ����? �ƴϸ� ����������Ʈ�� �ٽô¸��޴»���?
        }
    }

    // ����Ʈ�� �Ϸ� ��ư(Ŭ����)
    public void SucessButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onSucessQuest?.Invoke();
            
            Debug.Log("����Ʈ �Ϸ��ư");
        }
    }

    // ���� �������� ����Ʈ ����
    public void MyQuestButton(Gyu_QuestBaseData questData)
    {
        if (!myQuestBoxPanel.gameObject.activeSelf)
        {
            myQuestBoxPanel.gameObject.SetActive(true);
            if (questData != null)  //�������� ����Ʈ�� �ִ°�� 
            {
                myQuestBox.text = questData.Description;
            }
            else //���°�� 
            {
                myQuestBox.text = "�������� ������ �����ϴ�."; //�̰��� ���� ���־��ϴµ� �ϴ� �� �̷��� �ص���
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



    



    // ���.Panel �ʱ�ȭ
    public void initialize()
    {
        titleBox.text = "";
        descriptionBox.text = "";
        clearBox.text = "";

        npcImg.texture = null;
        nameBox.text = "";
        talkBox.text = "";

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
            foreach (char letter in text.ToCharArray())
            {
                talkBox.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
            talkBox.text += "\r\n";
        }
        isTalking = false;
    }
}
