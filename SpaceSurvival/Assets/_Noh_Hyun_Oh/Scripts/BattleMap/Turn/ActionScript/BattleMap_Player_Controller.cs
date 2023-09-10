using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��Ʋ�ʿ��� �÷��̾��� �̺�Ʈ�ڵ鷯������ ������ ������Ʈ  
/// Ŭ�������� �̺�Ʈ���� ��������ֱ�
/// </summary>
public class BattleMap_Player_Controller : MonoBehaviour
{

    /// <summary>
    /// ���̰� Ÿ�Ͽ� �浹������ üũ�� ���̾� ��
    /// �����ͻ󿡼� ���̾ ����������Ѵ�.
    ///  LayerMask.GetMask();// 2���ڵ�� �ۼ����־ ���� 0,1,2,4,8,16 ó�� 2�� ����� ���������� ������ִ� 
    ///  LayerMask.NameToLayer("");// �ٸ���ũ�ʹٸ��� ����ȼ��� �������´� 0,1,2,3,4,5,6,7 .... 
    ///  GameObject.Layer �� ������ ������ִ� 
    ///  readOnly �� �̸� �˻��õ��ߴ��� 
    /// </summary>
    [SerializeField]
    int tileLayerIndex;

    [SerializeField]
    int uiLayerIndex;

    /// <summary>
    /// �÷��̾ ������ Ȯ���ϱ����� �������� ������Ʈ
    /// </summary>
    PlayerTurnObject playerTurnObject;
    public PlayerTurnObject PlayerTurnObject 
    {
        get 
        {
            if (playerTurnObject == null) //�����Ͱ� ������ 
            {
                //playerTurnObject = FindObjectOfType<PlayerTurnObject>(); //���̾����� ã�·��� �����̴�.
                //������ �̿��� ã�ƺ��� 
                playerTurnObject = GetPlayerTurnObject?.Invoke(); //������ ����ϱ⿡�� �δ��̵Ǽ� �مf��.
            }
            return playerTurnObject;
        }
    }
    /// <summary>
    /// ��ü�� �ʱ�ȭ �Ǵ� Ÿ�̹��� Ʋ���� �����ص� ��������Ʈ
    /// </summary>
    public Func<PlayerTurnObject> GetPlayerTurnObject;
    
    /// <summary>
    /// ���̰� �ִ�� üũ�� �Ÿ�
    /// </summary>
    [SerializeField]
    float ray_Range = 15.0f;

    /// <summary>
    /// �̵������� Ÿ�� Ŭ���� ��ȣ���Ѱ��ش�
    /// </summary>
    public Action<Tile> onMoveActive;
    
    /// <summary>
    /// Ÿ�Ͽ� ���Ͱ� �ְ� Ŭ�������� ��ȣ�� �Ѱ��ش�
    /// </summary>
    public Action<Tile> onClickMonster;
   
    /// <summary>
    /// Ÿ�Ͽ� �������� �ְ� Ŭ�������� ��ȣ�� �Ѱ��ش�
    /// </summary>
    public Action<Tile> onClickItem;

    /// <summary>
    /// ĳ������ Ÿ���� Ŭ�������� ��ȣ�� �Ѱ��ش�
    /// </summary>
    public Action<Tile> onClickPlayer;

    /// <summary>
    /// �̺�Ʈ ���� �ϱ����� ��������Ʈ ���߿� �޴��� ���� ó���Ҷ� �ʿ� 
    /// </summary>
    //public Action<Tile> onOffEventHandle;

    private void Awake()
    {
        tileLayerIndex = LayerMask.NameToLayer("Ground");
        uiLayerIndex = LayerMask.NameToLayer("UI");

       
    }
    private void Start()
    {
        InputSystemController.Instance.OnBattleMap_Player_UnitMove += OnMove;
    }
    /// <summary>
    /// Ŭ�������� ���̸� ���� ���̿� �浹�� ��ü���� �������� 
    /// ���̾�� ������ ó���� �ϴ� ���� 
    /// </summary>
    private void OnMove()
    {
        if (PlayerTurnObject == null) //�÷��̾ ���� ���ΰ�츸 �����ϵ��� üũ
        {
            Debug.Log($"{playerTurnObject}�÷��̾ ���� �ȵ��ְų� �÷��̾ ���� ���̾ƴմϴ�.");
            return;
        }
        else if (!playerTurnObject.IsTurn)
        {
            Debug.Log($"�Ͼƴ϶�� �׸�Ŭ���� {playerTurnObject.IsTurn}");
            return;
        }
        else if (SpaceSurvival_GameManager.Instance.IsUICheck) 
        {
            Debug.Log("UI ������Դϴ�");
            return;
        }
            //Debug.Log($"��ǲ�ý��ۿ����� Ŭ���� ���� ������Ʈ������ �������´� �׷��� ���̷� ���� �����;��Ѵ�.");
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());      // ȭ�鿡�� ���� ���콺�� ��ġ�� ��� ��
        Debug.DrawRay(ray.origin, ray.direction * ray_Range, Color.red, 1.0f);              // ����׿� ������

        RaycastHit[] hitObjets = Physics.RaycastAll(ray, ray_Range); //���̸� ���� �浹�� ������Ʈ ����Ʈ�� �޾ƿ´�.
        
        foreach (RaycastHit hit in hitObjets) // ������ �ִ°�� ������ �����Ѵ�.
        {
            if (hit.collider.gameObject.layer == uiLayerIndex) 
            {
                Debug.Log($"{hit.collider.name}������Ʈ�� UI��");
            }
            if (hit.collider.gameObject.layer == tileLayerIndex) //Ÿ������ üũ�ϰ� 
            {
                OnTileClick(hit); //Ÿ�� Ŭ���������� ������ ���� 
                break; //�ѹ�Ŭ���� �ѹ��� ������ �����Ҽ��ְ� �극��ũ�� ��´�.
            }
             //Ŭ���̺�Ʈ ���⿡ �߰��ο��� 
        }

    }

    /// <summary>
    /// Ÿ�Ͽ� ������ ���� üũ��������� 
    /// </summary>
    /// <param name="hitInfo"></param>
    private void OnTileClick(RaycastHit hitInfo) 
    {
        Tile targetTile = hitInfo.transform.GetComponent<Tile>();
        if (targetTile != null) //Ÿ���� Ŭ�� ������� 
        {
            switch (targetTile.ExistType) //Ÿ�� ����Ȯ���ϰ� 
            {
                case Tile.TileExistType.None:
                    break;
                case Tile.TileExistType.Charcter:
                    Debug.Log($"�̵��Ұ� ĳ����: ����Ÿ��{hitInfo.transform.name} , ��ġ : {hitInfo.transform.position}");
                    onClickPlayer?.Invoke(targetTile); 
                    break;
                case Tile.TileExistType.Monster:
                    onClickMonster?.Invoke(targetTile);
                    //���� Ŭ���� ���Ϳ����� ������ ������ ���� �׼����ʿ�
                    Debug.Log($"�̵��Ұ� ����: ����Ÿ��{hitInfo.transform.name} , ��ġ : {hitInfo.transform.position}");
                    break;
                case Tile.TileExistType.Item:
                    onMoveActive?.Invoke(targetTile);//�̵����� ����
                    onClickItem?.Invoke(targetTile); //�������ִ°��� Ŭ�������� ��������
                    // �������� Ÿ�Ͽ��ִ°�� ������ ������ ������ ���� ������ �׼� 
                    break;
                case Tile.TileExistType.Prop:
                    Debug.Log($"�̵��Ұ� ��ֹ� : ����Ÿ��{hitInfo.transform.name} , ��ġ : {hitInfo.transform.position}");
                    break;
                case Tile.TileExistType.Move:
                    //Debug.Log(targetTile);
                    onMoveActive?.Invoke(targetTile);//�̵����� ����
                    Debug.Log($"�̵����� : ����Ÿ��{hitInfo.transform.name} , ��ġ : {hitInfo.transform.position}");
                    break;
                default:
                    Debug.Log($"���ٵǸ� �ȵȴ�.");
                    break;
            }
        }
    }


}
  
