using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public Action visibleUI;
    public Action invisibleUI;
    float speed = 20.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        visibleUI = OnVisible;
        invisibleUI = OnInvisible;
    }

    void OnVisible()
    {
        StartCoroutine(VisibleUI());
    }

    void OnInvisible()
    {
        StartCoroutine(InvisibleUI());
    }

    IEnumerator VisibleUI()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return null;
        }
        Debug.Log("ui 보임");
    }

    IEnumerator InvisibleUI()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        Debug.Log("ui 안 보임");
    }

}
