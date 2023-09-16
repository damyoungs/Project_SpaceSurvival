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
    public bool isAttacRange = false;


    /// <summary>
    /// �Ϲݰ����� ����ǥ�ó� 
    /// ��ų������ ����ǥ�ð� ���ۉ����� üũ�� ����
    /// </summary>
    public bool isSkillAndAttack = false;

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

    //---------- ���ݹ��� ǥ�ÿ� ����
    /// <summary>
    /// ���ݰ����� ������ Ÿ�ϵ��� ��Ƶ� ����Ʈ
    /// </summary>
    [SerializeField]
    List<Tile> attackRangeTiles;

    /// <summary>
    /// ���ݹ��� ǥ���ϱ����� �����ص� Ÿ�� Ÿ��
    /// attackRangeTiles �� ������ ��������Ѵ�.
    /// </summary>
    [SerializeField]
    List<Tile.TileExistType> revertTileTypes;




    //---------- ��ų(�Ϲݰ���) ���� ǥ�ÿ� ����
    /// <summary>
    /// �����̳� ��ų�� ǥ�õ� Ÿ�ϸ���Ʈ
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
                SkillRange_Tile_View(value);
            }
        }
    }

    /// <summary>
    /// ���� ���ݹ���ǥ������ Ÿ�� 
    /// </summary>
    AttackRangeType attackType = AttackRangeType.None;
    AttackRangeType AttackType 
    {
        get => attackType;
        set 
        {
            if (attackType != value)
            {
                attackType = value;
                //��� ��ų�� ��ü�ϸ� ���� 
            }
        }
    }
    /// <summary>
    /// ���� ���ݹ����� ���� ��
    /// </summary>
    DirectionRangeType attackDir = DirectionRangeType.North;
    DirectionRangeType AttackDir 
    {
        get => attackDir;
        set 
        {
            if (attackDir != value) 
            {
                attackDir = value;
                //���̳� ������ �ٲٴ� ��ǲ�� ���ö� ó��
            }
        }
    }
    /// <summary>
    /// ���� ������ ���ϴ� ���� 
    /// </summary>
    Player_ skill_Use_Charcter;

    /// <summary>
    /// ������ ����ϰ��ִ� ��ų 
    /// </summary>
    SkillData currentSkill;


    private void Awake()
    {
        attackRangeTiles = new();
        revertTileTypes = new();
        activeAttackTiles = new();
        revertAttackRangeTileType = new();


        SpaceSurvival_GameManager.Instance.GetAttackRangeComp = () => this; //������ �����ϱ� 
        tileLayerIndex = LayerMask.NameToLayer("Ground");
     

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
                    Tile cusorTile = hit.transform.GetComponent<Tile>();
                    if (cusorTile.ExistType == Tile.TileExistType.AttackRange) //���ݹ����ȿ����� �������Ѵ�. 
                    {
                        AttackCurrentTile = cusorTile; //ã�� Ÿ���� ��� �Է����ش�!!!
                    }
                                                                            //�ٸ��������..? �������ٲ��..�ǳ�?
                    //�ٸ���� Tile Ŭ���� ���ο��ٰ� OnMouseEnter �Լ��� �̿��ؼ� �����͸� �����¹�����ֱ��ѵ�.. ��ɾ���..
                    break; //�ѹ�ã���� ���̻� ã���ʿ������ ������.
                }
            }
        }
    }


    /// <summary>
    /// ���� ��ư ������ ������ 
    /// ���ݹ����� ǥ�����ִ� �Լ� 
    /// </summary>
    /// <param name="playerTile">ĳ���Ͱ� �ִ� Ÿ�� ��ġ</param>
    /// <param name="size">���ݰ����� ��Ÿ� ���� (�⺻���� 1)</param>
    public void AttackRangeTileView(Tile playerTile, float size = 1.0f)
    {
        if (!isAttacRange)
        {
            isAttacRange = true;                                                 //���ݹ���ǥ�� ���� üũ
            ClearLineRenderer();                                                // ������ ����Ʈ �ʱ�ȭ�ϰ� 
            SetAttackSize(playerTile, size);                                       // �����ϰ� 
            OpenLineRenderer();                                                 // �����ش�
        }
    }


    /// <summary>
    /// ����ǥ���� ���ݽ�ų�� ��ȯ�Ҷ� ������
    /// </summary>
    public void AttackSkillChange(SkillData skillData) 
    { 
        
    }

    /// <summary>
    /// ��ų�� ���ݹ����� ǥ���� �Լ� �Ϲݰ��� ���� 
    /// </summary>
    /// <param name="targetTile">������ ������ġ</param>
    public void SkillRange_Tile_View(Tile targetTile)
    {
        if (isAttacRange) //���ݹ����� Ȱ��ȭ�Ȼ��¸� ����ǰ� ����
        {
            //Ÿ�����̵��Ǹ� ��������ǥ�� �����ϰ� 
            if (revertAttackRangeTileType.Count > 0) 
            {
                for(int i = 0; i< revertAttackRangeTileType.Count; i++)
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i];
                }
                revertAttackRangeTileType.Clear();
                activeAttackTiles.Clear();
            }
            //���Ӱ� ���� ����
            activeAttackTiles.Add(targetTile);
            revertAttackRangeTileType.Add(targetTile.ExistType);


            //���±�ü��Ű�� 
            targetTile.ExistType = Tile.TileExistType.Attack_OR_Skill;
        }
    }

    /// <summary>
    /// ���ݹ��� ������ ������ 
    /// ������Ʈ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns>���������� �迭�ι�ȯ ������ null��ȯ</returns>
    public ICharcterBase[] GetEnemyArray() 
    {
        List<ICharcterBase> resultEnemyList = new(4);
        if (activeAttackTiles.Count > 0) 
        {
            ICharcterBase[] enemyArray = SpaceSurvival_GameManager.Instance.EnemyTeam; //��Ʋ���� �������� ���� ��� 
            int enemySize = enemyArray.Length;
            foreach (Tile attackTile in activeAttackTiles) { //���ݹ�����ŭ �˻��ϰ�
                for (int i = 0; i < enemySize; i++) //������ �˻��� ���� 
                {
                    if (enemyArray[i].CurrentTile.width == attackTile.width &&
                        enemyArray[i].CurrentTile.length == attackTile.length) //Ÿ���� ������ 
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
    /// �Ϲ� �����̳� ��ų������ ������ �����ϴ� �Լ� 
    /// </summary>
    /// <param name="playerTile">ĳ���Ͱ��ִ� Ÿ����ġ</param>
    /// <param name="size">���� Ÿ�Կ����� ���� (�⺻���� 1)</param>
    private void SetTileList(Tile playerTile, float size = 1.0f)
    {
        if (!isClear)
        {
            Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
            int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
            int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;


        }
    }




















    /// <summary>
    /// ���ݹ��� �ʱ�ȭ �ϱ� .
    /// ������ �ʱ�ȭ�� ������ �ִ°�츸 ������ ����ȴ�.
    /// </summary>
    public void ClearLineRenderer()
    {
        if (!isClear)
        {
            isClear = true;
            if (revertAttackRangeTileType.Count > 0) //��ų ���ݹ����� �����ϸ� 
            {
                for (int i = 0; i < revertAttackRangeTileType.Count; i++) //���� ����Ʈ ã�Ƽ� 
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i]; //������Ű�� 
                }
                revertAttackRangeTileType.Clear(); //�ʱ�ȭ�Ѵ�
                activeAttackTiles.Clear();          //�ʱ�ȭ
            }

            InputSystemController.InputSystem.Mouse.Get_Position.performed -= OnMouseMove;
            if (revertTileTypes.Count > 0) //�ʱ�ȭ �� Ÿ������������  
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
            InputSystemController.InputSystem.Mouse.Get_Position.performed += OnMouseMove;
            foreach (Tile tile in attackRangeTiles)
            {
                revertTileTypes.Add(tile.ExistType);
                tile.ExistType = Tile.TileExistType.AttackRange;
            }
        }
    }

  





    /// <summary>
    /// ���� ��ġ�������� ��Ÿ� ���� ���� ������ ���� �� ��ǥ����Ʈ�� ������������ �Լ�
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
                if (!attackRangeTiles.Contains(currentNode)) //�ߺ�������
                {
                    attackRangeTiles.Add(currentNode); //��ȯ ��ų ����Ʈ�� �߰��Ѵ�.
                }
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
                //�밢�� üũ ������ �̵��� �ȵǴµ� ������ �Ǹ� �ȵɰŰ��Ƽ� ���ܵ״�.
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
