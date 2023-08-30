//using System;
using Cinemachine;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapTest_2 : TestBase
{
    public GameObject player;               // �÷��̾�
    public Material material;               // �ʿ���� �� ���� �Ƹ� ���� ����

    public GameObject centerTile;           // �߾ӿ� ����� Ÿ��
    public GameObject sideTile;             // �ܰ��� ��ġ�� Ÿ��
    public GameObject vertexTile;           // ������ Ÿ��
    public GameObject wall;                 // �⺻ ��
    public GameObject pointLight;           // ����
    public GameObject pillar;               // ���

    public int sizeX = 0;                   // Ÿ�� ���� ����
    public int sizeY = 0;                   // Ÿ�� ���� ����
    public int tileCount = 0;               // Ÿ���� ��

    public List<GameObject> singleProps;    // 1ĭ�� �����ϴ� ��ü
    public List<GameObject> multiProps;     // 2ĭ �̻��� Ÿ���� �����ϴ� ��ü

    bool isExist = false;                   // Ÿ�� ���� ����
    bool isPropExist = false;

    Vector3 mainTileSize = Vector3.zero;    // �߾� Ÿ�� ������
    Vector3 sideTileSize = Vector3.zero;    // ���̵� Ÿ�� ������
    Vector3 vertexTileSize = Vector3.zero;  // ������ Ÿ�� ������

    GameObject[] mapTiles;                  // Ÿ�� ������Ʈ ��ü�� ���� �迭
    List<GameObject> props;                 // ���� ������ ���� �迭

    GameObject[] lights;                    // ����
    GameObject[] pillars;                   // ���
    Tile[] standardPos;                // ���� ��ġ(����� ����� ���� ��ġ)

    private void Start()
    {
        // �߾� Ÿ�� ������ ��ȯ     - ���� �� Ÿ�ϰ� ������ ����(Ȥ�� ���� �ؿ��� �������� �ʿ������ ���� ����)
        mainTileSize = centerTile.GetComponentInChildren<BoxCollider>().size;
        // ���̵� Ÿ�� ������ ��ȯ
        sideTileSize = sideTile.GetComponentInChildren<BoxCollider>().size;      
        // ������ Ÿ�� ������ ��ȯ
        vertexTileSize = vertexTile.GetComponentInChildren<BoxCollider>().size;


        ///nohyunoh �߰� 
        turnManager = WindowList.Instance.TurnManager;
        miniMap = FindObjectOfType<MiniMapCamera>(true);
        cameraOrigin = FindObjectOfType<CameraOriginTarget>(true);
        moveCam = FindObjectOfType<Camera_Move>(true);
        brain = FindObjectOfType<CinemachineBrain>(true);
    }

    /// <summary>
    /// Ÿ�� ���� ����
    /// </summary>
    //protected override void Test1(InputAction.CallbackContext _)
    //{

    //    if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
    //    {
    //        sizeX = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
    //        sizeY = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
    //        tileCount = sizeX * sizeY;          // �� Ÿ�� ����

    //        MapInstantiate();                       // ���� �� ����

    //        player.transform.position = GetTile(sizeX / 2, sizeY / 3).transform.position;         // �÷��̾� ��ġ �̵�(�ӽ�)
    //        GetTile(sizeX / 2, sizeY / 3).ExistType = Tile.TileExistType.monster;

    //        LightInstantiate();                     // ���� �� ��� ����
    //        //MiniMapInstantiate();                 // �̴ϸ� ���� ����(�ʿ���� �� ���� ���߿� ���� ����)

    //        isExist = true;         // �ߺ� �� ���� ����

    //    }
    //}

    ///// <summary>
    ///// Ÿ�� ����
    ///// </summary>
    //protected override void Test2(InputAction.CallbackContext _)
    //{
    //    if (isExist && !isPropExist)
    //    {
    //        MapDestroy();
    //    }
    //}

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
    /// ���� �� �����ϴ� �Լ�
    /// </summary>
    private void MapInstantiate()
    {

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
                TileInstantiate(i, vertexTile, Tile.MapTileType.vertexTile, width, length);      // ������ Ÿ�� ����
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
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                }
                else if (width == 0 && length == sizeY - 1)                         // ���� �Ʒ�
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                }
                else if (width == sizeX - 1 && length == 0)                         // ������ ��
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 0.0f, 0));
                }
                else if (width == sizeX - 1 && length == sizeY - 1)                 // ������ �Ʒ�
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                }
            }
            else if (width == 0 || width == sizeX - 1 || length == 0 || length == sizeY - 1)
            {
                // �����ڸ��� ���
                TileInstantiate(i, sideTile, Tile.MapTileType.sideTile, width, length);             // ���̵� Ÿ�� ����
                wallObject = Instantiate(wall, mapTiles[i].transform);                              // ���� �� ����
                wallObject.transform.Translate(new Vector3(1, 0.0f, -1.75f));                       // ���� �� �̵�

                if (width == 0)                                                                     // ���� ������
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 90.0f, 0));
                }
                else if (width == sizeX - 1)                                                        // ������ ������
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 270.0f, 0));
                }
                else if (length == 0)                                                               // �� ����
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 0.0f, 0));
                }
                else if (length == sizeY - 1)                                                       // �� �Ʒ���
                {
                    mapTiles[i].transform.Rotate(new Vector3(0, 180.0f, 0));
                }
            }
            else
            {
                // �����ڸ��� �ƴ� ���
                TileInstantiate(i, centerTile, Tile.MapTileType.centerTile, width, length);              //�߾� Ÿ�� ����
                mapTiles[i].transform.Rotate(new Vector3(0, 90.0f * Random.Range(0, 4), 0));        // �߾� Ÿ�� ���� ȸ��(�׳� �̰���)
            }

            mapTiles[i].transform.position = new Vector3(mainTileSize.x * width, 0, mainTileSize.z * length);
        }
    }

    /// <summary>
    /// �� �����ϴ� �Լ�
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
    /// ������ ��ǥ�� Ÿ�Ϸ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="width">���� �ε���</param>
    /// <param name="length">���� �ε���</param>
    /// <returns></returns>
    private Tile GetTile(int width, int length)
    {
        int index = sizeX * length + width;
        return mapTiles[index].GetComponent<Tile>();
    }

    /// <summary>
    /// Ÿ�Կ� ���� Ÿ�� ����
    /// </summary>
    /// <param name="i">��Ÿ�� �ε���</param>
    /// <param name="type">������ Ÿ���� Ÿ��</param>
    /// <param name="tileType">Ÿ�� ��ũ��Ʈ�� ������ Ÿ��</param>
    /// <param name="width">Ÿ���� ���� �ε���</param>
    /// <param name="length">Ÿ���� ���� �ε���</param>
    private void TileInstantiate(int i, GameObject type, Tile.MapTileType tileType, int width, int length)
    {
        mapTiles[i] = Instantiate(type, gameObject.transform);                  // type�� ���� Ÿ�� ����
        mapTiles[i].GetComponent<Tile>().TileType = tileType;              // Ÿ�� ��ũ��Ʈ�� Ÿ�� ����
        mapTiles[i].GetComponent<Tile>().Width = width;                         // Ÿ�� ���� �ε��� ����
        mapTiles[i].GetComponent<Tile>().Length = length;                       // Ÿ�� ���� �ε��� ����
        mapTiles[i].GetComponent<Tile>().Index = i;
    }

    /// <summary>
    /// ����� ��� ���� �� �̵�
    /// </summary>
    private void LightInstantiate()
    {
        standardPos = new Tile[4];         // ���� ��ġ ����
        pillars = new GameObject[4];            // ��� ���� ����
        lights = new GameObject[4];             // ���� ���� ����

        standardPos[0] = GetTile(sizeX / 3 - 1, sizeY / 3 - 1).GetComponent<Tile>();
        standardPos[1] = GetTile(sizeX - sizeX / 3 + 1, sizeY / 3 - 1).GetComponent<Tile>();
        standardPos[2] = GetTile(sizeX / 3 - 1, sizeY - sizeY / 3 + 1).GetComponent<Tile>();
        standardPos[3] = GetTile(sizeX - sizeX / 3 + 1, sizeY - sizeY / 3 + 1).GetComponent<Tile>();

        for (int i = 0; i < 4; i++)
        {
            standardPos[i].GetComponent<Tile>().ExistType = Tile.TileExistType.prop;                                 // ����� �ִ� Ÿ���� Ÿ�� ����

            pillars[i] = Instantiate(pillar, gameObject.transform);                                               // ��� ����
            pillars[i].transform.position = standardPos[i].transform.position;                                    // ��� �̵�

            lights[i] = Instantiate(pointLight, gameObject.transform);                                            // ���� ����
            lights[i].transform.position = standardPos[i].transform.position + new Vector3(0.0f, 20.0f, 0.0f);    // ���� �̵�
        }
    }

    /// <summary>
    /// �̴ϸ� ���� ����(���� ����)
    /// </summary>
    private void MiniMapInstantiate()
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
    /// ������ ���� �Լ�
    /// </summary>
    private void PropInstantiate()
    {
        if (props == null)
        {
            props = new List<GameObject>(16);               // �������� ��������� ����. 16�� ���Ƿ� ���� ����.
        }
        else
        {
            return;                         // �������� ������ ������ �� �̻� �������� ����
        }

        int chooseProp;     // ������ ���� �� ���� ����

        // ����� �������� ������ ������ ���� ������ �� ���� �ӽ� �迭(���� ���ο� ����)
        int[] tempArrayX = new int[4] { 0, standardPos[0].Width, standardPos[1].Width, sizeX };
        int[] tempArrayY = new int[4] { 0, standardPos[0].Length, standardPos[2].Length, sizeY };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // ���� Ȥ�� ���ΰ� 2 �̻��� ���̸� ���� ������ ����
                chooseProp = Random.Range(0, multiProps.Count);
                PropMultiMaking(chooseProp, tempArrayX[i], tempArrayX[i + 1], tempArrayY[j], tempArrayY[j + 1]);    
                if (i == 1 && j == 1)
                {
                    // ���� �� �����ϰ� ���ֱ� ���� �߾ӿ� �ϳ� �� ����
                    PropMultiMaking(chooseProp, tempArrayX[i], tempArrayX[i + 1], tempArrayY[j], tempArrayY[j + 1]);    
                }
            }
        }

        for (int i = 0; i < Random.Range(1, singleProps.Count + 1); i++)
        {
            chooseProp = Random.Range(0, singleProps.Count);
            PropSingleMaking(chooseProp);   // ���ο� ���ΰ� ���� 1�� ������ ����
        }

    }

    /// <summary>
    /// ���ο� ���ΰ� ���� 1�� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="chooseProp">������ ���� �ε���</param>
    private void PropSingleMaking(int chooseProp)
    {
        GameObject obj = Instantiate(singleProps[chooseProp]);      // ������ ����

        while (true)
        {
            Tile tile = GetTile(Random.Range(0, sizeX), Random.Range(0, sizeY));    // ������ Ÿ�� ����
            if (tile.ExistType != Tile.TileExistType.None)          // ������ Ÿ���� ����ִ� �� �ƴ� ���
            {
                continue;                                           // �ٽ� �̱�(�� ������ ���� �ݺ�). ���� ������ �Ʒ��� ������
            }
            obj.transform.position = tile.transform.position;       // �������� Ÿ���� ��ġ�� �̵�
            obj.transform.GetChild(0).rotation = Quaternion.Euler(0.0f, 90.0f * Random.Range(0, 4), 0.0f);  // ������ ȸ������ �ֱ�
            tile.ExistType = Tile.TileExistType.prop;               // �������� �ִ� Ÿ�� �������� �ִٰ� ǥ��
            break;                  // ���� ���� Ż��
        }
        
        props.Add(obj);             // ������ �迭�� �߰�

    }

    /// <summary>
    /// ���� Ȥ�� ������ �ε����� 2 �̻��� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="chooseProp">������ ���� �ε���</param>
    /// <param name="index1">���� �ε����� �ּ� ����</param>
    /// <param name="index2">���� �ε����� �ִ� ����</param>
    /// <param name="index3">���� �ε����� �ּ� ����</param>
    /// <param name="index4">���� �ε����� �ִ� ����</param>
    private void PropMultiMaking(int chooseProp, int index1, int index2, int index3, int index4)
    {
        GameObject obj = Instantiate(multiProps[chooseProp]);           // ������ ����
        PropData objData = obj.GetComponent<PropData>();                // �������� ������ ��ȯ
        Tile[] tempTile = new Tile[objData.width * objData.length];     // Ÿ�� üũ �� ��Ƴ��� �ӽ� �迭(�������� ���ο� ���� ������ ���� ����)
        bool isSuccess = false;                                         // ������ �̵��� �������� ����

        // ���� ������ ��ġ�� ������ �̵� �� �迭 �߰�
        while (!isSuccess)
        {
            Tile tile = GetTile(Random.Range(index1, index2), Random.Range(index3, index4));        // �������� ���� ���� ��ġ�� Ÿ���� ��������
            int randomRotation = Random.Range(0, 4);                                                // ���� �� ��ġ�� ���� ȸ�� (0 ~ 3). y�� ����.
            for (int count = 0; count < 4; count++)                         // (0, 90, 180, 270)���� ȸ���� �ؾ��ϱ⿡ 4�� ������.
            {
                randomRotation++;                                           // ���߿� ȸ�� ���� ���߱� ���� ������ ��� ����(�߿����� ����)
                randomRotation %= 4;                                        // ���������̼� ���� ��� 0~3 �� �ǵ��� ����
                int tileCount = 0;                                          // üũ�� Ÿ���� ������ ���� ���� ��
                for (int i = 0; i < objData.width; i++)         // �������� ���� ���� ��ŭ �ݺ� ������
                {
                    for (int j = 0; j < objData.length; j++)    // �������� ���� ���� ��ŭ �ݺ� ������
                    {
                        switch (randomRotation)         // ȸ�� ������ ���� üũ�ؾ��� Ÿ���� �ε����� �޶����� ������ ���� ���� ����ϵ��� ����
                        {
                            case 0:         // ȸ���� 0���� ��
                                if (GetTile(tile.Width + i, tile.Length + j).ExistType == Tile.TileExistType.prop ||    // Ÿ�Ͽ� �������� �����ְų�
                                    GetTile(tile.Width + i, tile.Length + j).TileType == Tile.MapTileType.sideTile ||   // Ÿ���� ���̵� Ÿ���̰ų�
                                    GetTile(tile.Width + i, tile.Length + j).TileType == Tile.MapTileType.vertexTile)   // ������ Ÿ���� ���
                                {
                                    i = objData.width;          // ������ for�� Ż���� ���� �ִ밪 ����
                                    j = objData.length;         // ������ for�� Ż���� ���� �ִ밪 ����
                                    break;                      // switch�� Ż��
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width + i, tile.Length + j);    // ���� ��쿡 �ش����� ������ �ӽ� �迭�� Ÿ�� ����
                                tileCount++;                                                           // �� ���� Ÿ���� üũ�ߴ��� Ȯ���ϱ� ���� Ÿ�� ī��Ʈ ����
                                break;                                                                 // switch�� Ż��
                            case 1:         // ȸ���� 90�� �� ��
                                if (GetTile(tile.Width + j, tile.Length - i).ExistType == Tile.TileExistType.prop ||        // ���� ����
                                    GetTile(tile.Width + j, tile.Length - i).TileType == Tile.MapTileType.sideTile ||
                                    GetTile(tile.Width + j, tile.Length - i).TileType == Tile.MapTileType.vertexTile)
                                {
                                    i = objData.width;
                                    j = objData.length;
                                    break;
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width + j, tile.Length - i);
                                tileCount++;
                                break;
                            case 2:         // ȸ���� 180�� �� ��
                                if (GetTile(tile.Width - i, tile.Length - j).ExistType == Tile.TileExistType.prop ||        // ���� ����
                                    GetTile(tile.Width - i, tile.Length - j).TileType == Tile.MapTileType.sideTile ||
                                    GetTile(tile.Width - i, tile.Length - j).TileType == Tile.MapTileType.vertexTile)
                                {
                                    i = objData.width;
                                    j = objData.length;
                                    break;
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width - i, tile.Length - j);
                                tileCount++;
                                break;
                            case 3:         // ȸ���� 270�� �� ��
                                if (GetTile(tile.Width - j, tile.Length + i).ExistType == Tile.TileExistType.prop ||        // ���� ����
                                    GetTile(tile.Width - j, tile.Length + i).TileType == Tile.MapTileType.sideTile ||
                                    GetTile(tile.Width - j, tile.Length + i).TileType == Tile.MapTileType.vertexTile)
                                {
                                    i = objData.width;
                                    j = objData.length;
                                    break;
                                }
                                tempTile[i * objData.length + j] = GetTile(tile.Width - j, tile.Length + i);
                                tileCount++;
                                break;
                            default:
                                break;
                        }       // switch�� ��
                    }       // ���� for�� (j) ��
                }       // ���� for�� (i) ��

                if (tileCount == objData.width * objData.length)    // ���� üũ�� Ÿ���� ������ üũ�ؾ� �� Ÿ���� ������ ���ٸ�
                {
                    isSuccess = true;           // �̵��� ������ ������ ����
                    count = 4;                  // ȸ���� ���� ������ for���� Ż���ϱ� ���� �ִ�ġ�� 4�� ����
                }
            }           // ȸ���� ���� ����ϴ� for�� (count) ��

            if (isSuccess)      // ���� �̵��� �����ϴٸ�
            {
                obj.transform.position = tile.transform.position;                                   // �� Ÿ���� ��ġ�� �������� �̵����� �ְ�
                obj.transform.rotation = Quaternion.Euler(0.0f, 90.0f * randomRotation, 0.0f);      // �������� ȸ����Ų��.
                props.Add(obj);                                                                     // ������ �迭�� �߰�
                break;                  // while �ݺ��� Ż��
            }
        }       // while �ݺ��� ��

        for (int i = 0; i < tempTile.Length; i++)           // �ʿ��� Ÿ���� ��Ƴ��� �迭�� ��ȯ��Ű��
        {
            tempTile[i].ExistType = Tile.TileExistType.prop;        // �� Ÿ���� �������� ������ ǥ��
        }
    }

    /// <summary>
    /// �������� �����ϴ� �Լ�
    /// </summary>
    private void PropDestroy()
    {
        foreach (var obj in props)
        {
            Destroy(obj);           // ������ �迭 ��ȸ�ϸ� ����
        }
        props.Clear();
        props = null;               // ���� null�� �ʱ�ȭ

        if (isExist)                // ���� ���� ���� ����
        {
            for (int i = 0; i < mapTiles.Length; i++)
            {
                mapTiles[i].GetComponent<Tile>().ExistType = Tile.TileExistType.None;   // Ÿ���� Ÿ���� None���� �ʱ�ȭ
            }
        }

        for (int i = 0; i < standardPos.Length; i++)
        {
            standardPos[i].ExistType = Tile.TileExistType.prop;     // ����� �ִ� Ÿ���� �ٽ� Prop���� ����
        }
    }

















    //�ؿ��� nohyunoh �ڵ� �����߰�


    protected override void Test5(InputAction.CallbackContext context)
    {
        InitTotalData(); //�ʱⵥ���ͻ���    
        if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
        {
            sizeX = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
            sizeY = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
            tileCount = sizeX * sizeY;          // �� Ÿ�� ����

            MapInstantiate();                       // ���� �� ����

            player.transform.position = GetTile(sizeX / 2, sizeY / 3).transform.position;         // �÷��̾� ��ġ �̵�(�ӽ�)
            GetTile(sizeX / 2, sizeY / 3).ExistType = Tile.TileExistType.monster;

            LightInstantiate();                     // ���� �� ��� ����
            //MiniMapInstantiate();                 // �̴ϸ� ���� ����(�ʿ���� �� ���� ���߿� ���� ����)

            isExist = true;         // �ߺ� �� ���� ����

        }
        SetBlock();

    }
    protected override void Test6(InputAction.CallbackContext context)
    {
        if (isExist)
        {
            MapDestroy(); //�ʻ���

            turnManager.ResetBattleData(); //������

            TurnBaseObject[] ps = GameObject.FindObjectsOfType<TurnBaseObject>(); //���游�� ������Ʈ ã��
            foreach (TurnBaseObject p in ps)
            {
                GameObject.Destroy(p.gameObject); //������Ʈ ����
            }

            InitTotalData();
            if (!isExist)                   // Ÿ���� �������� ���� ��쿡�� ����
            {
                sizeX = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
                sizeY = Random.Range(20, 31);       // Ÿ�� ���� ���� ���� ����
                tileCount = sizeX * sizeY;          // �� Ÿ�� ����

                MapInstantiate();                       // ���� �� ����
                LightInstantiate();                     // ���� �� ��� ����
                                                        //MiniMapInstantiate();                 // �̴ϸ� ���� ����(�ʿ���� �� ���� ���߿� ���� ����)

                //player.transform.position = GetTile(sizeX / 2, sizeY).transform.position;                     // �÷��̾� ��ġ �̵�
                player.transform.position = GetTile(sizeX / 2, sizeY / 3).transform.position;         // �÷��̾� ��ġ �̵�(�ӽ�)

                isExist = true;         // �ߺ� �� ���� ����

            }
            SetBlock();
        }
    }



    /// <summary>
    /// �� ���� 
    /// </summary>
    /// <param name="context"></param>
    protected override void Test7(InputAction.CallbackContext context)
    {
        ITurnBaseData node = turnManager.GetNode(); //������ ĳ���� �ʿ��� �ϿϷ� ��ư ȣ���ؾ��ϴµ� ĳ���� ����������  �׽�Ʈ�ڵ�� ã�ƿ´�.

        if (node == null)
        {
            Debug.Log("�ָ�ã��?");
            return;
        }
        Debug.Log($"{node.UnitBattleIndex}��° ���� : �� :{node.TurnActionValue} �Լ�����ϵ��ֳ�? : {node.TurnEndAction}");
        node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //�� ���� �� �ൿ�� ����ġ ���� �����ִ´�.

        node.TurnEndAction(node); //�ϿϷ� �� �˸���.
    }


    /// <summary>
    /// ������ ĳ������ ���¸� �߰�
    /// </summary>
    /// <param name="context"></param>
    protected override void Test8(InputAction.CallbackContext context)
    {
        TurnBaseObject tbo = (TurnBaseObject)turnManager.RandomGetNode();
        foreach (ICharcterBase charcter in tbo.CharcterList) 
        {
            charcter.BattleUI.AddOfStatus(EnumList.StateType.Poison);//�����̻� �߰��غ��� 
        }         
    }





    /// <summary>
    /// ī�޶� ������ʰ� �������ִ� �ڽ� �ö��̴�  
    /// �ǹ� ��ġ�� Z���� �߾ӿ��� ���� �Ʒ��� �ٲ� ��������
    /// </summary>
    public BoxCollider blockCamera;
    private void SetBlock()
    {
        if (blockCamera != null)
        {
            float blockHeight = 100.0f;//���� ���� ī�޶� ��������������� ����
            float cameraPositionCalibration = 1.0f;//������ ī�޶� �þ߿� ���� ����
            float tempX = sizeX * sideTileSize.x; //��ü ���λ����� ���ؿ���
            float tempY = sizeY * sideTileSize.x; //��ü ���λ����� ���ؿ���
            float halfX = tempX * 0.5f; //��ü X ������ �߰�
            float halfY = tempY * 0.5f; //��ü Z ������ �߰�
            float halfTileSize = sideTileSize.x * 0.5f; //Ÿ���� �߰���

            blockCamera.size = new Vector3(tempX - cameraPositionCalibration, //ī�޶� �þ߶����� ���̾Ⱥ��̰� �������� ����
                                            blockHeight,                    //�� �������
                                            tempY - cameraPositionCalibration //ī�޶� �þ߶����� ���̾Ⱥ��̰� �������� ����
                                            );
            blockCamera.center = new Vector3(halfX - halfTileSize, //x ���Ͱ��� �߾� ��ġ�Ҽ��ְ� ���� Ÿ�� ũ���� �����߰��λ��ش�
                                             1.0f, // ���� ���漳���س��� �̰��� ����
                                             halfY - halfTileSize //Z ���� �߾ӿ� ��ġ�Ҽ��ְ� ���� 
                                             );
        }

    }

    /// <summary>
    /// ī�޶� ������ʰ� �������ִ� �ڽ� �ö��̴� 
    /// �Ǻ� ��ġ Z �߾� ���ð� 
    /// </summary>
    private void SetBlock(bool pivotCenter = true)
    {
        if (blockCamera != null)
        {
            float blockHeight = 100.0f; //���� ���� ����\
            float widthPadding = 1.0f;  //��ũ�� �� �°� ī�޶� ���������� ���ѹ��� ũ��������
            float heightPadding = 0.5f; //��ũ�� �� �°� ī�޶� ���������� ���ѹ��� ũ��������

            float tempX = sizeX * sideTileSize.x; //��ü ���λ����� ���ؿ���
            float tempY = sizeY * sideTileSize.x; //��ü ���λ����� ���ؿ���
            blockCamera.size = new Vector3(tempX - widthPadding, blockHeight, tempY - heightPadding); //���� Width 2 : Height 1
            blockCamera.center = new Vector3(-1.0f, 1.0f, (tempY * 0.5f) + 1.0f); //���ѹ��� �߰���ġ�� ���ϱ�
        }

    }



    [Header("�� ���� �׽�Ʈ ����")]
    TurnManager turnManager;
    [Header("�ʱ�ȭ�� ������ ")]
    //TurnBaseObject turnUnit;
    MiniMapCamera miniMap;
    CameraOriginTarget cameraOrigin;
    Camera_Move moveCam;
    CinemachineBrain brain;
    private void InitTotalData()
    {
        turnManager.TestInit(); //�׽�Ʈ ������ �����ϰ� 
        turnManager.InitTurnData();//�ʱⵥ���� ���� 
        player = turnManager.GetPlayerCharcter().gameObject;
        ITurnBaseData temp = player.GetComponent<ITurnBaseData>();
        List<ICharcterBase> listC = temp.CharcterList;
        Transform tf = listC[0].transform;
        listC[0].transform.gameObject.SetActive(true);
        miniMap.player = tf.GetChild(1);
        cameraOrigin.Target = tf.GetChild(0);
        moveCam.Brain = brain;
        miniMap.gameObject.SetActive(true);
        cameraOrigin.gameObject.SetActive(true);
        moveCam.gameObject.SetActive(true);
    }



}
