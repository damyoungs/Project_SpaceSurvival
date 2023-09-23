using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_Status//아무것도 장비하지 않은 상태의 플레이어의 기본 공격력, 방어력을 저장. 버프사용, 사용중 장비, 해제 시 다시 설정할 때 사용
{
    public string name;
    public uint level;
    public uint base_HP;
    public uint base_Stamina;
    public uint base_ATT;
    public uint base_DP;
    public uint base_STR;
    public uint base_INT;
    public uint base_LUK;
    
    public Base_Status(Player_ player)
    {
        base_ATT = player.ATT;
        base_DP = player.DP;
    }
}
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
     TextMeshProUGUI hpText;
     TextMeshProUGUI staminaText;
     TextMeshProUGUI damageText;
     TextMeshProUGUI criticalDamageText;
     TextMeshProUGUI criticalRateText;
     TextMeshProUGUI avoidbilityText;

     Base_Status base_Status;


    private void Awake()
    {
        GetComponents();
    }
    void GetComponents()
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
        hpText = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        staminaText = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        damageText = transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalDamageText = transform.GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalRateText = transform.GetChild(2).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        avoidbilityText = transform.GetChild(2).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
