using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIobjectSpawner : Singleton<UIobjectSpawner>
{
    public GameObject slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform etc2_Below;

  
    private int slotMax;

    public Dictionary<Current_Inventory_State, List<GameObject>> slots;
  
    private int currentSlotCount = 0;
    public int CurrentSlotCount
    {
        get => currentSlotCount;
        set
        {
            currentSlotCount= value;
        }
    }
    public void InitializeSlot()
    {
        slots = new Dictionary<Current_Inventory_State, List<GameObject>>
        {
            { Current_Inventory_State.Equip, new List<GameObject>() },
            { Current_Inventory_State.Consume, new List<GameObject>() },
            { Current_Inventory_State.Etc, new List<GameObject>() },
            { Current_Inventory_State.Etc2, new List<GameObject>() }
        };

        CreateSlot(Current_Inventory_State.Equip, equip_Below, 10);
        CreateSlot(Current_Inventory_State.Consume, consume_Below, 10);
        CreateSlot(Current_Inventory_State.Etc, etc_Below, 10);
        CreateSlot(Current_Inventory_State.Etc2, etc2_Below, 10);
    }
    public void CreateSlot(Current_Inventory_State state, Transform parentTransform, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newSlot = Instantiate(slot);
            newSlot.name = $"Slot_{state}_{i}";
            newSlot.transform.SetParent(parentTransform, false);

            slots[state].Add(newSlot);
        }
    }
    public void CreateSlot_OnBtn()
    {

        for (int i = 0; i < 5; i++)
        {
            GameObject newSlot = Instantiate(slot);
            Transform parentTransform;
            switch (Inventory.state)
            {
                case Current_Inventory_State.Equip:
                    parentTransform = equip_Below;
                    break;
                case Current_Inventory_State.Consume:
                    parentTransform = consume_Below;
                    break;
                case Current_Inventory_State.Etc:
                    parentTransform = etc_Below;
                    break;
                case Current_Inventory_State.Etc2:
                    parentTransform = etc2_Below;
                    break;
                default:
                    parentTransform = null;
                    break;
            }
            if (parentTransform != null)
            {
                newSlot.name = $"{Inventory.state}_{i}";
                newSlot.transform.SetParent (parentTransform, false);
            }
        }
    }

}
