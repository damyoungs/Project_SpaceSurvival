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

    ItemData itemData;

    public ItemData ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                //ClearState 대리자 
                if (itemData != null)
                {
                    mixerState = ItemMixerState.SetItem;
                }
                else
                {
                    mixerState = ItemMixerState.ClearItem;
                }
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
                    onSetItem?.Invoke(ItemData);
                    break;
                case ItemMixerState.Confirm:
                    if (ItemData != null)
                    {
                        onConfirmButtonClick?.Invoke();
                    }
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
    }
 

    //
}
