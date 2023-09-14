using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamBorderStateUI : MonoBehaviour
{
    [SerializeField]
    float hpGaugeSpeed = 10.0f;
    [SerializeField]
    float stmGaugeSpeed = 2.0f;

    bool isHp_Change = false;
    bool isStm_Change = false;

    float hp_UI_Value = -1.0f;
    float stm_UI_Value = -1.0f;

    Slider hpSlider;
    TextMeshProUGUI hpText;
    TextMeshProUGUI hpMaxText;

    Slider stmSlider;
    TextMeshProUGUI stmText;
    TextMeshProUGUI stmMaxText;

    [SerializeField]
    Image[] stateTimer;
    public Image[] StateIconTimer => stateTimer;
    
    [SerializeField]
    Image[] imgIconArray;
    public Image[] StateIconImg => imgIconArray;

    IEnumerator hpGauge;
    IEnumerator stmGauge;


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
        Image image;
        for (int i = 0; i < childCount; i++)
        {
            image = child.GetChild(i).GetComponent<Image>();
            imgIconArray[i] = image; //남은시간 보여줄 이미지 객체 
            stateTimer[i] = child.GetChild(i).GetChild(0).GetComponent<Image>(); //이미지 아이콘 보여줄 스프라이트 객체 가져오기 
        }
    }
    public void SetStmGaugeAndText(float changeValue , float maxValue) 
    {
        //stmGauge = GaugeView(stmSlider,stmText,stmMaxText,changeValue,maxValue);
        stmGauge = Stm_GaugeSetting(changeValue,maxValue);
        StartCoroutine(stmGauge);

    } 
    public void SetHpGaugeAndText(float changeValue ,float maxValue)
    {
        //hpGauge = GaugeView(hpSlider,hpText,hpMaxText,changeValue,maxValue);
        hpGauge = HP_GaugeSetting(changeValue,maxValue);
        StartCoroutine(hpGauge);
    
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
                hpSlider.value = hp_UI_Value /maxValue;
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
                stmSlider.value = stm_UI_Value /maxValue;
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

    //IEnumerator GaugeView(Slider  slider ,TextMeshProUGUI currentValueText, TextMeshProUGUI maxValueText  , float changeValue ,float maxValue , bool isHp = false) 
    //{
    //    float timeElaspad = 0.0f;
    //    float currentValue = isHp ? viewHpValue : viewStmValue;
    //    if (changeValue > currentValue) //증가
    //    {

    //        while (changeValue > timeElaspad)
    //        {
    //            timeElaspad += Time.deltaTime * gaugeSpeed;
    //            yield return null;
    //            currentValueText.text = $"{timeElaspad:f0}";
    //            maxValueText.text = $"{maxValue}";
    //            slider.value = timeElaspad;
    //        }
    //        slider.value = changeValue;
    //        currentValueText.text = $"{changeValue:f0}";
    //        maxValueText.text = $"{maxValue}";
    //        Debug.Log($"증가 : {slider.transform.parent.name} 값은 : {changeValue}");
    //    }
    //    else if (currentValue > changeValue) //감소
    //    {
    //        while (changeValue < timeElaspad)
    //        {
    //            timeElaspad -= Time.deltaTime * gaugeSpeed;
    //            yield return null;
    //            currentValueText.text = $"{timeElaspad:f0}";
    //            maxValueText.text = $"{maxValue}";
    //            slider.value = timeElaspad;
    //        }
    //        slider.value = changeValue;
    //        currentValueText.text = $"{changeValue:f0}";
    //        maxValueText.text = $"{maxValue}";
    //        Debug.Log($"감소 : {slider.transform.parent.name} 값은 : {changeValue}");
    //    }
    //    else 
    //    {
    //        Debug.Log($"같다 : {slider.transform.parent.name} 값은 : {changeValue}");
    //        slider.value = changeValue;
    //        currentValueText.text = $"{changeValue:f0}";
    //        maxValueText.text = $"{maxValue}";
    //    }
    //}

}
