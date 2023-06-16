using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Equip,
    Consume,
    Etc,
    Craft
}
public class Item_Base : MonoBehaviour
{
    public static ItemType itemType;
    

}
