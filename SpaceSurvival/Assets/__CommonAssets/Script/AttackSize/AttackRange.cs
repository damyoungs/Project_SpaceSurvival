using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 공격 가능한 범위를 표시해줄 컴포넌트
/// 휠  , 캐릭터가 바라보는 방향 
/// </summary>
[Flags]
public enum AttackRangeType : byte
{
    None = 0,
    Dot = 1,                // 한칸 마우스가 있는지점
    Line = 2,               // 캐릭터로 부터 일직선 으로 나가는 라인
    Pie = 4,                // 캐릭터 기준으로부터 부채꼴로 펼쳐지는 라인 
    Cross =8,               // 마우스가 있는지점(가운데)에서 십자로  동서남북으로 뻗어나가는 두선
    Cube = 16,              // 마우스가 있는지점을 중점으로 정사각형으로 표시해주는 방법
    XLine = 32,             // 마우스가 있는지점(가운데)에서 대각선으로 X 형식으로 뻗어 나가는 두선
                            // 아래는 휠로 돌렸을때 모양이 바뀌는 설정값들 
    Horizontal = 64,        // 마우스가 있는지점(가운데)에서 가로로  좌 우로 뻗어나가는 일직선 
    Vertical = 128,         // 마우스가 있는지점(가운데)에서 세로로  위 아래로 뻗어나가는 일직선
}
/// <summary>
/// 기준점으로부터의 회전방향 
/// 카메라 회전도있기때문에 카메라가 회전이 안되있는값이 기준이된다.
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

    /// <summary>
    /// 유닛이 사용하고있는 스킬 
    /// </summary>
    SkillData currentSkill;


    private void Awake()
    {
        attackRangeTiles = new();
        revertTileTypes = new();
        activeAttackTiles = new();
        revertAttackRangeTileType = new();


        SpaceSurvival_GameManager.Instance.GetAttackRangeComp = () => this; //데이터 연결하기 
        tileLayerIndex = LayerMask.NameToLayer("Ground");
     

    }

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
    /// 공격 버튼 누를때 실행할 
    /// 공격범위를 표시해주는 함수 
    /// </summary>
    /// <param name="playerTile">캐릭터가 있는 타일 위치</param>
    /// <param name="size">공격가능한 사거리 범위 (기본값은 1)</param>
    public void AttackRangeTileView(Tile playerTile, float size = 1.0f)
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
    /// 범위표시중 공격스킬이 변환할때 값셋팅
    /// </summary>
    public void AttackSkillChange(SkillData skillData) 
    { 
        
    }

    /// <summary>
    /// 스킬의 공격범위를 표시할 함수 일반공격 포함 
    /// </summary>
    /// <param name="targetTile">공격할 원점위치</param>
    public void SkillRange_Tile_View(Tile targetTile)
    {
        if (isAttacRange) //공격범위가 활성화된상태만 실행되게 설정
        {
            //타일이이동되면 기존범위표시 삭제하고 
            if (revertAttackRangeTileType.Count > 0) 
            {
                for(int i = 0; i< revertAttackRangeTileType.Count; i++)
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i];
                }
                revertAttackRangeTileType.Clear();
                activeAttackTiles.Clear();
            }
            //새롭게 범위 셋팅
            activeAttackTiles.Add(targetTile);
            revertAttackRangeTileType.Add(targetTile.ExistType);


            //상태교체시키기 
            targetTile.ExistType = Tile.TileExistType.Attack_OR_Skill;
        }
    }

    /// <summary>
    /// 공격범위 선택을 했을시 
    /// 적리스트를 반환하는 함수
    /// </summary>
    /// <returns>적이있으면 배열로반환 없으면 null반환</returns>
    public ICharcterBase[] GetEnemyArray() 
    {
        List<ICharcterBase> resultEnemyList = new(4);
        if (activeAttackTiles.Count > 0) 
        {
            ICharcterBase[] enemyArray = SpaceSurvival_GameManager.Instance.EnemyTeam; //배틀맵의 몹정보를 전부 들고 
            int enemySize = enemyArray.Length;
            foreach (Tile attackTile in activeAttackTiles) { //공격범위만큼 검색하고
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
        }
        return resultEnemyList.ToArray();
    }



    /// <summary>
    /// 일반 공격이나 스킬공격의 범위를 셋팅하는 함수 
    /// </summary>
    /// <param name="playerTile">캐릭터가있는 타일위치</param>
    /// <param name="size">공격 타입에따른 범위 (기본값은 1)</param>
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
    /// 공격범위 초기화 하기 .
    /// 기존에 초기화할 내용이 있는경우만 로직이 실행된다.
    /// </summary>
    public void ClearLineRenderer()
    {
        if (!isClear)
        {
            isClear = true;
            if (revertAttackRangeTileType.Count > 0) //스킬 공격범위가 존재하면 
            {
                for (int i = 0; i < revertAttackRangeTileType.Count; i++) //원복 리스트 찾아서 
                {
                    activeAttackTiles[i].ExistType = revertAttackRangeTileType[i]; //원복시키고 
                }
                revertAttackRangeTileType.Clear(); //초기화한다
                activeAttackTiles.Clear();          //초기화
            }

            InputSystemController.InputSystem.Mouse.Get_Position.performed -= OnMouseMove;
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
            isClear = false;
        }
    }
    
    /// <summary>
    /// 공격 범위 표시하기 
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
    /// 현재 위치지점에서 사거리 기준 공격 가능한 범위 의 좌표리스트를 가져오기위한 함수
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
