using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Obj : MonoBehaviour
{
    OpenInput openInput;
    public GameObject inven;
    bool isOpen;

    private void Awake()
    {
        openInput = new OpenInput();
    }
    private void OnEnable()
    {
        openInput.Enable();
        openInput.Inventory.Open.started += OpenInventory;
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
