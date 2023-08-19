using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    Button popupButton;
    TextMeshProUGUI buttonText;
    string open = "¡ã¡ã";
    string close = "¡å¡å";


    RectTransform rectTransform;
    InputKeyMouse inputAction;
    QuickSlot[] quickSlots = null;

    Vector2 hidePos = Vector2.zero;
    public float popUpSpeed = 7.0f;
    bool isOpen = false;

    public QuickSlot this[QuickSlotList number] => quickSlots[(int) number];
    private void Awake()
    {
        inputAction = new InputKeyMouse();
        hidePos = new Vector2(0, -280.0f);
        rectTransform = GetComponent<RectTransform>();

        popupButton = transform.GetChild(8).GetComponent<Button>();
        buttonText = transform.GetChild(8).GetComponentInChildren<TextMeshProUGUI>();

        popupButton.onClick.AddListener(PopUp);

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
        buttonText.text = open;
    }
    private void Insert_performed(InputAction.CallbackContext context)
    {
    }
  
    IEnumerator PopUpCoroutine()
    {
        if (isOpen)
        {
            while (transform.position.y > hidePos.y)
            {
                transform.position +=  popUpSpeed * -Vector3.up;
                yield return null;
            }
            isOpen = false;
            buttonText.text = open;
        }
        else
        {
            while (transform.position.y < 0.0f)
            {
                transform.position +=  popUpSpeed * Vector3.up;
                yield return null;
            }
            isOpen = true;
            buttonText.text = close;
        }
     
    }
    void PopUp()
    {
        StartCoroutine(PopUpCoroutine());
    }
    private void QuickSlot_PopUp(InputAction.CallbackContext _)
    {
        StartCoroutine(PopUpCoroutine());
    }
    private void Space_performed(InputAction.CallbackContext context)
    {
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
