using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInput playerInput;
    public float moveSpeed = 3.0f;
    Vector3 dir;

    [SerializeField]
    GameObject inven;
    bool isOpen = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.OpenInven.started += OpenInventory;
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector3>();
    }
    private void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * moveSpeed * dir);
    }
    private void OnDisable()
    {
        
    }
    public void OpenInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isOpen)
        {
            inven.SetActive(true);
            isOpen = true;
        }
        else
        {
            inven.SetActive(false);
            isOpen = false;
        }

    }
}
