using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcTalkController : MonoBehaviour
{

    /// <summary>
    /// ���Ǿ� ��ȭ��� �����ϴ� Ŭ���� 
    /// </summary>
    TalkData_Gyu talkData;
    public TalkData_Gyu TalkData => talkData;



    /// <summary>
    /// ��ȭ �ڷ�ƾ �ߺ�üũ�� 
    /// </summary>
    bool isTalking = false;

    /// <summary>
    /// ���� ��ȭ���� 
    /// </summary>
    int currentTalkIndex = 0;




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
    /// ��ȭ���� ��ư
    /// </summary>
    Button talkEndButton;
    public Button TalkEndButton => talkEndButton;

    
    /// <summary>
    /// ���� ���� �ѱ�� ��ư
    /// </summary>
    Button nextButton;

    /// <summary>
    /// �α� ��� ��ư 
    /// </summary>
    Button logButton;





    public Action openTalkWindow;
    public Action closeTalkWindow;


    /// <summary>
    /// Func�� �����Ϳ���Ÿ�̹� ���߱⿡�� �������� �⺻������ null üũ�� �ؾߵǰ� �Լ�ȣ��� �о���°��̶�
    /// �������ۿ�����.
    /// </summary>
    public Func<NpcBase_Gyu> onTalkClick;

    /// <summary>
    /// ��ȭ ������ �����´�.
    /// </summary>
    public Func<int, string[]> getTalkDataArray;





    /// <summary>
    /// �α� �������� ������Ʈ
    /// </summary>
    LogManager logManager;
    public LogManager LogManager => logManager;


    private void Awake()
    {
        logManager = FindObjectOfType<LogManager>(true);
        talkData = FindObjectOfType<TalkData_Gyu>();

        npcImg = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        nameBox = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        talkBox = transform.GetChild(2).GetComponent<TextMeshProUGUI>();



        talkEndButton = transform.GetChild(3).GetComponent<Button>();
        talkEndButton.onClick.AddListener(ResetData);


        nextButton = transform.GetChild(4).GetComponent<Button>();
        nextButton.onClick.AddListener(() => Talk(currentTalkIndex + 1));


        logButton = transform.GetChild(5).GetComponent<Button>();
        logButton.onClick.AddListener(() => {
            logManager.LogBoxSetting(currentTalkIndex);
        });


       
    }
    private void Start()
    {
        ResetData();
    }
    public void ReTalking() 
    {
        isTalking = false;
        currentTalkIndex = 0;
        StopAllCoroutines();
        Talk(currentTalkIndex);
    }
    /// <summary>
    /// Npc ��縦 ���
    /// </summary>
    public void Talk(int talkIndex)
    {
        if (!isTalking)
        {
            gameObject.SetActive(true);
            openTalkWindow?.Invoke();
            NpcBase_Gyu npc = onTalkClick?.Invoke();
            npcImg.texture = npc.GetTexture;
            if (npc != null)
            {
                nameBox.text = npc.name;
                string[] talkString = getTalkDataArray?.Invoke(talkIndex);
                if (talkString != null && talkString.Length > 0)
                {

                    StartCoroutine(Typing(talkString, talkIndex));
                }
            }

            //OpenWindow(npc.);
        }
    }

    /// <summary>
    /// Ÿ���� ȿ�����ִ� �ؽ�Ʈ ��� �ϱ�
    /// </summary>
    /// <param name="textArray"></param>
    /// <returns></returns>
    IEnumerator Typing(string[] textArray, int talkIndex)
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
        currentTalkIndex = talkIndex;
        isTalking = false;
    }

    public void ResetData() 
    {
        closeTalkWindow?.Invoke();
        currentTalkIndex = 0;
        npcImg.texture = null;
        nameBox.text = "";
        talkBox.text = "";
        logManager.ResetData();
        gameObject.SetActive(false);
    }
}
