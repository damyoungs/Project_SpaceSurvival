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

    bool isExist = false;                   // Ÿ�� ���� ����

    Vector3 mainTileSize = Vector3.zero;    // �߾� Ÿ�� ������
    Vector3 sideTileSize = Vector3.zero;    // �ܰ� Ÿ�� ������
    Vector3 vertexTileSize = Vector3.zero;  // ������ Ÿ�� ������
    Vector3 wallSize = Vector3.zero;        // �� ������
    Vector3 startPos = new Vector3();       // ���Ŀ� ĳ���� ���� ��ġ. ������ �ӽ������� (0, 0, 0) ���� ����

    GameObject[] mapTiles;                // Ÿ�� ������Ʈ ��ü�� ���� �迭(Ȥ�ó� �;� ���߹迭 �ܿ� �ϳ��� �ϴ� �迭�� ���ܵ�)
    //GameObject[,] mapTiles;                 // Ÿ�� ������Ʈ ��ü�� ���� ���� �迭

    private void Start()
    {
        // �߾� Ÿ�� ������ ��ȯ
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size * centerTile.transform.GetChild(0).localScale.x;
        // �ܰ� Ÿ�� ������ ��ȯ
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
        
        
            for (int i = 0; i < tileCount; i++)
            {
                int width = i % sizeX;
                int length = i / sizeX;

                // ���̵� Ÿ�� ����
                if ((width == 0 && length == 0) || (width == 0 && length == sizeY - 1) || (width == sizeX - 1 && length == 0) || (width == sizeX - 1 && length == sizeY - 1))
                {
                    mapTiles[i] = Instantiate(vertexTile, gameObject.transform);
                }
                else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)              // ���̵� Ÿ�� ȸ��
                {
                    mapTiles[i] = Instantiate(sideTile, gameObject.transform);
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
                    mapTiles[i] = Instantiate(centerTile, gameObject.transform);                         // �߾� Ÿ�� ����
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));         // �߾� Ÿ�� ���� ȸ��(�׳� �̰���)
                }

                // Ÿ�� ��ġ �̵�. startPos�� �ӽ÷� �־���� ��(0, 0, 0)
                mapTiles[i].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * width,
                                                            0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * length);
            }
        
            isExist = true;
        }

        //if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
        //{
        //    sizeX = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
        //    sizeY = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
        //    tileCount = sizeX * sizeY;          // �� Ÿ�� ����
        //    mapTiles = new GameObject[sizeX, sizeY];   // �迭 ���� ����


        //    for (int j = 0; j < sizeY; j++)                             // Ÿ�� ���� i = ���� �������� ����, j = ���� �������� ����
        //    {
        //        for (int i = 0; i < sizeX; i++)
        //        {
        //            // ���̵� Ÿ�� ����
        //            if ((i == 0 && j == 0) || (i == 0 && j == sizeY - 1) || (i == sizeX - 1 && j == 0) || (i == sizeX - 1 && j == sizeY - 1))
        //            {
        //                mapTiles[i, j] = Instantiate(vertexTile);
        //            }
        //            else if (i == 0 || i == sizeX - 1 || j == 0 || j == sizeY - 1)              // ���̵� Ÿ�� ȸ��
        //            {
        //                mapTiles[i, j] = Instantiate(sideTile);
        //                if (i == 0)                                                             // ���� ������
        //                {
        //                    mapTiles[i, j].transform.Rotate(new Vector3(0, 90.0f, 0));
        //                }
        //                else if (i == sizeX - 1)                                                // ������ ������
        //                {
        //                    mapTiles[i, j].transform.Rotate(new Vector3(0, 270.0f, 0));
        //                }
        //                else if (j == 0)                                                        // �� ����
        //                {
        //                    mapTiles[i, j].transform.Rotate(new Vector3(0, 180.0f, 0));
        //                }
        //                //else if (j == sizeY - 1)                                              // �� �Ʒ���
        //                //{
        //                //    mapTiles[i, j].transform.Rotate(new Vector3(0, 360.0f, 0));
        //                //}
        //            }
        //            else
        //            {
        //                mapTiles[i, j] = Instantiate(centerTile);                                               // �߾� Ÿ�� ����
        //                mapTiles[i, j].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));         // �߾� Ÿ�� ���� ȸ��(�׳� �̰���)
        //            }
        //            // Ÿ�� ��ġ �̵�. startPos�� �ӽ÷� �־���� ��(0, 0, 0)
        //            mapTiles[i, j].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * i,
        //                                                            0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * j);
        //        }
        //    }

        //    isExist = true;             // Ÿ�� ���� ����(Ÿ���� �����Ǿ� �ִ�)
        //}
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

        //for (int i = 0; i < sizeX; i++)
        //{
        //    for(int j = 0;j < sizeY; j++)
        //    {
        //        Destroy(mapTiles[i, j]);
        //    }
        //}
        isExist = false;
    }
}
