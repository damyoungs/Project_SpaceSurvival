using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSplitter : MonoBehaviour
{
    /// <summary>
    /// �������� ��� ����
    /// </summary>
    Slot targetSlot;

    /// <summary>
    /// ��� �� �ִ� ������ ������ �ּҰ�
    /// </summary>
    const uint MinItemCount = 1;

    /// <summary>
    /// ��� ������ ����
    /// </summary>
    uint itemSplitCount = MinItemCount;

    /// <summary>
    /// ��� ������ ������ Ȯ���ϰ� �����ϴ� ������Ƽ
    /// </summary>
    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            // ��� ������ ���� = 1 ~ (��󽽷��� ���� ������ ���� - 1)
            itemSplitCount = Math.Clamp(value, MinItemCount, targetSlot.ItemCount - 1);

            inputField.text = itemSplitCount.ToString();    // ��ǲ �ʵ忡 ����
            slider.value = itemSplitCount;                  // �����̴��� ����
        }
    }

    /// <summary>
    /// OK ��ư�� �������� �� ����� ��������Ʈ(�Ķ����: ��� ��� ������ �ε���, ��� ����)
    /// </summary>
    public Action<uint, uint> onOkClick;

    /// <summary>
    /// Cancel��ư�� �������� �� ����� ��������Ʈ
    /// </summary>
    public Action onCancel;

    /// <summary>
    /// ��ǲ�׼ǵ�
    /// </summary>
    InputKeyMouse inputActions;

    // ������Ʈ��
    Image itemIcon;
    TMP_InputField inputField;
    Slider slider;

    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Button minus = transform.GetChild(1).GetComponent<Button>();
        Button plus = transform.GetChild(2).GetComponent<Button>();
        Button cancel = transform.GetChild(4).GetComponent<Button>();
        Button ok = transform.GetChild(5).GetComponent<Button>();

        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener((text) =>
        {
            // inputField�� ���� ����Ǿ��� �� ����� �Լ�
            if (uint.TryParse(text, out uint result))
            {
                ItemSplitCount = result;        // �ؽ�Ʈ�� ���ڷ� �ٲ㼭 ����
            }
            else
            {
                ItemSplitCount = MinItemCount;  // �߸��� �Է��� ���� �ּҰ����� ����
            }
        });

        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener((ratio) =>
        {
            // �����̴��� ���� ����Ǿ��� �� ����� �Լ�
            ItemSplitCount = (uint)ratio;   // �ִ��ּҰ� ������ ����Ǿ��־ �ٷ� ���� ����
        });


        plus.onClick.AddListener(() =>
        {
            ItemSplitCount++;   // plus��ư�� �������� ��� ���� 1����
        });


        minus.onClick.AddListener(() =>
        {
            ItemSplitCount--;   // minus��ư�� �������� ��� ���� 1����
        });


        ok.onClick.AddListener(() =>
        {
            onOkClick?.Invoke(targetSlot.Index, ItemSplitCount);    // ok ��ư�� �������� ��ȣ������ �ݱ�
            Close();
        });

        cancel.onClick.AddListener(() =>
        {
            onCancel?.Invoke();     // cancel��ư�� �������� ��ȣ������ �ݱ�
            Close();
        });

        inputActions = new InputKeyMouse();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += OnClick;
    }

    private void OnDisable()
    {
        inputActions.UI.Click.performed -= OnClick;
        inputActions.UI.Disable();
    }

    /// <summary>
    /// ������ �и�â�� ���� �Լ�
    /// </summary>
    /// <param name="target">�������� �и��� ��� ����</param>
    public void Open(Slot target)
    {
        if (!target.IsEmpty && target.ItemCount > MinItemCount)    // ��� �������� �ְ� �ּ� ���� �̻����� ���� ���� â ����
        {
            targetSlot = target;                            // ��� ����
            itemIcon.sprite = targetSlot.ItemData.itemIcon; // ������ ����
            slider.minValue = MinItemCount;                 // �����̴� �ּ�/�ִ� ����
            slider.maxValue = targetSlot.ItemCount - 1;
            ItemSplitCount = MinItemCount;                  // �и��� ������ �ּҰ����� ����
            gameObject.SetActive(true);                     // Ȱ��ȭ�ؼ� �����ֱ�
        }
    }

    /// <summary>
    /// ������ �и�â�� �ݴ� �Լ�
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);    // ��Ȱ��ȭ�ؼ� �Ⱥ����ֱ�
    }

    /// <summary>
    /// ���콺�� ������ Ŭ���� �ǰų� ���� Ŭ���� �� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="_"></param>
    private void OnClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();         // ���콺 ������ ��ġ ��������
        Vector2 posDiff = screenPos - (Vector2)transform.position;      // ������Ʈ �Ǻ� ��ġ���� �󸶳� ������ �ִ��� ���
        RectTransform rectTransform = (RectTransform)transform;

        if (!rectTransform.rect.Contains(posDiff))
        {
            Close();    // �� UI�� rect�ȿ� ������ �ȵǸ� �ݴ´�.
        }
    }
}