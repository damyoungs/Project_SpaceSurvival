using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempSlot_For_QuickSlot_Base : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI countText;

    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
