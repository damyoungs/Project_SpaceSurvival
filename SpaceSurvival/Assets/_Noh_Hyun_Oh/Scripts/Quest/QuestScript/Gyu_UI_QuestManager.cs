using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum Quest_State
{
    None = 0,               // 퀘스트 수락하지않은상태
    Quest_Start,            // 퀘스트 수락상태
    //Quest_Cancel,           // 퀘스트 수락하고 취소한 상태
    Quest_Complete,         // 퀘스트 완료한 상태
}
public class Gyu_UI_QuestManager : MonoBehaviour, IPopupSortWindow
{
   
    /// <summary>
    /// 퀘스트창 제목
    /// </summary>
    TextMeshProUGUI titleBox;

    /// <summary>
    /// 퀘스트창 내용
    /// </summary>
    TextMeshProUGUI descriptionBox;

    /// <summary>
    /// 퀘스트창 보상 내용
    /// </summary>
    TextMeshProUGUI clearBox;

    /// <summary>
    /// 퀘스트창 수락
    /// </summary>
    Button questAcceptButton;
    public Button QuestAcceptButton => questAcceptButton;
    /// <summary>
    /// 퀘스트창 취소 
    /// </summary>
    Button questCancelButton;
    public Button QuestCancelButton => questCancelButton;
    /// <summary>
    /// 퀘스트창 확인
    /// </summary>
    Button questSuccessButton;
    public Button QuestSuccessButton => questSuccessButton;

    /// <summary>
    /// 퀘스트 박스의 트랜스폼
    /// </summary>
    Transform questBoxPanel;



    /// <summary>
    /// 엔피씨 이미지
    /// </summary>
    RawImage npcImg;

    /// <summary>
    /// 엔피씨 이름
    /// </summary>
    TextMeshProUGUI nameBox;

    /// <summary>
    /// 엔피씨 대화목록
    /// </summary>
    TextMeshProUGUI talkBox;

    /// <summary>
    /// 퀘스트 확인 버튼
    /// </summary>
    Button questConfirm;
    public Button QuestConfirm => questConfirm;
    /// <summary>
    /// 대화종료 버튼
    /// </summary>
    Button talkEndButton;
    public Button TalkEndButton => talkEndButton;

    /// <summary>
    /// NPC 대화 버튼 
    /// </summary>
    Button npcTalkButton;
    public Button NpcTalkButton => npcTalkButton;

    /// <summary>
    /// 대화창의 트랜스폼
    /// </summary>
    Transform talkBoxPanel;



    /// <summary>
    /// 진행중인 퀘스트 내용 
    /// </summary>
    TextMeshProUGUI myQuestBox;

    /// <summary>
    /// 진행중인 텍스트박스의 트랜스폼 
    /// </summary>
    Transform myQuestBoxPanel;


    /// <summary>
    /// 퀘스트 리스트 보여줄 창의 트랜스폼
    /// </summary>
    Transform questListPanel;

    /// <summary>
    /// 퀘스트 리스트 를 담을 컨텐츠 의 트랜스폼
    /// </summary>
    Transform questListContentPanel;


    /// <summary>
    /// 퀘스트 리스트 에 보여줄 UI 목록 
    /// </summary>
    Quest_UI_Colum[] quest_UI_Array;

    /// <summary>
    /// 선택된 퀘스트 UI 오브젝트
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
    /// 대화 코루틴 중복체크용 
    /// </summary>
    bool isTalking = false;

    //----------------- 퀘스트 메니저(동적 데이터관리하는) 와 연결할 Action들
    /// <summary>
    /// questData 연결용으로 가져오기 
    /// 기능분리하기위해 따로빼서 추가함
    /// </summary>
    Gyu_QuestManager questManager;

    /// <summary>
    /// Func은 데이터연결타이밍 맞추기에는 편하지만 기본적으로 null 체크를 해야되고 함수호출로 읽어오는것이라
    /// 느릴수밖에없다.
    /// </summary>
    public Func<NpcBase_Gyu>   onTalkClick;

    /// <summary>
    /// 대화 내용을 가져온다.
    /// </summary>
    public Func<int, string[]> getTalkDataArray;
    
    /// <summary>
    /// 퀘스트 수락이됬다고 알려주는 델리게이트
    /// </summary>
    public Action   onAcceptQuest;
    
    /// <summary>
    /// 퀘스트 완료가 됬다고 알려주는 델리게이트 
    /// </summary>
    public Action   onSucessQuest;

    /// <summary>
    /// 퀘스트가 취소 됬다고 알려주는 델리게이트
    /// </summary>
    public Action onCancelQuest;

    /// <summary>
    /// 퀘스트 리스트에서 퀘스트가 선택됬다고 알리는 델리게이트
    /// </summary>
    public Action<Gyu_QuestBaseData> onSelectedQuest;


    /// <summary>
    /// 퀘스트 리스트의 진행중인 퀘스트 보여주는 버튼
    /// </summary>
    Button questList_CurrentViewBtn;
    /// <summary>
    /// 퀘스트 리스트의 완료된 퀘스트 보여주는 버튼
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
            //현재진행중인 퀘스트를 어디에 저장할지 정한뒤에 가져오는 로직 추가해야됨 
            //Gyu_QuestBaseData questData  =  questManager.Player.CurrentQuests[0];
            //MyQuestButton(questData);
            //리스트를 띄워준다.
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
    /// 엔피씨 근처로 갔을경우 대화하기 버튼 활성화 시키기 
    /// </summary>
    public void TalkEnableButton()
    {
        npcTalkButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// 엔피씨 에서 멀어질 경우 퀘스트 UI 초기화 시키기 
    /// </summary>
    public void TalkDisableButton()
    {
        initialize();
    }

    /// <summary>
    /// Npc 대사를 출력
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
    /// 퀘스트 리스트 닫기 함수
    /// </summary>
    private void QuestListPanelClose() 
    {
        questListPanel.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 캐릭터가 수락한 퀘스트 리스트를 보여주는 함수
    /// </summary>
    public void ToPlayerCurrentQuestListPanelOpen()
    {
        questBoxPanel.gameObject.SetActive(false); //퀘스트 상세 내용은 끄고 
        QuestListDataReset();   //기존 데이터리셋 
        ToPlayerQuestListWindowOpenAndDataSetting(questManager.Player.CurrentQuests); //데이터 셋팅후 열기 
    }

    /// <summary>
    /// 캐릭터가 완료한 퀘스트 리스트를 보여주는 함수
    /// </summary>
    public void ToPlayerCompleteQuestListPanelOpen()
    {
        questBoxPanel.gameObject.SetActive(false); //퀘스트 상세 내용은 끄고 
        QuestListDataReset();   //기존 데이터리셋 
        ToPlayerQuestListWindowOpenAndDataSetting(questManager.Player.ClearQuestList); //데이터 셋팅후 열기 
    }

    /// <summary>
    /// 퀘스트 리스트 데이터 셋팅후 활성화 시키는 함수 
    /// </summary>
    /// <param name="questDataList">셋팅할 퀘스트 리스트</param>
    private void ToPlayerQuestListWindowOpenAndDataSetting(List<Gyu_QuestBaseData> questDataList)
    {
        int questIndexCount = 0;
        int checkCount = quest_UI_Array.Length - 1;   //ui갯수 이상 데이터 셋팅하지않게 체크할 변수잡아두고
        foreach (Gyu_QuestBaseData quest_UI in questDataList)   //퀘스트 데이터 다긁어와서 
        {
            quest_UI_Array[questIndexCount].SetData(quest_UI);          // 데이터 셋팅하고 
            quest_UI_Array[questIndexCount].onClick = ToPlayerQuestItemClick;   // 액션 연결하고 
            //UI를 동적으로 생성을안하고있어서 체크하는 로직 추가 
            questIndexCount++;
            if (questIndexCount > checkCount) break;
        }
        questListPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 플레이어의 퀘스트리스트에서 
    /// 퀘스트리스트에서 원하는 퀘스트를 클릭했을때 실행할 내용 
    /// </summary>
    /// <param name="questData"> 클릭한 퀘스트 데이터</param>
    /// <param name="selectColum"> 클릭한 퀘스트 UI 오브젝트</param>
    private void ToPlayerQuestItemClick(Gyu_QuestBaseData questData, Quest_UI_Colum selectColum)
    {
        selectColum.IsSelectedCheck = true;
        MyQuestButton(questData);

        SelectedColum = selectColum;
    }

    /// <summary>
    /// NPC 가 수락 가능한 Quest List 를 출력해주는 창 열고 데이터 셋팅하기
    /// </summary>
    public void ToNpcQuestListPanelToggle()
    {
        myQuestBoxPanel.gameObject.SetActive(false);
        QuestListDataReset();   //기존 데이터리셋 
        QuestNPC currentNpc = (QuestNPC)questManager.Array_NPC[questManager.CurrentNpcIndex]; //현재 대화중인 엔피씨데이터 가져오기
        ToNpcQuestListWindowOpenAndDataSetting(currentNpc.OwnQuestList); //데이터 셋팅후 열기 
    }

    /// <summary>
    /// 퀘스트 리스트 데이터 셋팅후 활성화 시키는 함수 
    /// </summary>
    /// <param name="questDataList">셋팅할 퀘스트 리스트</param>
    private void ToNpcQuestListWindowOpenAndDataSetting(List<Gyu_QuestBaseData> questDataList) 
    {
        int questIndexCount = 0;
        int checkCount = quest_UI_Array.Length-1;   //ui갯수 이상 데이터 셋팅하지않게 체크할 변수잡아두고
        foreach (Gyu_QuestBaseData quest_UI in questDataList)   //퀘스트 데이터 다긁어와서 
        {
            Debug.Log($"퀘스트 인덱스는: {quest_UI.QuestId} , 퀘스트 제목은: {quest_UI.Title}");
            quest_UI_Array[questIndexCount].SetData(quest_UI);          // 데이터 셋팅하고 
            quest_UI_Array[questIndexCount].onClick = QuestItemClick;   // 액션 연결하고 
            //UI를 동적으로 생성을안하고있어서 체크하는 로직 추가 
            questIndexCount++;
            if (questIndexCount > checkCount) break;
        }
        questListPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 퀘스트 리스트 UI 데이터 초기화 시키는함수
    /// </summary>
    private void QuestListDataReset() 
    {
        foreach (Quest_UI_Colum quest_UI in quest_UI_Array)
        {
            quest_UI.ResetData();
        }
    }

    /// <summary>
    /// 퀘스트리스트에서 원하는 퀘스트를 클릭했을때 실행할 내용 
    /// </summary>
    /// <param name="questData"> 클릭한 퀘스트 데이터</param>
    /// <param name="selectColum"> 클릭한 퀘스트 UI 오브젝트</param>
    private void QuestItemClick(Gyu_QuestBaseData questData, Quest_UI_Colum selectColum) 
    {
        selectColum.IsSelectedCheck = true;
        NpcQuest(questData);
        onSelectedQuest?.Invoke(questData);
        SelectedColum = selectColum;
    }


    // Npc 상호작용 - 퀘스트 수락, 거절, 완료
    public void NpcQuest(Gyu_QuestBaseData questData)
    {
        questBoxPanel.gameObject.SetActive(true);

        if (questData.Quest_State == Quest_State.None)  //퀘스트 시작안됬으면 
        {
            ForQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if (questData.Quest_State != Quest_State.Quest_Complete && questData.IsSucess())    //퀘스트 완료 가능 상태
        {
            SucessQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if (questData.Quest_State == Quest_State.Quest_Start) // 퀘스트 진행중인상태 
        {
            ProgressQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if(questData.Quest_State == Quest_State.Quest_Complete) //완료된 퀘스트 
        {
            ProgressQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
    }

    // 퀘스트 수락 버튼
    public void AcceptButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onAcceptQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.Quest_Start;
        }
    }

    // 퀘스트 거절 버튼
    public void CancelButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onCancelQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.None;
        }
    }

    // 퀘스트를 완료 버튼(클리어)
    public void SucessButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onSucessQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.Quest_Complete;
        }
    }

    // 현재 진행중인 퀘스트 보기
    public void MyQuestButton(Gyu_QuestBaseData questData)
    {
        myQuestBoxPanel.gameObject.SetActive(true);
        if (questData != null)  //진행중인 퀘스트가 있는경우 
        {
            myQuestBox.text = questData.Description;
        }
        else //없는경우 
        {
            myQuestBox.text = "진행중인 내용이 없습니다."; 
            //이값도 따로 빼둬야하는데 일단 은 이렇게 해두자
        }
    }

    /// <summary>
    /// 수락 가능한 퀘스트
    /// </summary>
    void ForQuest()
    {
        questAcceptButton.gameObject.SetActive(true);
        questCancelButton.gameObject.SetActive(true);
        questSuccessButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// 완료 가능한 퀘스트
    /// </summary>
    void SucessQuest()
    {
        questAcceptButton.gameObject.SetActive(false);
        questCancelButton.gameObject.SetActive(false);
        questSuccessButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// 퀘스트가 진행중일때
    /// </summary>
    void ProgressQuest()
    {
        questAcceptButton.gameObject.SetActive(false);
        questCancelButton.gameObject.SetActive(false);
        questSuccessButton.gameObject.SetActive(false);
    }


    /// <summary>
    /// 퀘스트 UI 초기화 시키는 함수
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
    /// 타이핑 효과가있는 텍스트 출력 하기
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
