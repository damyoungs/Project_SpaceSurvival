using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTest : TestBase
{
    public GameObject centerTile;           // �߾ӿ� ����� Ÿ��
    public GameObject sideTile;             // �ܰ��� ��ġ�� Ÿ��
    public GameObject vertexTile;           // ������ Ÿ��
    public GameObject wall;                 // ��


    int sizeX = 0;                          // Ÿ�� ���� ����
    int sizeY = 0;                          // Ÿ�� ���� ����

    public int tileCount = 0;               // Ÿ���� ��
    public int wallCount = 0;

    bool isExist = false;                   // Ÿ�� ���� ����

    Vector3 mainTileSize = Vector3.zero;    // �߾� Ÿ�� ������
    Vector3 sideTileSize = Vector3.zero;    // ���̵� Ÿ�� ������
    Vector3 vertexTileSize = Vector3.zero;  // ������ Ÿ�� ������

    Vector3 startPos = new Vector3();       // ���Ŀ� ĳ���� ���� ��ġ. ������ �ӽ������� (0, 0, 0) ���� ����

    GameObject[] mapTiles;                // Ÿ�� ������Ʈ ��ü�� ���� �迭
    List<GameObject> walls;
    //GameObject[] walls;


    private void Start()
    {
        // �߾� Ÿ�� ������ ��ȯ
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size * centerTile.transform.GetChild(0).localScale.x;
        // ���̵� Ÿ�� ������ ��ȯ
        sideTileSize = sideTile.GetComponentInChildren<BoxCollider>().size * sideTile.transform.GetChild(0).localScale.x;      
        // ������ Ÿ�� ������ ��ȯ
        vertexTileSize = vertexTile.GetComponentInChildren<BoxCollider>().size * vertexTile.transform.GetChild(0).localScale.x;
    }

    /// <summary>
    /// Ÿ�� ���� ����
    /// </summary>
    protected override void Test1(InputAction.CallbackContext context)
    {
        if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
        {
            sizeX = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
            sizeY = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
            tileCount = sizeX * sizeY;          // �� Ÿ�� ����
            mapTiles = new GameObject[tileCount];   // �迭 ���� ����

            wallCount = 2 * sizeX + 2 * sizeY - 4;
            //walls = new GameObject[wallCount];      // �� Ÿ�� ���� ����
            walls = new List<GameObject>(wallCount);

            GameObject wallObject;

            for (int i = 0; i < tileCount; i++)
            {
                int width = i % sizeX;              // ���� �ε��� ��ȣ
                int length = i / sizeX;             // ���� �ε��� ��ȣ

                // Ÿ�� ����
                if ((width == 0 && length == 0) || (width == 0 && length == sizeY - 1) || (width == sizeX - 1 && length == 0) || (width == sizeX - 1 && length == sizeY - 1))
                {


                    mapTiles[i] = Instantiate(vertexTile, gameObject.transform);            // ������ Ÿ�� ����
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.vertexTile;       // Ÿ�� ��ũ��Ʈ�� Ÿ�� ����
                    wallObject = Instantiate(wall, mapTiles[i].transform);
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.7f));
                    //walls.Add(wallObject);
                    wallObject = Instantiate(wall, mapTiles[i].transform);
                    wallObject.transform.Translate(new Vector3(1.7f, 0.0f, 1.0f));
                    //walls.Add(wallObject);
                    if (width == 0 && length == 0)
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                    }
                    else if (width == 0 && length == sizeY - 1)
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                    }
                    else if (width == sizeX - 1 && length == 0)
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                    }
                    else if (width == sizeX - 1 && length == sizeY - 1)
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 360.0f, 0));
                    }
                }
                else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)              // ���̵� Ÿ�� ȸ��
                {
                    mapTiles[i] = Instantiate(sideTile, gameObject.transform);              // ���̵� Ÿ�� ����
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.sideTile;         // Ÿ�� ��ũ��Ʈ�� Ÿ�� ����
                    wallObject = Instantiate(wall, mapTiles[i].transform);
                    wallObject.transform.Translate(new Vector3(1, 0.0f, -1.7f));
                    //walls.Add(wallObject);

                    if (width == 0)                                                             // ���� ������
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                    }
                    else if (width == sizeX - 1)                                                // ������ ������
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                    }
                    else if (length == 0)                                                        // �� ����
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                    }
                    //else if (j == sizeY - 1)                                              // �� �Ʒ���
                    //{
                    //    mapTiles[i, j].transform.Rotate(new Vector3(0, 360.0f, 0));
                    //}
                }
                else
                {
                    mapTiles[i] = Instantiate(centerTile, gameObject.transform);                        // �߾� Ÿ�� ����
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));        // �߾� Ÿ�� ���� ȸ��(�׳� �̰���)
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.centerTile;                   // Ÿ�� ��ũ��Ʈ�� Ÿ�� ����
                }

                // Ÿ�� ��ġ �̵�. startPos�� �ӽ÷� �־���� ��(0, 0, 0)
                mapTiles[i].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * width,
                                                            0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * length);
            }
        
            //for (int i = 0; i < wallCount; i++)
            //{
            //    wallObject = Instantiate(wall);
            //    wallObject.transform.position = new Vector3(mapTiles[0].transform.position.x + 1.0f,
            //                                                mapTiles[0].transform.position.y,
            //                                                mapTiles[0].transform.position.z + 1.7f);
            //    //walls.Add(wallObject);
            //}

            isExist = true;
        }

    }

    /// <summary>
    /// Ÿ�� ����
    /// </summary>
    protected override void Test2(InputAction.CallbackContext context)
    {
        for (int i = 0; i < tileCount; i++)
        {
            Destroy(mapTiles[i]);
        }


        isExist = false;
    }
}
