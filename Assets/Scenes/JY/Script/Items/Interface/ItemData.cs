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
public interface I_ItemData //굳이 인터페이스를 만들필요 없어보임
{
    ItemType Type { get; }
    string Name { get; set; }
    bool Is_Stackable { get; }
}
