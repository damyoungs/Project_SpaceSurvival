using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapEnemyBase : EnemyBase_PoolObj ,ICharcterBase
{
    /// <summary>
    /// ���ʹ� ��Ʈ���Ҽ������� ���ĸ� �������
    /// </summary>
    public bool IsControll { get; set; }

    Enemy_ enemy_;
    public Enemy_ Enemy => enemy_;


    public bool IsMoveCheck { get; }
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
    public Transform BattleUICanvas => battleUICanvas;

    /// <summary>
    /// ���� �ڽ��� ��ġ�� Ÿ��
    /// </summary>
    public Tile currentTile;
    public Tile CurrentTile
    {
        get 
        {
            if (currentTile == null) 
            {
                currentTile = GetCurrentTile?.Invoke();
            }
            return currentTile;
        }
    }


    public Func<Tile> GetCurrentTile { get; set ; }

    /// <summary>
    /// �ൿ�� Ȥ�� �̵����� �Ÿ�
    /// </summary>
    protected float moveSize = 2.0f;
    public float MoveSize => moveSize;

    protected override void Awake()
    {
        base.Awake();
        enemy_ = GetComponentInChildren<Enemy_>();
        enemy_.on_Enemy_Stamina_Change += (stmValue) =>
        {
            float currentMoveSize = stmValue > moveSize? moveSize : stmValue;
            //TurnManager.Instance.CurrentTurn.TurnActionValue = stmValue;
            TurnManager.Instance.CurrentTurn.TurnActionValue = 20;
            //if(stmValue < 1.0f)
            //{
            //    TurnManager.Instance.CurrentTurn.TurnEndAction();
            //}
            if (battleUI != null)
            {
                BattleUI.stmGaugeSetting(stmValue, enemy_.MaxStamina); //�Ҹ�� �ൿ�� ǥ��
            }
        };
        enemy_.on_Enemy_HP_Change += (hpValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.hpGaugeSetting(hpValue, enemy_.MaxHp); //�Ҹ�� �ൿ�� ǥ��
            }
        };
        enemy_.on_Enemy_HP_Change += (hpValue) =>
        {
            if(enemy_.HP <= 0)
            {
                ResetData();
            }
        };
    }

    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI ���� ĵ������ġ
        InitUI();
    }

    protected override void OnEnable()
    {
        if (battleUICanvas != null)  //ĵ���� ��ġ�� ã�Ƴ�����
        {
            InitUI();//�ʱ�ȭ
        }
    }
    /// <summary>
    /// ������ UI �ʱ�ȭ �Լ� ����
    /// </summary>
    public void InitUI()
    {
        if (battleUI != null) //���� ������
        {
            battleUI.gameObject.SetActive(true); //Ȱ��ȭ�� ��Ų��
        }
        else //������ UI�� ���þȵ������� �����Ѵ�
        {
            battleUI = (TrackingBattleUI)Multiple_Factory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // ����ó�� �ʱ�ȭ�Ҷ� ��Ʋ UI �����ϰ� 
            battleUI.gameObject.name = $"{name} _ Tracking"; //�̸�Ȯ�ο�
            battleUI.transform.SetParent(battleUICanvas);//Ǯ�� ĵ���� �ؿ����⶧���� ��Ʋ��UI�� ������ ĵ���� ��ġ ������ �̵���Ų��.
            battleUI.gameObject.SetActive(true); //Ȱ��ȭ ��Ų��.
            battleUI.Player = transform.GetChild(0);     //UI �� ���ְ� 1:1 ��ġ�� ���־�� ������ ��Ƶд�.
        }
    }

    /// <summary>
    /// �������� ������ ������
    /// ���� �ʱ�ȭ ��Ű�� Ǯ�� ������ ť�� ������.
    /// </summary>
    public virtual void ResetData()
    {
        if (BattleUI != null) //��Ʋ UI�� ���õ������� 
        {
            BattleUI.ResetData();// ������ UI �ʱ�ȭ 
            BattleUI = null; // ����
        }
        Debug.Log("���� �ʱ�ȭ ");
        currentTile.ExistType = Tile.TileExistType.None; // �Ӽ� ������ 
        
        currentTile = null; //Ÿ�� ��������
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(poolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
    }

    public void EnemyAi(Tile PlayerTile)
    {
        Debug.Log($"{transform.name}�� ���� - [ü��:{enemy_.HP}] / [�ൿ��:{enemy_.Stamina}]\n[��ǥ:{CurrentTile.transform.position}] / [{currentTile.name}]");
        Cho_BattleMap_Enemy_AStar.PathFind(SpaceSurvival_GameManager.Instance.BattleMap, SpaceSurvival_GameManager.Instance.MapSizeX, SpaceSurvival_GameManager.Instance.MapSizeY, currentTile, PlayerTile);
    }



    public void CharcterMove(Tile selectedTile)
    {

    }


}
