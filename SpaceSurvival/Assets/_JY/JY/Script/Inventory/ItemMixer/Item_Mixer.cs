using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

 public enum ItemMixerState// ��ȭ���� â �ݱ� ��Ȱ��ȭ
{
    Open,
    SetItem,
    SetItemCanceled,
    Confirm,
    WaitforResult,
    Success,
    Fail,
    ClearItem,
    Close
}
public class Item_Mixer : MonoBehaviour
{
    Item_Mixing_Table mixing_table;
    public Item_Mixing_Table Mixing_Table => mixing_table;

    Item_Mixer_UI item_Mixer_UI;
    SlotManager slot_Manager;
    public Item_Mixer_UI MixerUI => item_Mixer_UI;

    public Action onOpen;
    public Action<ItemData> onSetItem;
    public Action onSetItemCanceled;
    public Action onWaitforResult;
    public Action onSuccess;
    public Action onFail;
    public Action onClearItem;
    public Action onClose;
    public Action onConfirmButtonClick;

    Mixer_Slot_Left left_Slot;
    Mixer_Slot_Middle middle_Slot;
    Mixer_Slot_Result result_Slot;
    public Mixer_Slot_Result ResultSlot => result_Slot;

    ItemData leftSlotData = null;
    ItemData middleSlotData = null;
    ItemData tempData = null;// ������ �����ϱ� �� slot�� �߰��� �������� �̸� �����س��� �뵵

    public Action<ItemData> onLeftSlotDataSet;
    public Action<ItemData> onMiddleSlotDataSet;

    //�ϴ� ���� �Ҵ��� �� �� ���ؼ� �ٸ��� �����ؾ���. �߰��� ������ �񱳸��ؼ� �Ϸ��� �ʱ⿡ �������� �߰��� �ȵǴ� ������ ����
    public ItemData LeftSlotData
    {
        get => leftSlotData;
        set
        {
            if (leftSlotData != value) 
            {
                tempData = leftSlotData;
                leftSlotData = value;
                CheckBothSlot();
                onLeftSlotDataSet?.Invoke(leftSlotData);// ���Ը޴������� �� ��������Ʈ�� �޾Ƽ� �������� ī��Ʈ�� ���ϰ� ������Ѵ�.null�̸� ���ϰ�  �ƴϸ� ���ְ�
                if (leftSlotData != null)
                {
                    StartCoroutine(LeftSlotDelay());
                }
                else
                {
                    slot_Manager.AddItem(tempData.code);
                }
            }
        }
    }
    IEnumerator LeftSlotDelay()
    {
        yield return new WaitForSeconds(0.01f);
        slot_Manager.RemoveItem(leftSlotData, slot_Manager.Index_JustChange_Slot);
    }
    IEnumerator MiddleSlotDelay()
    {
        yield return new WaitForSeconds(0.01f);
        slot_Manager.RemoveItem(middleSlotData, slot_Manager.Index_JustChange_Slot);
    }
    public ItemData MiddleSlotData
    {
        get => middleSlotData;
        set
        {
            if (middleSlotData != value)
            {
                tempData = middleSlotData;// �����Ͱ� null �� �Ǹ� �κ��丮 ���Կ� �������� �߰��ؾ��ϱ⶧���� null�� �Ǳ� �� �ӽ�����

                middleSlotData = value;
                CheckBothSlot();
                onMiddleSlotDataSet?.Invoke(middleSlotData);
                if (middleSlotData != null)
                {
                    StartCoroutine(MiddleSlotDelay());
                }
                else
                {
                    slot_Manager.AddItem(tempData.code);
                }
            }
        }
    }
    void CheckBothSlot()
    {
        if (leftSlotData != null && middleSlotData != null)//�� ���� ��� ���� �Ǿ��ٸ� 
        {// ���ո�Ͽ� �ִ��� Ȯ�� �ϴ� ���� �߰��ؾ���
            MixerState = ItemMixerState.SetItem;
        }
        else
        {
            MixerState = ItemMixerState.SetItemCanceled;
        }
    }
    //bool CompareSlotData()
    //{
    //    bool result = false;
    //    if (leftSlotData.ItemType)
    //    return result;
    //}
    ItemMixerState mixerState;
    public ItemMixerState MixerState
    {
        get => mixerState;
        set
        {
            if (mixerState != value)
            {
                mixerState = value;
            }
            switch (mixerState)
            {
                case ItemMixerState.Open:
                    onOpen?.Invoke();
                    break;
                case ItemMixerState.SetItem:
                    SetResultSlot();
                    onSetItem?.Invoke(result_Slot.ItemData);
                    break;
                case ItemMixerState.SetItemCanceled:
                    result_Slot.ItemData = null;
                    onSetItemCanceled?.Invoke();
                    break;
                case ItemMixerState.Confirm:
                    if (result_Slot.ItemData != null)//�������� ���õ� ��츸 �˾�
                    onConfirmButtonClick?.Invoke();
                    break;
                case ItemMixerState.WaitforResult:
                    onWaitforResult?.Invoke();
                    break;
                case ItemMixerState.Success:
                    onSuccess?.Invoke(); // inventory�� Itemdata �����ϰ� EnhancerUI Clear
                    break;
                case ItemMixerState.Fail:
                    onFail?.Invoke(); // inventory�� Itemdata �����ϰ� EnhancerUI Clear
                    break;
                case ItemMixerState.ClearItem:
                    onClearItem?.Invoke();
                    break;
                case ItemMixerState.Close:
                    onClose?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        item_Mixer_UI = GetComponent<Item_Mixer_UI>();
        MixerState = ItemMixerState.Close;
        left_Slot = GetComponentInChildren<Mixer_Slot_Left>();
        middle_Slot = GetComponentInChildren<Mixer_Slot_Middle>();
        result_Slot = GetComponentInChildren<Mixer_Slot_Result>();

        left_Slot.onClearLeftSlot += () => LeftSlotData = null;
        middle_Slot.onClearMiddleSlot += () => MiddleSlotData = null;
    }
    private void Start()
    {
        slot_Manager = GameManager.SlotManager;
        mixing_table = GameManager.Mixing_Table;
     
    }

    void SetResultSlot()
    {
        if (mixing_table.ValidData(leftSlotData.code, middleSlotData.code, out ItemCode resultCode))
        {
            result_Slot.ItemData = GameManager.Itemdata[resultCode];
        }
        else
        {
            Debug.Log("�Ұ����� �����Դϴ�.");
        }
    }
}
