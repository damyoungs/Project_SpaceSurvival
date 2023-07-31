using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enhancer_Slot_After : MonoBehaviour
{
    Slot slotComp;
    Image itemIcon;


    public Action onValueChange;
    public bool IsEmpty => ItemData == null;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티// 초기 
    private ItemData itemData = null;
    public ItemData ItemData//SlotManager의  GetItem 함수가 실행될때 Item의 정보를 받아오기위한 프로퍼티
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                onValueChange?.Invoke();
            }
        }
    }
    private void Awake()
    {
        // 상속받은 클래스에서 추가적인 초기화가 필요하기 때문에 가상함수로 만듬
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        onValueChange = Refresh;
        Refresh();
    }

    /// <summary>
    /// 슬롯 초기화용 함수
    /// </summary>
    /// <param name="slot">이 UI와 연결할 슬롯</param>
    public virtual void InitializeSlot(Slot slot)
    {
        this.slotComp = slot;                       // 슬롯 저장
        onValueChange = Refresh;   // 슬롯에 변화가 있을 때 실행될 함수 등록
        ItemData = null;
        Refresh();                              // 초기 모습 갱신
    }

    /// <summary>
    /// 슬롯이 보이는 모습을 갱신하는 함수
    /// </summary>
    protected virtual void Refresh()
    {
        if (IsEmpty)
        {
            // 비어있으면
            itemIcon.color = Color.clear; 
            itemIcon.sprite = null;         
        }
        else
        {
            // 아이템이 들어있으면
            itemIcon.sprite = ItemData.itemIcon;      // 아이콘에 이미지 설정
            itemIcon.color = Color.white;                       // 아이콘이 보이도록 투명도 제거
        }

        OnRefresh();        // 상속받은 클래스에서 개별로 실행하고 싶은 코드 실행
    }

    protected virtual void OnRefresh()
    {
    }
}