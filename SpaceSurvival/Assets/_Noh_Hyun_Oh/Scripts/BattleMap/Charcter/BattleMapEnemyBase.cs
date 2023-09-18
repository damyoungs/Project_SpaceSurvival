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

    EnemyStatus enemyData;
    public EnemyStatus EnemyData => enemyData;

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

    bool isEnemyTurn;

    /// <summary>
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;

    /// <summary>
    /// ���� �ڽ��� ��ġ�� Ÿ��
    /// </summary>
    Tile currentTile;
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
    protected float moveSize = 3.0f;
    public float MoveSize 
    {
        get => moveSize;
        set => moveSize = value;
    }

    protected override void Awake()
    {
        enemyData = GetComponentInChildren<EnemyStatus>();
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

    public void SetTile(Tile currentTile)
    {
        this.currentTile = currentTile;
    }


    /// <summary>
    /// �������� ������ ������
    /// ���� �ʱ�ȭ ��Ű�� Ǯ�� ������ ť�� ������.
    /// </summary>
    public void ResetData()
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

    public void CharcterMove(Tile selectedTile)
    {
        if (!IsMoveCheck)
        {
            List<Tile> path = Cho_BattleMap_AStar.PathFind(SpaceSurvival_GameManager.Instance.BattleMap,
                                                            SpaceSurvival_GameManager.Instance.MapSizeX,
                                                            SpaceSurvival_GameManager.Instance.MapSizeY,
                                                            this.currentTile,
                                                            selectedTile
                                                            );
        }
    }
}
