using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    TextMeshProUGUI itemDetail;
    CanvasGroup canvasGroup;

    Vector2 fixedPos = Vector2.zero;//퀵슬롯 전용 y값 보정용

    bool isPause = false;

    public float alphaChangeSpeed = 10.0f;
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if (isPause)
                Close();
        }
    }
    ItemData itemData = null;
    public ItemData ItemData => itemData;
    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemPrice = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDetail = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    //pause, open, close,
    public void Open(ItemData data)
    {
        if (!isPause && data != null)
        {
            ItemData_Equip equipItem = data as ItemData_Equip;
            ItemData_Enhancable enhancableItem = data as ItemData_Enhancable;
            ItemData_Bullet bullet = data as ItemData_Bullet;
            if (enhancableItem != null)
            {
                itemDetail.text = $"공격력 : {equipItem.attackPoint} \n방어력 : {equipItem.defencePoint}\n아이템 레벨 : {enhancableItem.itemLevel}";
            }
            else if (equipItem != null)
            {
                itemDetail.text = $"공격력 : {equipItem.attackPoint} \n방어력 : {equipItem.defencePoint}";
            }
            else if (bullet != null)
            {
                itemDetail.text = $"공격력 : {bullet.attckPoint}\n{bullet.itemDescription}";
            }
            else
            {
                itemDetail.text = $"{data.itemDescription}";
            }
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
            itemPrice.text = data.price.ToString("N0");
         
            StopAllCoroutines();
            StartCoroutine(FadeIn());

            MovePosition(Mouse.current.position.ReadValue());
        }
        
    }
    public void Toggle_IsPause()
    {
        IsPause = !IsPause;
    }
    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
    public void MovePosition(Vector2 mousePos)
    {
        //(rectTransform.sizeDelta.x + screenPos.x) 내 사이즈와 마우스좌표를 더한 후 스크린의 크기를 뺀 결과가 양수면 화면을 넘어갔다 판단.
        if (canvasGroup.alpha > 0.0f)
        {
            RectTransform rectTransform = (RectTransform)transform;
            int overX = (int)(rectTransform.sizeDelta.x + mousePos.x) - Screen.width;
            overX = Mathf.Max(0, overX);
            mousePos.x -= overX;

            transform.position = mousePos;
            if (rectTransform.anchoredPosition.y < -180.0f)
            {
                fixedPos.x = rectTransform.anchoredPosition.x;
                fixedPos.y = -180.0f;

                rectTransform.anchoredPosition = fixedPos;
            }

        }

    }
    IEnumerator FadeIn()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
        yield break;
    }
    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
        yield break;
     
    }
}
