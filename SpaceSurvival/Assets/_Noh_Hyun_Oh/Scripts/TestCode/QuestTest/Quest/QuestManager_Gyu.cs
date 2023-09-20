using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 유아이는 메니저를 캔버스로 이동시켜서 한곳에서 관리하도록한다.
/// </summary>
public class QuestManager_Gyu : MonoBehaviour
{
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
        Killcount,              //  토벌퀘스트
        Gathering,              //  수집퀘스트
        Comunication,           //  일반대화
        Story                   //  시나리오 퀘스트
    }
    /// <summary>
    /// 대화타입 설정하기
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
    /// 대사는 시작시점에 퀘스트 엔피씨 다 찾아서 셋팅하면되고 
    /// </summary>
    TalkData_Gyu talkData;   // 대사 모음


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
    /// <summary>
    /// 퀘스트창 취소 
    /// </summary>
    Button questCancelButton;
    /// <summary>
    /// 퀘스트창 확인
    /// </summary>
    Button questSuccessButton;

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

    /// <summary>
    /// 대화종료 버튼
    /// </summary>
    Button talkEndButton;

    /// <summary>
    /// NPC 대화 버튼 
    /// </summary>
    Button npcTalkButton;

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



    // 진행중인 퀘스트
    List<Quest_Gyu> quests;

    /// <summary>
    /// 맵에있는 NPC 들
    /// </summary>
    NpcBase_Gyu[] array_NPC;

    /// <summary>
    /// 마지막에 창을 열고있던 NPC 인덱스
    /// </summary>
    int currentIndex = -1;

    /// <summary>
    /// 현재 엔피씨가 진행중인 퀘스트
    /// </summary>
    Quest_Gyu currentQuest;


    /// <summary>
    /// 대화 코루틴 중복체크용 
    /// </summary>
    bool isTalking = false;

    private void Awake()
    {
        array_NPC = FindObjectsOfType<NpcBase_Gyu>(true);   //씬에있는 엔피씨 찾아서 담아두고
        foreach (NpcBase_Gyu npc in array_NPC)
        {
            npc.onTalkDisableButton = TalkDisableButton;     
            npc.onTalkEnableButton = TalkEnableButton;
        }
        /*
         테스트데이터생성 
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
            //퀘스트창의  수락버튼
            AcceptButton();
        });
        questCancelButton = questBoxPanel.GetChild(4).GetComponent<Button>();
        questCancelButton.onClick.AddListener(() => {
            //퀘스트창의  취소버튼
            CancelButton();
        });

        questSuccessButton = questBoxPanel.GetChild(5).GetComponent<Button>();
        questSuccessButton.onClick.AddListener(() => {
            //퀘스트창의  확인버튼
            SucessButton();
        });

        talkBoxPanel = transform.GetChild(1);
        npcImg = talkBoxPanel.GetChild(0).GetChild(0).GetComponent<RawImage>();
        nameBox = talkBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        talkBox = talkBoxPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
        questConfirm = talkBoxPanel.GetChild(3).GetComponent<Button>();
        questConfirm.onClick.AddListener(() => {
            //NPC 가주는 퀘스트창여는 버튼
            NpcQuest();
        });
        talkEndButton = talkBoxPanel.GetChild(4).GetComponent<Button>();
        talkEndButton.onClick.AddListener(() => {
            //대화창 닫기버튼
            initialize();
        });
        
        myQuestBoxPanel = transform.GetChild(2); // 이건 단축키로 열어보자 . 현재진행상황이니 
        myQuestBox = myQuestBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();

        npcTalkButton = transform.GetChild(3).GetComponent<Button>();
        npcTalkButton.onClick.AddListener(() => {
            //NPC 근처에가면 대화아이콘이 뜨고 그걸 누르면 발동
            Talk();
        });
        initialize();
    }

    // 화면 중앙 대화하기 버튼 활성화하기 
    private void TalkEnableButton(int npcId)
    {
        currentIndex = npcId;   //현재 대화창이 누구의 NPC 인지 셋팅

        npcTalkButton.gameObject.SetActive(true);
    }

    private void TalkDisableButton() 
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
            talkBoxPanel.gameObject.SetActive(true);
            nameBox.text = array_NPC[currentIndex].name;
            currentQuest = array_NPC[currentIndex].currentQuestGyu;
            string[] talkString = talkData.GetTalk(currentIndex);
            StartCoroutine(Typing(talkString));
        }
    }

    // Npc 상호작용 - 퀘스트 수락, 거절, 완료
    public void NpcQuest()
    {
        Debug.Log(questBoxPanel.gameObject.activeSelf);
        if (!questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(true);
            Quest_Gyu currentQuest = array_NPC[currentIndex].currentQuestGyu;
            // 퀘스트 클리어X, 진행X
            if (array_NPC[currentIndex].currentQuestGyu.IsSucess == false && array_NPC[currentIndex].currentQuestGyu.processValue < 0.1f)
            {
                ForQuest();
                titleBox.text = array_NPC[currentIndex].currentQuestGyu.Title;
                descriptionBox.text = array_NPC[currentIndex].currentQuestGyu.Description;
                clearBox.text = array_NPC[currentIndex].currentQuestGyu.ClearObjectives;
            }
            // 퀘스트 클리어X, 진행O
            else if (array_NPC[currentIndex].currentQuestGyu.IsSucess == false && array_NPC[currentIndex].currentQuestGyu.processValue > 0.1f)
            {
                ProgressQuest();
                titleBox.text = array_NPC[currentIndex].currentQuestGyu.Title;
                descriptionBox.text = array_NPC[currentIndex].currentQuestGyu.Description;
                clearBox.text = array_NPC[currentIndex].currentQuestGyu.ClearObjectives;
            }
            // 퀘스트 클리어O, 진행O
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

    // 퀘스트 수락 버튼
    public void AcceptButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            if (quests.Contains(currentQuest))
            {
                Debug.Log($"{currentQuest}퀘스트 추가가 되있다.");

            }
            else 
            {
                quests.Add(currentQuest);
            }
        }
    }

    // 퀘스트 거절 버튼
    public void CancelButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            Debug.Log("퀘스트 취소버튼");
        }
    }

    // 퀘스트를 완료 버튼(클리어)
    public void SucessButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            Debug.Log("퀘스트 완료버튼");
        }
        CompleteQuest();

        // 퀘스트 클리어 보상 수령
        //var itemC = (ItemCode)ItemNumber;
        //Debug.Log(itemC);
        //GameManager.SlotManager.AddItem(itemC);

    }

    // 현재 진행중인 퀘스트 보기
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
    /// 퀘스트 완료 처리 
    /// </summary>
    void CompleteQuest()
    {

    }

    // 모든.Panel 초기화
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
            foreach(char letter in text.ToCharArray()) 
            {
                talkBox.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
        }
        isTalking = false;
    }
}
