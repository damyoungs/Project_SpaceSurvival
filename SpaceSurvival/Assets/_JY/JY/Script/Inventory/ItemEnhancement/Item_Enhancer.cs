using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Enhancer : MonoBehaviour
{
    Item_Enhancer_UI item_Enhancer_UI;
    Item_Enhancer_Data item_Enhancer_Data;

    public Item_Enhancer_UI EnhancerUI => item_Enhancer_UI;
    public Item_Enhancer_Data EnhancerData => item_Enhancer_Data;

    private void Awake()
    {
        item_Enhancer_UI = GetComponent<Item_Enhancer_UI>();
        item_Enhancer_Data = new Item_Enhancer_Data();
    }

}
