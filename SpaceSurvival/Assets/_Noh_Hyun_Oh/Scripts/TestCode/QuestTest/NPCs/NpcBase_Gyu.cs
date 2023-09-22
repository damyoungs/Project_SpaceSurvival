using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase_Gyu : MonoBehaviour
{
    
    /// <summary>
    /// ���Ǿ��� ������ �ε���
    /// </summary>
    int npcId;
    public int Npcid => npcId;

    /// <summary>
    /// ���� NPC��  �������� ����Ʈ
    /// </summary>
    protected Gyu_QuestBaseData currentQuest;
    public Gyu_QuestBaseData CurrentQuest => currentQuest;

    /// <summary>
    /// ����Ʈ ����Ʈ �ʱ� ũ�� ���� �����ͻ󿡼� ����
    /// </summary>
    [SerializeField]
    int questCapasity = 4;

    /// <summary>
    /// �̿��Ǿ��� �������ִ� ����Ʈ ����Ʈ
    /// </summary>
    protected List<Gyu_QuestBaseData> ownQuestList;
    public List<Gyu_QuestBaseData> OwnQuestList => ownQuestList;
    /// <summary>
    /// ���Ǿ��� ���� ��ȭ����
    /// </summary>
    protected TalkType talkType;
    public TalkType TalkType => talkType;
    
    /// <summary>
    /// ��ȭ ���൵ 
    /// </summary>
    protected int talkDepth = 0;
    public int TalkDepth => talkDepth;



    /// <summary>
    /// ��ó���ͼ� ��ȭ�����ϵ��� �����Ű�� ��������Ʈ
    /// </summary>
    public Action<int> onTalkEnableButton;

    /// <summary>
    /// �ֺ��� ����� ��ȣ���� ��������Ʈ
    /// </summary>
    public Action onTalkDisableButton;


    /// <summary>
    /// NPC ���� ���� ī�޶� 
    /// </summary>
    Camera npcCharcterCamera;

    /// <summary>
    /// UI rawImage �� ������ ������ �ؽ��� 
    /// </summary>
    public RenderTexture GetTexture => npcCharcterCamera.targetTexture;
    private void Awake()
    {
        npcCharcterCamera = transform.GetComponentInChildren<Camera>();
        npcCharcterCamera.targetTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
    }

    /// <summary>
    /// ���Ǿ��� ��� �־���� ������ 
    /// </summary>
    /// <param name="npcIndex">���Ǿ� ��ȣ�ο��� ��</param>
    /// <param name="mainStoryQuestArray">���� ����Ʈ</param>
    /// <param name="killcountQuestArray">��� ����Ʈ</param>
    /// <param name="gatheringQuestArray">���� ����Ʈ</param>
    public void InitData(int npcIndex , Gyu_QuestBaseData[] mainStoryQuestArray, Gyu_QuestBaseData[] killcountQuestArray, Gyu_QuestBaseData[] gatheringQuestArray) 
    {
        npcId = npcIndex;
        ownQuestList = new(questCapasity);
        SetMainStoryQuest(mainStoryQuestArray);
        SetKillcountQuest(killcountQuestArray);
        SetGatheringQuest(gatheringQuestArray);
    }

    /// <summary>
    /// �� ���Ǿ��� �������ִ� ���ν��丮 ���ÿ� �Լ� 
    /// </summary>
    /// <param name="mainStoryQuestArray">���ӻ� �����ϴ� ���ν��丮 �迭</param>
    protected virtual void SetMainStoryQuest(Gyu_QuestBaseData[] mainStoryQuestArray)
    {

    }
    /// <summary>
    /// �� ���Ǿ��� �������ִ� ��� ����Ʈ ���ÿ� �Լ�
    /// </summary>
    /// <param name="killcountQuestArray">���ӻ� �����ϴ� ��� ����Ʈ �迭</param>
    protected virtual void SetKillcountQuest(Gyu_QuestBaseData[] killcountQuestArray)
    {

    }
    /// <summary>
    /// �� ���Ǿ��� ������ �ִ� ���� ����Ʈ ���ÿ� �Լ� 
    /// </summary>
    /// <param name="gatheringQuestArray">���ӻ� �����ϴ� ���� ����Ʈ �迭</param>
    protected virtual void SetGatheringQuest(Gyu_QuestBaseData[] gatheringQuestArray)
    {

    }

    // ����Ʈ ����Ʈ�߿� ����� �������� �������� ��ɵ� �ʿ� 
    // Ư�� �����Ȳ������ �Ҽ��ִ� ����Ʈ ����� 
    // �Ϸ�� ����Ʈ ���� �����Ѵٴ��� 
    // ����Ʈ���� �ִµ� ���̻� �����Ҽ����� ����Ʈ �����  ��� ������������ ����


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
