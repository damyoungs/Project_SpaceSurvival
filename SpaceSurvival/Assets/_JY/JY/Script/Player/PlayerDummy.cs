using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerDummy : MonoBehaviour, IHealth
{
    InputKeyMouse inputActions;

    public Transform EquipParent;

    public Action onOpenInven;

    public float moveSpeed = 0.0f;
    public float rotateSpeed = 0.0f;
    public float pickupRange = 3.0f;

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
    private void Awake()
    {
        inputActions = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.ItemPickUp.performed += ItemPickUp;
        inputActions.KeyBoard.Enable();
        inputActions.KeyBoard.InvenKey.performed += OpenInven;
    }

    private void OpenInven(InputAction.CallbackContext _)
    {
        //onOpenInven?.Invoke();
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
    void EquipItem()
    {
        while(EquipParent.childCount > 0)
        {
            Transform child = EquipParent.GetChild(0);
            Destroy(child.gameObject);
        }
       
    }
    void UnEquipItem()
    {

    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, pickupRange, 2.0f);
    }
#endif
}
