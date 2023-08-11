
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MixerDescription : MonoBehaviour
{


    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI attackPointText;
    public TextMeshProUGUI defencePointText;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI itemDescription;

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
    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    //pause, open, close,
    private void Start()
    {

        Mixer_Slot_Left leftSlot;
        Mixer_Slot_Middle middleSlot;
        leftSlot = GameManager.Mixer.MixerUI.Left_Slot;
        middleSlot = GameManager.Mixer.MixerUI.Middle_Slot;

       // leftSlot.onPointerEnter += Open_LeftSlot_Description;
       // middleSlot.onPointerEnter += Open_MiddleSlot_Description;
        leftSlot.onPointerExit += Close;
        middleSlot.onPointerExit += Close;
        leftSlot.onPointerMove += MovePosition;
        middleSlot.onPointerMove += MovePosition;
    }
    public void Open_LeftSlot_Description(ItemData_Enhancable data)
    {
        if (!isPause && data != null)
        {
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
            attackPointText.text = $"{data.attackPoint}";
            defencePointText.text = $"{data.defencePoint}";
            itemDescription.text = $"{data.itemDescription}";
            StopAllCoroutines();
            StartCoroutine(FadeIn());

            MovePosition(Mouse.current.position.ReadValue());
        }
    }
    public void Open_MiddleSlot_Description(ItemData_Enhancable data)
    {
        data.Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        if (!isPause && data != null)
        {
            itemIcon.sprite = data.itemIcon;
            itemName.text = itemname;
            attackPointText.text = $"{resultAttackPoint}";
            defencePointText.text = $"{resultDefencePoint}";
            itemDescription.text = $"{data.itemDescription}";
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
        while (canvasGroup.alpha < 1.0f)
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
