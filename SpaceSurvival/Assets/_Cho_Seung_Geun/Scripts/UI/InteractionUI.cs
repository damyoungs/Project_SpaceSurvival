using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{

    public Action visibleUI;
    public Action invisibleUI;

    float speed = 20.0f;

    CanvasGroup canvasGroup;
    Animator animator;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        visibleUI = OnVisible;
        invisibleUI = OnInvisible;
    }

    void OnVisible()
    {
        //StartCoroutine(VisibleUI());
        animator.SetBool("IsVisible", true);
    }

    void OnInvisible()
    {
        //StartCoroutine(InvisibleUI());
        animator.SetBool("IsVisible", false);
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
