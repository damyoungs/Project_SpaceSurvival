using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIobjectSpawner : MonoBehaviour
{
    public GameObject slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform etc2_Below;

  
    private int slotMax;

    public Dictionary<Current_Inventory_State, List<GameObject>> slots;
    public void InitializeSlot()
    {
        slots = new Dictionary<Current_Inventory_State, List<GameObject>>
        {
            { Current_Inventory_State.Equip, new List<GameObject>() },
            { Current_Inventory_State.Consume, new List<GameObject>() },
            { Current_Inventory_State.Etc, new List<GameObject>() },
            { Current_Inventory_State.Craft, new List<GameObject>() }
        };

        Add_Slot(15);
        Add_Slot(15);
        Add_Slot(15);
        Add_Slot(15);
    }
    //public void Initialize_Slot(Current_Inventory_State state, Transform parentTransform, int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        GameObject newSlot = Instantiate(slot);
    //        newSlot.name = $"Slot_{state}_{i}";
    //        newSlot.transform.SetParent(parentTransform, false);

    //        slots[state].Add(newSlot);
    //    }
    //}
    public void Add_Slot(int? additional_Value = null)
    {
        int? count = 5 + additional_Value;
        for (int i = 0; i < count; i++)
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
                case Current_Inventory_State.Craft:
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
                slots[Inventory.state].Add(newSlot);
            }
        }
    }

}
