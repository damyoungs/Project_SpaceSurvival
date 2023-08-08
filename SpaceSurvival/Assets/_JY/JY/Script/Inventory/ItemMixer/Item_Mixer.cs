using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum ItemMixerState// 강화도중 창 닫기 비활성화
{
    Open,
    SetItem,
    Confirm,
    WaitforResult,
    Success,
    Fail,
    ClearItem,
    Close
}
public class Item_Mixer : MonoBehaviour
{
    Item_Mixer_UI item_Mixer_UI;

    public Item_Mixer_UI MixerUI => item_Mixer_UI;

    public Action onOpen;
    public Action<ItemData> onSetItem;
    public Action onWaitforResult;
    public Action onSuccess;
    public Action onFail;
    public Action onClearItem;
    public Action onClose;
    public Action onConfirmButtonClick;

    Mixer_Slot_Left left_Slot;
    Mixer_Slot_Middle middle_Slot;

    ItemData leftSlotData = null;
    ItemData middleSlotData = null;

    public Action<ItemData> onLeftSlotDataChange;
    public Action<ItemData> onMiddleSlotDataChange;
    public ItemData LeftSLotData
    {
        get => leftSlotData;
        set
        {
            if (leftSlotData != value)
            {
                leftSlotData = value;
                onLeftSlotDataChange?.Invoke(leftSlotData);
            }
        }
    }
    public ItemData MiddleSlotData
    {
        get => middleSlotData;
        set
        {
            if (middleSlotData != value)
            {
                middleSlotData = value;
                onMiddleSlotDataChange?.Invoke(middleSlotData);
            }
        }
    }
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
                //    onSetItem?.Invoke(ItemData);
                    break;
                case ItemMixerState.Confirm:
                    //if (ItemData != null)
                    //{
                    //    onConfirmButtonClick?.Invoke();
                    //}
                    break;
                case ItemMixerState.WaitforResult:
                    onWaitforResult?.Invoke();
                    break;
                case ItemMixerState.Success:
                    onSuccess?.Invoke(); // inventory에 Itemdata 리턴하고 EnhancerUI Clear
                    break;
                case ItemMixerState.Fail:
                    onFail?.Invoke(); // inventory에 Itemdata 리턴하고 EnhancerUI Clear
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

        left_Slot.onClearLeftSlot += () => LeftSLotData = null;
        middle_Slot.onClearMiddleSlot += () => MiddleSlotData = null;
    }


    //
}
