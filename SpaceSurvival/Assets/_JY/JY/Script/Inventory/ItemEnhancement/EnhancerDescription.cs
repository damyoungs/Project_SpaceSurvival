using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnhancerDescription : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemLevel;
    public TextMeshProUGUI attackPointText;
    public TextMeshProUGUI defencePointText;
    public CanvasGroup canvasGroup;

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

    //pause, open, close,
    public void Open(ItemData data)
    {
        if (!isPause && data != null)
        {
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
            itemLevel.text = data.price.ToString("N0");
            attackPointText.text = data.itemDescription;
            defencePointText.text = data.itemDescription;
            StopAllCoroutines();
            StartCoroutine(FadeIn());

            MovePosition(Mouse.current.position.ReadValue());
        }
        
    }
    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
    public void MovePosition(Vector2 screenPos)
    {
        if (canvasGroup.alpha > 0.0f)
        {
            RectTransform rectTransform = (RectTransform)transform;
            int overX = (int)(rectTransform.sizeDelta.x + screenPos.x) - Screen.width; //화면을 넘어간 부분 구하기
            overX = Mathf.Max(0, overX);
            screenPos.x -= overX;

            transform.position = screenPos;
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
