using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerDummy : MonoBehaviour
{
    InputKeyMouse inputActions;
    Vector3 root2 = Vector3.zero;


    Vector3 dir = Vector3.zero;

    public float moveSpeed = 0.0f;
    public float rotateSpeed = 0.0f;

    public float pickupRange = 3.0f;

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
    private void Awake()
    {
        inputActions = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ChangeVectorDir;
        inputActions.Player.ItemPickUp.performed += ItemPickUp;
    }

    private void ItemPickUp(InputAction.CallbackContext _)
    {
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, pickupRange, LayerMask.GetMask("Item"));
        foreach(var collider in itemColliders)
        {
            ItemObject itemObj = collider.GetComponent<ItemObject>();
            IConsumable consumable = itemObj.ItemData as IConsumable;
            if (consumable != null)
            {
                consumable.Consume(this.gameObject);
                Destroy(itemObj.gameObject);
            }
            else if (GameManager.SlotManager.AddItem(itemObj.ItemData.code))
            {
                Destroy(itemObj.gameObject);
            }  
        }
    }

    private void Update()
    {
        Move();
        Rotate();
    }
    void Move()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);
    }
    void Rotate()
    {
         transform.LookAt(transform.position + dir);

    }

    private void ChangeVectorDir(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector3>();        
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, pickupRange, 2.0f);
    }
#endif
}
