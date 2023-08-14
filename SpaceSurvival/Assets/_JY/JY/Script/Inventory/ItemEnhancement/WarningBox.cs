using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarningBox : PopupWindowBase, IPopupSortWindow,IPointerDownHandler
{
    Item_Enhancer item_Enhancer;

    CanvasGroup canvasGroup;
    Button confirmButton;
    Button cancelButton;

    public Action onWarningBoxClose;

    public Action<IPopupSortWindow> PopupSorting { get; set ; }

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
        confirmButton = transform.GetChild(3).GetComponent<Button>();
        cancelButton = transform.GetChild(2).GetComponent<Button>();

        cancelButton.onClick.AddListener(Close);

    }
    void Start()
    {

        item_Enhancer = GameManager.Enhancer;
        item_Enhancer.onConfirmButtonClick += Open;
        item_Enhancer.onWaitforResult += Close;
        confirmButton.onClick.AddListener(() => item_Enhancer.EnhancerState = EnhancerState.WaitforResult);
    }

    void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        PopupSorting(this); //팝업창 맨앞으로 보이게 하는함수
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        switch (item_Enhancer.EnhancerState)//강화중이면 신호를 보내지 마라(신호가 보내지면 UI의 모든 interactable이 true가 됨)
        {
            case EnhancerState.Success:
            case EnhancerState.Fail:
                return;
            default:
                break;
        }
        onWarningBoxClose?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PopupSorting(this); //팝업창 맨앞으로 보이게 하는함수
    }
    public void OpenWindow()
    {
        Open();
    }

    /// <summary>
    /// 팝업창이 맨앞에있을때 esc 누르면 해당함수를 실행한다. 
    /// </summary>
    public void CloseWindow()
    {
        Close();
    }
}
