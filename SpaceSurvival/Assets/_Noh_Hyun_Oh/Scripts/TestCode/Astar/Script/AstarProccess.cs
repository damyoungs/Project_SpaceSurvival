using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


/// <summary>
/// A* 맵 하나에 사용될 매니저 클래스 
/// 기능 추가:  워프가있으면 해당좌표끼리 휴리스틱 값을 조정하는 기능을 추가해야한다.
/// </summary>
public class AstarProccess 
{
    public enum NodeState 
    {
        None =0,            //초기값
        Nomal,              //기본
        Inaccessible,       //접근불가능
    }
    /// <summary>
    /// A* 적용될 노드 데이터 이차 배열 x y  접근하기 쉽게 2차배열로 적용
    /// </summary>
    private Astar_Node[,] nodes;
    public Astar_Node[,] Nodes => nodes;

    /// <summary>
    /// 시작 될 노드 정보
    /// </summary>
    private Astar_Node startNode;
    
    /// <summary>
    /// 도착 할 노드 정보
    /// </summary>
    private Astar_Node endNode;

    /// <summary>
    /// 현재 검색중인 노드정보 
    /// </summary>
    private Astar_Node currentNode;


    /// <summary>
    /// 노드 오픈 리스트 
    /// </summary>
    private List<Astar_Node> openList;

    /// <summary>
    /// 노드 클로즈 리스트
    /// </summary>
    private List<Astar_Node> closeList;

    /// <summary>
    /// 정사각형의 직선 값 
    /// </summary>
    const float nomalLine = 1.0f;

    /// <summary>
    /// 정사각형의 대각선 값 1.414 배 
    /// 정사각형이라 배율이 변하지않는다.
    /// </summary>
    //readonly float diagonalLine = Mathf.Sqrt((nomalLine*nomalLine) + (nomalLine * nomalLine)); //대각선 값구하기 1.414 입력해도 되긴하다.
    readonly float diagonalLine = 1.414f;


    public AstarProccess()
    {
        // 리스트 생성해두기
        openList = new List<Astar_Node>(); 
        closeList = new List<Astar_Node>(); 

    }

    /// <summary>
    /// A* 적용할 데이터 입력후 초기화 시킨다.
    /// </summary>
    /// <param name="horizontalSize">가로 갯수</param>
    /// <param name="verticalSize"> 세로 갯수</param>
    /// <param name="obstaclesArray">장애물 인덱스 배열</param>
    public void InitData(int horizontalSize, int verticalSize, int[] obstaclesArray = null)
    {
        openList.Clear();

        closeList.Clear();

        CreateNodeArray(horizontalSize, verticalSize); // 맵 배열만들고 

        if (this.nodes != null && obstaclesArray != null) //못가는 지역이있으면
        {
            PlaceObstacles(obstaclesArray, horizontalSize, verticalSize); //못가는지역 등록한다.
        }
    }

    /// <summary>
    /// 접근 불가능한 지역 설정 
    /// obstaclesArray 값이 좌표값 기준으로 오름차순 정렬이 된상태여야 정상작동이된다.
    /// </summary>
    /// <param name="obstaclesArray">접근불가능한 지역 인덱스 저장해둔 배열</param>
    /// <param name="horizontalSize">가로 크기</param>
    /// <param name="verticalSize">세로 크기</param>
    private void PlaceObstacles(int[] obstaclesArray, int horizontalSize, int verticalSize)
    {
        int i = 0; // 인덱스 값 체크할 변수 
        int obstacleIndex = 0; //장애물 인덱스값 체크할 변수
        for (int y = 0; y < verticalSize; y++) // 전체의 가로갯수만큼 돌고
        {
            for (int x = 0; x < horizontalSize; x++) // 전체의 세로갯수만큼 돌아서 다돌린다.
            {
                if (obstaclesArray.Length > obstacleIndex && obstaclesArray[obstacleIndex] == i) //인덱스값이 들어있음으로 체크해서 
                {
                    nodes[y, x].State = NodeState.Inaccessible; //같은 인덱스값에 넣어준다.
                    obstacleIndex++;//다음 장애물 인덱스를 찾기위해 인덱스 증가
                }
                i++;//전체 인덱스 값 증가 
            }
        }
    }


    /// <summary>
    /// A* 적용할 데이터 입력후 초기화 시킨다.
    /// </summary>
    /// <param name="horizontalSize">가로 갯수</param>
    /// <param name="verticalSize"> 세로 갯수</param>
    /// <param name="obstaclesArray">장애물 인덱스 배열</param>
    public void InitData(int horizontalSize, int verticalSize, Vector2Int [] obstaclesArray = null)
    {
        openList.Clear();

        closeList.Clear();

        CreateNodeArray(horizontalSize, verticalSize); // 맵 배열만들고 

        if (this.nodes != null && obstaclesArray != null) //못가는 지역이있으면
        {
            PlaceObstacles(obstaclesArray, horizontalSize, verticalSize); //못가는지역 등록한다.
        }
    }

    /// <summary>
    /// 접근 불가능한 지역 설정 
    /// obstaclesArray 값이 좌표값 기준으로 오름차순 정렬이 된상태여야 정상작동이된다.
    /// </summary>
    /// <param name="obstaclesArray">접근불가능한 지역 좌표값을 저장해둔 배열</param>
    /// <param name="horizontalSize">가로 크기</param>
    /// <param name="verticalSize">세로 크기</param>
    private void PlaceObstacles(Vector2Int[] obstaclesArray, int horizontalSize, int verticalSize)
    {
        int i = 0; // 인덱스 값 체크할 변수 
        int obstacleIndex = 0; //장애물 인덱스값 체크할 변수
        for (int y = 0; y < verticalSize; y++) // 전체의 가로갯수만큼 돌고
        {
            for (int x = 0; x < horizontalSize; x++) // 전체의 세로갯수만큼 돌아서 다돌린다.
            {
                if (obstaclesArray.Length > obstacleIndex &&
                    obstaclesArray[obstacleIndex].x == x &&
                    obstaclesArray[obstacleIndex].y == y
                    ) //인덱스값이 들어있음으로 체크해서 
                {
                    nodes[y, x].State = NodeState.Inaccessible; //같은 인덱스값에 넣어준다.
                    obstacleIndex++;//다음 장애물 인덱스를 찾기위해 인덱스 증가
                }
                i++;//전체 인덱스 값 증가 
            }
        }
    }

    /// <summary>
    /// 위치값 두개를 가지고 가장 가까운 경로를 탐색한다.
    /// 시작위치와 도착 위치가 유효한지 체크도해서 유효안하면 근처로 바꿔야함.
    /// </summary>
    /// <param name="startIndex">시작위치 인덱스</param>
    /// <param name="endIndex">도착위치 인덱스</param>
    public Astar_Node GetShortPath(int startIndex, int endIndex)
    {
        ResetValue();                     // 경로와 G , H 값을 리셋 시킨다.

        int startX = startIndex == 0 ? 0 : startIndex % nodes.GetLength(0);  //시작위치 의 x좌표값 
        int startY = startIndex == 0 ? 0 : startIndex / nodes.GetLength(1);  //시작위치 의 y좌표값

        int endY = endIndex == 0 ? 0 : endIndex / nodes.GetLength(1);    //도착위치 의 y좌표값
        int endX = endIndex == 0 ? 0 : endIndex % nodes.GetLength(0);    //도착위치 의 x좌표값

        Debug.Log($"{nodes.GetLength(0)} , {nodes.GetLength(1)} , {startIndex} ,{endIndex}");
        Debug.Log($"start : {startX},{startY} end : {endX},{endY}");
        startNode = nodes[startY, startX];  // 시작위치와
        endNode = nodes[endY, endX];        // 도착위치를 셋팅하고 
        SetHeuristicsValue(endNode); //도착 지점 노드를 이용해 노드들의 휴리스틱 값으르 셋팅한다.

        return PathFinding(); //길찾기를 실행한다.
    }


    /// <summary>
    /// 위치값 두개를 가지고 가장 가까운 경로를 탐색한다.
    /// 시작위치와 도착 위치가 유효한지 체크도해서 유효안하면 근처로 바꿔야함.
    /// </summary>
    /// <param name="startPos">시작위치 인덱스</param>
    /// <param name="endPos">도착위치 인덱스</param>
    public Astar_Node ShortestdistanceLine(Vector2Int startPos, Vector2Int endPos)
    {
        ResetValue();                     // 경로와 G , H 값을 리셋 시킨다.

        startNode = nodes[startPos.y, startPos.x];  // 시작위치와
        endNode = nodes[endPos.y, endPos.x];        // 도착위치를 셋팅하고 
        SetHeuristicsValue(endNode); //도착 지점 노드를 이용해 노드들의 휴리스틱 값으르 셋팅한다.

        return PathFinding(); //길찾기를 실행한다.
    }




    /// <summary>
    /// 현재 위치지점에서 행동력 기준 이동가능한 범위 의 좌표리스트를 가져오기위한 함수
    /// </summary>
    /// <param name="currentNode">현재 노드위치 </param>
    /// <param name="moveCheck">행동력 값</param>
    /// <returns>캐릭터가 이동가능한 노드리스트</returns>
    public  List<Astar_Node> SetMoveSize(Astar_Node currentNode, float moveCheck)
    {
        List<Astar_Node> resultNode = new List<Astar_Node>(); 
        openList.Clear();   // 탐색이 필요한 노드 리스트 
        closeList.Clear();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 

        foreach (Astar_Node node in nodes) 
        {
            node.ResetMoveCheckValue(); // H 값을 1로 고정시키고 G 값을 초기화하여 계산 을 G값으로만 할수있게 한다.
        }
        currentNode.G = 0.0f; //위에서 값초기화 하고있음으로 처음값을 0으로 다시셋팅

        openList.Add(currentNode);


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

            OpenListAdd(currentNode); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
        }
        return resultNode;
    }






    /// <summary>
    /// 1. 특정노드의 주변노드를 검색해서 오픈 리스트에 담고
    /// 2. 주변노드의 G값을 갱신한다.
    /// </summary>
    /// <param name="currentNode">기준이되는 노드</param>
    private void OpenListAdd(Astar_Node currentNode)
    {
        int horizontalSize = nodes.GetLength(0);    //가로 길이 가져오고 (2차원배열의 ? 배열의 길이 [y,?]) 오른쪽 기준이라 햇갈린다.
        int verticalSize = nodes.GetLength(1);      //세로 길이 가져오고 (2차원배열의 ? 배열의 길이 [?,x])

        //범위벗어낫는지 체크하기
        int horizontalStartIndex = currentNode.X - 1; //현재 위치의 왼쪽 값 가져오기
        horizontalStartIndex = horizontalStartIndex < 0 ? 0 : horizontalStartIndex; //왼쪽끝 값이면 0으로 셋팅

        int horizontalEndIndex = currentNode.X + 1; //현재 위치의 오른쪽 값 가져오기
        horizontalEndIndex = horizontalEndIndex == horizontalSize ? // 맨오른쪽이면  
            horizontalSize    :                                     
            horizontalEndIndex + 1;                                 // 포문 돌값셋팅하기위해 +1로 셋팅 <= 안하고 <로 체크하기위해서 값추가 

        int verticalStartIndex = currentNode.Y - 1; //현재 위치의 아래쪽 값 가져오기
        verticalStartIndex = verticalStartIndex < 0 ? 0 : verticalStartIndex; // 맨아래 값이면 0으로 셋팅 

        int verticalEndIndex = currentNode.Y + 1;                   //현재 위치의 위쪽 값 가져오기
        verticalEndIndex = verticalEndIndex == verticalSize ?       //맨 위면 
                            verticalSize    :                          
                            verticalEndIndex + 1;                       //포문 돌값셋팅하기위해 +1로 셋팅 <= 안하고 <로 체크하기위해서 값추가 
        //범위 체크 끝

        float tempG = 0.0f; //비교할 G값을 담을 임시변수

       
        Astar_Node tempNode; // 체크할 객체 선언(포문안에서 매번 스택에 메모리안잡고 한번만하기위해 밖에서 선언)
        for (int y = verticalStartIndex; y < verticalEndIndex; y++) //범위 지정한 것만큼 포문 돌리기
        {
            for (int x = horizontalStartIndex; x < horizontalEndIndex; x++) //범위 지정한만큼 포문 돌리기
            {
                tempNode = nodes[y, x]; //주변노드를 받아와서 
                if (tempNode.State == NodeState.Inaccessible) continue; //찾아온 주변노드가 못가는 지역이면 다음 반복문으로 이동
                /// G 값 체크 시작 
                /// 
                //대각선인지 직선인지 체크
                if ((currentNode.X - x) == 0 || (currentNode.Y - y) == 0) //직선인경우 
                {
                    tempG = currentNode.G + nomalLine;
                    if (tempNode.G  > tempG) //새로 갱신한 값보다 이전값이 크면  
                    {
                        tempNode.G = tempG;     //G 값을 갱신
                        tempNode.PrevNode = currentNode; //G값이 바꼈으면 어디서왔는지 이전노드도 갱신
                    }
                }
                else //대각선인 경우
                {
                    //(currentNode.X - x) //값이 - 면 오른쪽 + 면 왼쪽 
                    //(currentNode.Y - y) //값이 - 면 위쪽   + 면 아래쪽

                    //장애물 체크
                    if (nodes[
                            currentNode.Y - (currentNode.Y - y ),   // 현재위치에서 검색하는 위치의 값을 빼면 +와 -가 반전됨으로 - 로 처리 
                            currentNode.X                           // ex) 현재 (1,1) 일때  (2,2)확인시 (1,2) , (2,1)을확인해야됨 저값이되려면 +처리로안된다.
                            ].State == NodeState.Inaccessible || //세로부분에 못가는지역이 있거나
                        nodes[
                            currentNode.Y, 
                            currentNode.X - (currentNode.X -  x)
                            ].State == NodeState.Inaccessible)   //가로부분에 못가는지역이 있으면
                    {
                        Debug.Log($"현재위치({currentNode.X},{currentNode.Y}) 장애물 위치 ({x},{y})");
                        continue;//처리안하고 다음 노드를 찾는다.
                    }

                    tempG = currentNode.G + diagonalLine; //새로 생신될 G값 셋팅 대각선값으로
                    if (tempNode.G > tempG) //새로 갱신한 값보다 이전값이 크면  
                    {
                        tempNode.G = tempG;   //G 값을 갱신
                        tempNode.PrevNode = currentNode; //G값이 바꼈으면 어디서왔는지 이전노드도 갱신
                    }
                }

                //G값 체크 끝

                if (!closeList.Contains(tempNode) && !openList.Contains(tempNode))// 탐색이 끝나지 않은 노드 이고 오픈리스트에도 없어야 
                {
                   openList.Add(tempNode); //오픈리스트에 담아둔다.
                }
            }
        }
    }

    /// <summary>
    /// 최적의 경로를 찾는다.
    /// </summary>
    /// <returns>경로가 저장된 노드를 반환한다.</returns>
    private Astar_Node PathFinding()
    {
        openList.Clear();   // 탐색이 필요한 노드 리스트 
        closeList.Clear();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 

        startNode.G = 0.0f; //시작 노드값은 G가 항상 0 이래야한다.
        openList.Add(startNode); // 맨 처음에는 시작 지점 등록해서 A* 로직 시작
  
        while (openList.Count > 0) //오픈리스트의 값이 있으면 계속 탐색 
        {
            currentNode = openList[0]; //현재 탐색 중인 노드를 담아둔다.

            if (currentNode == endNode) // 목적지에 도착했는지 체크한다.
            {
                return currentNode;     //도착했으면 경로가 담긴 노드를 반환한다.
            }

            openList.Remove(currentNode); // 탐색가능한 목록에서 현재 탐색중인 목록을 제거하고 
            closeList.Add(currentNode);   // 탐색종료한 리스트에 현재 목록을 담는다.

            OpenListAdd(currentNode); //현재위치에서 주변 노드를 찾아 G 값을 수정시키고 오픈리스트에 담는다.

            //오름차순 정렬 
            openList.Sort();
        }
        return null; //경로못찾으면 널을 리턴한다.
    }

    /// <summary>
    /// A* 에 사용할 다중배열을 생성한다.
    /// </summary>
    /// <param name="horizontalSize">가로 크기</param>
    /// <param name="verticalSize">세로 크기</param>
    private void CreateNodeArray(int horizontalSize, int verticalSize)
    {
        this.nodes = new Astar_Node[verticalSize, horizontalSize]; //전체크기만큼 배열잡고 
        int i = 0; // 인덱스 계산할 변수선언하고 
        for (int y = 0; y < verticalSize; y++) //세로 만큼
        {
            for (int x = 0; x < horizontalSize; x++)//가로만큼
            {
                this.nodes[y, x] = new Astar_Node(i , x, y); //2차원 배열 만들기 
                i++; //인덱스 증가
            }
        }
    }

    /// <summary>
    /// 노드들의 G,H 값 을 초기화 하는 함수
    /// 경로 재탐색시 필요함
    /// </summary>
    private void ResetValue()
    {
        foreach (var node in nodes)
        {
            node.G =float.MaxValue;
            node.H =float.MaxValue;
            node.PrevNode = null;
        }
    }
    /// <summary>
    /// 도착 위치 기준으로 노드리스트의 휴리스틱 값을 셋팅하는 함수 
    /// </summary>
    /// <param name="endNode">도착 노드 값</param>
    private void SetHeuristicsValue(Astar_Node endNode) 
    {
        float tempX = 0.0f;    //대각선 계산을위해 임시로 사용할 변수 
        float tempY = 0.0f;    //대각선 계산을위해 임시로 사용할 변수 
        float tempLine = 0.0f; //대각선 계산을위해 임시로 사용할 변수 

        foreach (Astar_Node node in nodes) //전체 리스트를 찾아서
        {
            node.H = float.MaxValue; //휴리스틱값을 다시셋팅할때 H 값을 초기화시킨다.

            if (node.X != endNode.X && node.Y != endNode.Y) //대각선인경우 ( x값이나 ,y 값이 같으면 같은라인인것을 이용해 체크)
            {
                // x 와 y 의 차이중 작은 값을 가지고 대각선을 긋고 두값의 차이만큼 더하면 끝 (ㅁ/ 이런모양)

                tempX = Math.Abs(endNode.X - node.X); //가로 라인의 거리값을 구하고 

                tempY = Math.Abs(endNode.Y - node.Y); //세로 라인의 거리값을 구하고

                tempLine = tempX == tempY ?                                         // 거리값이 같으면 정사각형임으로 
                            tempX * diagonalLine                                    // 한변에다가 대각선값을 곱하면끝
                            : tempX > tempY ?                                       // 세로 거리값이 더크면 
                            (tempY * diagonalLine) + ((tempX - tempY) * nomalLine)  // 가로 길이만큼 대각선길이를 곱하고 남은 세로 거리를 직선길이로 곱한다
                            :                                                       // 가로 거리값이 더크면 
                            (tempX * diagonalLine) + ((tempY - tempX) * nomalLine); // 세로 길이만큼 대각선길이를 곱하고 남은 가로 거리를 직선길이로 곱한다.

                node.H = tempLine; //계산된 값을 집어넣는다.
            }
            else if (node.X == endNode.X) //같은 가로 라인에 존재하면
            {
                node.H = Math.Abs(endNode.Y - node.Y) * nomalLine; //세로 차이만큼만 계산하면됨으로 직선계산   
            }
            else if (node.Y == endNode.Y) //같은 세로 라인에 존재하면 
            {
                node.H = Math.Abs(endNode.X - node.X) * nomalLine;  //가로 차이만큼만 직선계산
            }
            else 
            {
                Debug.Log($"무슨경우냐? 들어올일 없을거같은데  : node({node.X},{node.Y}) _ curruntNode({endNode.X},{endNode.Y})");
            }
            Debug.Log($"휴리스틱({node.X},{node.Y}) : {node.H} ");
        }
    }

    /// <summary>
    /// 노드가 가지고있는 경로를 찾아온다.
    /// </summary>
    /// <param name="node">경로 찾을 노드</param>
    /// <returns>노드까지 설정된 경로 </returns>
    public List<Vector3Int> GetPath(Astar_Node node)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        path.Add(new Vector3Int(node.X, node.Y, node.Z));
        Astar_Node prevNode = node.PrevNode;

        while (prevNode != null) //이전경로가 있을때까지 돌려!
        {
            path.Add(new Vector3Int(prevNode.X, prevNode.Y, prevNode.Z)); //노드 정보를 담는다.
            prevNode = prevNode.PrevNode; //이전노드 정보 찾아오기
        }
        path.Reverse(); //들어간 값을 꺼꾸로 뒤집는다.
        return path;
    }
    /// <summary>
    /// 인덱스에 해당하는 노드값 가져오기
    /// </summary>
    /// <param name="startIndex">인덱스값</param>
    /// <returns>인덱스에 해당하는 노드정보</returns>
    public Astar_Node GetNode(int startIndex)
    {
        if (nodes != null && nodes.Length > 0 && startIndex < nodes.Length)
        {
            if (startIndex < 1)
            {
                return nodes[0, 0];
            }
            int startX = startIndex == 0 ? 0 : startIndex % nodes.GetLength(0);  //시작위치 의 x좌표값 
            int startY = startIndex == 0 ? 0 : startIndex / nodes.GetLength(1);  //시작위치 의 y좌표값

            return nodes[startY, startX];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 인덱스에 해당하는 노드값 가져오기
    /// <param name="x">x인덱스 좌표</param>
    /// <param name="y">y인덱스 좌표</param>
    /// <param name="z">z인덱스 좌표? 값없음</param>
    /// <returns>인덱스에 해당하는 노드정보</returns>
    public Astar_Node GetNode(int x , int y , int z = 1)
    {
        if (x  < 0 || y < 0  || x > nodes.GetLength(0) || y >  nodes.GetLength(1))
        {
            return nodes[x, y];
        }
        else
        {
            return null;
        }
    }

    //----------- 테스트용 함수




    public void TestGLog()
    {
        string str = "";
        foreach (Astar_Node node in nodes)
        {
            str += $"좌표({node.X},{node.Y}) : G 값 : {node.G} \r\n";
        }
        Debug.Log(str);
    }

   

}