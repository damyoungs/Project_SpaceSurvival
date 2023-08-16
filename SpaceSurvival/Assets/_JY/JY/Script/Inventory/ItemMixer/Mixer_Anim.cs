using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mixer_Anim : MonoBehaviour
{
    public Sprite fail_Sprite;
    Item_Mixer mixer;
    Animator anim;
    CanvasGroup canvasGroup;

    Image result_Success_Image;
    Image success_Left_Image;
    Image success_Middle_Image;
    Button confirm_Button;
    public Button ConfirmButton => confirm_Button;


    public Image result_Fail_Image;
    Image fail_Left_Image;
    Image fail_Middle_Image;

    public Action done_With_Success_Anim;
    public Action done_With_Fail_Anim;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        anim = GetComponent<Animator>();
        result_Success_Image = transform.GetChild(0).GetComponent<Image>();
        success_Left_Image = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        success_Middle_Image = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        confirm_Button = transform.GetChild(0).GetChild(0).GetComponent<Button>();

        confirm_Button.onClick.AddListener(Confirm);

      //  result_Fail_Image = transform.GetChild(1).GetComponent<Image>();
        fail_Left_Image = transform.GetChild(1).GetChild(2).GetComponent<Image>();
        fail_Middle_Image = transform.GetChild(1).GetChild(3).GetComponent<Image>();
    }
    private void Start()
    {
        mixer = GameManager.Mixer;
        mixer.onSuccess += () => StartCoroutine(PopUp_Anim(true));
        mixer.onFail += () => StartCoroutine(PopUp_Anim(false));
    }
    IEnumerator PopUp_Anim(bool success)
    {
        SetActive(true);
        if (success)
        {
            anim.SetBool("Confirm", false);
            SetSuccess_Image();
            anim.SetTrigger("Success");
            yield return new WaitForSeconds(6.0f);// Success clip의 재생시간을 고려한 딜레이
            result_Success_Image.sprite = mixer.ResultSlot.ItemData.itemIcon;
            //   onTriggerLevelUp?.Invoke();
            // mixer.MixerState = ItemMixerState.ClearItem;

        }
        else
        {
            SetFailure_Image();
            anim.SetTrigger("Fail");
            yield return new WaitForSeconds(8.0f);//대기시간이 없으면 버튼 활성화가 너무 빨리된다.
            done_With_Fail_Anim?.Invoke();
            SetActive(false);
        }
      //  OpenInteractable();
    }
    


    void SetSuccess_Image()
    {
        result_Success_Image.sprite = mixer.ResultSlot.ItemData.itemIcon;
        success_Left_Image.sprite = mixer.LeftSlotData.itemIcon;
        success_Middle_Image.sprite = mixer.MiddleSlotData.itemIcon;
    }
    void SetFailure_Image()
    {
        result_Fail_Image.sprite = mixer.ResultSlot.ItemData.itemIcon;
        fail_Left_Image.sprite = mixer.LeftSlotData.itemIcon;
        fail_Middle_Image.sprite = mixer.MiddleSlotData.itemIcon;
    }

    void Confirm()
    {
        done_With_Success_Anim?.Invoke();
        SetActive(false);
        anim.SetBool("Confirm", true);
    }
    public void SetActive(bool active)
    {
        if (active)
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            anim.SetTrigger("Confirm");
        }
    }
}
