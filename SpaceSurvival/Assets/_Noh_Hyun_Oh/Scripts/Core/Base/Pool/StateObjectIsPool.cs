using EnumList;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateObjectIsPool : ObjectIsPool, IStateData
{
    /// <summary>
    /// ���̵� �Ǵ� �����̻��� ��������� Ǯ�� �������ֵ��� ��ġ�� ����
    /// </summary>
    Transform poolTransform;
    public Transform PoolTransform 
    {
        protected get => poolTransform;
        set 
        {
            if (poolTransform == null) //Ǯ��ġ�� ó�� �����ɶ� �ѹ��� ����  
            {
                poolTransform = value;
            }
        }
    }
    /// <summary>
    /// �����̻��� ����
    /// </summary>
    StateType stateType;
    public StateType Type 
    {
        get => stateType;
        set 
        {
            stateType = value;
        }
    }


    /// <summary>
    /// �����̻��� ������ �̹���
    /// </summary>
    private Sprite icon; 
    public Sprite Icon => icon;

    [SerializeField]
    [Range(0.0f,1.0f)]
    /// <summary>
    /// ���ϴ� ���ҵǴ� ��ġ 
    /// </summary>
    private float reducedDuration = 0.5f;
    public float ReducedDuration { get => reducedDuration; set => reducedDuration = value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// �����̻��� �ִ� ���ӽð�
    /// </summary>
    private float maxDuration = 1.0f;
    public float MaxDuration { get => maxDuration; set => maxDuration =value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// ���� ���ӽð�
    /// </summary>
    private float currentDuration = 0.0f;
    /// <summary>
    /// 0���� ���� �����ϸ鼭 MaxDuration ������ ũ�� �Ǹ� �����ȴ�.
    /// </summary>
    public float CurrentDuration { 
        get => currentDuration;

        set 
        {
            if (currentDuration != value) //���� ���������
            {
                if (value > MaxDuration) //���ӽð�üũ �ٵǸ�
                {
                    ResetData();//�ʱ�ȭ �ϰ�
                    return; //���ư���.
                }

                // ���ӽð� �����ִ°�쿣 
                if (value < 0.0f) // 0 ���Ϸδ� ������ �ȵǾ������� üũ
                {
                    currentDuration = 0.0f; 

                }
                else //�������ΰ��̸� 
                {
                    currentDuration = value; //�������ϰ� 
                }

                FillAmoutSetting(currentDuration); //������ ����
            }
            
        }
    }
    /// <summary>
    /// ������ ������ �̹���
    /// </summary>
    Image gaugeImg;
    /// <summary>
    /// �������ѹ����ϱ����� ���� �̸������ ����
    /// </summary>
    float computationalScale = -1.0f;
    private void Awake()
    {
        gaugeImg = GetComponent<Image>();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        gaugeImg.fillAmount = 0.0f;// �ʱ�ȭ 
        computationalScale = (MaxDuration / 1); //���� �̸����صα�
    }
    /// <summary>
    /// UI ������ �Լ� 
    /// </summary>
    /// <param name="value">������ ��</param>
    private void FillAmoutSetting(float value) 
    {
        gaugeImg.fillAmount = computationalScale * value; //�̸����ص� ������ ���� 
    }
    /// <summary>
    /// �ʱ�ȭ �۾�
    /// </summary>
    public void ResetData() 
    {
        //���°� �ʱ�ȭ�ϰ� 
        Type = StateType.None; //�����̻� ���� �ʱ�ȭ
        reducedDuration = -1.0f; //���ϴ� ����� ���ʱ�ȭ 
        maxDuration = -1.0f; // �ִ�ġ �ʱ�ȭ
        currentDuration = -1.0f; //���� ���ప �ʱ�ȭ
        computationalScale = -1.0f;//���� �ʱ�ȭ
        gaugeImg.fillAmount = 1.0f;// ������ �ʱ�ȭ 
        transform.SetParent(poolTransform); //Ǯ�� ������.
        gameObject.SetActive(false); //ť�� �ٽ� �ֱ����� ��Ȱ��ȭ 
        /*
         transform.SetParent �Լ��� ��ü�� Ȱ��ȭ ��Ȱ��ȭ ��ü�Ǵ½������� ���ɰ�� ����������.
         �׷��� Ȱ��ȭüũ���Ͽ� SetParent �� �����Ѵ�.
         ����Ȯ�� : ������Ʈ�� Ȱ��ȭ �����϶� gameobject.SetActive(false)������ �ٷ�  transform.SetParent(�θ�) �� �����ϴ� ������.
         */
    }

   
}
