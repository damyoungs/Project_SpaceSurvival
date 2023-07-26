using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapTest : TestBase
{
    public GameObject centerTile;           // �߾ӿ� ����� Ÿ��
    public GameObject sideTile;             // �ܰ��� ��ġ�� Ÿ��
    public GameObject vertexTile;           // ������ Ÿ��
    public GameObject wall;                 // �⺻ ��
    public GameObject pointLight;           // ����

    public int sizeX = 0;                          // Ÿ�� ���� ����
    public int sizeY = 0;                          // Ÿ�� ���� ����

    public int tileCount = 0;               // Ÿ���� ��

    bool isExist = false;                   // Ÿ�� ���� ����

    Vector3 mainTileSize = Vector3.zero;    // �߾� Ÿ�� ������
    Vector3 sideTileSize = Vector3.zero;    // ���̵� Ÿ�� ������
    Vector3 vertexTileSize = Vector3.zero;  // ������ Ÿ�� ������

    Vector3 startPos = new Vector3();       // ���Ŀ� ĳ���� ���� ��ġ. ������ �ӽ������� (0, 0, 0) ���� ����

    GameObject[] mapTiles;                // Ÿ�� ������Ʈ ��ü�� ���� �迭


    public GameObject player;
    public Material material;


    GameObject[] lights;


    private void Start()
    {
        // �߾� Ÿ�� ������ ��ȯ
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size;
        // ���̵� Ÿ�� ������ ��ȯ
        sideTileSize = sideTile.GetComponentInChildren<BoxCollider>().size;      
        // ������ Ÿ�� ������ ��ȯ
        vertexTileSize = vertexTile.GetComponentInChildren<BoxCollider>().size;
    }

    /// <summary>
    /// Ÿ�� ���� ����
    /// </summary>
    protected override void Test1(InputAction.CallbackContext context)
    {
        if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
        {
            sizeX = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
            sizeY = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
            tileCount = sizeX * sizeY;          // �� Ÿ�� ����
            mapTiles = new GameObject[tileCount];   // �迭 ���� ����

            lights = new GameObject[4];

            GameObject wallObject;          // �� ������Ʈ

            for (int i = 0; i < tileCount; i++)
            {
                int width = i % sizeX;              // ���� �ε��� ��ȣ
                int length = i / sizeX;             // ���� �ε��� ��ȣ

                // Ÿ�� ����
                if ((width == 0 && length == 0) || (width == 0 && length == sizeY - 1) || (width == sizeX - 1 && length == 0) || (width == sizeX - 1 && length == sizeY - 1))
                {
                    // �������� ���
                    TileInstantiate(i, vertexTile, MapTileType.vertexTile, width, length);      // ������ Ÿ�� ����
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // ���� ��1 ����
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // ���� ��1 �̵�
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // ���� ��2 ����
                    wallObject.transform.Rotate(new Vector3(0, -90.0f, 0));                     // ���� ��2 ȸ��
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // ���� ��2 �̵�
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // ������ �� ����
                    wallObject.transform.Rotate(new Vector3(0, -45.0f, 0));                     // ������ �� ȸ��
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -2.0f));             // ������ �� �̵�


                    if (width == 0 && length == 0)                                      // ���� ��
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                    }
                    else if (width == 0 && length == sizeY - 1)                         // ���� �Ʒ�
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                    }
                    else if (width == sizeX - 1 && length == 0)                         // ������ ��
                    {
                        mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                    }
                    //else if (width == sizeX - 1 && length == sizeY - 1)               // ������ �Ʒ�
                    //{
                    //    mapTiles[i].transform.Rotate(new Vector3(0, 360.0f, 0));
                    //}
                }
                else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)              
                {
                    // �����ڸ��� ���
                    TileInstantiate(i, sideTile, MapTileType.sideTile, width, length);              // ���̵� Ÿ�� ����
                    wallObject = Instantiate(wall, mapTiles[i].transform);                          // ���� �� ����
                    wallObject.transform.Translate(new Vector3(1, 0.0f, -1.75f));                   // ���� �� �̵�

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
                    // �����ڸ��� �ƴ� ���
                    TileInstantiate(i, centerTile, MapTileType.centerTile, width, length);              //�߾� Ÿ�� ����
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));        // �߾� Ÿ�� ���� ȸ��(�׳� �̰���)
                }

                // Ÿ�� ��ġ �̵�. startPos�� �ӽ÷� �־���� ��(0, 0, 0)
                mapTiles[i].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * width,
                                                            0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * length);
            }

            //player.transform.position = GetTile(sizeX / 2, sizeY).transform.position;       // �÷��̾� ��ġ �̵�
            player.transform.position = GetTile(sizeX / 2, sizeY - (sizeY / 3)).transform.position;       // �÷��̾� ��ġ �̵�(�ӽ�)

            // ����Ʈ ���� �� �̵�
            for (int i = 0; i < 4; i++)
            {
                lights[i] = Instantiate(pointLight);
            }
            lights[0].transform.position = GetTile(sizeX / 3, sizeY / 3).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
            lights[1].transform.position = GetTile(sizeX - sizeX / 3 + 1, sizeY / 3).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
            lights[2].transform.position = GetTile(sizeX / 3, sizeY - sizeY / 3 + 1).transform.position + new Vector3(0.0f, 20.0f, 0.0f);
            lights[3].transform.position = GetTile(sizeX - sizeX / 3 + 1, sizeY - sizeY / 3 + 1).transform.position + new Vector3(0.0f, 20.0f, 0.0f);


            //MiniMapInstantiate();       // �̴ϸ� ����

            isExist = true;         // �ߺ� �� ���� ����
        }

    }

    
    protected override void Test2(InputAction.CallbackContext context)
    {
        MapDestroy();
    }

    /// <summary>
    /// Ÿ�� ����
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
    /// ������ ��ǥ�� Ÿ�Ϸ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="width">���� �ε���</param>
    /// <param name="length">���� �ε���</param>
    /// <returns></returns>
    GameObject GetTile(int width, int length)
    {
        int index = sizeX * length + width;
        return mapTiles[index];
    }
    
    /// <summary>
    /// �̴ϸ� ����
    /// </summary>
    void MiniMapInstantiate()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // �̴ϸ� ��ġ�� ���� 0�� �ε����� �ʱ�ȭ
        Vector3 miniMapPos = mapTiles[0].transform.position;

        if (sizeX % 2 == 0)         // �������� ¦���� ���
        {
            miniMapPos.x = (mapTiles[(sizeX / 2) - 1].transform.position.x + mapTiles[sizeX / 2].transform.position.x) * 0.5f;
        }
        else                        // �������� Ȧ���� ���
        {
            miniMapPos.x = mapTiles[sizeX / 2].transform.position.x;
        }

        if (sizeY % 2 == 0)         // �������� ¦���� ���
        {
            miniMapPos.z = (mapTiles[((sizeY / 2) - 1) * sizeX].transform.position.z + mapTiles[(sizeY / 2) * sizeX].transform.position.z) * 0.5f;
        }
        else                        // �������� Ȧ���� ���
        {
            miniMapPos.z = mapTiles[(sizeY / 2) * sizeX].transform.position.z;
        }

        miniMapPos.y += (mainTileSize.y * 0.5f) + 40.0f;
        cube.transform.position = miniMapPos;
        cube.transform.localScale = new Vector3(mainTileSize.x * sizeX, mainTileSize.y, mainTileSize.z * sizeY);
        cube.GetComponent<MeshRenderer>().material = material;
    }

    /// <summary>
    /// Ÿ�Կ� ���� Ÿ�� ����
    /// </summary>
    /// <param name="i">��Ÿ�� �ε���</param>
    /// <param name="type">������ Ÿ���� Ÿ��</param>
    /// <param name="tileType">Ÿ�� ��ũ��Ʈ�� ������ Ÿ��</param>
    /// <param name="width">Ÿ���� ���� �ε���</param>
    /// <param name="length">Ÿ���� ���� �ε���</param>
    void TileInstantiate(int i, GameObject type, MapTileType tileType, int width, int length)
    {
        mapTiles[i] = Instantiate(type, gameObject.transform);                  // type�� ���� Ÿ�� ����
        mapTiles[i].GetComponent<Tile>().TileType = (int)tileType;              // Ÿ�� ��ũ��Ʈ�� Ÿ�� ����
        mapTiles[i].GetComponent<Tile>().Width = width;                         // Ÿ�� ���� �ε��� ����
        mapTiles[i].GetComponent<Tile>().Length = length;                       // Ÿ�� ���� �ε��� ����
    }

}
