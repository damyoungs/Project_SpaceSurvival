using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_deathPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Button title_Button;
    Button lobby_Button;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        title_Button = transform.GetChild(0).GetComponent<Button>();
        lobby_Button = transform.GetChild(1).GetComponent<Button>();
    }

    public void Activate_DeathPanel()
    {
        StartCoroutine(PopupPanel());
    }
    IEnumerator PopupPanel()
    {
        float waitTime = 0;
        while (waitTime < 3.0f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        float maxAlpha = 0.8f;
        float showTime = 2;
        float increase = maxAlpha / showTime;
        while(canvasGroup.alpha < maxAlpha)
        {
            canvasGroup.alpha += increase * Time.deltaTime;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void DeActivate_DeathPanel()
    {

    }

}
