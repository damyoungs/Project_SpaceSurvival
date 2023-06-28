using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTest : TestBase
{
    public GameObject centerTile;           // 중앙에 사용할 타일
    public GameObject sideTile;             // 외곽에 배치될 타일
    public GameObject vertexTile;           // 꼭지점 타일
    public GameObject wall;                 // 벽

    int sizeX = 0;                          // 타일 가로 갯수
    int sizeY = 0;                          // 타일 세로 갯수

    public int tileCount = 0;               // 타일의 수

    bool isExist = false;                   // 타일 존재 여부

    Vector3 mainTileSize = Vector3.zero;    // 중앙 타일 사이즈
    Vector3 sideTileSize = Vector3.zero;    // 사이드 타일 사이즈
    Vector3 vertexTileSize = Vector3.zero;  // 꼭지점 타일 사이즈

    Vector3 startPos = new Vector3();       // 추후에 캐릭터 놓을 위치. 지금은 임시적으로 (0, 0, 0) 으로 설정

    GameObject[] mapTiles;                // 타일 오브젝트 객체를 담을 배열


    public GameObject player;


    private void Start()
    {
        // 중앙 타일 사이즈 반환
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size * centerTile.transform.GetChild(0).localScale.x;
        // 사이드 타일 사이즈 반환
        sideTileSize = sideTile.GetComponentInChildren<BoxCollider>().size * sideTile.transform.GetChild(0).localScale.x;      
        // 꼭지점 타일 사이즈 반환
        vertexTileSize = vertexTile.GetComponentInChildren<BoxCollider>().size * vertexTile.transform.GetChild(0).localScale.x;
    }

    /// <summary>
    /// 타일 랜덤 생성
    /// </summary>
    protected override void Test1(InputAction.CallbackContext context)
    {
        if (!isExist)                   // 타일이 존재하지 않을 경우에만 생성
        {
            sizeX = Random.Range(10, 21);       // 타일 가로 갯수 랜덤 생성
            sizeY = Random.Range(10, 21);       // 타일 세로 갯수 랜덤 생성
            tileCount = sizeX * sizeY;          // 총 타일 갯수
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
                    mapTiles[i] = Instantiate(vertexTile, gameObject.transform);                // 꼭지점 타일 생성
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.vertexTile;           // 타일 스크립트에 타입 저장

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
                    // 사이드 타일 생성 및 회전
                    mapTiles[i] = Instantiate(sideTile, gameObject.transform);              // 사이드 타일 생성
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.sideTile;         // 타일 스크립트에 타입 저장
                    wallObject = Instantiate(wall, mapTiles[i].transform);
                    wallObject.transform.Translate(new Vector3(1, 0.0f, -1.75f));

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
                    mapTiles[i] = Instantiate(centerTile, gameObject.transform);                        // 중앙 타일 생성
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));        // 중앙 타일 랜덤 회전(그냥 미관상)
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.centerTile;                   // 타일 스크립트에 타입 저장
                }

                // 타일 위치 이동. startPos는 임시로 넣어놓은 값(0, 0, 0)
                mapTiles[i].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * width,
                                                            0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * length);
            }

            player.transform.position = GetTile(sizeX / 2 + 1, sizeY).transform.position;       // 플레이어 위치 이동

            isExist = true;         // 중복 맵 생성 방지
        }

    }

    /// <summary>
    /// 타일 제거
    /// </summary>
    protected override void Test2(InputAction.CallbackContext context)
    {
        for (int i = 0; i < tileCount; i++)
        {
            Destroy(mapTiles[i]);
        }


        isExist = false;
    }

    /// <summary>
    /// 이차원 좌표를 타일로 반환하는 함수
    /// </summary>
    /// <param name="가로 인덱스"></param>
    /// <param name="세로 인덱스"></param>
    /// <returns></returns>
    GameObject GetTile(int width, int length)
    {
        int index = sizeX * (length - 1) + width - 1;
        return mapTiles[index];
    }
}
