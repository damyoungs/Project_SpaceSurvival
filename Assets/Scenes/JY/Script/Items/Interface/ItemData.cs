using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Equip,
    Consume,
    Etc,
    Craft
}
public interface I_ItemData 
{
    ItemType Type { get; }
    string Name { get; set; }
    bool Is_Stackable { get; }
}
