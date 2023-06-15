using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTest : TestBase
{
    /// <summary>
    /// �ַ� ����ϴ� Ÿ��
    /// </summary>
    public GameObject mainTile;
    int sizeX = 0;                          // Ÿ�� ���� ����
    int sizeY = 0;                          // Ÿ�� ���� ����

    public int tileCount = 0;               // Ÿ���� ��

    bool isExist = false;                   // Ÿ�� ���� ����

    Vector3 mainTileSize = Vector3.zero;    // Ÿ�� ������
    Vector3 startPos = new Vector3();       // ���Ŀ� ĳ���� ���� ��ġ. ������ �ӽ������� (0, 0, 0) ���� ����

    GameObject[] mapTiles;                  // Ÿ�� ������Ʈ ��ü�� ���� �迭

    private void Start()
    {
        mainTileSize = mainTile.gameObject.GetComponent<BoxCollider>().size * mainTile.transform.localScale.x;  // Ÿ�� ������ ��ȯ
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
                int sizex = i % sizeX;
                int sizey = i / sizeX;
                mapTiles[i] = Instantiate(mainTile);

                // Ÿ�� ��ġ �̵�. startPos�� �ӽ÷� �־���� ��(0, 0, 0)
                mapTiles[i].transform.position = new Vector3(startPos.x - mainTileSize.x * sizeX / 2 + mainTileSize.x * sizex,
                                                            0, startPos.z + mainTileSize.z * sizeY - mainTileSize.z * sizey);
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
