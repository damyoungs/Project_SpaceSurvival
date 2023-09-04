using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TempSlot_For_QuickSlot_Base : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI countText;
    CanvasGroup canvasGroup;

    IEnumerator Moving_Coroutine;
    ItemData_Potion itemData;

    public Action<ItemData_Potion, uint> onEndDrag;
    public ItemData_Potion ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                if (itemData == null)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }
    }
    uint itemCount = 0;
    uint ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
            countText.text = itemCount.ToString();
        }
    }
    public bool IsOpen => canvasGroup.alpha == 1.0f;
    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        Moving_Coroutine = Moving();
    }
    void Open()
    {
        itemIcon.sprite = itemData.itemIcon;
        itemIcon.color = Color.white;

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(Moving_Coroutine);
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        StopCoroutine(Moving_Coroutine);
    }
    public void StartDrag(ItemData_Potion itemData, uint count)
    {
        ItemData = itemData;
        ItemCount = count;
    }
    public void EndDrag()
    {
        onEndDrag?.Invoke(ItemData, itemCount);
        ItemData = null;
    }
    IEnumerator Moving()
    {
        while (true)
        {
            transform.position = Mouse.current.position.ReadValue();
            yield return null;
        }
    }
}
