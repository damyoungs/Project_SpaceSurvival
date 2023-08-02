using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnhancerState
{
    Open,
    SetItem,
    WaitforResult,
    Success,
    Fail,
    ClearItem,
    Close
}
public class Item_Enhancer : MonoBehaviour
{
    Item_Enhancer_UI item_Enhancer_UI;
    Item_Enhancer_Data item_Enhancer_Data;

    public Item_Enhancer_UI EnhancerUI => item_Enhancer_UI;
    public Item_Enhancer_Data EnhancerData => item_Enhancer_Data;

    EnhancerState enhancerState;
    public EnhancerState EnhancerState
    {
        get => enhancerState;
        set
        {
            if (enhancerState != value)
            {
                enhancerState = value;
            }
            switch (enhancerState)
            {
                case EnhancerState.Open:
                    break;
                case EnhancerState.SetItem:
                    break;
                case EnhancerState.WaitforResult:
                    break;
                case EnhancerState.Success:
                    break;
                case EnhancerState.Fail:
                    break;
                case EnhancerState.ClearItem:
                    break;
                case EnhancerState.Close:
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        item_Enhancer_UI = GetComponent<Item_Enhancer_UI>();
        item_Enhancer_Data = new Item_Enhancer_Data();
    }

}
