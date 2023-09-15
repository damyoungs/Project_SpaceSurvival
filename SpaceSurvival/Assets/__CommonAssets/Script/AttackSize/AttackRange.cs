using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// ���� ������ ������ ǥ������ ������Ʈ
/// ��  , ĳ���Ͱ� �ٶ󺸴� ���� 
/// </summary>
[Flags]
public enum AttackRangeType : byte
{
    None = 0,
    Dot = 1,                // ��ĭ ���콺�� �ִ�����
    Line = 2,               // ĳ���ͷ� ���� ������ ���� ������ ����
    Pie = 4,                // ĳ���� �������κ��� ��ä�÷� �������� ���� 
    Cross =8,               // ���콺�� �ִ�����(���)���� ���ڷ�  ������������ ������� �μ�
    Cube = 16,              // ���콺�� �ִ������� �������� ���簢������ ǥ�����ִ� ���
    XLine = 32,             // ���콺�� �ִ�����(���)���� �밢������ X �������� ���� ������ �μ�
                            // �Ʒ��� �ٷ� �������� ����� �ٲ�� �������� 
    Horizontal = 64,        // ���콺�� �ִ�����(���)���� ���η�  �� ��� ������� ������ 
    Vertical = 128,         // ���콺�� �ִ�����(���)���� ���η�  �� �Ʒ��� ������� ������
}
/// <summary>
/// ���������κ����� ȸ������ 
/// ī�޶� ȸ�����ֱ⶧���� ī�޶� ȸ���� �ȵ��ִ°��� �����̵ȴ�.
/// </summary>
public enum DirectionRangeType 
{
    North,                      //
    East,
    South,
    West
}
public class AttackRange : MonoBehaviour
{

    /// <summary>
    /// ���ù�ư�̳� ��ų��ư�������� 
    /// ����ǥ���ϱ����� ������ ���������� üũ�� ����
    /// </summary>
    bool isAttacking = false;

    /// <summary>
    /// Ÿ���� ���̾�� �����ص� ����
    /// </summary>
    [SerializeField]
    int tileLayerIndex;

    /// <summary>
    /// ������ ���� 
    /// </summary>
    [SerializeField]
    float ray_Range = 30.0f;

    /// <summary>
    /// û�� �������� �ٽð˻��Ҽ��ְ� üũ�ϴº���
    /// </summary>
    bool isClear = false;

    /// <summary>
    /// ���ݰ����� ������ Ÿ�ϵ��� ��Ƶ� ����Ʈ
    /// </summary>
    List<Tile> attackRangeTiles;

    /// <summary>
    /// ���ݹ��� ǥ���ϱ����� �����ص� Ÿ�� Ÿ��
    /// attackRangeTiles �� ������ ��������Ѵ�.
    /// </summary>
    List<Tile.TileExistType> revertTileTypes;

    /// <summary>
    /// ������ ���������� �Ф�
    /// </summary>
    List<Tile> activeAttackTiles;
    
    /// <summary>
    /// ����Ÿ�Կ����� ������ų ����Ÿ�ϼӼ�
    /// activeAttackTiles �� ������ ������Ѵ�.
    /// </summary>
    List<Tile.TileExistType> revertAttackRangeTileType;

    /// <summary>
    /// ���ݹ��� ǥ������ Ÿ�� ��ġ
    /// </summary>
    [SerializeField]
    Tile attackCurrentTile;
    Tile AttackCurrentTile
    {
        get => attackCurrentTile;
        set
        {
            if (attackCurrentTile != value) //Ÿ���� �Ź����������� ���ؼ� �ٸ�Ÿ���϶��� 
            {
                attackCurrentTile = value;
                //������������

            }
        }
    }

  
    

    private void Awake()
    {
        attackRangeTiles = new List<Tile>();
        SpaceSurvival_GameManager.Instance.GetAttackRangeComp = () => this; //������ �����ϱ� 
        tileLayerIndex = LayerMask.NameToLayer("Ground");
        InputSystemController.InputSystem.Mouse.Get_Position.performed += OnMouseMove;
      
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPos = context.ReadValue<Vector2>();
        AttackRangeView(mouseScreenPos);
    }


    /// <summary>
    /// ���콺 ��ġ������ Ÿ�� ã��
    /// </summary>
    /// <param name="mouseScreenPos">���콺�� ��ũ�� ��ǥ</param>
    private void AttackRangeView(Vector2 mouseScreenPos)
    {
        if (!isClear) //Ÿ�ϰ����߿� �ߺ�����Ǹ� �ȵǴ� üũ
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);      // ȭ�鿡�� ���� ���콺�� ��ġ�� ��� ��
            Debug.DrawRay(ray.origin, ray.direction * ray_Range, Color.black, 1.0f);              // ����׿� ������

            RaycastHit[] hitObjets = Physics.RaycastAll(ray, ray_Range); //���̸� ���� �浹�� ������Ʈ ����Ʈ�� �޾ƿ´�.

            foreach (RaycastHit hit in hitObjets) // ������ �ִ°�� ������ �����Ѵ�.
            {
                if (hit.collider.gameObject.layer == tileLayerIndex) //Ÿ������ üũ�ϰ� 
                {
                    AttackCurrentTile = hit.transform.GetComponent<Tile>(); //ã�� Ÿ���� ��� �Է����ش�!!!
                                                                            //�ٸ��������..? �������ٲ��..�ǳ�?
                    //�ٸ���� Tile Ŭ���� ���ο��ٰ� OnMouseEnter �Լ��� �̿��ؼ� �����͸� �����¹�����ֱ��ѵ�.. ��ɾ���..
                    break; //�ѹ�ã���� ���̻� ã���ʿ������ ������.
                }
            }
        }
    }


    /// <summary>
    /// ���ݹ����� ǥ�����ִ� �Լ� 
    /// </summary>
    /// <param name="playerTile">ĳ���Ͱ� �ִ� Ÿ�� ��ġ</param>
    /// <param name="type">���� Ÿ�� </param>
    /// <param name="attackDir">���� ���� (�⺻���� ����)</param>
    /// <param name="size">���� Ÿ�Կ����� ���� (�⺻���� 1)</param>
    public void AttackRangeTileView(Tile playerTile, int size = 1) 
    {
        ClearLineRenderer();                                                // ������ ����Ʈ �ʱ�ȭ�ϰ� 
        SetTileList(playerTile,size);                       // �����ϰ� 
        OpenLineRenderer();                                                 // �����ش�
    }

    /// <summary>
    /// ���ݹ��� �ȿ� ���������� ã�Ƽ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns>���������� �迭�ι�ȯ ������ null��ȯ</returns>
    public ICharcterBase[] GetEnemyArray() 
    {
        List<ICharcterBase> resultEnemyList = new(4);
        if (attackRangeTiles.Count > 0) 
        {
            ICharcterBase[] enemyArray = SpaceSurvival_GameManager.Instance.EnemyTeam;
            int enemySize = enemyArray.Length;
            foreach (Tile attackTile in attackRangeTiles) { //���ݹ�����ŭ �˻��ϰ�
                for (int i = 0; i < enemySize; i++) //������ �˻��� ���� 
                {
                    if (enemyArray[i].CurrentTile.Equals(attackTile)) //Ÿ���� ������ 
                    {
                        resultEnemyList.Add(enemyArray[i]); //����Ʈ�� �߰�
                        break;//����Ÿ�ϰ˻������� ��������
                    }
                }
            }
        }
        return resultEnemyList.ToArray();
    }

    /// <summary>
    /// ���ݹ��� �ʱ�ȭ �ϱ� .
    /// ������ �ʱ�ȭ�� ������ �ִ°�츸 ������ ����ȴ�.
    /// </summary>
    private void ClearLineRenderer()
    {
        if (!isClear)
        {
            isClear = true;
            if (revertTileTypes != null && revertTileTypes.Count > 0) //�ʱ�ȭ �� Ÿ������������  
            {
                int listSize = revertTileTypes.Count; //���������ͼ�
                for (int i = 0; i < listSize; i++)
                {
                    attackRangeTiles[i].ExistType = revertTileTypes[i]; // ������ �����ص״� ������ �ٽõ����� 
                }
                attackRangeTiles.Clear();  // ���� ����  clear �Լ��� ���� �迭��Ҹ� �ʱ�ȭ�ϱ⶧���� null���� ����.
                revertTileTypes.Clear();    // ���� ����.
            }
            isClear = false;
        }
    }
    
    /// <summary>
    /// ���� ���� ǥ���ϱ� 
    /// </summary>
    private void OpenLineRenderer() 
    {
        if (!isClear) 
        {
            foreach (Tile tile in attackRangeTiles)
            {
                tile.ExistType = Tile.TileExistType.Attack;
            }
        }
    }

    /// <summary>
    /// ���ݹ��� �����ϴ� �Լ� 
    /// </summary>
    /// <param name="playerTile">ĳ���Ͱ��ִ� Ÿ����ġ</param>
    /// <param name="size">���� Ÿ�Կ����� ���� (�⺻���� 1)</param>
    private void SetTileList(Tile playerTile , int size = 1) 
    {
        if (!isClear) 
        {
            Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
            int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
            int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;

            
        }
    }






    /// <summary>
    /// ���� ��ġ�������� �ൿ�� ���� �̵������� ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// </summary>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="attackCheck">���ݰ����� �Ÿ� ��</param>
    /// <returns>ĳ���Ͱ� ���ݰ����� ��帮��Ʈ</returns>
    private void SetAttackSize(Tile currentNode, float attackCheck)
    {
        List<Tile> openList = new List<Tile>();   // Ž���� �ʿ��� ��� ����Ʈ 
        List<Tile> closeList = new List<Tile>();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; //���������� ���»��¶� �ƽ��� ������ ����� �ȵ��ư���.
            node.AttackCheckG = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.AttackCheckG = 0.0f; //����ġ�� g �� 0�̴�

        while (openList.Count > 0)
        {
            currentNode = openList[0];
            openList.Remove(currentNode); // Ž�������� ��Ͽ��� ���� Ž������ ����� �����ϰ� 
            closeList.Add(currentNode);   // Ž�������� ����Ʈ�� ���� ����� ��´�.

            if (currentNode.AttackCheckG > attackCheck) //G ���� ���� �̵� ������ �Ÿ����� ������  ���̻� Ž���� �ʿ�������� 
            {
                continue; //������ Ž�� 
            }
            else // �̵������� �Ÿ��� 
            {
                attackRangeTiles.Add(currentNode); //��ȯ ��ų ����Ʈ�� �߰��Ѵ�.
            }

            OpenListAdd(mapTiles, tileSizeX, tileSizeY, currentNode, openList, closeList); //�ֺ� 8������ ��带 ã�Ƽ� G�� �����ϰ�  ���¸���Ʈ�� ������������ ��´�.
            openList.Sort();            //ã�� G���� ���� ���������� ��Ž���̵ȴ�.
        }
    }
    private void OpenListAdd(Tile[] mapTiles, int tileSizeX, int tileSizeY, Tile currentNode, List<Tile> open, List<Tile> close)
    {
        Tile adjoinTile;
        float sideDistance = 1.0f;
        float diagonalDistance = 1.414f;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // ���̵� �˻� 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //���̵� �˻�
                    continue;

                adjoinTile = Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length + y, tileSizeX);    // ������ Ÿ�� ��������

                if (adjoinTile == currentNode)                                          // ������ Ÿ���� (0, 0)�� ���
                    continue;
                if (adjoinTile.ExistType == Tile.TileExistType.Prop)                // ������ Ÿ���� ��ֹ��϶�
                    continue;

                bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length, tileSizeX).ExistType == Tile.TileExistType.Prop ||
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width, currentNode.Length + y, tileSizeX).ExistType == Tile.TileExistType.Prop
                    )
                    continue;

                float distance;
                if (isDiagonal)
                {
                    distance = diagonalDistance;
                }
                else
                {
                    distance = sideDistance;
                }

                if (adjoinTile.AttackCheckG > currentNode.AttackCheckG + distance)
                {
                    open.Add(adjoinTile);
                    adjoinTile.AttackCheckG = currentNode.AttackCheckG + distance;
                }
            }
        }

    }


}
