using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapTest : TestBase
{
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


    public GameObject player;
    public Material material;


    GameObject[] lights;


    private void Start()
    {
        // 중앙 타일 사이즈 반환
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size;
        // 사이드 타일 사이즈 반환
        sideTileSize = sideTile.GetComponentInChildren<BoxCollider>().size;      
        // 꼭지점 타일 사이즈 반환
        vertexTileSize = vertexTile.GetComponentInChildren<BoxCollider>().size;
    }

    /// <summary>
    /// 타일 랜덤 생성
    /// </summary>
    protected override void Test1(InputAction.CallbackContext context)
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

            // 라이트 생성 및 이동
            for (int i = 0; i < 4; i++)
            {
                lights[i] = Instantiate(pointLight);
            }
            lights[0].transform.position = GetTile(sizeX / 3, sizeY / 3).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
            lights[1].transform.position = GetTile(sizeX - sizeX / 3 + 1, sizeY / 3).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
            lights[2].transform.position = GetTile(sizeX / 3, sizeY - sizeY / 3 + 1).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
            lights[3].transform.position = GetTile(sizeX - sizeX / 3 + 1, sizeY - sizeY / 3 + 1).transform.position + new Vector3(0.0f, 20.0f, 0.0f);


            //MiniMapInstantiate();       // 미니맵 생성

            isExist = true;         // 중복 맵 생성 방지
        }

    }

    
    protected override void Test2(InputAction.CallbackContext context)
    {
        MapDestroy();
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

}
