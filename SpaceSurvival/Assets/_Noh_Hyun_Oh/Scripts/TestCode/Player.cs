using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// TestCode 입니다
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// 캐릭터이동속도
    /// </summary>
    [SerializeField]
    float moveSpeed = 3.0f;

    /// <summary>
    /// 캐릭터 이동방향
    /// </summary>
    Vector3 rotateDir = Vector3.zero;

    
    InputKeyMouse inputAction;

    private void Awake()
    {
        inputAction = new();
        //GameObject.FindGameObjectsWithTag
        //GameObject.FindWithTag
        //GameObject.FindGameObjectWithTag
    }
    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        //inputAction.Player.Attack.performed += OnAttack;
    }


    private void OnDisable()
    {
        //inputAction.Player.Attack.performed -= OnAttack;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }

    private void Update()
    {
        transform.Translate(rotateDir * moveSpeed * Time.deltaTime);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           Vector3 tempDir = context.ReadValue<Vector2>();
           rotateDir.x = tempDir.x;
           rotateDir.z = tempDir.y;
           
        }
        else if (context.canceled) 
        {
           rotateDir =  Vector3.zero;
        }
        Debug.Log(rotateDir);
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log(1);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Handles.DrawWireDisc(transform.position,Vector3.up,10.0f);
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position,10.0f);
    }
#endif
}

