using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharcterMoveSizeCheck : TestBase
{
    /// <summary>
    /// 화면을 찍는 카메라
    /// </summary>
    Camera mainCamera;
    /// <summary>
    /// 찍은 위치로 이동할 캐릭터 속도
    /// </summary>
    float speed = 4.0f;
    /// <summary>
    /// 화면에서 쏘는 빛이 부딪히는 박스콜라이더
    /// </summary>
    BoxCollider target = null;
    /// <summary>
    /// 인풋시스템 클릭
    /// </summary>
    InputKeyMouse inputClick;

    MapTest_2 map;
    List<Tile> path = null;

    Tile currentPos = null;
    public Tile CurrentPos
    {
        get => currentPos;
        set
        {
            if (currentPos != value)
            {
                if (currentPos != null)
                {
                    currentPos.ExistType = Tile.TileExistType.None;
                }
                else
                {
                    // 승근씨거 충돌나니깐 승근씨 스크립트 연결해서 사용하지마시고 
                    // 복사해서 이름바꿔서 사용하도록하세요 픽스버전이아니라 머지할때마다 에러날수가있어요 
                    transform.position = value.transform.position;
                }
                currentPos = value;
                currentPos.ExistType = Tile.TileExistType.Monster;
            }
        }
    }

    
    Tile[] mapTiles;
    MapTest_2 mapData;
    //이동가능한 범위값
    [SerializeField]
    float moveSize = 3.0f;
    protected override void Awake()
    {
        base.Awake();
        inputClick = new InputKeyMouse();
    }

    private void OnEnable()
    {
        inputClick.Mouse.Enable();
        inputClick.Mouse.MouseClick.performed += onClick;

        mapData = FindObjectOfType<MapTest_2>();
        map = FindObjectOfType<MapTest_2>();
        path = new List<Tile>();
    }

    private void OnDisable()
    {
        inputClick.Mouse.MouseClick.performed -= onClick;
        inputClick.Mouse.Disable();

        if (path != null)
            path.Clear();
        path = null;
    }

    /// <summary>
    /// 마우스가 클릭했을 시 일어날 함수
    /// </summary>
    /// <param name="context"></param>
    private void onClick(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());      // 화면에서 현재 마우스의 위치로 쏘는 빛
        Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);              // 디버그용 레이저

        if (Physics.Raycast(ray, out RaycastHit hitInfo))                       // 만약 빛이 부딪히고
        {
            if (hitInfo.transform.gameObject.CompareTag("Tile"))                // 태그 "타일"과 충돌하면
            {
                target = (BoxCollider)hitInfo.collider;                         // 타겟의 박스콜라이더 반환
                Tile tile = target.gameObject.GetComponent<Tile>();
                if (tile.ExistType == Tile.TileExistType.None)
                {
                    path = AStar_2.PathFind(map, CurrentPos, tile);
                }
                //CurrentPos = tile;
                Debug.Log($"타일 위치 : {tile.Width}, {tile.Length}");
            }
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;           //  찍고 있는 카메라 가져오기
    }

    private void FixedUpdate()
    {
        // 타겟이 널포인트가 아니고 타겟이 도착하지 않았을 시 이동
        //if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.01f)
        //{
        //    transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
        //}

        if (path.Count > 0)
        {
            Tile destPath = path[0];

            Vector3 dir = destPath.transform.position - transform.position;

            if (dir.sqrMagnitude < 0.01f)
            {
                transform.position = destPath.transform.position;
                CurrentPos = destPath;
                path.RemoveAt(0);
            }
            else
            {
                transform.Translate(Time.fixedDeltaTime * speed * dir.normalized);
            }
        }

    }































    public void MoveSize() 
    {
        mapTiles = mapData.MapTiles;
        ClearLineRenderer();
        List<Tile> list = SetMoveSize(currentPos, moveSize); //이동 가능 리스트 가져오기
        OpenLineRenderer(list);
    }
 
    protected override void Test9(InputAction.CallbackContext context)
    {
        mapTiles = mapData.MapTiles;
        ClearLineRenderer();
        List<Tile> list = SetMoveSize(currentPos, moveSize); //이동 가능 리스트 가져오기
        OpenLineRenderer(list);

    }
    private void ClearLineRenderer() //기존 라인렌더러 끄기
    {
        foreach (Tile tile in mapTiles)
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>();
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
            }
            continue;
        }
    }
    private void OpenLineRenderer(List<Tile> moveTiles) //이동가능한범위 의 라인렌더러 키기
    {
        foreach (Tile tile in moveTiles)
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
        }
    }
    /// <summary>
    /// 현재 위치지점에서 행동력 기준 이동가능한 범위 의 좌표리스트를 가져오기위한 함수
    /// </summary>
    /// <param name="currentNode">현재 노드위치 </param>
    /// <param name="moveCheck">행동력 값</param>
    /// <returns>캐릭터가 이동가능한 노드리스트</returns>
    public List<Tile> SetMoveSize(Tile currentNode, float moveCheck)
    {
        List<Tile> resultNode = new List<Tile>();
        List<Tile> openList = new List<Tile>();   // 탐색이 필요한 노드 리스트 
        List<Tile> closeList = new List<Tile>();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 

        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; // H 값을 1000로 고정시키고 G 값을 초기화하여 계산 을 G값으로만 할수있게 한다.
            node.G = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.G = 0.0f; //내위치는 g 가 0이다

        while (openList.Count > 0)
        {
            currentNode = openList[0];  
            openList.Remove(currentNode); // 탐색가능한 목록에서 현재 탐색중인 목록을 제거하고 
            closeList.Add(currentNode);   // 탐색종료한 리스트에 현재 목록을 담는다.

            if (currentNode.G > moveCheck) //G 값이 현재 이동 가능한 거리보다 높으면  더이상 탐색이 필요없음으로 
            {
                continue; //다음거 탐색 
            }
            else // 이동가능한 거리면 
            {
                resultNode.Add(currentNode); //반환 시킬 리스트로 추가한다.
            }

            OpenListAdd(currentNode,openList,closeList); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
        }
        return resultNode;
    }

    /// <summary>
    /// 승근씨 8방향 탐색 로직 가져와서 적용
    /// </summary>
    /// <param name="currentNode">내위치</param>
    /// <param name="open">오픈리스트</param>
    /// <param name="close">클로즈리스트</param>
    private void OpenListAdd(Tile currentNode , List<Tile> open , List<Tile> close) 
    {
        Tile adjoinTile;
        float sideDistance = 1.0f;
        float diagonalDistance = 1.414f;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (currentNode.Width + x < 0 || currentNode.Width + x > mapData.sizeX - 1 || // 사이드 검색 
                    currentNode.Length + y < 0 || currentNode.Length + y > mapData.sizeY - 1) //사이드 검색
                    continue;

                adjoinTile = mapData.GetTile(currentNode.Width + x, currentNode.Length + y);    // 인접한 타일 가져오기

                if (adjoinTile == currentNode)                                          // 인접한 타일이 (0, 0)인 경우
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None)                // 인접한 타일이 None이 아닐 때
                    continue;
                if (close.Exists((inClose) => inClose == adjoinTile))             // close리스트에 있을 때
                    continue;

                bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
                    (mapData.IsWall(currentNode.Width + x, currentNode.Length) ||
                    mapData.IsWall(currentNode.Width, currentNode.Length + y)))
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

                if (adjoinTile.G > currentNode.G + distance)
                {
                    open.Add(adjoinTile);
                    adjoinTile.G = currentNode.G + distance;
                }
            }
        }

    }
    
}
