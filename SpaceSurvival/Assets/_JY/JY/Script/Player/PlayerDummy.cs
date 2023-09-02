using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using Cinemachine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerDummy : MonoBehaviour, IHealth
{
    InputKeyMouse inputActions;

    ItemDescription itemDescription;
    EquipBox_Description EquipBox_Description;
    EquipBox equipBox;

    public Action onOpenInven;

    public float moveSpeed = 0.0f;
    public float rotateSpeed = 0.0f;
    public float pickupRange = 3.0f;

    public Action<ItemData> onEquipItem;
    public Action<ItemData> onUnEquipItem;
    public Action onClearSlot;



    
    uint darkForce = 500;
    public uint DarkForce
    {
        get => darkForce;
        set
        {
            darkForce = value;

        }
    }
    int money = 0;
    public int Money
    {
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                Debug.Log(money);
            }
        }
    }
    float hp = 200;
    public float HP
    {
        get => hp;
        private set
        {
            if (hp != value)
            {
                hp = value;
                Debug.Log($"플레이어 HP : {hp:f0}");
            }
        }
    }
    float mp = 150;
    public float MP
    {
        get => mp;
        private set
        {
            if (mp != value)
            {
                mp = value;
            }
        }
    }
    uint att;
    public uint ATT
    {
        get => att;
        set
        {
            if (att != value)
            {
                att = value;
                Debug.Log($"플레이어 공격력 : {att}");
            }
        }
    }
    uint dp;
    public uint DP
    {
        get => dp;
        set
        {
            if (dp != value)
            {
                dp = value;
                Debug.Log($"플레이어 방어력 : {dp}");
            }
        }
    }
    private void Awake()
    {
        inputActions = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.ItemPickUp.performed += ItemPickUp;
        inputActions.Player.Equip_Item.performed += On_Equip_Item;
        inputActions.KeyBoard.Enable();
        inputActions.KeyBoard.InvenKey.performed += OpenInven;
    }
    private void Start()
    {
        itemDescription = GameManager.SlotManager.ItemDescription;
        equipBox = GameManager.EquipBox;
        EquipBox_Description = equipBox.Description;

        equipBox.on_Update_Status += Update_Status;
    }

    private void Update_Status(ItemData legacyData, ItemData newData)
    {
        ItemData_Hat hat = newData as ItemData_Hat;
        //itemdata_
        if( legacyData == null )
        {

        }
        else
        {

        }
    }
 
    private void On_Equip_Item(InputAction.CallbackContext _)
    {
        if (itemDescription.ItemData != null)
        {
            onEquipItem?.Invoke(itemDescription.ItemData);
        }
        else if (EquipBox_Description.ItemData != null)
        {
            onUnEquipItem?.Invoke(EquipBox_Description.ItemData);
        }
    }
 

    private void OpenInven(InputAction.CallbackContext _)
    {
        onOpenInven?.Invoke();
    }

    private void ItemPickUp(InputAction.CallbackContext _)
    {
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, pickupRange, LayerMask.GetMask("Item"));
        foreach(var collider in itemColliders)
        {
            ItemObject itemObj = collider.GetComponent<ItemObject>();
            //IConsumable consumable = itemObj.ItemData as IConsumable;
            //if (consumable != null)
            //{
            //    consumable.Consume(this.gameObject);
            //    Destroy(itemObj.gameObject);
            //}
            if (GameManager.SlotManager.AddItem(itemObj.ItemData.code))
            {
                Destroy(itemObj.gameObject);
            }  
        }
    }
    public void RecoveryHP_(int recoveryValue, float duration)
    {
        StartCoroutine(RecoveryHP(recoveryValue, duration));
    }
    IEnumerator RecoveryHP(int recoveryValue, float duration)
    {
        float regenPerSecond = recoveryValue / duration;
        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            HP += regenPerSecond * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RecoveryMP()
    {
        yield return null;
    }
    bool IsEquipped()
    {
        return false;
    }




#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, pickupRange, 2.0f);
    }
#endif
}
