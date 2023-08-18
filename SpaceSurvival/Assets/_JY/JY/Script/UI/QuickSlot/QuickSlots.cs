using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum QuickSlotList
{
    Shift = 0,
    Eight,
    Nine,
    Zero,
    Ctrl,
    Alt,
    Space,
    Insert
}
public class QuickSlots : MonoBehaviour
{
    InputKeyMouse inputAction;
    QuickSlot[] quickSlots = null;

    Vector2 hidePos = Vector2.zero;
    public float popUpSpeed = 3.0f;
    bool isOpen = false;
    public QuickSlot this[QuickSlotList number] => quickSlots[(int) number];
    private void Awake()
    {
        inputAction = new InputKeyMouse();
        hidePos = new Vector2(0, -280.0f);
    }
    private void OnEnable()
    {
        inputAction.QuickSlot.Enable();
        inputAction.QuickSlot.PopUp.performed += QuickSlot_PopUp;
        inputAction.QuickSlot.Shift.performed += Shift_performed;
        inputAction.QuickSlot.Eight.performed += Eight_performed;
        inputAction.QuickSlot.Nine.performed += Nine_performed;
        inputAction.QuickSlot.Zero.performed += Zero_performed;
        inputAction.QuickSlot.Ctrl.performed += Ctrl_performed;
        inputAction.QuickSlot.Alt.performed += Alt_performed;
        inputAction.QuickSlot.Space.performed += Space_performed;
        inputAction.QuickSlot.Insert.performed += Insert_performed;
    }
    private void Start()
    {
        Init();
    }
    private void Insert_performed(InputAction.CallbackContext context)
    {
    }
    IEnumerator HideCoroutine()
    {
        while(transform.position.y > hidePos.y)
        {
            transform.position += Time.deltaTime * popUpSpeed * -Vector3.up;
            yield return null;
        }
        isOpen = true;
    }
    IEnumerator PopUpCoroutine()
    {
        while (transform.position.y < 0.0f)
        {
            transform.position += Time.deltaTime * popUpSpeed * -Vector3.up;
            yield return null;
        }
        isOpen = true;
    }
    private void QuickSlot_PopUp(InputAction.CallbackContext context)
    {
        StartCoroutine(PopUpCoroutine());
    }
    private void Space_performed(InputAction.CallbackContext context)
    {
        HideCoroutine();
    }

    private void Alt_performed(InputAction.CallbackContext context)
    {
    }

    private void Ctrl_performed(InputAction.CallbackContext context)
    {
    }

    private void Zero_performed(InputAction.CallbackContext context)
    {
    }

    private void Nine_performed(InputAction.CallbackContext context)
    {
    }

    private void Eight_performed(InputAction.CallbackContext context)
    {
    }

 

    private void Shift_performed(InputAction.CallbackContext context)
    {
    }

  
    void Init()
    {
        quickSlots = new QuickSlot[transform.childCount];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = transform.GetChild(i).GetComponent<QuickSlot>();
        }
    }
}
