using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Object_Spawner : Singleton<UI_Object_Spawner>
{
    public GameObject slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform etc2_Below;

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
        AddSlot();
    }
    public void AddSlot()
    {
        CurrentSlotCount += 5;
        for (int i = 0; i < currentSlotCount; i++)
        {
            slot = Instantiate(slot);
            slot.name = $"{slot.name}_{i}";
            switch (state)
            {
                case Current_Inventory_State.Equip:
                    slot.transform.SetParent(equip_Below, false);
                    break;
                case Current_Inventory_State.Consume:
                    slot.transform.SetParent(consume_Below, false);
                    break;
                case Current_Inventory_State.Etc:
                    slot.transform.SetParent(etc_Below, false);
                    break;
                case Current_Inventory_State.Etc2:
                    slot.transform.SetParent(etc2_Below, false);
                    break;
            }
        }   
        Canvas.ForceUpdateCanvases();
    }
}
