//using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapTest : TestBase
{
    public GameObject player;               // 플레이어
    public Material material;               // 필요없을 것 같아 아마 지울 예정

    public GameObject centerTile;           // 중앙에 사용할 타일
    public GameObject sideTile;             // 외곽에 배치될 타일
    public GameObject vertexTile;           // 꼭지점 타일
    public GameObject wall;                 // 기본 벽
    public GameObject pointLight;           // 조명
    public GameObject pillar;               // 기둥

    public int sizeX = 0;                   // 타일 가로 갯수
    public int sizeY = 0;                   // 타일 세로 갯수
    public int tileCount = 0;               // 타일의 수

    public List<GameObject> singleProps;    // 1칸만 차지하는 물체
    public List<GameObject> multiProps;     // 2칸 이상의 타일을 차지하는 물체

    bool isExist = false;                   // 타일 존재 여부
    bool isPropExist = false;

    Vector3 mainTileSize = Vector3.zero;    // 중앙 타일 사이즈
    Vector3 sideTileSize = Vector3.zero;    // 사이드 타일 사이즈
    Vector3 vertexTileSize = Vector3.zero;  // 꼭지점 타일 사이즈

    GameObject[] mapTiles;                  // 타일 오브젝트 객체를 담을 배열
    List<GameObject> props;                 // 지형 지물을 담을 배열

    GameObject[] lights;                    // 조명
    GameObject[] pillars;                   // 기둥
    Tile[] standardPos;                // 기준 위치(조명과 기둥이 있을 위치)

    private void Start()
    {
        // 중앙 타일 사이즈 반환     - 밑의 두 타일과 사이즈 같음(혹시 몰라 밑에도 구했지만 필요없으면 지울 예정)
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size;
        // 사이드 타일 사이즈 반환
        sideTileSize = sideTile.GetComponentInChildren<BoxCollider>().size;      
        // 꼭지점 타일 사이즈 반환
        vertexTileSize = vertexTile.GetComponentInChildren<BoxCollider>().size;
    }

    /// <summary>
    /// 타일 랜덤 생성
    /// </summary>
    protected override void Test1(InputAction.CallbackContext _)
    {
        if (!isExist)                   // 타일이 존재하지 않을 경우에만 생성
        {
            sizeX = Random.Range(20, 31);       // 타일 가로 갯수 랜덤 생성
            sizeY = Random.Range(20, 31);       // 타일 세로 갯수 랜덤 생성
            tileCount = sizeX * sizeY;          // 총 타일 갯수

            MapInstantiate();                       // 메인 맵 생성
            LightInstantiate();                     // 조명 및 기둥 생성
            //MiniMapInstantiate();                 // 미니맵 판자 생성(필요없을 것 같아 나중에 지울 예정)

            //player.transform.position = GetTile(sizeX / 2, sizeY).transform.position;                     // 플레이어 위치 이동
            player.transform.position = GetTile(sizeX / 2, sizeY / 3).transform.position;         // 플레이어 위치 이동(임시)

            isExist = true;         // 중복 맵 생성 방지

        }
    }

    /// <summary>
    /// 타일 제거
    /// </summary>
    protected override void Test2(InputAction.CallbackContext _)
    {
        if (isExist)
        {
            MapDestroy();
        }
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        if (isExist)
        {
            PropInstantiate();
            isPropExist = true;
        }
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        if (isPropExist)
        {
            PropDestroy();
            isPropExist = false;
        }
    }


    /// <summary>
    /// 메인 맵 생성하는 함수
    /// </summary>
    private void MapInstantiate()
    {

        mapTiles = new GameObject[tileCount];   // 배열 동적 생성
        GameObject wallObject;          // 벽 오브젝트

        for (int i = 0; i < tileCount; i++)
        {
            int width = i % sizeX;              // 가로 인덱스 번호
            int length = i / sizeX;             // 세로 인덱스 번호

            // 타일 생성
            if ((width == 0 && length == 0) || (width == 0 && length == sizeY - 1) || (width == sizeX - 1 && length == 0) || (width == sizeX - 1 && length == sizeY - 1))
            {
                // 꼭지점인 경우
                TileInstantiate(i, vertexTile, Tile.MapTileType.vertexTile, width, length);      // 꼭지점 타일 생성
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
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                }
                else if (width == 0 && length == sizeY - 1)                         // 왼쪽 아래
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                }
                else if (width == sizeX - 1 && length == 0)                         // 오른쪽 위
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 0.0f, 0));
                }
                else if (width == sizeX - 1 && length == sizeY - 1)                 // 오른쪽 아래
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                }
            }
            else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)
            {
                // 가장자리일 경우
                TileInstantiate(i, sideTile, Tile.MapTileType.sideTile, width, length);             // 사이드 타일 생성
                wallObject = Instantiate(wall, mapTiles[i].transform);                              // 측면 벽 생성
                wallObject.transform.Translate(new Vector3(1, 0.0f, -1.75f));                       // 측면 벽 이동

                if (width == 0)                                                                     // 왼쪽 세로줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                }
                else if (width == sizeX - 1)                                                        // 오른쪽 세로줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                }
                else if (length == 0)                                                               // 맨 윗줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 0.0f, 0));
                }
                else if (length == sizeY - 1)                                                       // 맨 아랫줄
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                }
            }
            else
            {
                // 가장자리가 아닌 경우
                TileInstantiate(i, centerTile, Tile.MapTileType.centerTile, width, length);              //중앙 타일 생성
                mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));        // 중앙 타일 랜덤 회전(그냥 미관상)
            }

            mapTiles[i].transform.position = new Vector3(mainTileSize.x * width, 0, mainTileSize.z * length);
        }
    }

    /// <summary>
    /// 맵 제거하는 함수
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
            Destroy(pillars[i]);
        }

        isExist = false;
    }

    /// <summary>
    /// 이차원 좌표를 타일로 반환하는 함수
    /// </summary>
    /// <param name="width">가로 인덱스</param>
    /// <param name="length">세로 인덱스</param>
    /// <returns></returns>
    private GameObject GetTile(int width, int length)
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
    private void TileInstantiate(int i, GameObject type, Tile.MapTileType tileType, int width, int length)
    {
        mapTiles[i] = Instantiate(type, gameObject.transform);                  // type에 따른 타일 생성
        mapTiles[i].GetComponent<Tile>().TileType = tileType;              // 타일 스크립트에 타입 저장
        mapTiles[i].GetComponent<Tile>().Width = width;                         // 타일 가로 인덱스 저정
        mapTiles[i].GetComponent<Tile>().Length = length;                       // 타일 세로 인덱스 저정
        mapTiles[i].GetComponent<Tile>().Index = i;
    }

    /// <summary>
    /// 조명과 기둥 생성 및 이동
    /// </summary>
    private void LightInstantiate()
    {
        standardPos = new Tile[4];         // 기준 위치 생성
        pillars = new GameObject[4];            // 기둥 동적 생성
        lights = new GameObject[4];             // 조명 동적 생성

        standardPos[0] = GetTile(sizeX / 3 - 1, sizeY / 3 - 1).GetComponent<Tile>();
        standardPos[1] = GetTile(sizeX - sizeX / 3 + 1, sizeY / 3 - 1).GetComponent<Tile>();
        standardPos[2] = GetTile(sizeX / 3 - 1, sizeY - sizeY / 3 + 1).GetComponent<Tile>();
        standardPos[3] = GetTile(sizeX - sizeX / 3 + 1, sizeY - sizeY / 3 + 1).GetComponent<Tile>();

        for (int i = 0; i < 4; i++)
        {
            standardPos[i].GetComponent<Tile>().ExistTypes = Tile.ExistType.prop;                                 // 기둥이 있는 타일의 타입 지정

            pillars[i] = Instantiate(pillar, gameObject.transform);                                               // 기둥 생성
            pillars[i].transform.position = standardPos[i].transform.position;                                    // 기둥 이동

            lights[i] = Instantiate(pointLight, gameObject.transform);                                            // 조명 생성
            lights[i].transform.position = standardPos[i].transform.position + new Vector3(0.0f, 20.0f, 0.0f);    // 조명 이동
        }
    }

    /// <summary>
    /// 미니맵 판자 생성(지울 예정)
    /// </summary>
    private void MiniMapInstantiate()
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

    /// <summary>
    /// 구조물 생성 함수
    /// </summary>
    private void PropInstantiate()
    {
        if (props == null)
        {
            props = new List<GameObject>(9);
        }
        else
        {
            return;
        }

        int chooseProp;

        int[] tempArrayX = new int[4] { 0, standardPos[0].Width, standardPos[1].Width, sizeX };
        int[] tempArrayY = new int[4] { 0, standardPos[0].Length, standardPos[2].Length, sizeY };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                chooseProp = Random.Range(0, multiProps.Count);
                PropMultiMaking(chooseProp, tempArrayX[i], tempArrayX[i + 1], tempArrayY[j], tempArrayY[j + 1]);
            }
        }

        for (int i = 0; i < Random.Range(0, singleProps.Count); i++)
        {
            chooseProp = Random.Range(0, singleProps.Count);
            PropSingleMaking(chooseProp);
        }

    }


    private void PropSingleMaking(int chooseProp)
    {
        GameObject obj = Instantiate(singleProps[chooseProp]);
        obj.transform.position = GetTile(Random.Range(0, sizeX), Random.Range(0, sizeY)).transform.position;
        props.Add(obj);
    }

    private void PropMultiMaking(int chooseProp, int index1, int index2, int index3, int index4)
    {
        GameObject obj = Instantiate(multiProps[chooseProp]);
        //Vector3Int getPos = new Vector3Int(Random.Range(0, standardPos[0].Width * 2), 0, Random.Range(standardPos[0].Length * 2, sizeY * 2));
        GameObject tile = GetTile(Random.Range(index1, index2), Random.Range(index3, index4));

        obj.transform.position = tile.transform.position;

        props.Add(obj);
    }

    private void PropMultiMaking2(int chooseProp, int index1, int index2, int index3, int index4)
    {
        GameObject obj = Instantiate(multiProps[chooseProp]);

        Vector3Int getPos = new Vector3Int(Random.Range(index1 * 2, index2 * 2), 0, Random.Range(index3 * 2, index4 * 2));
        //Vector3Int getPos = new Vector3Int(Random.Range(0, standardPos[0].Width * 2), 0, Random.Range(standardPos[0].Length * 2, sizeY * 2));

        if (obj.GetComponent<PropData>().width % 2 == 1)
        {
            if (getPos.x % 2 == 1)
            {
                getPos.x++;
            }
        }
        else
        {
            if (getPos.x % 2 == 0)
            {
                getPos.x++;
            }
        }

        if (obj.GetComponent<PropData>().length % 2 == 1)
        {
            if (getPos.z % 2 == 1)
            {
                getPos.z++;
            }
        }
        else
        {
            if (getPos.z % 2 == 0)
            {
                getPos.z++;
            }
        }

        obj.transform.position = getPos;

        props.Add(obj);
    }

    private void PropDestroy()
    {
        foreach (var obj in props)
        {
            Destroy(obj);
        }
        props.Clear();
        props = null;
    }
}
