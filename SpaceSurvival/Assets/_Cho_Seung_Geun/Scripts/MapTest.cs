using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapTest : TestBase
{
    [Header("맵 관련 테스트변수")]
    public GameObject centerTile;           // 중앙에 사용할 타일
    public GameObject sideTile;             // 외곽에 배치될 타일
    public GameObject vertexTile;           // 꼭지점 타일
    public GameObject wall;                 // 기본 벽
    public GameObject pointLight;           // 조명

    public int sizeX = 0;                          // 타일 가로 갯수
    public int sizeY = 0;                          // 타일 세로 갯수

    public int tileCount = 0;               // 타일의 수

    bool isExist = false;                   // 타일 존재 여부

    Vector3 mainTileSize = Vector3.zero;    // 중앙 타일 사이즈
    Vector3 sideTileSize = Vector3.zero;    // 사이드 타일 사이즈
    Vector3 vertexTileSize = Vector3.zero;  // 꼭지점 타일 사이즈

    Vector3 startPos = new Vector3();       // 추후에 캐릭터 놓을 위치. 지금은 임시적으로 (0, 0, 0) 으로 설정

    GameObject[] mapTiles;                // 타일 오브젝트 객체를 담을 배열


    GameObject player;


    public Material material;


    GameObject[] lights;
  

    protected override void Awake()
    {
        base.Awake();
        miniMap = GameObject.FindObjectOfType<MiniMapCamera>(true);
        cameraOrigin = GameObject.FindObjectOfType<CameraOriginTarget>(true);
        moveCam = GameObject.FindObjectOfType<Camera_Move>(true);
    }

    private void Start()
    {
        // 중앙 타일 사이즈 반환
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size;
        // 사이드 타일 사이즈 반환
        sideTileSize = sideTile.GetComponentInChildren<BoxCollider>().size;      
        // 꼭지점 타일 사이즈 반환
        vertexTileSize = vertexTile.GetComponentInChildren<BoxCollider>().size;
        
        ///턴관련 변수추가
        turnManager = TurnManager.Instance;
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

   

    /// <summary>
    /// 타일 랜덤 생성
    /// </summary>
    private void MapGenerater() 
    {
        if (!isExist)                   // 타일이 존재하지 않을 경우에만 생성
        {
            
            sizeX = Random.Range(20, 31);       // 타일 가로 갯수 랜덤 생성
            sizeY = Random.Range(20, 31);       // 타일 세로 갯수 랜덤 생성
            tileCount = sizeX * sizeY;          // 총 타일 갯수
            mapTiles = new GameObject[tileCount];   // 배열 동적 생성

            lights = new GameObject[4];

            GameObject wallObject;          // 벽 오브젝트

            for (int i = 0; i < tileCount; i++)
            {
                int width = i % sizeX;              // 가로 인덱스 번호
                int length = i / sizeX;             // 세로 인덱스 번호

                // 타일 생성
                if ((width == 0 && length == 0) || (width == 0 && length == sizeY - 1) || (width == sizeX - 1 && length == 0) || (width == sizeX - 1 && length == sizeY - 1))
                {
                    // 꼭지점인 경우
                    TileInstantiate(i, vertexTile, MapTileType.vertexTile, width, length);      // 꼭지점 타일 생성
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // 측면 벽1 생성
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // 측면 벽1 이동
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // 측면 벽2 생성
                    wallObject.transform.Rotate(new Vector3(0, -90.0f, 0));                     // 측면 벽2 회전
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // 측면 벽2 이동
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // 꼭지점 벽 생성
                    wallObject.transform.Rotate(new Vector3(0, -45.0f, 0));                     // 꼭지점 벽 회전
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -2.0f));             // 꼭지점 벽 이동


                    if (width == 0 && length == 0)                                      // 왼쪽 위
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                    }
                    else if (width == 0 && length == sizeY - 1)                         // 왼쪽 아래
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                    }
                    else if (width == sizeX - 1 && length == 0)                         // 오른쪽 위
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                    }
                    //else if (width == sizeX - 1 && length == sizeY - 1)               // 오른쪽 아래
                    //{
                    //    mapTiles[i].transform.Rotate(new Vector3(0, 360.0f, 0));
                    //}
                }
                else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)
                {
                    // 가장자리일 경우
                    TileInstantiate(i, sideTile, MapTileType.sideTile, width, length);              // 사이드 타일 생성
                    wallObject = Instantiate(wall, mapTiles[i].transform);                          // 측면 벽 생성
                    wallObject.transform.Translate(new Vector3(1, 0.0f, -1.75f));                   // 측면 벽 이동

                    if (width == 0)                                                             // 왼쪽 세로줄
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                    }
                    else if (width == sizeX - 1)                                                // 오른쪽 세로줄
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                    }
                    else if (length == 0)                                                        // 맨 윗줄
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                    }
                    //else if (j == sizeY - 1)                                              // 맨 아랫줄
                    //{
                    //    mapTiles[i, j].transform.Rotate(new Vector3(0, 360.0f, 0));
                    //}
                }
                else
                {
                    // 가장자리가 아닌 경우
                    TileInstantiate(i, centerTile, MapTileType.centerTile, width, length);              //중앙 타일 생성
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));        // 중앙 타일 랜덤 회전(그냥 미관상)
                }

                // 타일 위치 이동. startPos는 임시로 넣어놓은 값(0, 0, 0)
                mapTiles[i].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * width,
                                                            0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * length);
            }

            //player.transform.position = GetTile(sizeX / 2, sizeY).transform.position;       // 플레이어 위치 이동
            player.transform.position = GetTile(sizeX / 2, sizeY - (sizeY / 3)).transform.position;       // 플레이어 위치 이동(임시)

            // 라이트 생성
            LightInstantiate();


            //MiniMapInstantiate();       // 미니맵 생성

            isExist = true;         // 중복 맵 생성 방지
        }
        SetBlock(true);
    }

  

    /// <summary>
    /// 타일 제거
    /// </summary>
    private void MapDestroy()
    {
        for (int i = 0; i < tileCount; i++)
        {
            Destroy(mapTiles[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            Destroy(lights[i]);
        }

        isExist = false;
    }

    /// <summary>
    /// 이차원 좌표를 타일로 반환하는 함수
    /// </summary>
    /// <param name="width">가로 인덱스</param>
    /// <param name="length">세로 인덱스</param>
    /// <returns></returns>
    GameObject GetTile(int width, int length)
    {
        int index = sizeX * length + width;
        return mapTiles[index];
    }

    /// <summary>
    /// 타입에 따른 타일 생성
    /// </summary>
    /// <param name="i">맵타일 인덱스</param>
    /// <param name="type">생성할 타일의 타입</param>
    /// <param name="tileType">타일 스크립트에 저장할 타입</param>
    /// <param name="width">타일의 가로 인덱스</param>
    /// <param name="length">타일의 세로 인덱스</param>
    void TileInstantiate(int i, GameObject type, MapTileType tileType, int width, int length)
    {
        mapTiles[i] = Instantiate(type, gameObject.transform);                  // type에 따른 타일 생성
        mapTiles[i].GetComponent<Tile>().TileType = (int)tileType;              // 타일 스크립트에 타입 저장
        mapTiles[i].GetComponent<Tile>().Width = width;                         // 타일 가로 인덱스 저정
        mapTiles[i].GetComponent<Tile>().Length = length;                       // 타일 세로 인덱스 저정
    }

    /// <summary>
    /// 라이트 생성 및 이동
    /// </summary>
    private void LightInstantiate()
    {
        for (int i = 0; i < 4; i++)
        {
            lights[i] = Instantiate(pointLight);
        }
        lights[0].transform.position = GetTile(sizeX / 3, sizeY / 3).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
        lights[1].transform.position = GetTile(sizeX - sizeX / 3 + 1, sizeY / 3).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
        lights[2].transform.position = GetTile(sizeX / 3, sizeY - sizeY / 3 + 1).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
        lights[3].transform.position = GetTile(sizeX - sizeX / 3 + 1, sizeY - sizeY / 3 + 1).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
    }

    /// <summary>
    /// 미니맵 생성
    /// </summary>
    void MiniMapInstantiate()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 미니맵 위치를 위해 0번 인덱스로 초기화
        Vector3 miniMapPos = mapTiles[0].transform.position;

        if (sizeX % 2 == 0)         // 가로축이 짝수인 경우
        {
            miniMapPos.x = (mapTiles[(sizeX / 2) - 1].transform.position.x + mapTiles[sizeX / 2].transform.position.x) * 0.5f;
        }
        else                        // 가로축이 홀수인 경우
        {
            miniMapPos.x = mapTiles[sizeX / 2].transform.position.x;
        }

        if (sizeY % 2 == 0)         // 세로축이 짝수인 경우
        {
            miniMapPos.z = (mapTiles[((sizeY / 2) - 1) * sizeX].transform.position.z + mapTiles[(sizeY / 2) * sizeX].transform.position.z) * 0.5f;
        }
        else                        // 세로축이 홀수인 경우
        {
            miniMapPos.z = mapTiles[(sizeY / 2) * sizeX].transform.position.z;
        }

        miniMapPos.y += (mainTileSize.y * 0.5f) + 40.0f;
        cube.transform.position = miniMapPos;
        cube.transform.localScale = new Vector3(mainTileSize.x * sizeX, mainTileSize.y, mainTileSize.z * sizeY);
        cube.GetComponent<MeshRenderer>().material = material;
    }











    protected override void Test1(InputAction.CallbackContext context)
    {
        InitTotalData(); //초기데이터생성    
        MapGenerater();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        if (isExist)
        {
            MapDestroy(); //맵삭제

            turnManager.ResetBattleData(); //리셋후

            TurnBaseObject[] ps = GameObject.FindObjectsOfType<TurnBaseObject>(); //대충만든 오브젝트 찾기
            foreach (TurnBaseObject p in ps)
            {
                GameObject.Destroy(p.gameObject); //오브젝트 삭제
            }

            InitTotalData();
            MapGenerater(); //맵생성
        }
    }



    /// <summary>
    /// 턴 진행 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test3(InputAction.CallbackContext context)
    {
        ITurnBaseData node = turnManager.GetNode(); //원래는 캐릭터 쪽에서 턴완료 버튼 호출해야하는데 캐릭터 가없음으로  테스트코드로 찾아온다.
       
        if (node == null)
        {
            Debug.Log("왜못찾냐?");
            return;
        }
        Debug.Log($"{node.UnitBattleIndex}번째 옵젝 : 값 :{node.TurnActionValue} 함수가등록되있냐? : {node.TurnEndAction}");
        node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //턴 진행 시 행동력 감소치 대충 때려넣는다.

        node.TurnEndAction(node); //턴완료 를 알린다.
    }


    /// <summary>
    /// 랜덤한 캐릭터의 상태를 추가
    /// </summary>
    /// <param name="context"></param>
    protected override void Test4(InputAction.CallbackContext context)
    {
        TurnBaseObject tbo = (TurnBaseObject)turnManager.RandomGetNode();
        tbo.BattleUI.AddOfStatus(EnumList.StateType.Poison);//상태이상 추가해보기 
    }





    /// <summary>
    /// 카메라가 벗어나지않게 설정해주는 박스 컬라이더  
    /// 피벗 위치가 Z기준 중앙에서 왼쪽 아래로 바뀌어서 로직수정
    /// </summary>
    public BoxCollider blockCamera;
    private void SetBlock()
    {
        if (blockCamera != null)
        {
            float blockHeight = 100.0f;//높이 대충 카메라가 벗어나지않을정도로 높게
            float cameraPositionCalibration = 4.0f;//보정값 카메라 시야에 따라 수정
            float tempX = sizeX * sideTileSize.x; //전체 가로사이즈 구해오기
            float tempY = sizeY * sideTileSize.x; //전체 세로사이즈 구해오기
            float halfX = tempX * 0.5f; //전체 X 길이의 중간
            float halfY = tempY * 0.5f; //전체 Z 길이의 중간
            float halfTileSize = sideTileSize.x * 0.5f; //타일의 중간값

            blockCamera.size = new Vector3(tempX - cameraPositionCalibration, //카메라 시야때문에 밖이안보이게 보정값을 적용
                                            blockHeight,                    //값 대충셋팅
                                            tempY - cameraPositionCalibration //카메라 시야때문에 밖이안보이게 보정값을 적용
                                            );
            blockCamera.center = new Vector3(halfX - halfTileSize, //x 셋터값은 중앙 위치할수있게 셋팅 타일 크기의 반을추가로빼준다
                                             1.0f, // 높이 대충설정해놔서 이값도 대충
                                             halfY - halfTileSize //Z 값도 중앙에 위치할수있게 반을 
                                             );
        }

    }

    /// <summary>
    /// 카메라가 벗어나지않게 설정해주는 박스 컬라이더 
    /// 피봇 위치 Z 중앙 셋팅값 
    /// </summary>
    private void SetBlock(bool pivotCenter = true)
    {
        if (blockCamera != null)
        {
            float blockHeight = 100.0f; //높이 대충 높게\
            float widthPadding = 1.0f;  //맵크기 에 맞게 카메라 돌리기위한 제한범위 크기조절값
            float heightPadding = 0.5f; //맵크기 에 맞게 카메라 돌리기위한 제한범위 크기조절값

            float tempX = sizeX * sideTileSize.x; //전체 가로사이즈 구해오기
            float tempY = sizeY * sideTileSize.x; //전체 세로사이즈 구해오기
            blockCamera.size = new Vector3(tempX - widthPadding, blockHeight, tempY - heightPadding); //비율 Width 2 : Height 1
            blockCamera.center = new Vector3(-1.0f, 1.0f, (tempY * 0.5f) + 1.0f); //제한범위 중간위치값 구하기
        }

    }



    [Header("턴 관련 테스트 변수")]
    TurnManager turnManager;
    [Header("초기화할 데이터 ")]
    //TurnBaseObject turnUnit;
    MiniMapCamera miniMap;
    CameraOriginTarget cameraOrigin;
    Camera_Move moveCam;
    CinemachineBrain brain;
    private void InitTotalData()
    {
        turnManager.InitTurnData();//초기데이터 셋팅 
        player = turnManager.GetNode().gameObject;
        miniMap.Player = player.transform;
        cameraOrigin.Target = player;
        moveCam.Brain = brain;
        miniMap.gameObject.SetActive(true);
        cameraOrigin.gameObject.SetActive(true);
        moveCam.gameObject.SetActive(true);
    }
}
