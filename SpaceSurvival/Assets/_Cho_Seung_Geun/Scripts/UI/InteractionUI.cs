using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public Action visibleUI;
    public Action invisibleUI;

    Animator animator;

    readonly int Hash_Visible = Animator.StringToHash("IsVisible");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        visibleUI = OnVisible;
        invisibleUI = OnInvisible;
    }

    void OnVisible()
    {
        animator.SetBool(Hash_Visible, true);   // f키 보임
    }

    void OnInvisible()
    {
        animator.SetBool(Hash_Visible, false);  // f키 안 보임
    }
}
