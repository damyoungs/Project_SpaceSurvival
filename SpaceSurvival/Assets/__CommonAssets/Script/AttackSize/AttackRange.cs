using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackRange : MonoBehaviour
{
    /// <summary>
    /// ���� ������ ������ ǥ������ ������Ʈ
    /// ��  , ĳ���Ͱ� �ٶ󺸴� ����  
    /// </summary>
    [Flags]
    enum AttackRangeType : byte
    {
        None = 0,
        Dot = 1,                // ��ĭ ���콺�� �ִ�����
        Line = 2,               // ĳ���ͷ� ���� ������ ���� ������ ����
        Pie = 4,                // ĳ���� �������κ��� ��ä�÷� �������� ���� 
        Cross = 8,               // ���콺�� �ִ�����(���)���� ���ڷ�  ������������ ������� �μ�
        Cube = 16,              // ���콺�� �ִ������� �������� ���簢������ ǥ�����ִ� ���
        XLine = 32,             // ���콺�� �ִ�����(���)���� �밢������ X �������� ���� ������ �μ�
                                // �Ʒ��� �ٷ� �������� ����� �ٲ�� �������� 
        Horizontal = 64,        // ���콺�� �ִ�����(���)���� ���η�  �� ��� ������� ������ 
        Vertical = 128,         // ���콺�� �ִ�����(���)���� ���η�  �� �Ʒ��� ������� ������
    }
    /// <summary>
    /// ���������κ����� ȸ������ 
    /// ī�޶� ȸ�����ֱ⶧���� ī�޶� ȸ���� �ȵ��ִ°��� �����̵ȴ�. 
    /// 
    /// </summary>
    [Flags]
    enum DirectionRangeType : byte
    {
        None = 0,               //0000 0000
        North = 1,              //0000 0001       
        East = 2,               //0000 0010
        South = 4,              //0000 0100
        West = 8,               //0000 1000
        //All = 15,               //0000 1111
    }
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
    Player_ Skill_Use_Charcter 
    {
        get => skill_Use_Charcter;
        set 
        {
            if (skill_Use_Charcter != value) //�ٸ�ĳ������ �ٲ����� 
            {
                if (skill_Use_Charcter != null)  //����ĳ���� ������ 
                {
                    skill_Use_Charcter.on_ActiveSkill = null; //�׼ǿ������ 
                }
                skill_Use_Charcter = value; //���Ӱ� ��Ʈ���� ĳ���� �����ϰ� 
                skill_Use_Charcter.on_ActiveSkill = ActiveSkill; //�׼ǿ��� �ٽ��Ѵ�.
            }
        }
    }

    public Func<Player_> playerCharcter;

    /// <summary>
    /// ������ ����ϰ��ִ� ��ų 
    /// </summary>
    SkillData currentSkill;
    SkillData CurrentSkill 
    {
        get=> currentSkill;
        set 
        {
            if (currentSkill != value)  //������� ��ų�� �ٲ�
            {
                currentSkill = value;                   // �������ϰ� 
                SkillRange_Tile_View(attackCurrentTile);// ����ǥ�ôٽ�ó��
            }
        }
    }
    /// <summary>
    /// 8���� ��ǥ�� ������ �����س��� 
    /// ����(Penetrate)������ ��� 
    /// </summary>
    Vector2Int[] penetrateRotateValues = new Vector2Int[]
    {
        new Vector2Int(0,1),new Vector2Int(1,1),new Vector2Int(1,0),new Vector2Int(1,-1),
        new Vector2Int(0,-1),new Vector2Int(-1,-1),new Vector2Int(-1,0),new Vector2Int(-1,1)
    };

    private void Awake()
    {
        attackRangeTiles = new();
        revertTileTypes = new();
        activeAttackTiles = new();
        revertAttackRangeTileType = new();

        tileLayerIndex = LayerMask.NameToLayer("Ground");

        SpaceSurvival_GameManager.Instance.GetAttackRangeComp = () => this; //������ �����ϱ� 
    }

    /// <summary>
    /// �� �����Ҷ� �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="controllUnit">��Ʈ���� ����</param>
    public void InitDataSet(Player_ controllUnit) 
    {
        if (controllUnit != null) 
        {
            Skill_Use_Charcter = controllUnit;
        }
    }


    /// <summary>
    /// ĳ���� �ʿ��� ��ų�� �����ų� ����Ű�� ��ų�� ����Ҷ� �ߵ��ϴ� �Լ� 
    /// ���ʿ����� ���� ��ų�� ������ �����ؼ� ǥ�����ָ�ȴ�.
    /// </summary>
    /// <param name="skillData">��ų������ ������ �������ִ� ������</param>
    private void ActiveSkill(SkillData skillData)
    {
        currentSkill = skillData;
        if (isAttacRange) //������ ��ų����� �϶��� üũ�ؼ� 
        {
            ClearLineRenderer();        //������ ������� ���� ������ ����� 
            isAttacRange = false;       //��� ������� 

        }
        if (TurnManager.Instance.CurrentTurn is PlayerTurnObject pto) //���� ������ üũ�ϰ� ����ȯ�����ϸ�true �ϱ� �Ʊ���
        {
            BattleMapPlayerBase player = (BattleMapPlayerBase)pto.CurrentUnit; //�Ʊ����̸� �Ʊ������� �������������� �׳�����ȯ��Ų��.
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(player.CurrentTile); //�̵� ����� 

            switch (currentSkill.SkillType)
            {
                case SkillType.Sniping:
                case SkillType.Normal:
                    AttackRangeTileView(player.CurrentTile, skillData.AttackRange); //���ݹ���ǥ��
                    break;

                case SkillType.Penetrate:
                    PenetrateAttackRangeTileView(player.CurrentTile, skillData.AttackRange); //
                    break;

                case SkillType.rampage:
                    break;
                default:
                    break;
            }
            CurrentSkill = skillData;   // �������������� ��ų�����͵� ����
        }
       
    }

    /// <summary>
    /// ���콺 �̵� ������ ��ǲ�ý��� ���� �Լ�
    /// </summary>
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
    /// ��ų�� ���ݹ����� ǥ���� �Լ� �Ϲݰ��� ���� 
    /// ����Ÿ�̹� : Ÿ���� �ٲ�ų� ��ų�� �ٲ� ȣ���
    /// </summary>
    /// <param name="targetTile">������ ������ġ</param>
    private void SkillRange_Tile_View(Tile targetTile)
    {
        if (isAttacRange) //���ݹ����� Ȱ��ȭ�Ȼ��¸� ����ǰ� ����
        {
            //Ÿ�����̵��Ǹ� ��������ǥ�� �����ϰ� 
            if (revertAttackRangeTileType.Count > 0)
            {
                for (int i = 0; i < revertAttackRangeTileType.Count; i++)
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i];
                }
                revertAttackRangeTileType.Clear();
                activeAttackTiles.Clear();
            }
            //���Ӱ� ���� ����

            switch (currentSkill.SkillType)
            {
                //���� ǥ�� 
                case SkillType.Sniping:
                case SkillType.Normal:
                    activeAttackTiles.Add(targetTile);
                    revertAttackRangeTileType.Add(targetTile.ExistType);
                    break;
                //������ ǥ�� 
                case SkillType.Penetrate:
                    Set_Penetrate_Attack(targetTile, currentSkill.AttackRange);
                    break;
                // �� ĳ���� �������� ������� ,
                // �������� ���� 5ĭ 3ĭ ǥ�� 
                case SkillType.rampage:
                    break;
                default:
                    break;
            }

            //���±�ü��Ű�� 
            targetTile.ExistType = Tile.TileExistType.Attack_OR_Skill;
        }
    }


    /// <summary>
    /// ���� ���� ǥ���ϱ� 
    /// </summary>
    private void OpenLineRenderer()
    {
        if (!isClear)
        {
            InputSystemController.InputSystem.Mouse.Get_Position.performed += OnMouseMove; //���̸� ����۾� ���� ���� Ÿ�ϰ��������۾�

            foreach (Tile tile in attackRangeTiles)
            {
                revertTileTypes.Add(tile.ExistType);
                tile.ExistType = Tile.TileExistType.AttackRange;
            }
        }
    }

    /// <summary>
    /// ���ݹ��� �ʱ�ȭ �ϱ� .
    /// ������ �ʱ�ȭ�� ������ �ִ°�츸 ������ ����ȴ�.
    /// </summary>
    public void ClearLineRenderer()
    {
        if (!isClear)   //û�Ҹ� �ߺ������ϸ�ȵǴ� üũ�ѹ��ϰ� 
        {
            isClear = true; //û�ҽ��� ����
            if (revertAttackRangeTileType.Count > 0) //��ų ���ݹ����� �����ϸ� 
            {
                for (int i = 0; i < revertAttackRangeTileType.Count; i++) //���� ����Ʈ ã�Ƽ� 
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i]; //������Ű�� 
                }
                revertAttackRangeTileType.Clear(); //�ʱ�ȭ�Ѵ�
                activeAttackTiles.Clear();          //�ʱ�ȭ
            }

            InputSystemController.InputSystem.Mouse.Get_Position.performed -= OnMouseMove; //���̸� ��� �۾� ���� ����Ÿ�ϰ��������۾�

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
            isClear = false;//û�ҳ� ����
        }
    }

    /// <summary>
    /// ���ݹ��� ������ ������ 
    /// ������Ʈ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns>���������� �迭�ι�ȯ ������ null��ȯ</returns>
    public ICharcterBase[] GetEnemyArray(out float LastDamage)
    {
        if (activeAttackTiles.Count > 0)
        {
            LastDamage = currentSkill.FinalDamage;  //�������� �� ������ ����

            ICharcterBase[] enemyArray = SpaceSurvival_GameManager.Instance.EnemyTeam; //��Ʋ���� �������� ���� ��� 

            int enemySize = enemyArray.Length;      // ��Ʋ�ʿ� �����ִ� ������ ���� ��������

            List<ICharcterBase> resultEnemyList = new List<ICharcterBase>(enemySize); //�ִ�ũ��� ���� ����Ʈ���� Ŭ���������� �׳� �ִ������

            foreach (Tile attackTile in activeAttackTiles) //���ݹ�����ŭ �˻��ϰ�
            {
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
            return resultEnemyList.ToArray();
        }
        //���� ���������̾����� ���°�
        LastDamage = 0.0f;  //��� �ʱ�ȭ��
        return null;
    }




    // ----------------------------- ���� ���� ǥ���ϴ� �Լ���

    /// <summary>
    /// ��Ÿ����� ������ ���� ǥ�����ִ� �Լ�
    /// ���� Ÿ�� ������ ���δ�. (����,�Ϲݰ���)
    /// </summary>
    /// <param name="playerTile">ĳ���Ͱ� �ִ� Ÿ�� ��ġ</param>
    /// <param name="size">���ݰ����� ��Ÿ� ���� (�⺻���� 1)</param>
    private void AttackRangeTileView(Tile playerTile, float size = 1.0f)
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
    /// ������� ���ݹ��� ǥ�ÿ� �Լ�
    /// </summary>
    /// <param name="playerTile">ĳ���Ͱ� �ִ� Ÿ�� ��ġ</param>
    /// <param name="size">���ݰ����� ��Ÿ� ���� (�⺻���� 1)</param>
    private void PenetrateAttackRangeTileView(Tile playerTile, float size = 1.0f)
    {
        if (!isAttacRange)
        {
            isAttacRange = true;                                                 //���ݹ���ǥ�� ���� üũ
            ClearLineRenderer();                                                // ������ ����Ʈ �ʱ�ȭ�ϰ� 
            PenetrateSetAttackSize(playerTile, size);                                       // �����ϰ� 
            OpenLineRenderer();                                                 // �����ش�
        }
    }

    /// <summary>
    /// ���� ���ݹ��� ǥ�ÿ� �Լ�
    /// </summary>
    /// <param name="playerTile">�÷��̾����� ��ġ</param>
    /// <param name="size">������</param>
    private void PenetrateSetAttackSize(Tile playerTile, float size = 1.0f)
    {
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
        int currentX = playerTile.width;
        int currentY = playerTile.length;
        int searchIndex = 0;
        int forSize = 0;
        Tile addTile = null;
        int rotateSize = penetrateRotateValues.Length;
        for (int i = 0; i < rotateSize; i++)
        {
            forSize = SetRangeSizeCheck(currentX, currentY, penetrateRotateValues[i].x, penetrateRotateValues[i].y, tileSizeX, tileSizeY, size);
            Debug.Log($" ����Ƚ�� : {i}��°  �÷��̾���ġ :{playerTile}");
            for (int j = forSize; j > 0; j--)
            {
                searchIndex = (currentX + (penetrateRotateValues[i].x * j)) + ((currentY + (penetrateRotateValues[i].y * j)) * tileSizeY); //�ε������ϱ� 
                //Debug.Log($"�ε����� : {searchIndex} forSize :{for}");
                addTile = mapTiles[searchIndex];
                Debug.Log($"X :{(currentX + (penetrateRotateValues[i].x * j))} , Y:{(currentY + (penetrateRotateValues[i].y * j))} , sX:{penetrateRotateValues[i].x} ,sY:{penetrateRotateValues[i].y}, i:{i},j:{j} , tileSizeY:{tileSizeY}");
                if (!attackRangeTiles.Contains(addTile)) //�ߺ�������
                {
                    attackRangeTiles.Add(addTile); //��ȯ ��ų ����Ʈ�� �߰��Ѵ�.
                }
            }
        }//   15 14 365 for5       9   -11 
        //15 + -1 * 1

       
    }


    /// <summary>
    /// ������ ǥ�� 
    /// </summary>
    /// <param name="playerTile">ĳ���Ͱ��ִ� Ÿ����ġ</param>
    /// <param name="size">���� Ÿ�Կ����� ���� (�⺻���� 1)</param>
    private void Set_Penetrate_Attack(Tile playerTile, float size = 1.0f)
    {
        if (!isClear)
        {
            Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
            int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
            int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
            int searchX = 0;
            int searchY = 0;

            switch (attackDir)
            {
                case DirectionRangeType.None:
                    //��ǥ��
                    break;
                case DirectionRangeType.North:
                    searchY = 1;
                    //�������� �޿� ǥ�� 
                    break;
                case DirectionRangeType.North | DirectionRangeType.East:
                    searchY = 1;
                    searchX = 1;
                    //�ϵ��� ǥ��
                    break;
                case DirectionRangeType.East:
                    searchX = 1;
                    //�������� �޿� ǥ��
                    break;
                case DirectionRangeType.East | DirectionRangeType.South:
                    searchY = -1;
                    searchX = 1;
                    //���������� �޿� ǥ��
                    break;
                case DirectionRangeType.South:
                    searchY = -1;
                    //�������� �޿� ǥ��
                    break;
                case DirectionRangeType.West | DirectionRangeType.South:
                    searchY = -1;
                    searchX = -1;
                    //���������� �޿� ǥ��
                    break;
                case DirectionRangeType.West:
                    searchX = -1;
                    //�������� �޿� ǥ��
                    break;
                case DirectionRangeType.North | DirectionRangeType.West:
                    searchX = -1;
                    searchY = 1;
                    //�ϼ��� ǥ��
                    break;
                default:
                    break;
            }
            int currentX = playerTile.width;
            int currentY = playerTile.length;
            int searchIndex = 0;
            //�����ڸ� üũ�ϴ·��� 
            int forSize = SetRangeSizeCheck(currentX,currentY,searchX,searchY,tileSizeX,tileSizeY,size);
            Tile addTile;
            for (int i = 1; i < forSize; i++)
            {
                searchIndex = (currentX + (searchX * i)) + (currentY + (searchY * i) * tileSizeY); //�ε������ϱ� 
                addTile = mapTiles[searchIndex];
                if (addTile.ExistType != Tile.TileExistType.AttackRange) //���ݹ����ȿ������� 
                {
                    activeAttackTiles.Add(addTile); //�߰�
                    revertAttackRangeTileType.Add(addTile.ExistType);
                }
            }

        }
    }


    /// <summary>
    /// ���ݹ����� �ʳ����� üũ�ϴ� ����
    /// ���ݹ��� float �̶� �Ҽ������ϴ� ���󰥼����ִ�. ��ǥ�� int �� 
    /// </summary>
    /// <param name="currentX">������ġ x��ǥ��</param>
    /// <param name="currentY">������ġ y��ǥ��</param>
    /// <param name="searchX">�˻�x ���� (-1,0,1)</param>
    /// <param name="searchY">�˻�y ���� (-1,0,1)</param>
    /// <param name="tileMaxX">Ÿ�� ���� �ִ� ����</param>
    /// <param name="tileMaxY">Ÿ�� ���� �ִ� ����</param>
    /// <param name="rangeSize">���� ���ݹ���</param>
    /// <returns>for������ ������� ��ȯ</returns>
    private int SetRangeSizeCheck(int currentX, int currentY , int searchX , int searchY, int tileMaxX, int tileMaxY , float rangeSize)
    {
        //����������ġ�� ���ʳ����� üũ�ؼ� ���
        float tempIndex = currentX + (searchX * rangeSize); // �¿� ��갪 
        if (tempIndex < 0) //���ʳ��� �Ѿ���� üũ   
        {
            // -1 �����ϰ��  0��ǥ������ ǥ�õǾߵ�
            // ����ġ�� 3�ϰ��  3, 2, 1, 0  4�� �������µ� ����ġ���� �ʿ������ 3���� Ÿ��ã�ƿ���ȴ�  
            return currentX + 1; //������ ���۰��� 0�̾ƴ� 1�� �������̱⶧���� +1 �� ���ش�
        }
        else if(tempIndex > tileMaxX - 1)  //�������� �Ѿ���� üũ
        {
            // �ִ� Ÿ�ϰ����� 5����� �����ϰ� ���� �ε����� 3�̶�� ġ�� ������ϸ� 6�� ���̳����µ�  
            // ������ Ÿ���ε����� 4���̴� �ڽ���Ÿ�� 3�� �����ϸ�  1ĭ�� ���Ǹ�ȴ� . 
            // ������ ������ 1�̰� 1���� ����Ǵ� 5 - 3 ���� ����ؼ� ���Ͻ�Ų��.
            return tileMaxX - currentX; //������ ������ 0�̾ƴ� 1�̴� -1�� �����ʰ� ����Ѵ�.
        }

        tempIndex = currentY + (searchY * rangeSize);
        if (tempIndex < 0) //�Ʒ��� �Ѿ���� üũ 
        {
            return currentY + 1; //x ������ ���� 
        }
        else if (tempIndex > tileMaxY - 1)  //���� �Ѿ���� üũ
        {
            return tileMaxY - currentY; // x ������ ����
        }

        return (int)rangeSize+1; //���� ������ �Ȱɸ��� �׳� ������ ��ȯ
    }

























    /// <summary>
    /// ���� ��ġ�������� ��Ÿ� ���� ���� ������ ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// �Ϲݰ��� , ���� ������ �����Ҽ� �ִ� ������ ����ǥ�õǾߵǴ� ��ų�� ���� �Լ�
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
