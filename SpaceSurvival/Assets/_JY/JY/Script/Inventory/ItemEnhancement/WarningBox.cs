using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningBox : MonoBehaviour
{
    Item_Enhancer item_Enhancer;
    Item_Mixer item_Mixer;

    CanvasGroup canvasGroup;
    Button confirmButton;
    Button cancelButton;

    public Action onWarningBoxClose;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        confirmButton = transform.GetChild(3).GetComponent<Button>();
        cancelButton = transform.GetChild(2).GetComponent<Button>();

        cancelButton.onClick.AddListener(Close);

    }
    void Start()
    {

        item_Enhancer = FindObjectOfType<Item_Enhancer>();
        item_Enhancer.onConfirmButtonClick += Open;
        item_Enhancer.onWaitforResult += Close;
        confirmButton.onClick.AddListener(() => item_Enhancer.EnhancerState = EnhancerState.WaitforResult);


        item_Mixer = FindObjectOfType<Item_Mixer>();
        item_Mixer.onConfirmButtonClick += Open;
        item_Mixer.onWaitforResult += Close;
        confirmButton.onClick.AddListener(() => item_Mixer.MixerState = ItemMixerState.WaitforResult);
    }

    void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        switch (item_Mixer.MixerState)//��ȭ���̸� ��ȣ�� ������ ����(��ȣ�� �������� UI�� ��� interactable�� true�� ��)
        {
            case ItemMixerState.Success:
            case ItemMixerState.Fail:
                return;
            default:
                break;
        }
        onWarningBoxClose?.Invoke();//
    }
}
