using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuickSlotList
{
    Shift = 0,
    Eight,
    Nine,
    Zero,
    Ctrl,
    Alt,
    Space,
    Insert
}
public class QuickSlots : MonoBehaviour
{
    QuickSlot[] quickSlots = null;
    public QuickSlot this[QuickSlotList number] => quickSlots[(int) number];
    private void Start()
    {
        Init();
    }
    void Init()
    {
        quickSlots = new QuickSlot[transform.childCount];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
        }
    }
}
