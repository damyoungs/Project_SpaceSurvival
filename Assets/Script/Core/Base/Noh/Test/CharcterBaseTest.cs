using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharcterBaseTest : MonoBehaviour
{
    InputKeyMouse moveEvent;
    [SerializeField]
    GameObject cameraObj;
    [SerializeField]
    Vector3 charcterDistance = Vector3.one;
    public float lerp;
    //float angleValue = 40;

    Vector3 direction = Vector3.zero;
    [SerializeField]
    float defaultSpeed = 1.0f;
    [SerializeField]
    float boosterSpeed = 1.0f;

    const float nomalSpeed = 1.0f;
    float boosterValue = nomalSpeed;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        moveEvent = new InputKeyMouse();
       
    }

    protected virtual void OnEnable()
    {
        moveEvent.Enable();
        moveEvent.Charcter.MoveAndUpDown.performed += OnMove;
        moveEvent.Charcter.MoveBooster.performed += OnBooster;
        moveEvent.Charcter.MoveBooster.canceled += OnBooster;
    }


    protected virtual void OnDisable()
    {
        moveEvent.Charcter.MoveBooster.canceled -= OnBooster;
        moveEvent.Charcter.MoveBooster.performed -= OnBooster;
        moveEvent.Charcter.MoveAndUpDown.performed -= OnMove;
        moveEvent.Disable();
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Time.deltaTime * defaultSpeed * boosterValue *  direction,Space.World);
        SetCamera();
    }
    protected virtual void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector3>();
        
    }
    protected virtual void OnBooster(InputAction.CallbackContext context)
    {
        if (context.performed) {
            boosterValue = boosterSpeed;
        }
        else if (context.canceled){
            boosterValue = nomalSpeed;
        }
    }
    protected virtual void SetCamera() 
    {
        cameraObj.transform.position = transform.position + charcterDistance;
    }
}
