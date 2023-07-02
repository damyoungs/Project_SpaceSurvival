using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    PlayerInput inputActions;
    private void Awake()
    {
        inputActions = new PlayerInput();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Test1.started += Test1;
        inputActions.Player.Test2.started += Test2;
        inputActions.Player.Test3.started += Test3;
        inputActions.Player.Test4.started += Test4;
        inputActions.Player.Test5.started += Test5;
        inputActions.Player.Test6.started += Test6;
        inputActions.Player.Test7.started += Test7;
        inputActions.Player.Test8.started += Test8;
        inputActions.Player.Test9.started += Test9;
        inputActions.Player.Test10.started += Test10;
    }



    protected virtual void Test1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
  
    }
    protected virtual void Test2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
   
    }
    protected virtual void Test3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }


    protected virtual void Test4(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }
    protected virtual void Test5(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
 
    }
    protected virtual void Test6(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }
    protected virtual void Test7(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }
    protected virtual void Test8(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
   
    }
    protected virtual void Test9(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }
    protected virtual void Test10(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }




}
