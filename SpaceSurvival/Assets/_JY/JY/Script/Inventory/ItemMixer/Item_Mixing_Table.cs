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
        resultCode = ItemCode.Advanced_Dark_Crystal;//�ǹ̾��� null �� �ȵǱ� ������ ���ǵ����� �ο� 
        foreach (KeyValuePair<ItemCode, ItemCode[]> entry in combinationTable)
        {
            ItemCode matchedCode = entry.Key;
            ItemCode[] ingredients = entry.Value;
            //�������̺��� �ڵ�� �Ķ���ͷ� ���� �ڵ尡 ������ ������� ��ġ�ϴٸ� 
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
