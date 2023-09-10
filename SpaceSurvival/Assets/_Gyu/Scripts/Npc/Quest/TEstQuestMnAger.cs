using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TEstQuestMnAger : MonoBehaviour
{
    public static TEstQuestMnAger instance;
    public TalkData talkData;   // 대사 모음
    public int Npcid;           // 고유 id

    // UI상태 체크
    private bool isTalk;
    private bool isNpcQuest;
    private bool isMyQuest;

    // 대화창 UI
    public GameObject[] Buttons;
    public GameObject TalkPanel;
    public TextMeshProUGUI NameBox;
    public TextMeshProUGUI TalkBox;

    // Npc 퀘스트창 UI
    public GameObject QuestPanel;
    public TextMeshProUGUI TitleBox;
    public TextMeshProUGUI DescriptionBox;
    public TextMeshProUGUI ClearBox;

    // Player 퀘스트창 UI
    public GameObject MyQuestPanel;
    public TextMeshProUGUI MyQeustBox;

    // Npc퀘스트
    public List<Quest> quests;

    // 캔버스 위치
    public GameObject CanvasLocation;

    private void Awake()
    {
        talkData = GameObject.FindAnyObjectByType<TalkData>();
        Buttons = new GameObject[3];

        CanvasLocation = GameObject.Find("Canvas");
        TalkPanel = CanvasLocation.transform.GetChild(0).gameObject;
        QuestPanel = CanvasLocation.transform.GetChild(1).gameObject;
        MyQuestPanel = CanvasLocation.transform.GetChild(2).gameObject;
        NameBox = TalkPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TalkBox = TalkPanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        DescriptionBox = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TitleBox = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        ClearBox = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[2];
        MyQeustBox = MyQuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        for (int i = 0; i < 3; i++)
            Buttons[i] = QuestPanel.GetComponentsInChildren<Button>()[i].gameObject;

        if (instance == null)
            instance = this;

        initialize();
        MyQuestPanel.SetActive(false);
    }

    public void Action()
    {
        Talk(TEstQuestMnAger.instance.Npcid);
    }

    public void Talk(int id)
    {
        if (!isTalk)
        {
            if (PlayerQuest.instance.myquest.questClear.clearType == QuestClear.ClearType.Comunication)
            {
                PlayerQuest.instance.myquest.isSucess = true;
            }

            NameBox.text = transform.name;
            isTalk = true;
            TalkPanel.SetActive(isTalk);
            string talkString = talkData.GetTalk(id);
            StartCoroutine(Typing(talkString));
        }
        else
        {
            initialize();
            TalkPanel.SetActive(isTalk);
        }
    }

    public void initialize()
    {
        isTalk = false;
        TalkPanel.SetActive(false);
        NameBox.text = null;
        TalkBox.text = null;

        isNpcQuest = false;
        QuestPanel.SetActive(false);
        TitleBox.text = null;
        DescriptionBox.text = null;
        ClearBox.text = null;
    }

    IEnumerator Typing(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            TalkBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
