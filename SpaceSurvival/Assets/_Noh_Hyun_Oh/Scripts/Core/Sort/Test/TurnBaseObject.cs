using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/// <summary>
/// �⺻ ���̽��� ITurnBaseData �������̽�
/// ���� ����� ���ֵ��� �⺻ Ʋ.
/// ��Ʋ�ʿ��� ���� ����ϴ� ���ֵ�(PC,NPC)�� ITurnBaseData  �������̽��� �߰��� Base Ŭ���� ����� �װ��� ��ӹް� ����������Ѵ� .
/// �ƴϸ� �ش�Ŭ������ ��ӹް� �������ָ�ȴ� - �ش�Ŭ������ӽ� ITurnBaseData �� �߰���� ������Ѵ�.  �ش�Ŭ������ ����Ҽ��ֵ��� �������̴�.
/// 
/// SetParent ���� ���ǻ��� 
///     - �̵��� ��ü�� Ȱ��ȭ ���¿��� �̵��� transform�� �������� ������ü���� ��Ȱ��ȭ ���ִ°�� �ڵ����� OnDisable �� ȣ��ȴ� .
///     - Ȱ��ȭ ��ü -> ��Ȱ��ȭ �θ�  �ݴ�� ��Ȱ��ȭ ��ü -> Ȱ��ȭ �θ� ���� ���º�ȭ�� �ڵ����� �̷���� OnEnable�� OnDisable �Լ��� ȣ��ȴ�.
/// </summary>
public class TurnBaseObject : BattleMapUnitIsPool, ITurnBaseData
{
    /// <summary>
    /// ��Ʋ������ ����ũ�� Ű������ �����Ϸ����Ѵ�. - �̼���
    /// </summary>
    [SerializeField]
    int battleIndex = -1;
    public int UnitBattleIndex { get => battleIndex; set => battleIndex= value; }

    /// <summary>
    /// ���̰���� ȸ���� �ൿ�°� 
    /// </summary>
    [SerializeField]
    float turnAddValue = 0.50f;
    public float TurnAddValue 
    {
        get => turnAddValue;
        set 
        {
            turnAddValue = value;
        }
    }

    [SerializeField]
    /// <summary>
    /// �ൿ�� �ִ�ġ (ȸ�����Ѽ�)
    /// </summary>
    float maxTurnValue = 1.5f;
    public float MaxTurnValue 
    {
        get => maxTurnValue;
        protected set 
        {
            maxTurnValue = value;
        }
    }

    /// <summary>
    /// ���� ���� ���ప 
    /// �̰����� ���ƿü����� ���Ѵ�.
    /// </summary>
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
    /// �ϰ���� ȸ���� �ൿ�°�
    /// </summary>
    public float TurnEndActionValue => turnAddValue;


    Action<ITurnBaseData> turnEndAction;
    /// <summary>
    /// ������� ������ ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get=> turnEndAction; set =>  turnEndAction = value; }

    /// <summary>
    /// ������� �������� �����ϰų� �Ҷ� ȣ��� �Լ�
    /// </summary>
    Action<ITurnBaseData> turnRemove;
    public Action<ITurnBaseData> TurnRemove { get => turnRemove; set => turnRemove = value; }

    [SerializeField]
    /// <summary>
    /// ������ UI 
    /// </summary>
    private TrackingBattleUI battleUI = null;
    public TrackingBattleUI BattleUI
    {
        get => battleUI;
        set => battleUI = value;

    }

    /// <summary>
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform battleUICanvas;
    [SerializeField]
    /// <summary>
    /// �ϰ����� UI 
    /// </summary>
    private GaugeUnit gaugeUnit = null;
    public GaugeUnit GaugeUnit 
    {
        get => gaugeUnit;
        set 
        {
            gaugeUnit = value;
        }
    }
    /// <summary>
    /// �� UI�� ��� ĵ���� ��ġ
    /// </summary>
    Transform turnGaugeCanvas;

    private void Awake()
    {
        Transform windowList = FindObjectOfType<WindowList>().transform; //�̱����� �����ȵ��ִ»��¶� �˻��ؼ� �����´�.
        battleUICanvas = windowList.GetChild(0).GetChild(0);  // TrackingUI ���� ĵ������ġ
        turnGaugeCanvas = windowList.GetChild(0).GetChild(1).GetChild(0);// �� ������ ���� ĵ������ġ
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitUI(); 
    }

    /// <summary>
    /// ������ UI �ʱ�ȭ �Լ� ����
    /// </summary>
    public void InitUI()
    {
        if (battleIndex < 0) return; //�ε����� ��Ʋ�ʿ��� ��⶧���� �ε��� ���õǸ� UI�����Ѵ�.
        if (BattleUI == null) //������ UI�� ���þȵ������� 
        {//���� 
            GameObject obj = MultipleObjectsFactory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // ����ó�� �ʱ�ȭ�Ҷ� ��Ʋ UI �����ϰ� 
            obj.gameObject.name = $"{this.name} _ Tracking _ {battleIndex}"; //�̸�Ȯ�ο�
            obj.transform.SetParent(battleUICanvas);//Ǯ�� ĵ���� �ؿ����⶧���� ��Ʋ��UI�� ������ ĵ���� ��ġ ������ �̵���Ų��.
            obj.gameObject.SetActive(true); //Ȱ��ȭ ��Ų��.
            BattleUI = obj.GetComponent<TrackingBattleUI>(); //UI ���۳�Ʈ ã�ƿ´�.
            BattleUI.Player = this ;     //UI �� ���ְ� 1:1 ��ġ�� ���־�� ������ ��Ƶд�.
        }
        if (GaugeUnit == null) //�� ������UI �� ���� �ȵ�������
        {//����
            GameObject obj = MultipleObjectsFactory.Instance.
               GetObject(EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL);// Ǯ���� ������ �����´�.
            obj.gameObject.name = $"{this.name} _ TurnGauge _{battleIndex}"; //�̸�Ȯ�ο�
            obj.transform.SetParent(turnGaugeCanvas); //�θ���ġ ���� 
            obj.gameObject.SetActive(true); //Ȱ��ȭ ��Ų��.
            GaugeUnit = obj.GetComponent<GaugeUnit>(); //�� ���ֿ� ������ ���� ĳ�� 
            GaugeUnit.ProgressValue = TurnActionValue; //�ʱⰪ ����
        }
    }

    /// <summary>
    /// �������� ������ ������
    /// ���� �ʱ�ȭ ��Ű�� Ǯ�� ������ ť�� ������.
    /// </summary>
    public void ResetData()
    {
        //��Ʋ ���� UI�� ������ ������쵵 �߰��� �ʿ� - ��� ����ε��ư��� �߰�
        if (BattleUI != null) 
        {
            BattleUI.ResetData();// ������ UI �ʱ�ȭ 
            BattleUI = null; // ����
        }
        if (GaugeUnit != null) 
        {
            GaugeUnit.ResetData();// �� ������ UI  �ʱ�ȭ 
            GaugeUnit = null; // ����
        }
        turnEndAction = null;
        turnRemove = null;
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(PoolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
        UnitBattleIndex = -1;//�ε��� �ʱ�ȭ
    }

    /// <summary>
    /// �� ���۽� ������ �Լ� 
    /// </summary>
    public void TurnStartAction()
    {
        /*
         ���⿡ ��ɱ��� PC�� �׼� �������� Ȱ��ȭ�Ͽ� �̵� ,���� , ��ų �� �׼��� ���Ҽ��ְ� �ٲٰ� 
         NPC �� ���⿡ �ڵ������� �����Ͽ� �������� TrunEndAction?.Invoke(this); �� �����Ͽ� �ϸ޴����� ������� �ѱ��.
         
         */
        if(TurnActionValue > 1.0f)TurnActionValue -= UnityEngine.Random.Range(0.3f, 1.0f);// �ൿ�� �Ҹ��� 
        //Debug.Log($"TurnStartAction : {this} :{this.TurnActionValue}");
    }
}
