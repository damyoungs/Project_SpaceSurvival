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
    public GameObject cornerWall;           // �ڳ� ��

    int sizeX = 0;                          // Ÿ�� ���� ����
    int sizeY = 0;                          // Ÿ�� ���� ����

    public int tileCount = 0;               // Ÿ���� ��

    bool isExist = false;                   // Ÿ�� ���� ����

    Vector3 mainTileSize = Vector3.zero;    // �߾� Ÿ�� ������
    Vector3 sideTileSize = Vector3.zero;    // ���̵� Ÿ�� ������
    Vector3 vertexTileSize = Vector3.zero;  // ������ Ÿ�� ������

    Vector3 startPos = new Vector3();       // ���Ŀ� ĳ���� ���� ��ġ. ������ �ӽ������� (0, 0, 0) ���� ����

    GameObject[] mapTiles;                // Ÿ�� ������Ʈ ��ü�� ���� �迭

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

            GameObject wallObject;          // �� ������Ʈ

            for (int i = 0; i < tileCount; i++)
            {
                int width = i % sizeX;              // ���� �ε��� ��ȣ
                int length = i / sizeX;             // ���� �ε��� ��ȣ

                // Ÿ�� ����
                if ((width == 0 && length == 0) || (width == 0 && length == sizeY - 1) || (width == sizeX - 1 && length == 0) || (width == sizeX - 1 && length == sizeY - 1))
                {
                    // �������� ���
                    mapTiles[i] = Instantiate(vertexTile, gameObject.transform);                // ������ Ÿ�� ����
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.vertexTile;           // Ÿ�� ��ũ��Ʈ�� Ÿ�� ����
                    GameObject cornerObject = Instantiate(cornerWall, mapTiles[i].transform);   // ������ �� ����
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // ���� ��1 ����
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // ���� ��1 �̵�
                    wallObject = Instantiate(wall, mapTiles[i].transform);                      // ���� ��2 ����
                    wallObject.transform.Rotate(new Vector3(0, -90.0f, 0));                     // ���� ��2 ȸ��
                    wallObject.transform.Translate(new Vector3(1.0f, 0.0f, -1.75f));            // ���� ��2 �̵�

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
                else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)              // ���̵� Ÿ�� ȸ��
                {
                    mapTiles[i] = Instantiate(sideTile, gameObject.transform);              // ���̵� Ÿ�� ����
                    mapTiles[i].GetComponent<Tile>().Type = (int)TileType.sideTile;         // Ÿ�� ��ũ��Ʈ�� Ÿ�� ����
                    wallObject = Instantiate(wall, mapTiles[i].transform);
                    wallObject.transform.Translate(new Vector3(1, 0.0f, -1.75f));

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
