using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipBox_Description : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    TextMeshProUGUI itemDetail;
    CanvasGroup canvasGroup;

    Vector2 fixedPos = Vector2.zero;//������ ���� y�� ������

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
            ItemData_Hat hat = data as ItemData_Hat;
            ItemData_Enhancable enhancableItem = data as ItemData_Enhancable;
            ItemData_Craft jewel = data as ItemData_Craft;
            ItemData_Armor armor = data as ItemData_Armor;
            if (enhancableItem != null)
            {
                itemDetail.text = $"���ݷ� : {enhancableItem.attackPoint} \n���� : {enhancableItem.defencePoint}\n������ ���� : {enhancableItem.itemLevel}";
            }
            else if (hat != null)
            {
                itemDetail.text = $"���ݷ� : {hat.attack_Point} \n���� : {hat.defence_Point}";
            }
            else if (jewel != null)
            {
                itemDetail.text = $"���ݷ� : {jewel.attack_Point}\n{jewel.itemDescription}";
            }
            else if (armor != null)
            {
                itemDetail.text = $"���ݷ� : {armor.attack_Point} \n���� : {armor.defence_Point}";
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
        //(rectTransform.sizeDelta.x + screenPos.x) �� ������� ���콺��ǥ�� ���� �� ��ũ���� ũ�⸦ �� ����� ����� ȭ���� �Ѿ�� �Ǵ�.
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