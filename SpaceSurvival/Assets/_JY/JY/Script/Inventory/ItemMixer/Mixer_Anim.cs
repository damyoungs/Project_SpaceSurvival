using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mixer_Anim : MonoBehaviour
{
    Item_Mixer mixer;
    Animator anim;
    CanvasGroup canvasGroup;

    Image result_Success_Image;
    Image success_Left_Image;
    Image success_Middle_Image;
    Button confirm_Button;

    Image result_Fail_Image;
    Image fail_Left_Image;
    Image fail_Middle_Image;
    Image fail_Sliced_Left_Image;
    Image fail_Sliced_Middle_Image;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        anim = GetComponent<Animator>();
        result_Success_Image = transform.GetChild(0).GetComponent<Image>();
        success_Left_Image = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        success_Middle_Image = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        confirm_Button = transform.GetChild(0).GetChild(0).GetComponent<Button>();


        result_Fail_Image = transform.GetChild(1).GetComponent<Image>();
        fail_Left_Image = transform.GetChild(1).GetChild(2).GetComponent<Image>();
        fail_Middle_Image = transform.GetChild(1).GetChild(3).GetComponent<Image>();
        fail_Sliced_Left_Image = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        fail_Sliced_Middle_Image = transform.GetChild(1).GetChild(1).GetComponent<Image>();
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
            anim.SetTrigger("Success");
            yield return new WaitForSeconds(7.0f);// Success clip의 재생시간을 고려한 딜레이
         //   onTriggerLevelUp?.Invoke();
           // mixer.MixerState = ItemMixerState.ClearItem;
            Debug.Log("State 변경 ");
            
        }
        else
        {
            anim.SetTrigger("Fail");
            yield return new WaitForSeconds(8.0f);//대기시간이 없으면 버튼 활성화가 너무 빨리된다.
            SetActive(false);
        }
      //  OpenInteractable();
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
