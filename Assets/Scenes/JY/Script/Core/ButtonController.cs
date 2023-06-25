using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : TestBase 
{
    public void Open_Inventory_Button() { GameManager.Inventory.Open_Inventory(); }
    public void Add_Slot_Button() { GameManager.UI_Spawner.Add_Slot(); }
    public void Equip_Button() { GameManager.Inventory.SwitchTab_To_Equip(); }
    public void Consume_Button() { GameManager.Inventory.SwitchTab_To_Consume(); }
    public void Etc_Button() { GameManager.Inventory.SwitchTab_To_Etc(); }
    public void Craft_Button() { GameManager.Inventory.SwitchTab_To_Craft(); }
    protected override void Test1(InputAction.CallbackContext obj)
    {
        
    }
}
