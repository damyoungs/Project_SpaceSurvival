using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleMapPlayerBase : PlayerBase_PoolObj, ICharcterBase
{
    /// <summary>
    /// ���� ĳ���� ��Ʈ���Ҽ��ִ»������� üũ
    /// </summary>
    bool isControll = false;
    public bool IsControll
    {
        get => isControll;
        set => isControll = value;
    }

    /// <summary>
    /// ĳ���� ������ ������ ����
    /// </summary>
    Player_ charcterData;
    public Player_ CharcterData => charcterData;
    /// <summary>
    /// ���¹̳� �ִ�ġ 
    /// </summary>
    const float charcterStaminaMaxValue = 10.0f;


    /// <summary>
    /// �̵����װ� �����ؼ� üũ�ϴ� ����
    /// </summary>
    bool isMoveCheck = false;
    public bool IsMoveCheck => isMoveCheck;

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
    /// ���� ����ġ���ִ� Ÿ��
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

    public Func<Tile> GetCurrentTile { get; set; }
    
    /// <summary>
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;

    /// <summary>
    /// �ൿ�� Ȥ�� �̵� �Ÿ�
    /// </summary>
    protected float moveSize = 5.0f;
    public float MoveSize 
    {
        get => moveSize;
        set => moveSize = value;
    }


    /// <summary>
    /// ������ܿ��ִ� ĳ���� ����â
    /// </summary>
    UICamera viewPlayerCamera;


    protected override void Awake()
    {
        base.Awake();
        charcterData = GetComponentInChildren<Player_>();
        charcterData.on_Player_Stamina_Change += (stmValue) => {
            
            TurnManager.Instance.CurrentTurn.TurnActionValue = stmValue;
            moveSize = stmValue;
            if (battleUI != null) 
            {
                BattleUI.stmGaugeSetting(stmValue, charcterStaminaMaxValue); //�Ҹ�� �ൿ�� ǥ��
            }
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentTile);
            SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(currentTile, moveSize);//�̵�����ǥ�����ֱ� 
            if (stmValue < 1.0f) //�ּ��ൿ��? ���� ������ 
            {
                TurnManager.Instance.CurrentTurn.TurnEndAction();//������ 
            }
        };
    }

    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI ���� ĵ������ġ
        InitUI();//��ó�� 
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
        charcterMove = CharcterMoveCoroutine(null,0.0f);
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
        if (viewPlayerCamera == null)  //ī�޶� ���þȵ������� 
        {
            viewPlayerCamera = EtcObjects.Instance.TeamCharcterView;// EtcObject �� �̸� ������ ���ӿ�����Ʈ �������� ť�� �������̴� 
            Transform cameraTarget = transform.GetChild(0); //ĳ������ġ
            viewPlayerCamera.TargetObject = cameraTarget.GetChild(cameraTarget.childCount-1); //ĳ���;ȿ� �ǹؿ� ī�޶� Ÿ���� �����־ߦi�ƴٴѴ�.
            viewPlayerCamera.gameObject.SetActive(true); //���ó������� Ȱ��ȭ��Ű��
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
        if (viewPlayerCamera != null)
        {
            viewPlayerCamera.TargetObject = null; //Ÿ�� �����
            viewPlayerCamera.gameObject.SetActive(false); // ��Ȱ��ȭ ��Ű�� ���������� ť�� ������.
            viewPlayerCamera = null; //���� �����
        }
        currentTile.ExistType = Tile.TileExistType.None; // �Ӽ� ������ 
        currentTile = null; //Ÿ�� ��������
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(poolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
    }


    public void SetTile(Tile currentTile) 
    {
        this.currentTile = currentTile;
    } 
    /// <summary>
    /// �±پ� AStar ���� ���ݼ����ѳ��� �����ͼ� ��� �ϰ� 
    /// ������������ �̵���Ų��.
    /// </summary>
    /// <param name="currentTile"></param>
    public void CharcterMove(Tile currentTile)
    {
        if (!isMoveCheck) //�̵��߰��� ����� Ÿ�Ϲ��װ� �߻��� üũ�ϴ·����߰� 
        {
            List<Tile> path = Cho_BattleMap_AStar.PathFind(
                                                            SpaceSurvival_GameManager.Instance.BattleMap,
                                                            SpaceSurvival_GameManager.Instance.MapSizeX,
                                                            SpaceSurvival_GameManager.Instance.MapSizeY,
                                                            this.currentTile,
                                                            currentTile
                                                            );
            //StopAllCoroutines();
            //StartCoroutine(CharcterMove(path));
            StopCoroutine(charcterMove);
            charcterMove = CharcterMoveCoroutine(path, currentTile.MoveCheckG);
            StartCoroutine(charcterMove);
           
        }
        
    }

    [SerializeField]
    Animator unitAnimator;
    int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 3.0f;
    [SerializeField]
    float rotateSpeed = 10.0f;
    IEnumerator charcterMove;
    /// <summary>
    /// �±پ��� ¥�� ��ã�� ��������
    /// 
    /// �̵����� ������ 
    /// - ��� ��Ȳ���� �߻��ϴ����� �ľ��̾ȵǳ� Ÿ���� ���� charcter �� �����̾ȵǴ� ��Ȳ�� �߻� 
    ///   �̵��� �ش�������� �����͸� �ٲٰ��ֱ⶧���� �����ΰŰ����� ��Ȯ�ϰ� �ľ��� ���ϰ�����.
    /// </summary>
    /// <param name="path">A��Ÿ �ִܰŸ� Ÿ�ϸ���Ʈ</param>
    /// <param name="useTurnValue">�̵������� �Ҹ�� ��</param>
    /// <returns></returns>
    IEnumerator CharcterMoveCoroutine(List<Tile> path , float useTurnValue)
    {
        isMoveCheck = true; //�̵� ������ üũ�ϱ� 
        Vector3 targetPos = currentTile.transform.position;
        unitAnimator.SetBool(isWalkingHash, true); //�̵��ִϸ��̼� ��� ����
        foreach (Tile tile in path)  // �����ִ°�� 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //���ο� ��ġ��� 
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //�ش���� �ٶ󺸰� 
            this.currentTile.ExistType = Tile.TileExistType.Move;// ������ġ �̵������ϰ� �ٲٰ�  
            //Debug.Log($"{this.currentTile.Index}Ÿ�� ������Ʈ �̵��߿� Ÿ�� �������ϴ� move�κ���");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� �� �����Ͱ� ����Ǿߵȴ� charcter �� ");
            tile.ExistType = Tile.TileExistType.Charcter; //�̵�����ġ ������ �ٲ۴�.
            //Debug.Log($"{this.currentTile.Index}Ÿ�� �� �����Ͱ� charcter ����Ǿ���.");
            
            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //�̵�����
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, false);

        charcterData.Stamina -= this.currentTile.MoveCheckG; //�����̵��� �Ÿ���ŭ ���¹̳��� ��´�.

        isMoveCheck = false; //�̵��������� üũ
    }

}
