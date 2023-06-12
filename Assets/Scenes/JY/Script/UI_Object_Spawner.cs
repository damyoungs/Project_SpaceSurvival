using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Object_Spawner : Singleton<UI_Object_Spawner>
{
    public GameObject Slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;

    public Current_Inventory_State state;
    private int slotMax;
  
    private int currentSlotCount = 0;
    public int CurrentSlotCount
    {
        get => currentSlotCount;
        set
        {
            currentSlotCount= value;
        }
    }
    private void Start()
    {
        AddSlot(state);
    }
    void AddSlot(Current_Inventory_State state)
    {
        CurrentSlotCount += 5;
        for (int i = 0; i < currentSlotCount; i++)
        {
            GameObject slot = Instantiate(Slot);
            slot.name = $"{slot.name}_{i}";
            slot.transform.SetParent(equip_Below, false);
        }
        Canvas.ForceUpdateCanvases();
    }
}
