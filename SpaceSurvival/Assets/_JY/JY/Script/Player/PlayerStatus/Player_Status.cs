using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_Status//�ƹ��͵� ������� ���� ������ �÷��̾��� �⺻ ���ݷ�, ������ ����. �������, ����� ���, ���� �� �ٽ� ������ �� ���
{
    string name;
    uint level;
    uint baseATT;
    float base_DP;
    uint base_STR;
    uint base_INT;
    uint base_LUK;
    uint base_DEX;
    uint abilityPoint;

    //detail
    uint base_MaxHP;
    uint base_HP;
    uint base_Stamina;
    uint base_MaxStamina;
    uint damage;
    uint critical_Min;
    uint critical_Max;
    float criticalRate;
    float dodgeRate;
    //detail

    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnNameChange?.Invoke(name);
        }
    }
    public uint Level
    {
        get => level;
        set
        {
            level = value;
            OnLevelChange?.Invoke(level);
        }
    }
    public uint BaseMaxHP
    {
        get => base_MaxHP;
        set
        {
            base_MaxHP = value;
            OnMaxHPChange?.Invoke(base_MaxHP);
        }
    }
    public uint BaseMaxStamina
    {
        get => base_MaxStamina;
        set
        {
            base_MaxStamina = value;
            OnMaxStaminaChange?.Invoke(base_MaxStamina);
        }
    }
    public uint BaseATT
    {
        get => baseATT;
        set
        {
            baseATT = value;
            OnBaseATTChange?.Invoke(baseATT);
        }
    }
    public float BaseDP
    {
        get => base_DP;
        set
        {
            base_DP = value;
            OnBaseDPChange?.Invoke(base_DP);
        }
    }
    public uint BaseSTR
    {
        get => base_STR;
        set
        {
            base_STR = value;
            BaseDP += 0.8f;
            OnBaseSTRChange?.Invoke(base_STR);
        }
    }

    public uint BaseINT
    {
        get => base_INT;
        set
        {
            base_INT = value;
            OnBaseINTChange?.Invoke(base_INT);
        }
    }
    public uint BaseLUK
    {
        get => base_LUK;
        set
        {
            base_LUK = value;
            CriticalRate = base_LUK * 0.5f;
            OnBaseLUKChange?.Invoke(base_LUK);
        }
    }
    public uint BaseDEX
    {
        get => base_DEX;
        set
        {
            base_DEX = value;
            DodgeRate += base_DEX * 0.3f;
            OnBaseDEXChange?.Invoke(base_DEX);
        }
    }
    public uint AbilityPoint
    {
        get => abilityPoint;
        set
        {
            abilityPoint = value;
            OnAbilityPointChange?.Invoke(abilityPoint);
        }
    }
    public uint BaseHP
    {
        get => base_HP;
        set
        {
            base_HP = value;
            OnBaseHPChange?.Invoke(base_HP);
        }
    }
    public uint BaseStamina
    {
        get => base_Stamina;
        set
        {
            base_Stamina = value;
            OnBaseStaminaChange?.Invoke(base_Stamina);
        }
    }
    public uint Damage
    {
        get => damage;
        set
        {
            damage = value;
            OnDamageChange?.Invoke(damage);
        }
    }
    public uint CriticalMin
    {
        get => critical_Min;
        set
        {
            critical_Min = value;
            OnCriticalMinChange?.Invoke(critical_Min);
        }
    }
    public uint CriticalMax
    {
        get => critical_Max;
        set
        {
            critical_Max = value;
            OnCriticalMaxChange?.Invoke(critical_Max);
        }
    }
    public float CriticalRate
    {
        get => criticalRate;
        set
        {
            criticalRate = value;
            OnCriticalRateChange?.Invoke(criticalRate);
        }
    }
    public float DodgeRate
    {
        get => dodgeRate;
        set
        {
            dodgeRate = value;
            OnDodgeRateChange?.Invoke(dodgeRate);
        }
    }


    public Action<string> OnNameChange;
    public Action<uint> OnLevelChange;
    public Action<uint> OnMaxHPChange;
    public Action<uint> OnMaxStaminaChange;

    public Action<uint> OnBaseATTChange;
    public Action<float> OnBaseDPChange;
    public Action<uint> OnBaseSTRChange;
    public Action<uint> OnBaseINTChange;
    public Action<uint> OnBaseLUKChange;
    public Action<uint> OnBaseDEXChange;
    public Action<uint> OnAbilityPointChange;
    public Action<uint> OnBaseHPChange;
    public Action<uint> OnBaseStaminaChange;
    public Action<uint> OnDamageChange;
    public Action<uint> OnCriticalMinChange;
    public Action<uint> OnCriticalMaxChange;
    public Action<float> OnCriticalRateChange;
    public Action<float> OnDodgeRateChange;
    public void Init()
    {
        Name = "Player";
        BaseHP = 100;
        BaseStamina = 100;

        BaseATT = 10;
        BaseDP = 10;
        BaseSTR = 5;
        BaseINT = 5;
        BaseLUK = 5;
        BaseDEX = 5;
        AbilityPoint = 50;
    }
}
public class Player_Status : MonoBehaviour// , �������, �������� �÷��̾�� ��ȣ�޾Ƽ� Base_Status�� ���� ������Ʈ
                                          // ��ư�� ������ �� �� �������� Base_Status�� �ɷ�ġ�� ������Ʈ�ϴ� �Լ� �ۼ�
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
    TextMeshProUGUI maxHP_Text;
    TextMeshProUGUI maxStamina_Text;
    TextMeshProUGUI staminaText;
    TextMeshProUGUI damageText;
    TextMeshProUGUI criticalDamageText;
    TextMeshProUGUI criticalRateText;
    TextMeshProUGUI dodgeRateText;

    Button strButton;
    Button intButton;
    Button lukButton;
    Button dexButton;
    Button detailOpen_Button;

    Base_Status base_Status;



    private void Awake()
    {
        GetComponents();

        detailOpen_Button.onClick.AddListener(ToggleDetail_OpenClose);

        closeDetail();
        Close();
    }
    private void Start()
    {
        InputSystemController.Instance.On_StatusOpen = ToggleOpenClose;
        base_Status = new();
        base_Status.OnNameChange += (name) => nameText.text = name;
        base_Status.OnLevelChange += (level) => levelText.text = $"{level}";
        base_Status.OnBaseATTChange += (att) => attText.text = $"{att}";
        base_Status.OnBaseDPChange += (dp) => dpText.text = $"{dp:f1}";
        base_Status.OnBaseSTRChange += (str) => strText.text = $"{str}";
        base_Status.OnBaseINTChange += (intelligence) => intText.text = $"{intelligence}";
        base_Status.OnBaseLUKChange += (luk) => lukText.text = $"{luk}";
        base_Status.OnBaseDEXChange += (dex) => dexText.text = $"{dex}";
        base_Status.OnAbilityPointChange += (ap) => abilityPoint_Text.text = $"{ap}";
        base_Status.OnBaseHPChange += (hp) => hpText.text = $"{hp}";
        base_Status.OnMaxHPChange += (maxHP) => maxHP_Text.text =$"{maxHP}";
        base_Status.OnBaseStaminaChange += (stamina) => staminaText.text = $"{stamina}";
        base_Status.OnMaxStaminaChange += (maxStamina) => maxStamina_Text.text = $"{maxStamina}";
        base_Status.OnDamageChange += (damage) => damageText.text = $"{damage}";
        base_Status.OnCriticalMinChange += (critMin) => criticalDamageText.text = $"{critMin}"; // CriticalMin�� ǥ�õ� Text�� ������
        base_Status.OnCriticalMaxChange += (critMax) => criticalDamageText.text = $"{critMax}"; // CriticalMax�� ǥ�õ� Text�� ������
        base_Status.OnCriticalRateChange += (critRate) => criticalRateText.text = $"{critRate:f1}";
        base_Status.OnDodgeRateChange += (dodgeRate) => dodgeRateText.text = $"{dodgeRate:f1}";

        base_Status.Init();

        strButton.onClick.AddListener(RiseStrength);
        intButton.onClick.AddListener(RiseIntelligence);
        lukButton.onClick.AddListener(RiseLuck);
        dexButton.onClick.AddListener(Rise_Dexterity);
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
        maxHP_Text = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        staminaText = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        maxStamina_Text = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        damageText = transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalDamageText = transform.GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalRateText = transform.GetChild(2).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dodgeRateText = transform.GetChild(2).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        detailOpen_Button = transform.GetChild(1).GetChild(9).GetChild(0).GetChild(0).GetComponent<Button>();
        strButton = transform.GetChild(1).GetChild(4).GetChild(1).GetChild(1).GetComponent<Button>();
        intButton = transform.GetChild(1).GetChild(5).GetChild(1).GetChild(1).GetComponent<Button>();
        lukButton = transform.GetChild(1).GetChild(6).GetChild(1).GetChild(1).GetComponent<Button>();
        dexButton = transform.GetChild(1).GetChild(7).GetChild(1).GetChild(1).GetComponent<Button>();
    }
    void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    void OpenDetail()
    {
        detailCanvasGroup.alpha = 1.0f;
        detailCanvasGroup.blocksRaycasts = true;
        detailCanvasGroup.interactable = true;
    }
    void closeDetail()
    {
        detailCanvasGroup.alpha = 0;
        detailCanvasGroup.blocksRaycasts = false;
        detailCanvasGroup.interactable = false;
    }
    void ToggleOpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    void ToggleDetail_OpenClose()
    {
        if (detailCanvasGroup.alpha == 1)
        {
            closeDetail();
        }
        else
        {
            OpenDetail();
        }
    }

    void LevelUp()
    {

    }
    void Rise_Dexterity()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.BaseDEX++;
        }
    }
    void RiseStrength()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.BaseSTR++;
        }
    }
    void RiseIntelligence()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.BaseINT++;
        }
    }
    void RiseLuck()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.BaseLUK++;
        }
    }
}
