using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_UI_Item : Base_PoolObj
{
    [SerializeField]
    uint slotItemCount;
    [SerializeField]
    ItemData slotItemData;



    public void InitData(ItemData itemData, uint itemCount) 
    {
        slotItemCount = itemCount;
        slotItemData = itemData;
    }

    public void ResetData()
    {
        slotItemCount = 0;
        slotItemData = null;
        transform.SetParent(poolTransform);
        gameObject.SetActive(false);
    }
}
