using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapPlayerBase : PlayerBase_PoolObj , ICharcterBase
{
    /// <summary>
    /// ���� ĳ���� ��Ʈ���Ҽ��ִ»������� üũ
    /// </summary>
    public bool isControll = false;

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
    public float MoveSize => moveSize;


    /// <summary>
    /// ������ܿ��ִ� ĳ���� ����â
    /// </summary>
    UICamera viewPlayerCamera;


    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI ���� ĵ������ġ
        InitUI();//��ó�� 
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
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
        this.currentTile = null; //Ÿ������ �����
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(poolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
    }


    public void SetTile(Tile currentTile) 
    {
        this.currentTile = currentTile;
    } 
    public void CharcterMove(Tile currentTile)
    {
        List<Tile> path = Cho_BattleMap_AStarDouble.PathFind(
                                                        SpaceSurvival_GameManager.Instance.BattleMapDoubleArray,
                                                        this.currentTile,
                                                        currentTile
                                                        );
        StopAllCoroutines();
        StartCoroutine(CharcterMove(path));
    }

    [SerializeField]
    Animator unitAnimator;
    int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 3.0f;
    [SerializeField]
    float rotateSpeed = 10.0f;
    /// <summary>
    /// �±پ��� ¥�� ��ã�� ��������
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator CharcterMove(List<Tile> path)
    {
        Vector3 targetPos = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, true);
        foreach (Tile tile in path) 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position;
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
            
            while ((targetPos - transform.position).sqrMagnitude > 0.04f) 
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
            
            this.currentTile.ExistType = Tile.TileExistType.Move;// ������ġ �̵������ϰ� �ٲٰ�  
            this.currentTile = tile;
            this.currentTile.ExistType = Tile.TileExistType.Monster; //�̵�����ġ ������ �ٲ۴�.
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, false);
    }
}
