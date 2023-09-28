using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestReward;

/// <summary>
/// ���� ���� ����� �̳Ѱ�
/// </summary>
public enum Merchant_Selected
{
    Buy,
    Sell,

}
public class Merchant_Manager : MonoBehaviour
{
    /// <summary>
    /// ��ȭ �������� üũ�ϱ�
    /// </summary>
    public bool isTalking = false;


    /// <summary>
    /// �������� â�� �����ִ� NPC �ε���
    /// </summary>
    int currentNpcIndex = 0;
    public int CurrentNpcIndex => currentNpcIndex;


    /// <summary>
    /// ���Ǿ� ��ȭ��� �����ϴ� Ŭ���� 
    /// </summary>
    TalkData_Gyu talkData;
    public TalkData_Gyu TalkData => talkData;


    /// <summary>
    /// �������� �Ǹ����� ���� ��
    /// </summary>
    Merchant_Selected selected;
    public Merchant_Selected Selected
    {
        get => selected;
        set
        {
            if (selected != value)
            {
                selected = value;
            }
        }
    }

    Current_Inventory_State merchant_State;
    public Current_Inventory_State Merchant_State 
    {
        get => merchant_State;
        set 
        {
            if (merchant_State != value)
            {
                merchant_State = value;
                switch (merchant_State)
                {
                    case Current_Inventory_State.None:
                        break;
                    case Current_Inventory_State.Equip:
                        break;
                    case Current_Inventory_State.Consume:
                        break;
                    case Current_Inventory_State.Etc:
                        break;
                    case Current_Inventory_State.Craft:
                        break;
                    default:
                        break;
                }
            }
        }
    }


    /// <summary>
    /// ������ �Ǹ����� �����۸�� 
    /// </summary>
    [SerializeField]
    ItemData[] merchantItemArray;

    /// <summary>
    /// ����ǸŸ��
    /// </summary>
    List<ItemData> equipItems;
    public List<ItemData> EquipItems => equipItems;
    /// <summary>
    /// ����ǸŸ���� ����
    /// </summary>
    int[] equipItemsCount; 
    public int[] EquipItemsCount => equipItemsCount;

    /// <summary>
    /// �Һ��ǸŸ��
    /// </summary>
    List<ItemData> consumeItems;
    public List<ItemData> ConsumeItems => consumeItems;
    /// <summary>
    /// �Һ� �ǸŸ���� ����
    /// </summary>
    int[] consumeItemsCount;
    public int[] ConsumeItemsCount => consumeItemsCount;

    /// <summary>
    /// ��Ÿ �ǸŸ��
    /// </summary>
    List<ItemData> etcItems;
    public List<ItemData> EtcItems => etcItems;
    /// <summary>
    /// ��Ÿ �ǸŸ���� ����
    /// </summary>
    int[] etcItemsCount;
    public int[] EtcItemsCount => etcItemsCount;

    /// <summary>
    /// ���� �ǸŸ�� 
    /// </summary>
    List<ItemData> craftItems;
    public List<ItemData> CraftItems => craftItems;
    /// <summary>
    /// �����ǸŸ���� ���� 
    /// </summary>
    int[] craftItemsCount;
    public int[] CraftItemsCount => craftItemsCount;


    /// <summary>
    /// ������ �ش繰ǰ�� �󸶳� �ΰ� Ȥ�� ��ΰ� �������������� �������� ������ �迭
    /// merchantItemArray �� �ε����� ���� ������ ���� 
    /// </summary>
    //int[] merchantItemCoinValue;
    //public int[] MerchantItemCoinValue => merchantItemCoinValue;


    /// <summary>
    /// ������ ������ ������ ��� 
    /// �߸��Ǹ��Ѱ� �ٽû�¿뵵�� ��� 
    /// </summary>
    List<ItemData> sellsItemArray;
    public List<ItemData> SellsItemArray => sellsItemArray;


    /// <summary>
    /// �籸�� �� ������ ���� 
    /// sellsItemArray �� �ε����� ���� ������ �����̴�.
    /// </summary>
    int[] merchantSellCountArray;
    public int[] MerchantSellCountArray => merchantSellCountArray;


    MerchantNPC[] merchantNPCs;
    

    Merchant_UI_Manager merchant_UI_Manager;

    NpcTalkController talkController;

    private void Awake()
    {
        int capacity = (int)(merchantItemArray.Length * 0.25f);

        merchant_UI_Manager = GetComponent<Merchant_UI_Manager>();
        talkController = FindObjectOfType<NpcTalkController>();
        equipItems = new List<ItemData>(capacity);
        consumeItems = new List<ItemData>(capacity);
        etcItems = new List<ItemData>(capacity);
        craftItems = new List<ItemData>(capacity);


        foreach (ItemData item in merchantItemArray)
        {
            switch (item.ItemType)
            {
                case ItemType.Equip:
                    equipItems.Add(item);
                    break;
                case ItemType.Consume:
                    consumeItems.Add(item);
                    break;
                case ItemType.Etc:
                    etcItems.Add(item);
                    break;
                case ItemType.Craft:
                    craftItems.Add(item);
                    break;
            }
        }
        equipItemsCount = new int[equipItems.Count];
        int forSize = equipItemsCount.Length;
        for (int i = 0; i < forSize; i++)
        {
            equipItemsCount[i] = 100;
        }
        consumeItemsCount = new int[consumeItems.Count];
        forSize = consumeItemsCount.Length;
        for (int i = 0; i < forSize; i++)
        {
            consumeItemsCount[i] = 100;
        }

        etcItemsCount= new int[etcItems.Count];
        forSize = etcItemsCount.Length;
        for (int i = 0; i < forSize; i++)
        {
            etcItemsCount[i] = 100;
        }

        craftItemsCount = new int[craftItems.Count];
        forSize = craftItemsCount.Length;
        for (int i = 0; i < forSize; i++)
        {
            craftItemsCount[i] = 100;
        }

    }

    private void Start()
    {
        merchantNPCs = FindObjectsOfType<MerchantNPC>(true);
        InputSystemController.InputSystem.Player.Action.performed += (_) => {
            if (isTalking)
            {
                isTalking = false;
                talkController.Talk(0);
            }
        };
    }

    /// <summary>
    /// �������� �ǸŹ� ��� ������ ������ ó���Ǵ� �Լ�
    /// </summary>
    /// <param name="item">ó���� ������</param>
    /// <param name="itemCount">ó���� ����</param>
    public void ItemClick(ItemData item, uint itemCount,Slot slot = null) 
    {
        if (selected == Merchant_Selected.Buy)
        {
            for (int i = 0; i < itemCount; i++)
            {
                GameManager.SlotManager.AddItem(item.code);
            }
        } 
        else if (selected == Merchant_Selected.Sell) 
        {
            if (slot != null)
            {
                GameManager.SlotManager.RemoveItem(slot.ItemData, slot.Index, itemCount);
                merchant_UI_Manager.ReFresh_Merchant_Item();
                return;
            }
            Debug.Log("������ �־���? �Ĵµ� ");
        }
    }



    public void InitDataSetting()
    {

        // ���丮�� �ҽ� ���Ǿ� ��ġ�� � �ĺ������ΰ� �������� �����Ű�°� �������ҰŰ���.
        // �ʱ�ȭ �ϴ°��� ���⸻�� �ٸ������� ���� �ؾߵɰŰ��� .. ���丮 �� ������Ų�ڿ� �����ִ��ϸ� �ɰŰ����ѵ�.. 
        MerchantNPC[] array_NPC = FindObjectsOfType<MerchantNPC>(true);   //�����ִ� ���Ǿ� ã�Ƽ� ��Ƶΰ� ( ã�� ������ �ٲ���������� �ٸ������ ã�ƺ���.)
        for (int i = 0; i < array_NPC.Length; i++)
        {
            //��ġ�� ����� �����Ű�� �ɰŰ��⵵�ѵ�.. �ϴ� �������غ���..
            array_NPC[i].InitData(i); //npc �� �ʱ�ȭ ��Ų��.
            array_NPC[i].onTalkDisableButton += () =>
            {
                isTalking = false;
                talkController.ResetData();
                talkController.openTalkWindow = null;
                talkController.closeTalkWindow = null;
                talkController.onTalkClick = null;
                talkController.getTalkDataArray = null;
                talkController.LogManager.getLogTalkDataArray = null;
            };
            array_NPC[i].onTalkEnableButton = (npcId) =>
            {
                Cursor.lockState = CursorLockMode.None;
                talkController.ResetData();
                talkController.openTalkWindow = merchant_UI_Manager.OpenWindow;
                talkController.closeTalkWindow = merchant_UI_Manager.CloseWindow;
                currentNpcIndex = npcId;
                talkController.onTalkClick = () => array_NPC[currentNpcIndex];

                talkController.getTalkDataArray = (talkIndex) =>
                {
                    return talkController.TalkData.GetTalk(array_NPC[currentNpcIndex].TalkType, talkIndex);
                };
                talkController.LogManager.getLogTalkDataArray = (talkIndex) => {
                    return talkController.TalkData.GetLog(array_NPC[currentNpcIndex].TalkType, talkIndex);
                };
                isTalking = true;
            };

           
        }
    }

}