using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Quest_UI_Colum : MonoBehaviour ,IPointerClickHandler
{
    /// <summary>
    /// ����Ʈ������ ���� ����
    /// </summary>
    [SerializeField]
    Quest_State quest_State = Quest_State.None;
    public Quest_State Quest_State 
    {
        get => quest_State;
        set 
        {
            if (quest_State != value)
            {
                quest_State = value;
                Set_State_UI(quest_State);
            }
        }
    }
    [SerializeField]
    bool isSelectedCheck = false;
    public bool IsSelectedCheck
    {
        get => isSelectedCheck;
        set 
        {
            isSelectedCheck = value;
            if (isSelectedCheck)
            {
                iconBackGroundImg.color = selected_Colum_Color;
            }
            else 
            {
                Set_State_UI(quest_State);
            }
        }
    }


    /// <summary>
    /// ����Ʈ ������ UI ��׶��� �̹���
    /// </summary>
    [SerializeField]
    Image iconBackGroundImg;

    /// <summary>
    /// ����Ʈ UI ��׶��� �̹���
    /// </summary>
    [SerializeField]
    Image backGroundImg;
    
    /// <summary>
    /// ���丮 �����϶� ���� 
    /// </summary>
    [SerializeField]
    Color stroyType;
    
    /// <summary>
    /// ��� ����Ʈ �϶��� ����
    /// </summary>
    [SerializeField]
    Color killCountType;
    
    /// <summary>
    /// ���� ����Ʈ �϶��� ����
    /// </summary>
    [SerializeField]
    Color gatheringType;



    /// <summary>
    /// ����Ʈ �⺻ �����϶��� ����
    /// </summary>
    [SerializeField]
    Color quest_None_Color;

    /// <summary>
    /// ����Ʈ ��ŸƮ �����϶��� ����
    /// </summary>
    [SerializeField]
    Color quest_Start_Color;

    /// <summary>
    /// ����Ʈ �������� ���� ���� 
    /// </summary>
    [SerializeField]
    Color quest_Cancel_Color;

    /// <summary>
    /// ����Ʈ Ŭ���� ������ ����
    /// </summary>
    [SerializeField]
    Color quest_Complete_Color;

    /// <summary>
    /// ������ ���������� ����
    /// </summary>
    [SerializeField]
    Color selected_Colum_Color;

    /// <summary>
    /// �ش�UI �� �������ִ� ����Ʈ ����  �ϴ� ���߿� ����Ҽ����־ �����س���.
    /// </summary>
    Gyu_QuestBaseData thisQuestData;
    Gyu_QuestBaseData ThisQuestData 
    {
        get => thisQuestData;
        set 
        {
            if (value != null) 
            {
                thisQuestData = value;
                Quest_State = thisQuestData.Quest_State;
                Set_Type_UI(ThisQuestData.QuestType);
            }
        }
    }

    /// <summary>
    /// ����Ʈ ������ 
    /// </summary>
    [SerializeField]
    Image iconImg;

    /// <summary>
    /// ����Ʈ �̸�
    /// </summary>
    [SerializeField]
    TextMeshProUGUI questTitle;

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    [SerializeField]
    TextMeshProUGUI rewardItemCount;
    [SerializeField]
    Image rewardItemIcon;
    /// <summary>
    /// ���� ��ȭ ����
    /// </summary>
    [SerializeField]
    TextMeshProUGUI rewardCoinCount;
    [SerializeField]
    Image rewardCoinIcon;

    /// <summary>
    /// ����Ʈ Ŭ���� �̺�Ʈ ���ۿ� ��������Ʈ
    /// </summary>
    public Action<Gyu_QuestBaseData,Quest_UI_Colum> onClick;

    ItemDataManager testDataManager;

    private void Awake()
    {
        testDataManager = FindObjectOfType<ItemDataManager>();  
        backGroundImg = GetComponent<Image>();
        iconBackGroundImg = transform.GetChild(0).GetComponent<Image>();
      
        Transform child = transform.GetChild(0);
        iconImg = child.GetChild(0).GetComponent<Image>();

        child = transform.GetChild(1);    
        questTitle = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        rewardItemCount = child.GetChild(1).GetComponentInChildren<TextMeshProUGUI>(true);
        rewardItemIcon  = child.GetChild(1).GetChild(0).GetComponent<Image>();


        rewardCoinCount = child.GetChild(2).GetComponentInChildren<TextMeshProUGUI>(true);
        rewardCoinIcon  = child.GetChild(2).GetChild(0).GetComponent<Image>();
        ResetData();
    }

    /// <summary>
    /// ������ ���ÿ� �Լ� 
    /// </summary>
    /// <param name="questData">����Ʈ ������ </param>
    public void SetData(Gyu_QuestBaseData questData) 
    {
        ThisQuestData = questData;
        iconImg.sprite = questData.IconImage;
        questTitle.text = $"{questData.Title}";
        if (questData.RewardItem.Length > 1) //����������� �Ѱ��̻��̸�  
        {
            rewardItemCount.text = $"�� {questData.RewardItem.Length}��";
        }
        else 
        {
            rewardItemCount.text = $"{questData.ItemCount[0]} ��";
        }
        //rewardItemIcon.sprite = GameManager.Itemdata.itemDatas[(int)questData.RewardItem[0]].itemIcon; //�����ۼ����� �̳Ѽ����������� �׳� ��Ʈ�� �����ؼ� ��� 
        rewardItemIcon.sprite = testDataManager.itemDatas[(int)questData.RewardItem[0]].itemIcon; //�����ۼ����� �̳Ѽ����������� �׳� ��Ʈ�� �����ؼ� ��� 

        //rewardCoinIcon.sprite = GameManager.Itemdata.itemDatas[0].itemIcon; //0���� �����̴� ���ξ����� �����´� 
        rewardCoinIcon.sprite = testDataManager.itemDatas[0].itemIcon; //0���� �����̴� ���ξ����� �����´� 
        rewardCoinCount.text = $"{questData.RewardCoin} G";

        gameObject.SetActive(true); //���� �������� Ȱ��ȭ���Ѽ� �����ش� 
    }

    /// <summary>
    /// ����Ʈ ���¿����� ȭ�鿡 ������ �̹������������
    /// </summary>
    /// <param name="state">����Ʈ ����</param>
    private void Set_State_UI(Quest_State state) 
    {
        switch (state)
        {
            case Quest_State.None:
                iconBackGroundImg.color = quest_None_Color;
                break;
            case Quest_State.Quest_Start:
                iconBackGroundImg.color = quest_Start_Color;
                break;
            //case Quest_State.Quest_Cancel:
            //    iconBackGroundImg.color = quest_Cancel_Color;
                //break;
            case Quest_State.Quest_Complete:
                iconBackGroundImg.color = quest_Complete_Color;
                break;
            default:
                break;
        }
    }

    private void Set_Type_UI(QuestType type) 
    {
        switch (type)
        {
            case QuestType.Killcount:
                backGroundImg.color = killCountType;
                break;
            case QuestType.Gathering:
                backGroundImg.color = gatheringType;
                break;
            case QuestType.Story:
                backGroundImg.color = stroyType;
                break;
        }
    }

    /// <summary>
    /// UI ������ ���¿� �Լ�
    /// </summary>
    public void ResetData() 
    {
        thisQuestData = null;
        iconImg.sprite = null;
        questTitle.text = "";
        rewardCoinCount.text = "";
        rewardItemCount.text = "";

        onClick = null;

        gameObject.SetActive(false);
    }


    /// <summary>
    /// Ŭ���̺�Ʈ ���� 
    /// </summary>
    /// <param name="_"></param>
    public void OnPointerClick(PointerEventData _)
    {
        //Ŭ��
        onClick?.Invoke(thisQuestData,this);
        Debug.Log($"Ŭ���߾� ���´� :{quest_State} , ����Ʈ��  :{thisQuestData}");
    }
}
