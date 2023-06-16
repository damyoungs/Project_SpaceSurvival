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

    int sizeX = 0;                          // Ÿ�� ���� ����
    int sizeY = 0;                          // Ÿ�� ���� ����

    public int tileCount = 0;               // Ÿ���� ��

    bool isExist = false;                   // Ÿ�� ���� ����

    Vector3 mainTileSize = Vector3.zero;    // �߾� Ÿ�� ������
    Vector3 sideTileSize = Vector3.zero;    // �ܰ� Ÿ�� ������
    Vector3 startPos = new Vector3();       // ���Ŀ� ĳ���� ���� ��ġ. ������ �ӽ������� (0, 0, 0) ���� ����

    //GameObject[] mapTiles;                // Ÿ�� ������Ʈ ��ü�� ���� �迭(Ȥ�ó� �;� ���߹迭 �ܿ� �ϳ��� �ϴ� �迭�� ���ܵ�)
    GameObject[,] mapTiles;                 // Ÿ�� ������Ʈ ��ü�� ���� ���� �迭

    private void Start()
    {
        //mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size * centerTile.GetComponentInChildren<Transform>().localScale.x;  // �߾� Ÿ�� ������ ��ȯ
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size * centerTile.transform.GetChild(0).localScale.x;
        //sideTileSize = sideTile.gameObject.GetComponent<BoxCollider>().size * sideTile.transform.localScale.x;      // �ܰ� Ÿ�� ������ ��ȯ
    }

    /// <summary>
    /// Ÿ�� ���� ����
    /// </summary>
    protected override void Test1(InputAction.CallbackContext context)
    {
        //if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
        //{
        //    sizeX = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
        //    sizeY = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
        //    tileCount = sizeX * sizeY;          // �� Ÿ�� ����
        //    mapTiles = new GameObject[tileCount];   // �迭 ���� ����
        //
        //
        //    for (int i = 0; i < tileCount; i++)
        //    {
        //        int sizex = i % sizeX;
        //        int sizey = i / sizeX;
        //        mapTiles[i] = Instantiate(centerTile);
        //
        //        // Ÿ�� ��ġ �̵�. startPos�� �ӽ÷� �־���� ��(0, 0, 0)
        //        mapTiles[i].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * sizex,
        //                                                    0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * sizey);
        //    }
        //
        //    isExist = true;
        //}

        if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
        {
            sizeX = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
            sizeY = Random.Range(10, 21);       // Ÿ�� ���� ���� ���� ����
            tileCount = sizeX * sizeY;          // �� Ÿ�� ����
            mapTiles = new GameObject[sizeX, sizeY];   // �迭 ���� ����


            for (int j = 0; j < sizeY; j++)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    if (i == 0 || i == sizeX - 1 || j == 0 || j == sizeY - 1)
                    {
                        mapTiles[i, j] = Instantiate(sideTile);
                    }
                    else
                    {
                        mapTiles[i, j] = Instantiate(centerTile);
                        mapTiles[i, j].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));
                    }
                    // Ÿ�� ��ġ �̵�. startPos�� �ӽ÷� �־���� ��(0, 0, 0)
                    mapTiles[i, j].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * i,
                                                                    0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * j);
                }
            }

            isExist = true;
        }
    }

    /// <summary>
    /// Ÿ�� ����
    /// </summary>
    protected override void Test2(InputAction.CallbackContext context)
    {
        //for (int i = 0; i < tileCount; i++)
        //{
        //    Destroy(mapTiles[i]);
        //}

        for (int i = 0; i < sizeX; i++)
        {
            for(int j = 0;j < sizeY; j++)
            {
                Destroy(mapTiles[i, j]);
            }
        }
        isExist = false;
    }
}
