using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBox : MonoBehaviour, IPopupSortWindow
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI darkForce_Text;

    SkillData[] skillDatas;
    public SkillData[] SkillDatas => skillDatas;

    public Action<IPopupSortWindow> PopupSorting { get; set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        darkForce_Text = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        skillDatas = new SkillData[5];
        int i = 0; 
        while (i < skillDatas.Length)
        {
            skillDatas[i] = transform.GetChild(i + 2).GetComponent<SkillData>();
            i++;
        }
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
        InputSystemController.InputSystem.UI_Inven.SkillBox_Open.performed += Toggle_Open_Close;
        GameManager.Player_.on_DarkForce_Change += Refresh;
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
        darkForce_Text.text = GameManager.Player_.DarkForce.ToString();
    }

    public void OpenWindow()
    {
        Open();
    }

    public void CloseWindow()
    {
        Close();
    }
}
