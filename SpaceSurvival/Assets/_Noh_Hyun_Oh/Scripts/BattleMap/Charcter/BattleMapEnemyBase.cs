using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapEnemyBase : Base_PoolObj ,ICharcterBase 
{
    /// <summary>
    /// ���ʹ� ��Ʈ���Ҽ������� ���ĸ� �������
    /// </summary>
    public bool IsControll { get; set; }

    Enemy_ enemy_;
    public Enemy_ Enemy => enemy_;


    public virtual bool IsMoveCheck { get; }

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

    public Action<BattleMapEnemyBase> onDie;

    /// <summary>
    /// �ൿ�� Ȥ�� �̵����� �Ÿ�
    /// </summary>
    [SerializeField]
    protected float moveSize = 4.0f;
    public float MoveSize
    {
        get => moveSize;
        set => moveSize = value;
    }

    protected override void Awake()
    {
        base.Awake();
        enemy_ = GetComponentInChildren<Enemy_>();
        enemy_.on_Enemy_Stamina_Change += (stmValue) =>
        {
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
            if (enemy_.HP < 0)
            {
                GameManager.PlayerStatus.GetExp((uint)enemy_.EnemyExp);
                ResetData();
                onDie?.Invoke(this);
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
    public void ResetData()
    {
        if (BattleUI != null) //��Ʋ UI�� ���õ������� 
        {
            BattleUI.ResetData();// ������ UI �ʱ�ȭ 
            BattleUI = null; // ����
        }
        Debug.Log($"{currentTile.width},{currentTile.length} ,{currentTile.ExistType}  ���� �ʱ�ȭ ");
        currentTile.ExistType = Tile.TileExistType.None; // �Ӽ� ������ 
        
        currentTile = null; //Ÿ�� ��������
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(poolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
    }


  

    /// <summary>
    /// �̵����װ� �����ؼ� üũ�ϴ� ����
    /// </summary>
    bool isMoveCheck = false;

  
    /// <summary>
    /// ���ݹ����ȿ� ������ �����ϴ� �Լ� 
    /// </summary>
    private void IsAttackAction()
    {
        Tile attackTile = Cho_BattleMap_Enemy_AStar.SetEnemyAttackSize(currentTile, enemy_.AttackRange);
        //Debug.Log(SpaceSurvival_GameManager.Instance.PlayerTeam[0]);

        if (attackTile != null)
        {
            Attack_Enemy(SpaceSurvival_GameManager.Instance.PlayerTeam[0].CharcterData);
        }
    }


    public void Attack_Enemy(IBattle target)
    {
        target.Defence(enemy_.AttackPower);
    }

    public void Defence(float damage, bool isCritical = false)
    {
        float finalDamage = Mathf.Max(0, damage - enemy_.DefencePower);
        enemy_.HP -= finalDamage;
        GameManager.EffectPool.GetObject(finalDamage, transform, isCritical);
    }


    public void CharcterMove(Tile selectedTile)
    {
        List<Tile> path = Cho_BattleMap_Enemy_AStar.PathFind(
                                                           SpaceSurvival_GameManager.Instance.BattleMap,
                                                           SpaceSurvival_GameManager.Instance.MapSizeX,
                                                           SpaceSurvival_GameManager.Instance.MapSizeY,
                                                           this.currentTile,
                                                           selectedTile,
                                                           moveSize
                                                           );
        EnemyMove(path);
    }
    //[SerializeField]
    //Animator unitAnimator;
    //int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 3.0f;
    private void EnemyMove(List<Tile> path)
    {
        isMoveCheck = true; //�̵� ������ üũ�ϱ� 
        Vector3 targetPos = currentTile.transform.position; //���̾��°�� ���� Ÿ����ġ ����
        //unitAnimator.SetBool(isWalkingHash, true); //�̵��ִϸ��̼� ��� ����
        foreach (Tile tile in path)  // �����ִ°�� 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //���ο� ��ġ��� 
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //�ش���� �ٶ󺸰� 
            this.currentTile.ExistType = Tile.TileExistType.None;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� ������Ʈ �̵��߿� Ÿ�� �������ϴ� move�κ���");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� �� �����Ͱ� ����Ǿߵȴ� charcter �� ");
            tile.ExistType = Tile.TileExistType.Monster;

            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //�̵�����
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        //unitAnimator.SetBool(isWalkingHash, false);

        enemy_.Stamina -= this.currentTile.MoveCheckG; //�����̵��� �Ÿ���ŭ ���¹̳��� ��´�.

        isMoveCheck = false; //�̵��������� üũ


        IsAttackAction(); //���� �����ȿ��ִ��� üũ
    }

    public void EnemyAi(Tile PlayerTile)
    {
        CharcterMove(PlayerTile);
    }


}
