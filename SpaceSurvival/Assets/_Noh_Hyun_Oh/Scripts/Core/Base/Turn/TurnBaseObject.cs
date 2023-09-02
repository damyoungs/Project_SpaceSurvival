using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;
/// <summary>
/// �⺻ ���̽��� ITurnBaseData �������̽�
/// ���� ����� ���ֵ��� �⺻ Ʋ.
/// ��Ʋ�ʿ��� ���� ����ϴ� ���ֵ�(PC,NPC)��  ������ �����ؼ� �����Ѵ�.
/// 
/// SetParent ���� ���ǻ��� 
///     - �̵��� ��ü�� Ȱ��ȭ ���¿��� �̵��� transform�� �������� ������ü���� ��Ȱ��ȭ ���ִ°�� �ڵ����� OnDisable �� ȣ��ȴ� .
///     - Ȱ��ȭ ��ü -> ��Ȱ��ȭ �θ�  �ݴ�� ��Ȱ��ȭ ��ü -> Ȱ��ȭ �θ� ���� ���º�ȭ�� �ڵ����� �̷���� OnEnable�� OnDisable �Լ��� ȣ��ȴ�.
/// </summary>
public class TurnBaseObject : BattleMapTurnUnit_PoolObj, ITurnBaseData
{
    /// <summary>
    /// �ʿ� ������ ���� ����� ������Ʈ�� �ε���  �Ŵ������� �ش� ������Ʈ �����Ҷ� �ε����� �ٰ��̴�.
    /// </summary>
    [SerializeField]
    protected int battleIndex = -1;
    public int UnitBattleIndex 
    {
        get => battleIndex;
        set => battleIndex = value;
    }

    /// <summary>
    /// ��Ʋ �ε��� ������ ��������Ʈ
    /// </summary>
    public Func<int> GetBattleIndex;


    /// <summary>
    /// ���̰���� ȸ���� �ൿ�°� 
    /// </summary>
    [SerializeField]
    [Range(0.01f,0.5f)]
    protected float turnAddValue = 0.50f;
    public float TurnEndActionValue => turnAddValue;

    /// <summary>
    /// �ൿ�� �ִ�ġ (ȸ�����Ѽ�)
    /// </summary>
    [SerializeField]
    [Range(1.0f, 2.0f)]
    protected float maxTurnValue = 1.5f;

    /// <summary>
    /// ���� ���� ���ప 
    /// �̰����� ���ƿü����� ���Ѵ�.
    /// </summary>
    [SerializeField]
    [ReadOnly]
    float turnWaitingValue = 1.0f;
    public float TurnActionValue
    {
        get => turnWaitingValue;
        set
        {
            turnWaitingValue = value;
            if (turnWaitingValue < 0.0f) // ���� �ּҰ��� �����ְ�
            {
                turnWaitingValue = 0.0f; 
            } else if (turnWaitingValue > maxTurnValue) //�ִ�� ���ü��ִ°��� ��������  
            {
                turnWaitingValue = maxTurnValue;
            }
            if (gaugeUnit != null)  // �ϰ������� ���õ������� 
            {
                gaugeUnit.ProgressValue = value; //�ϰ��������� ���� 

            }
        }
    }

    /// <summary>
    /// ������� ������ ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set ; }


    /// <summary>
    /// ��Ʋ�ʿ��� Ư�� ���׷� �� ������ ���ּ��� ���ٸ� �� ���൵ �ȵǾ������� ó���� ���� ���� 
    /// </summary>
    public Action<ITurnBaseData> TurnRemove { get; set; }


    /// <summary>
    /// ���� �ش� ���� ������ ���ֵ�
    /// </summary>
    protected List<ICharcterBase> charcterList;
    public List<ICharcterBase> CharcterList => charcterList;

    /// <summary>
    /// �ϰ����� UI 
    /// </summary>
    TurnGaugeUnit gaugeUnit = null;
    public TurnGaugeUnit GaugeUnit => gaugeUnit;
   
    /// <summary>
    /// �� UI�� ��� ĵ���� ��ġ
    /// </summary>
    Transform turnGaugeCanvas;

    
    protected override void Awake()
    {
        base.Awake();
        turnGaugeCanvas = FindObjectOfType<WindowList>().transform.
                            GetChild(0).
                            GetChild(WindowList.Instance.transform.GetChild(0).childCount - 1).
                            GetChild(0); // �� ������ ���� ĵ������ġ
        charcterList = new(); //����Ʈ �ʱ�ȭ 
    }
    /// <summary>
    /// Ȱ��ȭ�� �ϰ����� �����ϰ� Ȱ��ȭ 
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        //����Ÿ���� �ȸ¾Ƽ� üũ�ϴ� ���ۿ�..
        if (gaugeUnit != null) //������ �������� 
        {
            gaugeUnit.gameObject.SetActive(true); //Ű��.
        }
        else if (turnGaugeCanvas != null)// ĵ���� ���� Ȥ�� ������ ��������  
        {
            TurnGaugeInit();//������ ����
        }
    }

    /// <summary>
    /// �������� ������ ������
    /// ���� �ʱ�ȭ ��Ű�� Ǯ�� ������ ť�� ������.
    /// ������ OnDisable �Լ��ȿ� �����Ű�� �ȵȴ� . SetParent �� ������ �߻��Ұ��̴�.
    /// </summary>
    public virtual void ResetData()
    {
        name = "";
        gaugeUnit.ResetData();// �� ������ UI  �ʱ�ȭ 
        gaugeUnit = null; // �� ������ UI ����
        TurnEndAction = null; //������ �׼ǵ� ����
        GetBattleIndex = null; // ������ ����
        TurnRemove = null; //�ش������ֿ��� �����Ǵ� ���ֵ鵵 �����.
        battleIndex = -1;//�ε��� �ʱ�ȭ
        foreach (ICharcterBase charcter in charcterList)//ĳ���� ���õ������� 
        {
            charcter.ResetData(); //������ �ʱ�ȭ �����Ű�� 
        }
        charcterList.Clear(); //���ο� �ʱ�ȭ 
        gameObject.SetActive(false); // ť�� ������.
    }

    /// <summary>
    /// �� ���۽� ������ �Լ� base���ִ°� �׽�Ʈ���̴� �����ε� �Ҷ� �Ƚᵵ�ȴ�.
    /// </summary>
    public virtual void TurnStartAction()
    {
        /*
         �ش� ���� �Ͻ��۽� ó���� ���� ȭ�鿡 UI�����شٵ簡 ī�޶� �̵���Ų�ٵ簡  ���
         
         */
        if(TurnActionValue > 1.0f)TurnActionValue -= UnityEngine.Random.Range(0.3f, 1.0f);// �ൿ�� �Ҹ��� �׽�Ʈ �� 
    }



    /// <summary>
    /// �����Ѵٰ������� ��ɸ������� �ϴ� ������ ��Ų��. 
    /// UI�ϴ� �ϰ������� ���� UI ������Ʈ �����ϱ�
    /// </summary>
    private void TurnGaugeInit()
    {
        gaugeUnit = (TurnGaugeUnit)Multiple_Factory.Instance.
           GetObject(EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL);// Ǯ���� ������ �����´�.
        gaugeUnit.gameObject.name = $"{this.name} _ TurnGauge "; //������Ʈ�� �̸� �־�д� ����Ή���� Ȯ���ϱ⽱��.
        gaugeUnit.transform.SetParent(turnGaugeCanvas); //�θ���ġ ���� 
        gaugeUnit.gameObject.SetActive(true); //Ȱ��ȭ ��Ų��.
        gaugeUnit.ProgressValue = TurnActionValue; //�ʱⰪ ����
    }

    /// <summary>
    /// ���ʱ⿡ ������ ���������� ������ �Լ�
    /// </summary>
    public virtual void InitData(){}
}
