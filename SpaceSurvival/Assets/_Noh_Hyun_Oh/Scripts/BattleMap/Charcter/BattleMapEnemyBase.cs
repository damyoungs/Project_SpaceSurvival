using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class BattleMapEnemyBase : Base_PoolObj ,ICharcterBase 
{
    /// <summary>
    /// ���ʹ� ��Ʈ���Ҽ������� ���ĸ� �������
    /// </summary>
    public bool IsControll { get; set; }


    Enemy_ enemyData;
    public Enemy_ EnemyData => enemyData;

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

        enemyData = GetComponentInChildren<Enemy_>();
        enemyData.on_Enemy_Stamina_Change += (stmValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.stmGaugeSetting(stmValue, enemyData.MaxStamina); //�Ҹ�� �ൿ�� ǥ��
            }
        };
        enemyData.on_Enemy_HP_Change += (hpValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.hpGaugeSetting(hpValue, enemyData.MaxHp); //�Ҹ�� �ൿ�� ǥ��
            }
            if (enemyData.HP < 0)
            {
                GameManager.PlayerStatus.GetExp((uint)enemyData.EnemyExp);
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
    /// ���ݻ��¸� üũ�ϴ� ����
    /// </summary>
    bool isAttackCheck = false;

    /// <summary>
    /// ���ݹ����ȿ� ������ �����ϴ� �Լ� 
    /// </summary>
    private void IsAttackAction()
    {
        Tile attackTile = Cho_BattleMap_Enemy_AStar.SetEnemyAttackSize(currentTile, enemyData.AttackRange);
        //Debug.Log(SpaceSurvival_GameManager.Instance.PlayerTeam[0]);

        if (attackTile != null)
        {
            isAttackCheck = true;
            enemyData.Attack_Enemy(SpaceSurvival_GameManager.Instance.PlayerTeam[0].CharcterData);
        }
    }

    public void Defence(float damage, bool isCritical = false)
    {
        float finalDamage = Mathf.Max(0, damage - enemyData.DefencePower);
        enemyData.HP -= finalDamage;
        GameManager.EffectPool.GetObject(finalDamage, transform, isCritical);
    }

    public void CharcterMove(Tile selectedTile)
    {
        List<Tile> path = Cho_BattleMap_Enemy_AStar.PathFind(SpaceSurvival_GameManager.Instance.BattleMap, SpaceSurvival_GameManager.Instance.MapSizeX, SpaceSurvival_GameManager.Instance.MapSizeY,
                                                           this.currentTile, selectedTile, moveSize);
        StopCoroutine(EnemyMove(null));
        StartCoroutine(EnemyMove(path));
    }
    //[SerializeField]
    //Animator unitAnimator;
    //int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 5.0f;
    IEnumerator EnemyMove(List<Tile> path)
    {
        isMoveCheck = true; //�̵� ������ üũ�ϱ� 
        enemyData.Anima.SetFloat(enemyData.Moving, 1.0f);
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
                timeElaspad += Time.deltaTime / moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        //unitAnimator.SetBool(isWalkingHash, false);

        enemyData.Stamina -= this.currentTile.MoveCheckG; //�����̵��� �Ÿ���ŭ ���¹̳��� ��´�.

        isMoveCheck = false; //�̵��������� üũ
        enemyData.Anima.SetFloat(enemyData.Moving, 0.0f);
        //IsAttackAction(); //���� �����ȿ��ִ��� üũ

        yield return new WaitForSeconds(3.0f);
    }

    
    public void EnemyTurnAction(Tile PlayerTile)
    {
        Debug.Log($"{transform.name}�� ���� - [ü��:{enemyData.HP}] / [�ൿ��:{enemyData.Stamina}]");
        
        IsAttackAction();
        CharcterMove(PlayerTile);
        if(isAttackCheck == false)
        {
            IsAttackAction();
        }
        isAttackCheck = false;
    }

}
