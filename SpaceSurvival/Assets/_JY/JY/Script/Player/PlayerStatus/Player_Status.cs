using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Player_;

public class Base_Status//�ƹ��͵� ������� ���� ������ �÷��̾��� �⺻ ���ݷ�, ������ ����. �������, ����� ���, ���� �� �ٽ� ������ ���� �����͸� Save, Load �� �� ���
{
    public uint AbilityPoint { get; set; }
    uint exp;

    public string Name { get; set; }

    public uint Level { get; set; }
    public uint Exp
    {
        get => exp;
        set
        {
            uint expOver = 0; ;
            if (value > ExpMax)
            {
                expOver = value - ExpMax;
            }
            exp = (uint)Mathf.Clamp(value, 0, ExpMax); 
            if (exp >= ExpMax)
            {
                OnLevelUp?.Invoke();
                Exp += expOver;
            }
            OnExpChange?.Invoke(exp);
        }
    }
    public uint ExpMax { get; set; }
    public uint Base_MaxHP { get; set; }
    public uint Base_HP { get; set; }
    public uint Base_Stamina { get; set; }
    public uint Base_MaxStamina { get; set; }

    public uint Base_ATT { get; set; }
    public uint Base_DP { get; set; }
    public uint Base_STR { get; set; }

    public uint Base_INT { get; set; }
    public uint Base_LUK { get; set; }
    public uint Base_DEX { get; set; }
    public uint Base_CriticalDamage { get; set; }

    public float Base_CriticalRate { get; set; }
    public float Base_DodgeRate { get; set; }

    public Action OnLevelUp;
    public Action<uint> OnExpChange;
   

    public void Init()
    {
        Name = "Player";
        Base_MaxHP = 100;
        Base_MaxStamina = 100;
        ExpMax = 50;
        Exp = 0;
        Base_ATT = 10;
        Base_DP = 10;
        Base_STR = 5;
        Base_INT = 5;
        Base_LUK = 5;
        Base_DEX = 5;
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
    TextMeshProUGUI expText;
    TextMeshProUGUI expMax_Text;

    Button strButton;
    Button intButton;
    Button lukButton;
    Button dexButton;
    Button detailOpen_Button;
    Player_ player;
    Base_Status base_Status;
    Equipments_Data_Server equipments_DataServer;
    EquipBox equipBox;

    string playerName;
    uint hp;
    uint stamina;
    uint str;
    uint intelligence;
    uint luk;
    uint dex;
    uint att;
    uint dp;
    uint criticalDamage;
    float criticalRate;
    uint abilityPoint;
    uint expMax;
    uint level;
    uint maxHP;
    uint maxStamina;
    float dodgeRate;


    public uint AbilityPoint
    {
        get => abilityPoint;
        set
        {
            abilityPoint = value;
        }
    }

    public string Name
    {
        get => playerName;
        set
        {
            playerName = value;
        }
    }

    public uint Level 
    {
        get => level;
        set
        {
            level = value;
        }
    }

    public uint ExpMax
    {
        get => expMax;
        set
        {
            expMax = value;
            // �߰� ����
        }
    }

    public uint MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
            // �߰� ����
        }
    }

    public uint MaxStamina
    {
        get => maxStamina;
        set
        {
            maxStamina = value;
            // �߰� ����
        }
    }
    public uint Stamina
    {
        get => stamina;
        set
        {
            stamina = value;
        }
    }
    public uint ATT
    {
        get => att;
        set
        {
            att = value;
            // �߰� ����
        }
    }

    public uint DP
    {
        get => dp;
        set
        {
            dp = value;
            // �߰� ����
        }
    }

    public uint STR
    {
        get => str;
        set
        {
            str = value;
            // �߰� ����
        }
    }

    public uint INT
    {
        get => intelligence;
        set
        {
            intelligence = value;
            // �߰� ����
        }
    }

    public uint LUK
    {
        get => luk;
        set
        {
            luk = value;
            // �߰� ����
        }
    }

    public uint DEX
    {
        get => dex;
        set
        {
            dex = value;
            // �߰� ����
        }
    }

    public uint HP
    {
        get => hp;
        set
        {
            hp = value;
            // �߰� ����
        }
    }

    public uint CriticalDamage
    {
        get => criticalDamage;
        set
        {
            criticalDamage = value;
            // �߰� ����
        }
    }

    public float CriticalRate
    {
        get => criticalRate;
        set
        {
            criticalRate = value;
            // �߰� ����
        }
    }

    public float DodgeRate
    {
        get => dodgeRate;
        set
        {
            dodgeRate = value;
            // �߰� ����
        }
    }
 
    //int



    private void Awake()
    {
        GetComponents();

        detailOpen_Button.onClick.AddListener(ToggleDetail_OpenClose);
        strButton.onClick.AddListener(RiseStrength);
        intButton.onClick.AddListener(RiseIntelligence);
        lukButton.onClick.AddListener(RiseLuck);
        dexButton.onClick.AddListener(Rise_Dexterity);
        closeDetail();
        Close();
    }
    private void Start()
    {
        Init();

    }

    private void Init()
    {
        InputSystemController.Instance.On_StatusOpen = ToggleOpenClose;
        player = GameManager.Player_;
        equipBox = GameManager.EquipBox;
        equipments_DataServer = new(equipBox);
        base_Status = new();
        base_Status.OnLevelUp += LevelUp;

        base_Status.Init();
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
        expText = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        expMax_Text = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        hpText = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        maxHP_Text = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        staminaText = transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        maxStamina_Text = transform.GetChild(2).GetChild(2).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        damageText = transform.GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalDamageText = transform.GetChild(2).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalRateText = transform.GetChild(2).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dodgeRateText = transform.GetChild(2).GetChild(6).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
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
        base_Status.Level++;
        base_Status.AbilityPoint += 5;
        base_Status.Base_MaxHP += increaseMaxHP();
        base_Status.Base_HP = base_Status.Base_MaxHP;
        base_Status.Base_MaxStamina += increaseMaxStamina();
        base_Status.Base_Stamina = base_Status.Base_MaxStamina;
        base_Status.Exp = 0;
        base_Status.ExpMax = (uint)(base_Status.ExpMax * 1.2f);
    }

    // totalATT, TotalDP ���� ������Ʈ�ϴ� �Լ� ����
    //@@�۾��� �� this.ATT = base_Status.base_ATT + equipments_Total_ATT_DP.Total_ATT;//�÷��̾��� ���ݷ� = �⺻���ݷ� + �������۵��� ���ݷ� �� ��
    // this.DP = base_Status.base_DP + equipments_Total_ATT_DP.Total_DP;
    public void Reset_Status()
    {
        equipments_DataServer = equipments_DataServer.GetEquipments_Total_ATT_DP();
        this.ATT = equipments_DataServer.Total_ATT + base_Status.Base_ATT;
        this.DP = equipments_DataServer.Total_DP + base_Status.Base_DP;
    }

    uint increaseMaxHP()
    {
        float increaseBase= 10;
        float result = increaseBase + STR;
        return (uint)result;
    }
    uint increaseMaxStamina()
    {
        float increaseBase = 1;
        float result = increaseBase + (INT * 0.5f);
        return (uint)result;
    }
    void Rise_Dexterity()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.Base_DEX++;
        }
    }
    void RiseStrength()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.Base_STR++;
        }
    }
    void RiseIntelligence()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.Base_INT++;
        }
    }
    void RiseLuck()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            base_Status.Base_LUK++;
        }
    }
    public void GetExp(uint exp)
    {
        base_Status.Exp += exp;
    }
}
