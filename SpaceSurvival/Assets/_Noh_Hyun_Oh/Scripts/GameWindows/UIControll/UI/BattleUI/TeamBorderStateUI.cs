using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamBorderStateUI : MonoBehaviour
{
    [SerializeField]
    float hpGaugeSpeed = 10.0f;
    [SerializeField]
    float stmGaugeSpeed = 2.0f;
    [SerializeField]
    float expGaugeSpeed = 2.0f;

    bool isHp_Change = false;
    bool isStm_Change = false;
    bool isExp_Change = false;

    float hp_UI_Value = -1.0f;
    float stm_UI_Value = -1.0f;
    float exp_UI_Value = -1.0f;

    Slider hpSlider;
    TextMeshProUGUI hpText;
    TextMeshProUGUI hpMaxText;

    Slider stmSlider;
    TextMeshProUGUI stmText;
    TextMeshProUGUI stmMaxText;


    Slider expSlider;
    TextMeshProUGUI expText;
    TextMeshProUGUI expMaxText;


    [SerializeField]
    int buffMaxSize = 4;

    /// <summary>
    /// 버프타입
    /// </summary>
    [SerializeField]
    SkillType[] buffType;

    /// <summary>
    /// 타이머 조절할 이미지
    /// </summary>
    [SerializeField]
    Image[] stateTimer;
    public Image[] StateIconTimer => stateTimer;
    
    /// <summary>
    /// 버프 아이콘 보여줄 이미지
    /// </summary>
    [SerializeField]
    Image[] imgIconArray;
    public Image[] StateIconImg => imgIconArray;

    List<SkillData> state_UI_Datas;
    IEnumerator hpGauge;
    IEnumerator stmGauge;
    IEnumerator expGauge;


    private void Awake()
    {
        Transform child = transform.GetChild(0); //hp
        hpSlider = child.GetChild(0).GetComponent<Slider>();
        hpText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // 현재피
        hpMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>() ; // 최대피
        
        child = transform.GetChild(1);  //stm
        stmText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // 현재스태미나
        stmMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // 최대스태미나
        stmSlider = child.GetChild(0).GetComponent<Slider>();
        
        child = transform.GetChild(2); //상태이상 
        int childCount = child.childCount;
        imgIconArray = new Image[childCount];
        stateTimer = new Image[childCount];
        state_UI_Datas = new List<SkillData>(childCount);

        child.gameObject.SetActive(false);      //상태이상안쓰고 경험치로 바꾸기위해 가리기
        
        Image image;
        for (int i = 0; i < childCount; i++)
        {
            image = child.GetChild(i).GetChild(0).GetComponent<Image>(); //이미지 아이콘 보여줄 스프라이트 객체 가져오기 
            stateTimer[i] = child.GetChild(i).GetComponent<Image>();
            imgIconArray[i] = image; //남은시간 보여줄 이미지 객체 
            
        }
        hpGauge = HP_GaugeSetting(0.0f, 0.0f);
        stmGauge = Stm_GaugeSetting(0.0f, 0.0f);
        expGauge = Exp_GaugeSetting(0.0f, 0.0f);

        child = transform.GetChild(3);  //exp
        expText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // 현재경험치
        expMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // 최대경험치
        expSlider = child.GetChild(0).GetComponent<Slider>();

    }

    public void SetStmGaugeAndText(float changeValue , float maxValue) 
    {
        StopCoroutine(stmGauge);
        stmGauge = Stm_GaugeSetting(changeValue,maxValue);
        StartCoroutine(stmGauge);

    }

    public void SetHpGaugeAndText(float changeValue, float maxValue)
    {
        StopCoroutine(hpGauge);
        hpGauge = HP_GaugeSetting(changeValue, maxValue);
        StartCoroutine(hpGauge);

    }

    public void SetExpGaugeAndText(float changeValue, float maxValue)
    {
        StopCoroutine(expGauge);
        expGauge = Exp_GaugeSetting(changeValue, maxValue);
        StartCoroutine(expGauge);

    }

    IEnumerator HP_GaugeSetting(float change_HpValue,float maxValue)
    {
        isHp_Change = true; //코루틴 여러번 실행되는것을 방지하기위해 체크
        if (change_HpValue > hp_UI_Value) //회복 
        {
            hpMaxText.text = $"{maxValue}";

            while (hp_UI_Value < change_HpValue) //들어온값보다 작으면 수치계속변경
            {
                hp_UI_Value += Time.deltaTime * hpGaugeSpeed; //부드럽게~
                hpText.text = $"{hp_UI_Value:f0}";
                hpSlider.value = hp_UI_Value / maxValue;
                yield return null;
            }
            hpText.text = $"{change_HpValue:f0}";
            hpSlider.value = change_HpValue / maxValue;
            hp_UI_Value = change_HpValue;  

        }
        else if (change_HpValue < hp_UI_Value) //데미지  
        {
            hpMaxText.text = $"{maxValue}";

            while (hp_UI_Value > change_HpValue) //들어온값보다 작으면 수치계속변경
            {
                hp_UI_Value -= Time.deltaTime * hpGaugeSpeed; //부드럽게~
                hpText.text = $"{hp_UI_Value:f0}";
                hpSlider.value = hp_UI_Value / maxValue;
                yield return null;
            }
            hpText.text = $"{change_HpValue:f0}";
            hpSlider.value = change_HpValue / maxValue;
            hp_UI_Value = change_HpValue;
        }
        isHp_Change = false;
    }

    /// <summary>
    /// 스테미나 UI 조절용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Stm_GaugeSetting(float change_StmValue, float maxValue)
    {
        isStm_Change = true; //코루틴 여러번 실행되는것을 방지하기위해 체크
        if (change_StmValue > stm_UI_Value) //회복 
        {
            stmMaxText.text = $"{maxValue}";

            while (stm_UI_Value < change_StmValue) //들어온값보다 작으면 수치계속변경
            {
                stm_UI_Value += Time.deltaTime * stmGaugeSpeed; //부드럽게~
                stmText.text = $"{stm_UI_Value:f0}";
                stmSlider.value = stm_UI_Value / maxValue;
                yield return null;
            }
            stmText.text = $"{change_StmValue:f0}";
            stmSlider.value = change_StmValue / maxValue;
            stm_UI_Value = change_StmValue;

        }
        else if (change_StmValue < stm_UI_Value) //데미지  
        {
            stmMaxText.text = $"{maxValue}";

            while (stm_UI_Value > change_StmValue) //들어온값보다 작으면 수치계속변경
            {
                stm_UI_Value -= Time.deltaTime * stmGaugeSpeed; //부드럽게~
                stmText.text = $"{stm_UI_Value:f0}";
                stmSlider.value = stm_UI_Value / maxValue;
                yield return null;
            }
            stmText.text = $"{change_StmValue:f0}";
            stmSlider.value = change_StmValue / maxValue;
            stm_UI_Value = change_StmValue;
        }
        isStm_Change = false;
    }
    /// <summary>
    /// 경험치 UI 조절용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Exp_GaugeSetting(float change_ExpValue, float maxValue)
    {
        isExp_Change = true; //코루틴 여러번 실행되는것을 방지하기위해 체크
        if (change_ExpValue > exp_UI_Value) //회복 
        {
            expMaxText.text = $"{maxValue}";

            while (exp_UI_Value < change_ExpValue) //들어온값보다 작으면 수치계속변경
            {
                exp_UI_Value += Time.deltaTime * expGaugeSpeed; //부드럽게~
                expText.text = $"{exp_UI_Value:f0}";
                expSlider.value = exp_UI_Value / maxValue;
                yield return null;
            }
            expText.text = $"{change_ExpValue:f0}";
            expSlider.value = change_ExpValue / maxValue;
            exp_UI_Value = change_ExpValue;

        }
        else if (change_ExpValue < exp_UI_Value) //데미지  
        {
            expMaxText.text = $"{maxValue}";

            while (exp_UI_Value > change_ExpValue) //들어온값보다 작으면 수치계속변경
            {
                exp_UI_Value -= Time.deltaTime * expGaugeSpeed; //부드럽게~
                expText.text = $"{exp_UI_Value:f0}";
                expSlider.value = exp_UI_Value / maxValue;
                yield return null;
            }
            expText.text = $"{change_ExpValue:f0}";
            expSlider.value = change_ExpValue / maxValue;
            exp_UI_Value = change_ExpValue;
        }
        isExp_Change = false;
    }


    // 상태이상은 진짜 .. 기존에짜둔거 수정하기 너무 양이많아서 하드코딩이많다.. 어짜피 한개뿐이라..
    /// <summary>
    /// 버프 UI 셋팅 
    /// </summary>
    /// <param name="i">UI 순번</param>
    /// <param name="sprite">상태이상 아이콘</param>
    public void AddState(SkillData skill)
    {
        state_UI_Datas.Add(skill);
        imgIconArray[0].sprite = skill.skill_sprite;
        stateTimer[0].fillAmount = 0.0f;
    }

    public void TrunActionValueSetting(float value) 
    {
        stateTimer[0].fillAmount = value;
    }
    /// <summary>
    /// 버프 UI 초기화
    /// </summary>
    /// <param name="i">UI 순번</param>
    /// <param name="sprite">상태이상 아이콘</param>
    public void RemoveState()
    {
        state_UI_Datas.Clear();
        imgIconArray[0].sprite = null;
        stateTimer[0].fillAmount = 0.0f;
    }
}
