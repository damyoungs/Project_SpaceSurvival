using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Base : MonoBehaviour
{
    Slot slotComp;
    Image itemIcon;

    TextMeshProUGUI itemCountText;

    protected virtual void Awake()
    {
        // 상속받은 클래스에서 추가적인 초기화가 필요하기 때문에 가상함수로 만듬
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCountText = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯 초기화용 함수
    /// </summary>
    /// <param name="slot">이 UI와 연결할 슬롯</param>
    public virtual void InitializeSlot(Slot slot)
    {
        this.slotComp = slot;                       // 슬롯 저장
        slot.onItemChange = Refresh;   // 슬롯에 변화가 있을 때 실행될 함수 등록
        Refresh();                              // 초기 모습 갱신
    }

    /// <summary>
    /// 슬롯이 보이는 모습을 갱신하는 함수
    /// </summary>
    protected virtual void Refresh()
    {
        if (slotComp.IsEmpty)
        {
            // 비어있으면
            itemIcon.color = Color.clear;   // 아이콘 안보이게 투명화
            itemIcon.sprite = null;         // 아이콘에서 이미지 제거
            itemCountText.text = string.Empty;  // 개수도 안보이게 글자 제거
        }
        else
        {
            // 아이템이 들어있으면
            itemIcon.sprite = slotComp.ItemData.itemIcon;      // 아이콘에 이미지 설정
            itemIcon.color = Color.white;                       // 아이콘이 보이도록 투명도 제거
            itemCountText.text = slotComp.ItemCount.ToString();    // 아이템 개수 설정
        }

        OnRefresh();        // 상속받은 클래스에서 개별로 실행하고 싶은 코드 실행
    }

    /// <summary>
    /// 상속받은 클래스에서 개별적으로 실행하고 싶은 코드를 모아놓은 함수
    /// </summary>
    protected virtual void OnRefresh()
    {
    }
}