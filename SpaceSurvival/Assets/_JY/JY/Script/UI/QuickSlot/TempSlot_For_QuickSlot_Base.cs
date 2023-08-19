using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempSlot_For_QuickSlot_Base : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI countText;
    CanvasGroup canvasGroup;

    public bool IsOpen => canvasGroup.alpha == 1.0f;
    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
