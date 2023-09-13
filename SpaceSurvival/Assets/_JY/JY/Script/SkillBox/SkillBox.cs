using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBox : MonoBehaviour
{
    InputKeyMouse inputAction;
    CanvasGroup canvasGroup;
    TextMeshProUGUI darkForce_Text;
    
    

    private void Awake()
    {
        inputAction = new();
        canvasGroup = GetComponent<CanvasGroup>();
        darkForce_Text = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        inputAction.UI_Inven.Enable();
        inputAction.UI_Inven.SkillBox_Open.performed += Toggle_Open_Close;
    }

    private void Toggle_Open_Close(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if (canvasGroup.alpha > 0)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Start()
    {
        GameManager.Player__.on_DarkForce_Change += Refresh;
        Refresh();

    }
    public void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    void Refresh()
    {
        darkForce_Text.text = GameManager.Player__.DarkForce.ToString();
    }
}
