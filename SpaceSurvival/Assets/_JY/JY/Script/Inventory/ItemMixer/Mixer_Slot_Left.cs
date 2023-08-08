using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mixer_Slot_Left : Mixer_Slot_Base, IPointerClickHandler
{
    public Action onClearLeftSlot;
    protected override void Awake()
    {
        base.Awake();
        GameManager.Mixer.onLeftSlotDataChange += (itemData) => this.ItemData = itemData;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ItemData != null)
        {
            onClearLeftSlot?.Invoke();// mixer���� ItemData �� null �� ����⶧���� ���� ���⼭ null�� �������� �ʴ´�.
        }
    }
}