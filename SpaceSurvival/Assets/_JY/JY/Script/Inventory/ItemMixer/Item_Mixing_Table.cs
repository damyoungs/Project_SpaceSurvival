using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;


public class Item_Mixing_Table : MonoBehaviour
{
    Dictionary<ItemCode, ItemCode[]> combinationTable = new Dictionary<ItemCode, ItemCode[]>();
    private void Start()
    {
        Init();
    }
    void Init()
    {
        combinationTable.Add(ItemCode.Intermidiate_Blue_Crystal, new ItemCode[2] { ItemCode.BlueCrystal, ItemCode.BlueCrystal });
        combinationTable.Add(ItemCode.Intermidiate_Dark_Crystal, new ItemCode[2] { ItemCode.DarkCrystal, ItemCode.DarkCrystal });
        combinationTable.Add(ItemCode.Intermidiate_Green_Crystal, new ItemCode[2] { ItemCode.Green_Crystal, ItemCode.Green_Crystal});
    }
    public bool ValidData(ItemCode leftCode, ItemCode middleCode, out ItemCode resultCode)
    {
        bool result = false;
        resultCode = ItemCode.Advanced_Dark_Crystal;//의미없음 null 이 안되기 때문에 임의데이터 부여 
        foreach (KeyValuePair<ItemCode, ItemCode[]> entry in combinationTable)
        {
            ItemCode matchedCode = entry.Key;
            ItemCode[] ingredients = entry.Value;
            //조합테이블의 코드와 파라미터로 받은 코드가 순서에 관계없이 일치하다면 
            if (leftCode == ingredients[0] && middleCode == ingredients[1] || leftCode == ingredients[1] && middleCode == ingredients[0])
            {
                result = true;
                resultCode = matchedCode;
                break;
            }
        }
        return result;
    }
}
