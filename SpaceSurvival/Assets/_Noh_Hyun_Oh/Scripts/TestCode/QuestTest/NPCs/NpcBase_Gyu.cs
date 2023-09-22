using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase_Gyu : MonoBehaviour
{
    
    /// <summary>
    /// 엔피씨의 관리할 인덱스
    /// </summary>
    int npcId;
    public int Npcid => npcId;

    /// <summary>
    /// 현재 NPC가  진행중인 퀘스트
    /// </summary>
    protected Gyu_QuestBaseData currentQuest;
    public Gyu_QuestBaseData CurrentQuest => currentQuest;

    /// <summary>
    /// 퀘스트 리스트 초기 크기 잡기용 에디터상에서 설정
    /// </summary>
    [SerializeField]
    int questCapasity = 4;

    /// <summary>
    /// 이엔피씨가 가지고있는 퀘스트 리스트
    /// </summary>
    protected List<Gyu_QuestBaseData> ownQuestList;
    public List<Gyu_QuestBaseData> OwnQuestList => ownQuestList;
    /// <summary>
    /// 엔피씨의 현재 대화종류
    /// </summary>
    protected TalkType talkType;
    public TalkType TalkType => talkType;
    
    /// <summary>
    /// 대화 진행도 
    /// </summary>
    protected int talkDepth = 0;
    public int TalkDepth => talkDepth;



    /// <summary>
    /// 근처에와서 대화가능하도록 실행시키는 델리게이트
    /// </summary>
    public Action<int> onTalkEnableButton;

    /// <summary>
    /// 주변을 벗어나면 신호보낼 델리게이트
    /// </summary>
    public Action onTalkDisableButton;


    /// <summary>
    /// NPC 얼굴을 찍을 카메라 
    /// </summary>
    Camera npcCharcterCamera;

    /// <summary>
    /// UI rawImage 에 연결할 렌더러 텍스쳐 
    /// </summary>
    public RenderTexture GetTexture => npcCharcterCamera.targetTexture;
    private void Awake()
    {
        npcCharcterCamera = transform.GetComponentInChildren<Camera>();
        npcCharcterCamera.targetTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
    }

    /// <summary>
    /// 엔피씨가 들고 있어야할 데이터 
    /// </summary>
    /// <param name="npcIndex">엔피씨 번호부여할 값</param>
    /// <param name="mainStoryQuestArray">메인 퀘스트</param>
    /// <param name="killcountQuestArray">토벌 퀘스트</param>
    /// <param name="gatheringQuestArray">수집 퀘스트</param>
    public void InitData(int npcIndex , Gyu_QuestBaseData[] mainStoryQuestArray, Gyu_QuestBaseData[] killcountQuestArray, Gyu_QuestBaseData[] gatheringQuestArray) 
    {
        npcId = npcIndex;
        ownQuestList = new(questCapasity);
        SetMainStoryQuest(mainStoryQuestArray);
        SetKillcountQuest(killcountQuestArray);
        SetGatheringQuest(gatheringQuestArray);
    }

    /// <summary>
    /// 이 엔피씨가 가지고있는 메인스토리 셋팅용 함수 
    /// </summary>
    /// <param name="mainStoryQuestArray">게임상에 존재하는 메인스토리 배열</param>
    protected virtual void SetMainStoryQuest(Gyu_QuestBaseData[] mainStoryQuestArray)
    {

    }
    /// <summary>
    /// 이 엔피씨가 가지고있는 토벌 퀘스트 셋팅용 함수
    /// </summary>
    /// <param name="killcountQuestArray">게임상에 존재하는 토벌 퀘스트 배열</param>
    protected virtual void SetKillcountQuest(Gyu_QuestBaseData[] killcountQuestArray)
    {

    }
    /// <summary>
    /// 이 엔피씨가 가지고 있는 수집 퀘스트 셋팅용 함수 
    /// </summary>
    /// <param name="gatheringQuestArray">게임상에 존재하는 수집 퀘스트 배열</param>
    protected virtual void SetGatheringQuest(Gyu_QuestBaseData[] gatheringQuestArray)
    {

    }

    // 퀘스트 리스트중에 어떤값을 가져올지 가져오는 기능도 필요 
    // 특정 진행상황에서만 할수있는 퀘스트 라던가 
    // 완료된 퀘스트 따로 저장한다던가 
    // 리스트에는 있는데 더이상 진행할수없는 퀘스트 라던가  등등 생각날때마다 적자


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
