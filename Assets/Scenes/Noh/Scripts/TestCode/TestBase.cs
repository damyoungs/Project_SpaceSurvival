using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    InputKeyMouse inputKeyMouse;

    private void Awake()
    {
        inputKeyMouse = new InputKeyMouse();
    }

    private void OnEnable()
    {
        inputKeyMouse.Test.Enable();
        inputKeyMouse.Test.Test1.performed += Test1;
        inputKeyMouse.Test.Test2.performed += Test2;
        inputKeyMouse.Test.Test3.performed += Test3;
        inputKeyMouse.Test.Test4.performed += Test4;
        inputKeyMouse.Test.Test5.performed += Test5;
    }

    private void OnDisable()
    {
        inputKeyMouse.Test.Test5.performed -= Test5;
        inputKeyMouse.Test.Test4.performed -= Test4;
        inputKeyMouse.Test.Test3.performed -= Test3;
        inputKeyMouse.Test.Test2.performed -= Test2;
        inputKeyMouse.Test.Test1.performed -= Test1;
        inputKeyMouse.Test.Disable();
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
}
