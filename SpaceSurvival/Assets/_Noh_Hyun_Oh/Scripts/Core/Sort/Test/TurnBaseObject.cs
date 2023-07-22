using System;
using UnityEngine;
/// <summary>
/// �⺻ ���̽��� ITurnBaseData �������̽�
/// ���� ����� ���ֵ��� �⺻ Ʋ.
/// ��Ʋ�ʿ��� ���� ����ϴ� ���ֵ�(PC,NPC)�� ITurnBaseData  �������̽��� �߰��� Base Ŭ���� ����� �װ��� ��ӹް� ����������Ѵ� .
/// �ƴϸ� �ش�Ŭ������ ��ӹް� �������ָ�ȴ� - �ش�Ŭ������ӽ� ITurnBaseData �� �߰���� ������Ѵ�.  �ش�Ŭ������ ����Ҽ��ֵ��� �������̴�.
/// </summary>
public class TurnBaseObject : BattleMapUnitIsPool, ITurnBaseData
{
    [SerializeField]
    int battleIndex = -1;
    public int UnitBattleIndex { get => battleIndex; set => battleIndex= value; }
    /// <summary>
    /// ���̰���� ȸ���� �ൿ�°� 
    /// </summary>
    float turnAddValue = 0.50f;
    public float TurnAddValue 
    {
        get => turnAddValue;
        protected set 
        {
            turnAddValue = value;
        }
    }
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
    public float TurnWaitingValue {
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
        }
    }

    /// <summary>
    /// ������ ���ذ� ������Ƽ
    /// </summary>
    public float TurnActionValue { 
            get => TurnWaitingValue; 
            set => TurnWaitingValue = value; 
    }

    /// <summary>
    /// �ϰ���� ȸ���� �ൿ�°�
    /// </summary>
    public float TurnEndActionValue => turnAddValue;


    /// <summary>
    /// ������� ������ ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    /// <summary>
    /// ������� �������� �����ϰų� �Ҷ� ȣ��� �Լ�
    /// </summary>
    Action<ITurnBaseData> turnRemove;
    public Action<ITurnBaseData> TurnRemove { get => turnRemove; set => turnRemove = value; }
    
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

    private void Awake()
    {
        //�׽�Ʈ
        turnAddValue = UnityEngine.Random.Range(0.0f ,0.1f); //������ø��� �����Ǵ� �ൿ�°� ���� ���� -�׽�Ʈ
        TurnWaitingValue = UnityEngine.Random.Range(0.0f,8.0f); // -�׽�Ʈ�� ����


        //ã�ƿ���
        battleUICanvas = FindObjectOfType<WindowList>().transform.GetChild(0).GetChild(0); // TrackingUI ���� ĵ������ġ

    }

    /// <summary>
    /// awake ������ ���丮�� �����ȵɼ����־ Start ���� �ʱ�ȭ�� �����ϴ�.
    /// </summary>
    private void Start()
    {
        InitTrackingUI();//������ �ʱ�ȭ 
    }
    protected override  void OnEnable()
    {
        base.OnEnable();
        if (BattleUI != null) //Ǯ���� �׻� Visible �� ���� �ϱ⶧���� üũ�� �ʿ��ϴ� . �ʱ�ȭŸ�̹��� Start ���� ���� �߻��ϱ⶧���� �ʿ��� ��üũ
        {
            InitTrackingUI();
        }
    }
    /// <summary>
    /// ������ UI �ʱ�ȭ �Լ� ����
    /// </summary>
    public void InitTrackingUI()
    {
        GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // ����ó�� �ʱ�ȭ�Ҷ� ��Ʋ UI �����ϰ� 
        BattleUI = obj.GetComponent<TrackingBattleUI>(); //UI ���۳�Ʈ ã�ƿ´�.
        BattleUI.gameObject.name = $"{this.name} _ {battleIndex}"; //�ε����� �׻�ٲ�⶧���� �ۼ�
        BattleUI.Player = this ;     //UI �� ���ְ� 1:1 ��ġ�� ���־�� ������ ��Ƶд�.
        BattleUI.transform.SetParent(battleUICanvas);//Ǯ�� ĵ���� �ؿ����⶧���� ��Ʋ��UI�� ������ ĵ���� ��ġ ������ �̵���Ų��.
        BattleUI.gameObject.SetActive(true); //Ȱ��ȭ ��Ų��.

    }
 
    /// <summary>
    /// �������� ������ ������
    /// ���� �ʱ�ȭ ��Ű�� Ǯ�� ������ ť�� ������.
    /// </summary>
    public void InitValue() 
    {
        BattleUI.InitValue(); //��Ʋ UI�ʱ�ȭ ��Ų��.
        transform.SetParent(PoolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
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
        if(TurnActionValue > 1.0f)TurnWaitingValue -= UnityEngine.Random.Range(0.3f, 1.0f);// �ൿ�� �Ҹ��� 
        //Debug.Log($"TurnStartAction : {this} :{this.TurnActionValue}");
    }
}
