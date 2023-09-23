using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
     CanvasGroup canvasGroup;
     CanvasGroup detailCanvasGroup;
    
     TextMeshProUGUI nameText;
     TextMeshProUGUI levelText;
     TextMeshProUGUI attText;
     TextMeshProUGUI dpText;
     TextMeshProUGUI strText;
     TextMeshProUGUI intText;
     TextMeshProUGUI lukText;
     TextMeshProUGUI dexText;
     TextMeshProUGUI abilityPoint_Text;
     TextMeshProUGUI damageText;
     TextMeshProUGUI criticalDamageText;
     TextMeshProUGUI criticalRateText;
     TextMeshProUGUI avoidbilityText;



    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        detailCanvasGroup = transform.GetChild(2).GetComponent<CanvasGroup>();

        nameText = transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        levelText = transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        attText = transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dpText = transform.GetChild(1).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        strText = transform.GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        intText = transform.GetChild(1).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        lukText = transform.GetChild(1).GetChild(6).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dexText = transform.GetChild(1).GetChild(7).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        abilityPoint_Text = transform.GetChild(1).GetChild(8).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        damageText = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent <TextMeshProUGUI>();
        criticalDamageText = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalRateText = transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        avoidbilityText = transform.GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
