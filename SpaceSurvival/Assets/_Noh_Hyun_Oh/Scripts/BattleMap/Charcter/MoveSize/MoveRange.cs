using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이동 가능한 범위 표시해줄 컴포넌트
/// </summary>
public class MoveRange : MonoBehaviour
{

    /// <summary>
    /// 생성된 타일을 담아둘 배열변수
    /// </summary>
    Tile[] mapTiles;


    /// <summary>
    /// 생성된 타일의 가로 갯수
    /// </summary>
    int tileSizeX;

    /// <summary>
    /// 생성된 타일의 세로 갯수
    /// </summary>
    int tileSizeY;

    /// <summary>
    /// 이동 가능한 타일 담아둘 리스트
    /// </summary>
    List<Tile> activeMoveTiles;

    private void Awake()
    {
        SpaceSurvival_GameManager.Instance.GetMoveRangeComp = () => this; //데이터 연결하기 
        activeMoveTiles = new List<Tile>(); //이동가능한 타일 리스트 초기화 
    }

    /// <summary>
    /// 맵에대한 정보 받아와서 초기화 하기
    /// </summary>
    /// <param name="mapTiles">맵정보</param>
    /// <param name="tileSizeX">맵타일 가로갯수</param>
    /// <param name="tileSizeY">맵타일 세로갯수</param>
    public void InitDataSetting(Tile[] mapTiles, int tileSizeX, int tileSizeY)
    {
        this.mapTiles = mapTiles;
        this.tileSizeX = tileSizeX;
        this.tileSizeY = tileSizeY;
    }
   

    /// <summary>
    /// 데이터 초기화
    /// </summary>
    public void ResetData()
    {
        this.mapTiles = null;
        this.tileSizeX = 0;
        this.tileSizeY = 0;
    }

    /// <summary>
    /// 바닥에 이동가능한 범위를 표시하는 로직 
    /// </summary>
    /// <param name="mapTiles">맵타일 정보 </param>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="moveSize">이동가능한 거리 값</param>
    /// <param name="tileSizeX">맵타일의 최대 가로갯수</param>
    /// <param name="tileSizeY">맵타일의 최대 세로갯수</param>
    public void MoveSizeView(Tile currentNode, float moveSize)
    {
        if (mapTiles == null) return; //초기화 안됬으면 실행안되게 반환

        ClearLineRenderer();
        SetMoveSize(currentNode, moveSize); //이동 가능 리스트 가져오기
        OpenLineRenderer();
    }

    /// <summary>
    /// 기존 이동가능한 내용 초기화 하기
    /// </summary>
    private void ClearLineRenderer() //기존 라인렌더러 끄기
    {
        foreach (Tile tile in activeMoveTiles) 
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>(); // 타일에다가 해당정보를 미리담아두면 좋은데.. 수정이필요하다.
            lineRenderer.enabled = false;
            tile.ExistType = Tile.TileExistType.None; //초기값으로 셋팅 
        }
    }

    /// <summary>
    /// 갈수있는지역 표시하기 
    /// </summary>
    /// <param name="moveTiles">맵타일 정보 </param>
    private void OpenLineRenderer()
    {
        foreach (Tile tile in activeMoveTiles)
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>(); // 타일에다가 해당정보를 미리담아두면 좋은데.. 수정이필요하다.
            lineRenderer.enabled = true;
            tile.ExistType = Tile.TileExistType.Move; //이동 가능하도록 셋팅  
        }
    }


    /// <summary>
    /// 현재 위치지점에서 행동력 기준 이동가능한 범위 의 좌표리스트를 가져오기위한 함수
    /// </summary>
    /// <param name="mapTiles">맵타일 정보 </param>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="moveCheck">이동가능한 거리 값</param>
    /// <param name="tileSizeX">맵타일의 최대 가로갯수</param>
    /// <param name="tileSizeY">맵타일의 최대 세로갯수</param>
    private void SetMoveSize(Tile currentNode, float moveCheck)
    {
        
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
                activeMoveTiles.Add(currentNode); //반환 시킬 리스트로 추가한다.
            }

            OpenListAdd(currentNode, openList, closeList); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
        }
    }

    /// <summary>
    /// 승근씨 8방향 탐색 로직 가져와서 적용
    /// 타입이 none 인것만 가져온다 
    /// </summary>
    /// <param name="mapTiles">맵타일 정보 </param>
    /// <param name="tileSizeX">맵타일의 최대 가로갯수</param>
    /// <param name="tileSizeY">맵타일의 최대 세로갯수</param>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="open">A* 오픈리스트</param>
    /// <param name="close">A* 클로즈 리스트</param>

    private void OpenListAdd(Tile currentNode, List<Tile> open, List<Tile> close)
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

                adjoinTile = mapTiles[(currentNode.Width + x) * tileSizeY + currentNode.Length + y];    // 인접한 타일 가져오기

                if (adjoinTile == currentNode)                                          // 인접한 타일이 (0, 0)인 경우
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None)                // 인접한 타일이 None이 아닐 때
                    continue;
                if (close.Exists((inClose) => inClose == adjoinTile))             // close리스트에 있을 때
                    continue;

                bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
                    mapTiles[(currentNode.Width + x) * tileSizeY + currentNode.Length].ExistType == Tile.TileExistType.Prop ||
                    mapTiles[(currentNode.Width) * tileSizeY + currentNode.Length + y].ExistType == Tile.TileExistType.Prop
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

                if (adjoinTile.G > currentNode.G + distance)
                {
                    open.Add(adjoinTile);
                    adjoinTile.G = currentNode.G + distance;
                }
            }
        }

    }










    /// <summary>
    /// 생성된 타일을 담아둘 배열변수
    /// </summary>
    Tile[,] mapTilesDoubleArray;
    List<Tile> moveTiles;
    /// <summary>
    /// 청소 끝나고나서 다시검색할수있게 체크하는변수
    /// </summary>
    bool isClear = false;
    public void InitDataSetting(Tile[,] mapTiles, int tileSizeX, int tileSizeY)
    {
        this.mapTilesDoubleArray = mapTiles;
        this.tileSizeX = tileSizeX;
        this.tileSizeY = tileSizeY;
    }
    /// <summary>
    /// 데이터 초기화
    /// </summary>
    public void ResetDoubleData()
    {
        this.mapTilesDoubleArray = null;
        this.tileSizeX = 0;
        this.tileSizeY = 0;
    }

    /// <summary>
    /// 바닥에 이동가능한 범위를 표시하는 로직 
    /// </summary>
    /// <param name="mapTiles">맵타일 정보 </param>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="moveSize">이동가능한 거리 값</param>
    /// <param name="tileSizeX">맵타일의 최대 가로갯수</param>
    /// <param name="tileSizeY">맵타일의 최대 세로갯수</param>
    public void MoveSizeDoubleView(Tile currentNode, float moveSize)
    {
        if (!isClear)
        {
            SetMoveDoubleSize(currentNode, moveSize); //이동 가능 리스트 설정하기
            OpenDoubleLineRenderer(currentNode);
        }
    }
    /// <summary>
    /// 갈수있는지역 초기화 하기 내위치까지 초기화되기때문에 내위치는 남겨둔다.
    /// </summary>
    public void ClearDoubleLineRenderer(Tile currentTile) //기존 라인렌더러 끄기
    {
        if (!isClear) 
        {
            isClear = true;
            if (activeMoveTiles.Count > 0) //초기화 할 타일이있을때만  
            {
                Tile.TileExistType currentTileType = currentTile.ExistType; //포문에서 매번 체크하지않기위해 따로담고 
                foreach (Tile tile in activeMoveTiles)
                {
                    tile.ExistType = Tile.TileExistType.None;
                }
                currentTile.ExistType = currentTileType; //수정끝났으면 저장해뒀던 값을 담는다.
                activeMoveTiles.Clear();//초기화끝낫으면 내용 비우기
            }
            isClear = false;
        }
    }

    /// <summary>
    /// 갈수있는지역 표시하기 
    /// </summary>
    private void OpenDoubleLineRenderer(Tile currentTile) //이동가능한범위 의 라인렌더러 키기
    {
        Tile.TileExistType currentTileType = currentTile.ExistType; //포문에서 매번 체크하지않기위해 따로담고 
        foreach (Tile tile in activeMoveTiles)
        {
            tile.ExistType = Tile.TileExistType.Move;
        }
        currentTile.ExistType = currentTileType; //수정끝났으면 저장해뒀던 값을 담는다.
    }
    /// <summary>
    /// 현재 위치지점에서 행동력 기준 이동가능한 범위 의 좌표리스트를 가져오기위한 함수
    /// </summary>
    /// <param name="mapTiles">맵타일 정보 </param>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="moveCheck">이동가능한 거리 값</param>
    /// <param name="tileSizeX">맵타일의 최대 가로갯수</param>
    /// <param name="tileSizeY">맵타일의 최대 세로갯수</param>
    /// <returns>캐릭터가 이동가능한 노드리스트</returns>
    private void SetMoveDoubleSize(Tile currentNode, float moveCheck)
    {
        List<Tile> openList = new List<Tile>();   // 탐색이 필요한 노드 리스트 
        List<Tile> closeList = new List<Tile>();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 

        foreach (Tile node in mapTilesDoubleArray)
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
                activeMoveTiles.Add(currentNode); //반환 시킬 리스트로 추가한다.
            }

            OpenDoubleListAdd(currentNode, openList, closeList); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
        }
    }
    private void OpenDoubleListAdd(Tile currentNode, List<Tile> open, List<Tile> close)
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

                adjoinTile = mapTilesDoubleArray[currentNode.Length + y, currentNode.Width + x];    // 인접한 타일 가져오기

                if (adjoinTile == currentNode)                                          // 인접한 타일이 (0, 0)인 경우
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None)                // 인접한 타일이 None이 아닐 때
                    continue;
                if (close.Exists((inClose) => inClose == adjoinTile))             // close리스트에 있을 때
                    continue;

                bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
                    mapTilesDoubleArray[currentNode.Length, currentNode.Width + x].ExistType == Tile.TileExistType.Prop ||
                    mapTilesDoubleArray[currentNode.Length + y, currentNode.Width].ExistType == Tile.TileExistType.Prop
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

                if (adjoinTile.G > currentNode.G + distance)
                {
                    open.Add(adjoinTile);
                    adjoinTile.G = currentNode.G + distance;
                }
            }
        }

    }

    /// <summary>
    /// 랜덤으로 셋팅 가능한(none인값) 을 찾아서 반환하는 함수  못찾으면 null 반환
    /// </summary>
    /// <param name="tileType">타일위에 올라갈 타입</param>
    /// <returns>추가가능한 타일을반환 하거나 못잡으면 null 반환</returns>
    public Tile GetRandomTile(Tile.TileExistType tileType) 
    {
        int x = mapTilesDoubleArray.GetLength(1)-1;
        int y = mapTilesDoubleArray.GetLength(0)-1;
        Tile result = null;
        int maxCount = 100; //최대 100번만돈다.
        int count = 0;
        //Debug.Log($"{x},{y}");
        while (count < maxCount) //무한 루프 방지용 
        {
            
            result = mapTilesDoubleArray[Random.Range(0, y), Random.Range(0, x)];
            if (result.ExistType == Tile.TileExistType.None)//갈수있는곳이면 
            {
                result.ExistType = tileType; //설정되야될 타입으로 바꾼뒤 
                return result;//빠져나간다.
            }
            else 
            {
                result = null; //못가는 지역이면 초기화 시킨다.   
            }

            count++; //무한루프 방지용
        }
        Debug.Log($"{count} 만큼 돌았는데 다꽉차있는지 못찾았어 ");
        return null;
    }

}
