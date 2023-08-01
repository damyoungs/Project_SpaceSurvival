using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Enhancable", menuName = "Scriptable Object/Item Data/ItemData - Enhancable", order = 3)]
public class ItemData_Enhancable : ItemData, IEnhancable
{
    public bool TryEnhance()
    {
        bool result = false;
        return result;
    }
}
