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
        hpText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // ������
        hpMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>() ; // �ִ���
        child = transform.GetChild(1);  //stm
        stmText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // ���罺�¹̳�
        stmMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // �ִ뽺�¹̳�
        stmSlider = child.GetChild(0).GetComponent<Slider>();
        child = transform.GetChild(2); //�����̻� 
        int childCount = child.childCount;
        imgIconArray = new Image[childCount];
        stateTimer = new Image[childCount];
        Image image;
        for (int i = 0; i < childCount; i++)
        {
            image = child.GetChild(i).GetComponent<Image>();
            imgIconArray[i] = image; //�����ð� ������ �̹��� ��ü 
            stateTimer[i] = child.GetChild(i).GetChild(0).GetComponent<Image>(); //�̹��� ������ ������ ��������Ʈ ��ü �������� 
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
        isHp_Change = true; //�ڷ�ƾ ������ ����Ǵ°��� �����ϱ����� üũ
        if (change_HpValue > hp_UI_Value) //ȸ�� 
        {
            hpMaxText.text = $"{maxValue}";

            while (hp_UI_Value < change_HpValue) //���°����� ������ ��ġ��Ӻ���
            {
                hp_UI_Value += Time.deltaTime * hpGaugeSpeed; //�ε巴��~
                hpText.text = $"{hp_UI_Value:f0}";
                hpSlider.value = hp_UI_Value /maxValue;
                yield return null;
            }
            hpText.text = $"{change_HpValue:f0}";
            hpSlider.value = change_HpValue / maxValue;
            hp_UI_Value = change_HpValue;  

        }
        else if (change_HpValue < hp_UI_Value) //������  
        {
            hpMaxText.text = $"{maxValue}";

            while (hp_UI_Value > change_HpValue) //���°����� ������ ��ġ��Ӻ���
            {
                hp_UI_Value -= Time.deltaTime * hpGaugeSpeed; //�ε巴��~
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
    /// ���׹̳� UI ������ �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator Stm_GaugeSetting(float change_StmValue, float maxValue)
    {
        isStm_Change = true; //�ڷ�ƾ ������ ����Ǵ°��� �����ϱ����� üũ
        if (change_StmValue > stm_UI_Value) //ȸ�� 
        {
            stmMaxText.text = $"{maxValue}";

            while (stm_UI_Value < change_StmValue) //���°����� ������ ��ġ��Ӻ���
            {
                stm_UI_Value += Time.deltaTime * stmGaugeSpeed; //�ε巴��~
                stmText.text = $"{stm_UI_Value:f0}";
                stmSlider.value = stm_UI_Value /maxValue;
                yield return null;
            }
            stmText.text = $"{change_StmValue:f0}";
            stmSlider.value = change_StmValue / maxValue;
            stm_UI_Value = change_StmValue;

        }
        else if (change_StmValue < stm_UI_Value) //������  
        {
            stmMaxText.text = $"{maxValue}";

            while (stm_UI_Value > change_StmValue) //���°����� ������ ��ġ��Ӻ���
            {
                stm_UI_Value -= Time.deltaTime * stmGaugeSpeed; //�ε巴��~
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
    //    if (changeValue > currentValue) //����
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
    //        Debug.Log($"���� : {slider.transform.parent.name} ���� : {changeValue}");
    //    }
    //    else if (currentValue > changeValue) //����
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
    //        Debug.Log($"���� : {slider.transform.parent.name} ���� : {changeValue}");
    //    }
    //    else 
    //    {
    //        Debug.Log($"���� : {slider.transform.parent.name} ���� : {changeValue}");
    //        slider.value = changeValue;
    //        currentValueText.text = $"{changeValue:f0}";
    //        maxValueText.text = $"{maxValue}";
    //    }
    //}

}
