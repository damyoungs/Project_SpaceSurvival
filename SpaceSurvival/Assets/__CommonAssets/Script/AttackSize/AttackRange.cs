using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackRange : MonoBehaviour
{
    /// <summary>
    /// 공격 가능한 범위를 표시해줄 컴포넌트
    /// 휠  , 캐릭터가 바라보는 방향  
    /// </summary>
    [Flags]
    enum AttackRangeType : byte
    {
        None = 0,
        Dot = 1,                // 한칸 마우스가 있는지점
        Line = 2,               // 캐릭터로 부터 일직선 으로 나가는 라인
        Pie = 4,                // 캐릭터 기준으로부터 부채꼴로 펼쳐지는 라인 
        Cross = 8,               // 마우스가 있는지점(가운데)에서 십자로  동서남북으로 뻗어나가는 두선
        Cube = 16,              // 마우스가 있는지점을 중점으로 정사각형으로 표시해주는 방법
        XLine = 32,             // 마우스가 있는지점(가운데)에서 대각선으로 X 형식으로 뻗어 나가는 두선
                                // 아래는 휠로 돌렸을때 모양이 바뀌는 설정값들 
        Horizontal = 64,        // 마우스가 있는지점(가운데)에서 가로로  좌 우로 뻗어나가는 일직선 
        Vertical = 128,         // 마우스가 있는지점(가운데)에서 세로로  위 아래로 뻗어나가는 일직선
    }
    /// <summary>
    /// 기준점으로부터의 회전방향 
    /// 카메라 회전도있기때문에 카메라가 회전이 안되있는값이 기준이된다. 
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
    /// 어택버튼이나 스킬버튼을눌러서 
    /// 범위표시하기위한 로직을 실행중인지 체크할 변수
    /// </summary>
    public bool isAttacRange = false;


    /// <summary>
    /// 일반공격의 범위표시나 
    /// 스킬공격의 범위표시가 시작됬음을 체크할 변수
    /// </summary>
    public bool isSkillAndAttack = false;

    /// <summary>
    /// 타일의 레이어값을 저장해둘 변수
    /// </summary>
    [SerializeField]
    int tileLayerIndex;

    /// <summary>
    /// 레이의 길이 
    /// </summary>
    [SerializeField]
    float ray_Range = 30.0f;

    /// <summary>
    /// 청소 끝나고나서 다시검색할수있게 체크하는변수
    /// </summary>
    bool isClear = false;

    //---------- 공격범위 표시용 변수
    /// <summary>
    /// 공격가능한 범위의 타일들을 담아둘 리스트
    /// </summary>
    [SerializeField]
    List<Tile> attackRangeTiles;

    /// <summary>
    /// 공격범위 표시하기전에 저장해둘 타일 타입
    /// attackRangeTiles 과 순서를 맞춰줘야한다.
    /// </summary>
    [SerializeField]
    List<Tile.TileExistType> revertTileTypes;




    //---------- 스킬(일반공격) 범위 표시용 변수
    /// <summary>
    /// 공격이나 스킬이 표시될 타일리스트
    /// </summary>
    List<Tile> activeAttackTiles;
    
    /// <summary>
    /// 공격타입에따른 복원시킬 이전타일속성  
    /// activeAttackTiles 과 순서를 맞춰야한다.
    /// </summary>
    List<Tile.TileExistType> revertAttackRangeTileType;


    /// <summary>
    /// 공격범위 표시해줄 타일 위치
    /// </summary>
    [SerializeField]
    Tile attackCurrentTile;
    Tile AttackCurrentTile
    {
        get => attackCurrentTile;
        set
        {
            if (attackCurrentTile != value) //타일이 매번들어오겟지만 비교해서 다른타일일때만 
            {
                attackCurrentTile = value;
                //로직실행하자
                SkillRange_Tile_View(value);

            }
        }
    }

    /// <summary>
    /// 현재 공격범위표시해줄 타입 
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
                //사용 스킬을 교체하면 실행 
            }
        }
    }

    /// <summary>
    /// 현재 공격방향을 정할 값
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
                //휠이나 방향을 바꾸는 인풋이 들어올때 처리
            }
        }
    }

    /// <summary>
    /// 현재 공격을 행하는 유닛 
    /// </summary>
    Player_ skill_Use_Charcter;
    Player_ Skill_Use_Charcter 
    {
        get => skill_Use_Charcter;
        set 
        {
            if (skill_Use_Charcter != value) //다른캐릭으로 바꼈으면 
            {
                if (skill_Use_Charcter != null)  //기존캐릭터 있을때 
                {
                    skill_Use_Charcter.on_ActiveSkill = null; //액션연결끊고 
                }
                skill_Use_Charcter = value; //새롭게 컨트롤할 캐릭터 셋팅하고 
                skill_Use_Charcter.on_ActiveSkill = ActiveSkill; //액션연결 다시한다.
            }
        }
    }

    public Func<Player_> playerCharcter;

    /// <summary>
    /// 유닛이 사용하고있는 스킬 
    /// </summary>
    SkillData currentSkill;
    SkillData CurrentSkill 
    {
        get=> currentSkill;
        set 
        {
            if (currentSkill != value)  //사용중인 스킬이 바뀔때
            {
                currentSkill = value;                   // 값셋팅하고 
                SkillRange_Tile_View(attackCurrentTile);// 범위표시다시처리
            }
        }
    }
    /// <summary>
    /// 8방향 좌표값 순서로 저장해놓기 
    /// 관통(Penetrate)설정시 사용 
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

        SpaceSurvival_GameManager.Instance.GetAttackRangeComp = () => this; //데이터 연결하기 
    }

    /// <summary>
    /// 턴 시작할때 초기화할 함수
    /// </summary>
    /// <param name="controllUnit">컨트롤할 유닛</param>
    public void InitDataSet(Player_ controllUnit) 
    {
        if (controllUnit != null) 
        {
            Skill_Use_Charcter = controllUnit;
        }
    }


    /// <summary>
    /// 캐릭터 쪽에서 스킬을 누르거나 단축키로 스킬을 사용할때 발동하는 함수 
    /// 내쪽에서는 들어온 스킬로 범위를 구분해서 표시해주면된다.
    /// </summary>
    /// <param name="skillData">스킬에대한 정보를 가지고있는 데이터</param>
    private void ActiveSkill(SkillData skillData)
    {
        currentSkill = skillData;
        if (isAttacRange) //기존에 스킬사용중 일때를 체크해서 
        {
            ClearLineRenderer();        //기존에 사용중인 범위 데이터 지우고 
            isAttacRange = false;       //사용 제어를끄고 

        }
        if (TurnManager.Instance.CurrentTurn is PlayerTurnObject pto) //현재 턴인지 체크하고 형변환가능하면true 니깐 아군턴
        {
            BattleMapPlayerBase player = (BattleMapPlayerBase)pto.CurrentUnit; //아군턴이면 아군유닛이 무조건있음으로 그냥형변환시킨다.
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(player.CurrentTile); //이동 지우고 

            switch (currentSkill.SkillType)
            {
                case SkillType.Sniping:
                case SkillType.Normal:
                    AttackRangeTileView(player.CurrentTile, skillData.AttackRange); //공격범위표시
                    break;

                case SkillType.Penetrate:
                    PenetrateAttackRangeTileView(player.CurrentTile, skillData.AttackRange); //
                    break;

                case SkillType.rampage:
                    break;
                default:
                    break;
            }
            CurrentSkill = skillData;   // 범위셋팅했으면 스킬데이터도 갱신
        }
       
    }

    /// <summary>
    /// 마우스 이동 감지용 인풋시스템 연결 함수
    /// </summary>
    private void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPos = context.ReadValue<Vector2>();
        AttackRangeView(mouseScreenPos);
    }

    /// <summary>
    /// 마우스 위치에따른 타일 찾기
    /// </summary>
    /// <param name="mouseScreenPos">마우스의 스크린 좌표</param>
    private void AttackRangeView(Vector2 mouseScreenPos)
    {
        if (!isClear) //타일갱신중에 중복실행되면 안되니 체크
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);      // 화면에서 현재 마우스의 위치로 쏘는 빛
            Debug.DrawRay(ray.origin, ray.direction * ray_Range, Color.black, 1.0f);              // 디버그용 레이저

            RaycastHit[] hitObjets = Physics.RaycastAll(ray, ray_Range); //레이를 쏴서 충돌한 오브젝트 리스트를 받아온다.

            foreach (RaycastHit hit in hitObjets) // 내용이 있는경우 내용을 실행한다.
            {
                if (hit.collider.gameObject.layer == tileLayerIndex) //타일인지 체크하고 
                {
                    Tile cusorTile = hit.transform.GetComponent<Tile>();
                    if (cusorTile.ExistType == Tile.TileExistType.AttackRange) //공격범위안에서만 보여야한다. 
                    {
                        AttackCurrentTile = cusorTile; //찾은 타일을 계속 입력해준다!!!

                    }
                    //다른방법없나..? 구조를바꿔야..되나?
                    //다른방법 Tile 클래스 내부에다가 OnMouseEnter 함수를 이용해서 데이터를 덮어씌우는방법도있긴한데.. 어떤걸쓸가..
                    break; //한번찾으면 더이상 찾을필요없으니 나가자.
                }
            }
        }
    }


    /// <summary>
    /// 스킬의 공격범위를 표시할 함수 일반공격 포함 
    /// 실행타이밍 : 타일이 바뀌거나 스킬이 바뀔때 호출됨
    /// </summary>
    /// <param name="targetTile">공격할 원점위치</param>
    private void SkillRange_Tile_View(Tile targetTile)
    {
        if (isAttacRange) //공격범위가 활성화된상태만 실행되게 설정
        {
            //타일이이동되면 기존범위표시 삭제하고 
            if (revertAttackRangeTileType.Count > 0)
            {
                for (int i = 0; i < revertAttackRangeTileType.Count; i++)
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i];
                }
                revertAttackRangeTileType.Clear();
                activeAttackTiles.Clear();
            }
            //새롭게 범위 셋팅

            switch (currentSkill.SkillType)
            {
                //한점 표시 
                case SkillType.Sniping:
                case SkillType.Normal:
                    activeAttackTiles.Add(targetTile);
                    revertAttackRangeTileType.Add(targetTile.ExistType);
                    break;
                //일직선 표시 
                case SkillType.Penetrate:
                    Set_Penetrate_Attack(targetTile, currentSkill.AttackRange);
                    break;
                // 내 캐릭터 기준으로 정면방향 ,
                // 원점으로 부터 5칸 3칸 표시 
                case SkillType.rampage:
                    break;
                default:
                    break;
            }

            //상태교체시키기 
            targetTile.ExistType = Tile.TileExistType.Attack_OR_Skill;
        }
    }


    /// <summary>
    /// 공격 범위 표시하기 
    /// </summary>
    private void OpenLineRenderer()
    {
        if (!isClear)
        {
            InputSystemController.InputSystem.Mouse.Get_Position.performed += OnMouseMove; //레이를 쏘는작업 시작 원점 타일가져오기작업

            foreach (Tile tile in attackRangeTiles)
            {
                revertTileTypes.Add(tile.ExistType);
                tile.ExistType = Tile.TileExistType.AttackRange;
            }
        }
    }

    /// <summary>
    /// 공격범위 초기화 하기 .
    /// 기존에 초기화할 내용이 있는경우만 로직이 실행된다.
    /// </summary>
    public void ClearLineRenderer()
    {
        if (!isClear)   //청소를 중복으로하면안되니 체크한번하고 
        {
            isClear = true; //청소시작 셋팅
            if (revertAttackRangeTileType.Count > 0) //스킬 공격범위가 존재하면 
            {
                for (int i = 0; i < revertAttackRangeTileType.Count; i++) //원복 리스트 찾아서 
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i]; //원복시키고 
                }
                revertAttackRangeTileType.Clear(); //초기화한다
                activeAttackTiles.Clear();          //초기화
            }

            InputSystemController.InputSystem.Mouse.Get_Position.performed -= OnMouseMove; //레이를 쏘는 작업 끄기 원점타일가져오는작업

            if (revertTileTypes.Count > 0) //초기화 할 타일이있을때만  
            {
                int listSize = revertTileTypes.Count; //갯수가져와서
                for (int i = 0; i < listSize; i++)
                {
                    attackRangeTiles[i].ExistType = revertTileTypes[i]; // 기존에 저장해뒀던 값으로 다시돌리고 
                }
                attackRangeTiles.Clear();  // 내용 비우고  clear 함수는 내부 배열요소만 초기화하기때문에 null보다 낫다.
                revertTileTypes.Clear();    // 내용 비운다.
            }
            isClear = false;//청소끝 셋팅
        }
    }

    /// <summary>
    /// 공격범위 선택을 했을시 
    /// 적리스트를 반환하는 함수
    /// </summary>
    /// <returns>적이있으면 배열로반환 없으면 null반환</returns>
    public ICharcterBase[] GetEnemyArray(out float LastDamage)
    {
        if (activeAttackTiles.Count > 0)
        {
            LastDamage = currentSkill.FinalDamage;  //몬스터한테 줄 데미지 셋팅

            ICharcterBase[] enemyArray = SpaceSurvival_GameManager.Instance.EnemyTeam; //배틀맵의 몹정보를 전부 들고 

            int enemySize = enemyArray.Length;      // 배틀맵에 나와있는 몬스터의 갯수 가져오고

            List<ICharcterBase> resultEnemyList = new List<ICharcterBase>(enemySize); //최대크기는 몬스터 리스트보다 클수없음으로 그냥 최대로잡자

            foreach (Tile attackTile in activeAttackTiles) //공격범위만큼 검색하고
            {
                for (int i = 0; i < enemySize; i++) //적들을 검색을 진행 
                {
                    if (enemyArray[i].CurrentTile.width == attackTile.width &&
                        enemyArray[i].CurrentTile.length == attackTile.length) //타일이 같으면 
                    {
                        resultEnemyList.Add(enemyArray[i]); //리스트에 추가
                        break;//다음타일검색을위해 빠져나감
                    }
                }
            }
            return resultEnemyList.ToArray();
        }
        //여긴 공격할적이없을때 오는곳
        LastDamage = 0.0f;  //기냥 초기화값
        return null;
    }




    // ----------------------------- 공격 범위 표시하는 함수들

    /// <summary>
    /// 사거리안의 범위를 전부 표시해주는 함수
    /// 단일 타겟 용으로 쓰인다. (저격,일반공격)
    /// </summary>
    /// <param name="playerTile">캐릭터가 있는 타일 위치</param>
    /// <param name="size">공격가능한 사거리 범위 (기본값은 1)</param>
    private void AttackRangeTileView(Tile playerTile, float size = 1.0f)
    {
        if (!isAttacRange)
        {
            isAttacRange = true;                                                 //공격범위표시 시작 체크
            ClearLineRenderer();                                                // 기존의 리스트 초기화하고 
            SetAttackSize(playerTile, size);                                       // 셋팅하고 
            OpenLineRenderer();                                                 // 보여준다
        }
    }




    /// <summary>
    /// 관통관련 공격범위 표시용 함수
    /// </summary>
    /// <param name="playerTile">캐릭터가 있는 타일 위치</param>
    /// <param name="size">공격가능한 사거리 범위 (기본값은 1)</param>
    private void PenetrateAttackRangeTileView(Tile playerTile, float size = 1.0f)
    {
        if (!isAttacRange)
        {
            isAttacRange = true;                                                 //공격범위표시 시작 체크
            ClearLineRenderer();                                                // 기존의 리스트 초기화하고 
            PenetrateSetAttackSize(playerTile, size);                                       // 셋팅하고 
            OpenLineRenderer();                                                 // 보여준다
        }
    }

    /// <summary>
    /// 직선 공격범위 표시용 함수
    /// </summary>
    /// <param name="playerTile">플레이어유닛 위치</param>
    /// <param name="size">범위값</param>
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
            Debug.Log($" 포문횟수 : {i}번째  플레이어위치 :{playerTile}");
            for (int j = forSize; j > 0; j--)
            {
                searchIndex = (currentX + (penetrateRotateValues[i].x * j)) + ((currentY + (penetrateRotateValues[i].y * j)) * tileSizeY); //인덱스구하기 
                //Debug.Log($"인덱스값 : {searchIndex} forSize :{for}");
                addTile = mapTiles[searchIndex];
                Debug.Log($"X :{(currentX + (penetrateRotateValues[i].x * j))} , Y:{(currentY + (penetrateRotateValues[i].y * j))} , sX:{penetrateRotateValues[i].x} ,sY:{penetrateRotateValues[i].y}, i:{i},j:{j} , tileSizeY:{tileSizeY}");
                if (!attackRangeTiles.Contains(addTile)) //중복값방지
                {
                    attackRangeTiles.Add(addTile); //반환 시킬 리스트로 추가한다.
                }
            }
        }//   15 14 365 for5       9   -11 
        //15 + -1 * 1

       
    }


    /// <summary>
    /// 일직선 표시 
    /// </summary>
    /// <param name="playerTile">캐릭터가있는 타일위치</param>
    /// <param name="size">공격 타입에따른 범위 (기본값은 1)</param>
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
                    //비표시
                    break;
                case DirectionRangeType.North:
                    searchY = 1;
                    //북쪽으로 쭈욱 표시 
                    break;
                case DirectionRangeType.North | DirectionRangeType.East:
                    searchY = 1;
                    searchX = 1;
                    //북동쪽 표시
                    break;
                case DirectionRangeType.East:
                    searchX = 1;
                    //동쪽으로 쭈욱 표시
                    break;
                case DirectionRangeType.East | DirectionRangeType.South:
                    searchY = -1;
                    searchX = 1;
                    //남동쪽으로 쭈욱 표시
                    break;
                case DirectionRangeType.South:
                    searchY = -1;
                    //남쪽으로 쭈욱 표시
                    break;
                case DirectionRangeType.West | DirectionRangeType.South:
                    searchY = -1;
                    searchX = -1;
                    //남서쪽으로 쭈욱 표시
                    break;
                case DirectionRangeType.West:
                    searchX = -1;
                    //서쪽으로 쭈욱 표시
                    break;
                case DirectionRangeType.North | DirectionRangeType.West:
                    searchX = -1;
                    searchY = 1;
                    //북서쪽 표시
                    break;
                default:
                    break;
            }
            int currentX = playerTile.width;
            int currentY = playerTile.length;
            int searchIndex = 0;
            //가장자리 체크하는로직 
            int forSize = SetRangeSizeCheck(currentX,currentY,searchX,searchY,tileSizeX,tileSizeY,size);
            Tile addTile;
            for (int i = 1; i < forSize; i++)
            {
                searchIndex = (currentX + (searchX * i)) + (currentY + (searchY * i) * tileSizeY); //인덱스구하기 
                addTile = mapTiles[searchIndex];
                if (addTile.ExistType != Tile.TileExistType.AttackRange) //공격범위안에있으면 
                {
                    activeAttackTiles.Add(addTile); //추가
                    revertAttackRangeTileType.Add(addTile.ExistType);
                }
            }

        }
    }


    /// <summary>
    /// 공격범위가 맵끝인지 체크하는 로직
    /// 공격범위 float 이라 소수점이하는 날라갈수도있다. 좌표는 int 라서 
    /// </summary>
    /// <param name="currentX">현재위치 x좌표값</param>
    /// <param name="currentY">현재위치 y좌표값</param>
    /// <param name="searchX">검색x 범위 (-1,0,1)</param>
    /// <param name="searchY">검색y 범위 (-1,0,1)</param>
    /// <param name="tileMaxX">타일 가로 최대 갯수</param>
    /// <param name="tileMaxY">타일 세로 최대 갯수</param>
    /// <param name="rangeSize">현재 공격범위</param>
    /// <returns>for문돌릴 사이즈값을 반환</returns>
    private int SetRangeSizeCheck(int currentX, int currentY , int searchX , int searchY, int tileMaxX, int tileMaxY , float rangeSize)
    {
        //범위최종위치가 왼쪽끝인지 체크해서 계산
        float tempIndex = currentX + (searchX * rangeSize); // 좌우 계산값 
        if (tempIndex < 0) //왼쪽끝을 넘어갓는지 체크   
        {
            // -1 이하일경우  0좌표까지만 표시되야됨
            // 내위치가 3일경우  3, 2, 1, 0  4개 가나오는데 내위치값은 필요없으니 3개만 타일찾아오면된다  
            return currentX + 1; //포문의 시작값을 0이아닌 1로 잡을것이기때문에 +1 을 해준다
        }
        else if(tempIndex > tileMaxX - 1)  //오른쪽을 넘어갓는지 체크
        {
            // 최대 타일갯수가 5개라고 가정하고 현재 인덱스를 3이라고 치고 계산을하면 6가 값이나오는데  
            // 마지막 타일인덱스는 4번이니 자신의타일 3을 제외하면  1칸만 계산되면된다 . 
            // 포문의 시작은 1이고 1번만 돌면되니 5 - 3 으로 계산해서 리턴시킨다.
            return tileMaxX - currentX; //포문의 시작은 0이아닌 1이니 -1을 하지않고 계산한다.
        }

        tempIndex = currentY + (searchY * rangeSize);
        if (tempIndex < 0) //아래를 넘어갓는지 체크 
        {
            return currentY + 1; //x 계산법과 동일 
        }
        else if (tempIndex > tileMaxY - 1)  //위를 넘어갓는지 체크
        {
            return tileMaxY - currentY; // x 계산법과 동일
        }

        return (int)rangeSize+1; //위에 조건이 안걸리면 그냥 설정값 반환
    }

























    /// <summary>
    /// 현재 위치지점에서 사거리 기준 공격 가능한 범위 의 좌표리스트를 가져오기위한 함수
    /// 일반공격 , 저격 과같은 공격할수 있는 범위가 전부표시되야되는 스킬에 사용될 함수
    /// </summary>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="attackCheck">공격가능한 거리 값</param>
    /// <returns>캐릭터가 공격가능한 노드리스트</returns>
    private void SetAttackSize(Tile currentNode, float attackCheck)
    {
        List<Tile> openList = new List<Tile>();   // 탐색이 필요한 노드 리스트 
        List<Tile> closeList = new List<Tile>();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; //도착지점이 없는상태라서 맥스값 넣으니 제대로 안돌아간다.
            node.AttackCheckG = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.AttackCheckG = 0.0f; //내위치는 g 가 0이다

        while (openList.Count > 0)
        {
            currentNode = openList[0];
            openList.Remove(currentNode); // 탐색가능한 목록에서 현재 탐색중인 목록을 제거하고 
            closeList.Add(currentNode);   // 탐색종료한 리스트에 현재 목록을 담는다.

            if (currentNode.AttackCheckG > attackCheck) //G 값이 현재 이동 가능한 거리보다 높으면  더이상 탐색이 필요없음으로 
            {
                continue; //다음거 탐색 
            }
            else // 이동가능한 거리면 
            {
                if (!attackRangeTiles.Contains(currentNode)) //중복값방지
                {
                    attackRangeTiles.Add(currentNode); //반환 시킬 리스트로 추가한다.
                }
            }

            OpenListAdd(mapTiles, tileSizeX, tileSizeY, currentNode, openList, closeList); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
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
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // 사이드 검색 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //사이드 검색
                    continue;

                adjoinTile = Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length + y, tileSizeX);    // 인접한 타일 가져오기

                if (adjoinTile == currentNode)                                          // 인접한 타일이 (0, 0)인 경우
                    continue;
                if (adjoinTile.ExistType == Tile.TileExistType.Prop)                // 인접한 타일이 장애물일때
                    continue;
                bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length, tileSizeX).ExistType == Tile.TileExistType.Prop ||
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width, currentNode.Length + y, tileSizeX).ExistType == Tile.TileExistType.Prop
                    )
                    continue;
                //대각선 체크 이유는 이동도 안되는데 공격은 되면 안될거같아서 남겨뒀다.
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
