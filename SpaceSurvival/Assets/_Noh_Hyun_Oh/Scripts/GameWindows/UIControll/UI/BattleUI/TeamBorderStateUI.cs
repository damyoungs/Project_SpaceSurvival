using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamBorderStateUI : MonoBehaviour
{
    Slider hpSlider;
    public Slider HpSlider => hpSlider;

    TextMeshProUGUI hpText;
    public TextMeshProUGUI HpText => hpText;
    TextMeshProUGUI hpMaxText;
    public TextMeshProUGUI HpMaxText => hpMaxText;

    Slider stmSlider;
    public Slider StmSlider => stmSlider;

    TextMeshProUGUI stmText;
    public TextMeshProUGUI StmText => stmText;
    TextMeshProUGUI stmMaxText;
    public TextMeshProUGUI StmMaxText =>stmMaxText;

    [SerializeField]
    Image[] stateTimer;
    public Image[] StateIconTimer => stateTimer;
    
    [SerializeField]
    Image[] imgIconArray;
    public Image[] StateIconImg => imgIconArray;

    private void Awake()
    {
        Transform child = transform.GetChild(0); //hp
        hpSlider = child.GetComponent<Slider>();
        hpText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // ������
        hpMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>() ; // �ִ���
        child = transform.GetChild(1);  //stm
        stmText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // ���罺�¹̳�
        stmMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // �ִ뽺�¹̳�
        stmSlider = child.GetComponent<Slider>();
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


}
