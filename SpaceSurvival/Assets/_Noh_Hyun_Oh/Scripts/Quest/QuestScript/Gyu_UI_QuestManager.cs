using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum Quest_State
{
    None = 0,               // ����Ʈ ����������������
    Quest_Start,            // ����Ʈ ��������
    //Quest_Cancel,           // ����Ʈ �����ϰ� ����� ����
    Quest_Complete,         // ����Ʈ �Ϸ��� ����
}
public class Gyu_UI_QuestManager : MonoBehaviour, IPopupSortWindow
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


    /// <summary>
    /// ����Ʈ ����Ʈ �� ������ UI ��� 
    /// </summary>
    Quest_UI_Colum[] quest_UI_Array;

    /// <summary>
    /// ���õ� ����Ʈ UI ������Ʈ
    /// </summary>
    Quest_UI_Colum selectedColum;
    Quest_UI_Colum SelectedColum 
    {
        get => selectedColum;
        set 
        {
            if (selectedColum != value) 
            {
                if (selectedColum != null) 
                {

                    selectedColum.IsSelectedCheck = false;
                }
                selectedColum = value;
            }
        }
    }

    public Action<IPopupSortWindow> PopupSorting { get ; set ; }

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
    public Func<int, string[]> getTalkDataArray;
    
    /// <summary>
    /// ����Ʈ �����̉�ٰ� �˷��ִ� ��������Ʈ
    /// </summary>
    public Action   onAcceptQuest;
    
    /// <summary>
    /// ����Ʈ �Ϸᰡ ��ٰ� �˷��ִ� ��������Ʈ 
    /// </summary>
    public Action   onSucessQuest;

    /// <summary>
    /// ����Ʈ�� ��� ��ٰ� �˷��ִ� ��������Ʈ
    /// </summary>
    public Action onCancelQuest;

    /// <summary>
    /// ����Ʈ ����Ʈ���� ����Ʈ�� ���É�ٰ� �˸��� ��������Ʈ
    /// </summary>
    public Action<Gyu_QuestBaseData> onSelectedQuest;


    /// <summary>
    /// ����Ʈ ����Ʈ�� �������� ����Ʈ �����ִ� ��ư
    /// </summary>
    Button questList_CurrentViewBtn;
    /// <summary>
    /// ����Ʈ ����Ʈ�� �Ϸ�� ����Ʈ �����ִ� ��ư
    /// </summary>
    Button questList_ClearViewBtn;

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
        questConfirm.onClick.AddListener(ToNpcQuestListPanelToggle);

        talkEndButton = talkBoxPanel.GetChild(4).GetComponent<Button>();
        talkEndButton.onClick.AddListener(initialize);


        myQuestBoxPanel = transform.GetChild(2); 
        myQuestBox = myQuestBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();


        npcTalkButton = transform.GetChild(3).GetComponent<Button>();
        npcTalkButton.onClick.AddListener(Talk);

        questListPanel = transform.GetChild(4);
        questList_CurrentViewBtn = questListPanel.GetChild(2).GetComponent<Button>();
        questList_CurrentViewBtn.onClick.AddListener(() => {
            ToPlayerCurrentQuestListPanelOpen();
        });
        questList_ClearViewBtn = questListPanel.GetChild(3).GetComponent<Button>();
        questList_ClearViewBtn.onClick.AddListener(() => {
            ToPlayerCompleteQuestListPanelOpen();
        });

        questListContentPanel = questListPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;
        quest_UI_Array = questListContentPanel.GetComponentsInChildren<Quest_UI_Colum>(true);
        initialize();

       
        
    }
    private void Start()
    {
        InputSystemController.InputSystem.Options.Quest.performed += (_) => {
            //������������ ����Ʈ�� ��� �������� ���ѵڿ� �������� ���� �߰��ؾߵ� 
            //Gyu_QuestBaseData questData  =  questManager.Player.CurrentQuests[0];
            //MyQuestButton(questData);
            //����Ʈ�� ����ش�.
            if (!questListPanel.gameObject.activeSelf)
            {
                ToPlayerCurrentQuestListPanelOpen();
                OpenWindow();
            }
            else 
            {
                QuestListPanelClose();
            }
        };
        InputSystemController.InputSystem.Common.Esc.performed += (_) => {
            initialize();
        };
    }

    /// <summary>
    /// ���Ǿ� ��ó�� ������� ��ȭ�ϱ� ��ư Ȱ��ȭ ��Ű�� 
    /// </summary>
    public void TalkEnableButton()
    {
        npcTalkButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���Ǿ� ���� �־��� ��� ����Ʈ UI �ʱ�ȭ ��Ű�� 
    /// </summary>
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
                string[] talkString = getTalkDataArray?.Invoke(0);
                if (talkString != null &&  talkString.Length > 0) 
                {
                    StartCoroutine(Typing(talkString));
                }
            }
            OpenWindow();
        }
    }

    /// <summary>
    /// ����Ʈ ����Ʈ �ݱ� �Լ�
    /// </summary>
    private void QuestListPanelClose() 
    {
        questListPanel.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// ĳ���Ͱ� ������ ����Ʈ ����Ʈ�� �����ִ� �Լ�
    /// </summary>
    public void ToPlayerCurrentQuestListPanelOpen()
    {
        questBoxPanel.gameObject.SetActive(false); //����Ʈ �� ������ ���� 
        QuestListDataReset();   //���� �����͸��� 
        ToPlayerQuestListWindowOpenAndDataSetting(questManager.Player.CurrentQuests); //������ ������ ���� 
    }

    /// <summary>
    /// ĳ���Ͱ� �Ϸ��� ����Ʈ ����Ʈ�� �����ִ� �Լ�
    /// </summary>
    public void ToPlayerCompleteQuestListPanelOpen()
    {
        questBoxPanel.gameObject.SetActive(false); //����Ʈ �� ������ ���� 
        QuestListDataReset();   //���� �����͸��� 
        ToPlayerQuestListWindowOpenAndDataSetting(questManager.Player.ClearQuestList); //������ ������ ���� 
    }

    /// <summary>
    /// ����Ʈ ����Ʈ ������ ������ Ȱ��ȭ ��Ű�� �Լ� 
    /// </summary>
    /// <param name="questDataList">������ ����Ʈ ����Ʈ</param>
    private void ToPlayerQuestListWindowOpenAndDataSetting(List<Gyu_QuestBaseData> questDataList)
    {
        int questIndexCount = 0;
        int checkCount = quest_UI_Array.Length - 1;   //ui���� �̻� ������ ���������ʰ� üũ�� ������Ƶΰ�
        foreach (Gyu_QuestBaseData quest_UI in questDataList)   //����Ʈ ������ �ٱܾ�ͼ� 
        {
            quest_UI_Array[questIndexCount].SetData(quest_UI);          // ������ �����ϰ� 
            quest_UI_Array[questIndexCount].onClick = ToPlayerQuestItemClick;   // �׼� �����ϰ� 
            //UI�� �������� ���������ϰ��־ üũ�ϴ� ���� �߰� 
            questIndexCount++;
            if (questIndexCount > checkCount) break;
        }
        questListPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// �÷��̾��� ����Ʈ����Ʈ���� 
    /// ����Ʈ����Ʈ���� ���ϴ� ����Ʈ�� Ŭ�������� ������ ���� 
    /// </summary>
    /// <param name="questData"> Ŭ���� ����Ʈ ������</param>
    /// <param name="selectColum"> Ŭ���� ����Ʈ UI ������Ʈ</param>
    private void ToPlayerQuestItemClick(Gyu_QuestBaseData questData, Quest_UI_Colum selectColum)
    {
        selectColum.IsSelectedCheck = true;
        MyQuestButton(questData);

        SelectedColum = selectColum;
    }

    /// <summary>
    /// NPC �� ���� ������ Quest List �� ������ִ� â ���� ������ �����ϱ�
    /// </summary>
    public void ToNpcQuestListPanelToggle()
    {
        myQuestBoxPanel.gameObject.SetActive(false);
        QuestListDataReset();   //���� �����͸��� 
        QuestNPC currentNpc = (QuestNPC)questManager.Array_NPC[questManager.CurrentNpcIndex]; //���� ��ȭ���� ���Ǿ������� ��������
        ToNpcQuestListWindowOpenAndDataSetting(currentNpc.OwnQuestList); //������ ������ ���� 
    }

    /// <summary>
    /// ����Ʈ ����Ʈ ������ ������ Ȱ��ȭ ��Ű�� �Լ� 
    /// </summary>
    /// <param name="questDataList">������ ����Ʈ ����Ʈ</param>
    private void ToNpcQuestListWindowOpenAndDataSetting(List<Gyu_QuestBaseData> questDataList) 
    {
        int questIndexCount = 0;
        int checkCount = quest_UI_Array.Length-1;   //ui���� �̻� ������ ���������ʰ� üũ�� ������Ƶΰ�
        foreach (Gyu_QuestBaseData quest_UI in questDataList)   //����Ʈ ������ �ٱܾ�ͼ� 
        {
            Debug.Log($"����Ʈ �ε�����: {quest_UI.QuestId} , ����Ʈ ������: {quest_UI.Title}");
            quest_UI_Array[questIndexCount].SetData(quest_UI);          // ������ �����ϰ� 
            quest_UI_Array[questIndexCount].onClick = QuestItemClick;   // �׼� �����ϰ� 
            //UI�� �������� ���������ϰ��־ üũ�ϴ� ���� �߰� 
            questIndexCount++;
            if (questIndexCount > checkCount) break;
        }
        questListPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// ����Ʈ ����Ʈ UI ������ �ʱ�ȭ ��Ű���Լ�
    /// </summary>
    private void QuestListDataReset() 
    {
        foreach (Quest_UI_Colum quest_UI in quest_UI_Array)
        {
            quest_UI.ResetData();
        }
    }

    /// <summary>
    /// ����Ʈ����Ʈ���� ���ϴ� ����Ʈ�� Ŭ�������� ������ ���� 
    /// </summary>
    /// <param name="questData"> Ŭ���� ����Ʈ ������</param>
    /// <param name="selectColum"> Ŭ���� ����Ʈ UI ������Ʈ</param>
    private void QuestItemClick(Gyu_QuestBaseData questData, Quest_UI_Colum selectColum) 
    {
        selectColum.IsSelectedCheck = true;
        NpcQuest(questData);
        onSelectedQuest?.Invoke(questData);
        SelectedColum = selectColum;
    }


    // Npc ��ȣ�ۿ� - ����Ʈ ����, ����, �Ϸ�
    public void NpcQuest(Gyu_QuestBaseData questData)
    {
        questBoxPanel.gameObject.SetActive(true);

        if (questData.Quest_State == Quest_State.None)  //����Ʈ ���۾ȉ����� 
        {
            ForQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if (questData.Quest_State != Quest_State.Quest_Complete && questData.IsSucess())    //����Ʈ �Ϸ� ���� ����
        {
            SucessQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if (questData.Quest_State == Quest_State.Quest_Start) // ����Ʈ �������λ��� 
        {
            ProgressQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if(questData.Quest_State == Quest_State.Quest_Complete) //�Ϸ�� ����Ʈ 
        {
            ProgressQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
    }

    // ����Ʈ ���� ��ư
    public void AcceptButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onAcceptQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.Quest_Start;
        }
    }

    // ����Ʈ ���� ��ư
    public void CancelButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onCancelQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.None;
        }
    }

    // ����Ʈ�� �Ϸ� ��ư(Ŭ����)
    public void SucessButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onSucessQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.Quest_Complete;
        }
    }

    // ���� �������� ����Ʈ ����
    public void MyQuestButton(Gyu_QuestBaseData questData)
    {
        myQuestBoxPanel.gameObject.SetActive(true);
        if (questData != null)  //�������� ����Ʈ�� �ִ°�� 
        {
            myQuestBox.text = questData.Description;
        }
        else //���°�� 
        {
            myQuestBox.text = "�������� ������ �����ϴ�."; 
            //�̰��� ���� ���־��ϴµ� �ϴ� �� �̷��� �ص���
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
    /// ����Ʈ UI �ʱ�ȭ ��Ű�� �Լ�
    /// </summary>
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
        questListPanel.gameObject.SetActive(false);
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

    public void OpenWindow()
    {
        PopupSorting(this);
    }

    public void CloseWindow()
    {
        initialize();
    }
}
