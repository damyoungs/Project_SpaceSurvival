using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI ��ư �̺�Ʈ�� �������� ������Ʈ
/// </summary>
public class OpeningButtonManager : MonoBehaviour
{
    public Action speedUpButton;

    public Action skipButton;

    private void Awake()
    {
        Transform child =  transform.GetChild(0);
        Button speedUpBt = child.GetComponent<Button>();
        speedUpBt.onClick.AddListener(() => {
            speedUpButton?.Invoke();
        });
        child = transform.GetChild(1);
        Button skipBt = child.GetComponent<Button>();
        skipBt.onClick.AddListener(() => {
            skipButton?.Invoke();
        });
    }
}
