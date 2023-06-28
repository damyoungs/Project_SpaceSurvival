using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Object_Spawner : Singleton<UI_Object_Spawner>
{
    public GameObject Slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;

    private int slotMax;
  
    private int currentSlotCount;
    public int CurrentSlotCount
    {
        get => currentSlotCount;
        set
        {
            currentSlotCount= value;
            AddSlot(currentSlotCount);
        }
    }
    private void Start()
    {
        CurrentSlotCount = 4;
    }
    void AddSlot(int currentSlotCount)
    {
        for (int i = 0; i < currentSlotCount; i++)
        {
            GameObject slot = Instantiate(Slot);
            slot.transform.SetParent(equip_Below, false);

        }
        Canvas.ForceUpdateCanvases();

    }
}
