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
public class ItemBase : MonoBehaviour
{
    public static ItemType ItemType;

    protected virtual void OnEnable()
    {
        InitializeValue();
    }
    protected virtual void InitializeValue()
    {

    }
}
