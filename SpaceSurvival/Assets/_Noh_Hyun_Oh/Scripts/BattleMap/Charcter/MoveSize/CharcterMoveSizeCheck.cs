using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharcterMoveSizeCheck : TestBase
{
    /// <summary>
    /// ȭ���� ��� ī�޶�
    /// </summary>
    Camera mainCamera;
    /// <summary>
    /// ���� ��ġ�� �̵��� ĳ���� �ӵ�
    /// </summary>
    float speed = 4.0f;
    /// <summary>
    /// ȭ�鿡�� ��� ���� �ε����� �ڽ��ݶ��̴�
    /// </summary>
    BoxCollider target = null;
    /// <summary>
    /// ��ǲ�ý��� Ŭ��
    /// </summary>
    InputKeyMouse inputClick;

    MapTest_2 map;
    List<Tile> path = null;

    Tile currentPos = null;
    public Tile CurrentPos
    {
        get => currentPos;
        set
        {
            if (currentPos != value)
            {
                if (currentPos != null)
                {
                    currentPos.ExistType = Tile.TileExistType.None;
                }
                else
                {
                    // �±پ��� �浹���ϱ� �±پ� ��ũ��Ʈ �����ؼ� ����������ð� 
                    // �����ؼ� �̸��ٲ㼭 ����ϵ����ϼ��� �Ƚ������̾ƴ϶� �����Ҷ����� �����������־�� 
                    transform.position = value.transform.position;
                }
                currentPos = value;
                currentPos.ExistType = Tile.TileExistType.Monster;
            }
        }
    }

    
    Tile[] mapTiles;
    MapTest_2 mapData;
    //�̵������� ������
    [SerializeField]
    float moveSize = 3.0f;
    protected override void Awake()
    {
        base.Awake();
        inputClick = new InputKeyMouse();
    }

    private void OnEnable()
    {
        inputClick.Mouse.Enable();
        inputClick.Mouse.MouseClick.performed += onClick;

        mapData = FindObjectOfType<MapTest_2>();
        map = FindObjectOfType<MapTest_2>();
        path = new List<Tile>();
    }

    private void OnDisable()
    {
        inputClick.Mouse.MouseClick.performed -= onClick;
        inputClick.Mouse.Disable();

        if (path != null)
            path.Clear();
        path = null;
    }

    /// <summary>
    /// ���콺�� Ŭ������ �� �Ͼ �Լ�
    /// </summary>
    /// <param name="context"></param>
    private void onClick(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());      // ȭ�鿡�� ���� ���콺�� ��ġ�� ��� ��
        Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);              // ����׿� ������

        if (Physics.Raycast(ray, out RaycastHit hitInfo))                       // ���� ���� �ε�����
        {
            if (hitInfo.transform.gameObject.CompareTag("Tile"))                // �±� "Ÿ��"�� �浹�ϸ�
            {
                target = (BoxCollider)hitInfo.collider;                         // Ÿ���� �ڽ��ݶ��̴� ��ȯ
                Tile tile = target.gameObject.GetComponent<Tile>();
                if (tile.ExistType == Tile.TileExistType.None)
                {
                    path = AStar_2.PathFind(map, CurrentPos, tile);
                }
                //CurrentPos = tile;
                Debug.Log($"Ÿ�� ��ġ : {tile.Width}, {tile.Length}");
            }
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;           //  ��� �ִ� ī�޶� ��������
    }

    private void FixedUpdate()
    {
        // Ÿ���� ������Ʈ�� �ƴϰ� Ÿ���� �������� �ʾ��� �� �̵�
        //if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.01f)
        //{
        //    transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
        //}

        if (path.Count > 0)
        {
            Tile destPath = path[0];

            Vector3 dir = destPath.transform.position - transform.position;

            if (dir.sqrMagnitude < 0.01f)
            {
                transform.position = destPath.transform.position;
                CurrentPos = destPath;
                path.RemoveAt(0);
            }
            else
            {
                transform.Translate(Time.fixedDeltaTime * speed * dir.normalized);
            }
        }

    }































    public void MoveSize() 
    {
        mapTiles = mapData.MapTiles;
        ClearLineRenderer();
        List<Tile> list = SetMoveSize(currentPos, moveSize); //�̵� ���� ����Ʈ ��������
        OpenLineRenderer(list);
    }
 
    protected override void Test9(InputAction.CallbackContext context)
    {
        mapTiles = mapData.MapTiles;
        ClearLineRenderer();
        List<Tile> list = SetMoveSize(currentPos, moveSize); //�̵� ���� ����Ʈ ��������
        OpenLineRenderer(list);

    }
    private void ClearLineRenderer() //���� ���η����� ����
    {
        foreach (Tile tile in mapTiles)
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>();
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
            }
            continue;
        }
    }
    private void OpenLineRenderer(List<Tile> moveTiles) //�̵������ѹ��� �� ���η����� Ű��
    {
        foreach (Tile tile in moveTiles)
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
        }
    }
    /// <summary>
    /// ���� ��ġ�������� �ൿ�� ���� �̵������� ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// </summary>
    /// <param name="currentNode">���� �����ġ </param>
    /// <param name="moveCheck">�ൿ�� ��</param>
    /// <returns>ĳ���Ͱ� �̵������� ��帮��Ʈ</returns>
    public List<Tile> SetMoveSize(Tile currentNode, float moveCheck)
    {
        List<Tile> resultNode = new List<Tile>();
        List<Tile> openList = new List<Tile>();   // Ž���� �ʿ��� ��� ����Ʈ 
        List<Tile> closeList = new List<Tile>();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 

        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; // H ���� 1000�� ������Ű�� G ���� �ʱ�ȭ�Ͽ� ��� �� G�����θ� �Ҽ��ְ� �Ѵ�.
            node.G = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.G = 0.0f; //����ġ�� g �� 0�̴�

        while (openList.Count > 0)
        {
            currentNode = openList[0];  
            openList.Remove(currentNode); // Ž�������� ��Ͽ��� ���� Ž������ ����� �����ϰ� 
            closeList.Add(currentNode);   // Ž�������� ����Ʈ�� ���� ����� ��´�.

            if (currentNode.G > moveCheck) //G ���� ���� �̵� ������ �Ÿ����� ������  ���̻� Ž���� �ʿ�������� 
            {
                continue; //������ Ž�� 
            }
            else // �̵������� �Ÿ��� 
            {
                resultNode.Add(currentNode); //��ȯ ��ų ����Ʈ�� �߰��Ѵ�.
            }

            OpenListAdd(currentNode,openList,closeList); //�ֺ� 8������ ��带 ã�Ƽ� G�� �����ϰ�  ���¸���Ʈ�� ������������ ��´�.
            openList.Sort();            //ã�� G���� ���� ���������� ��Ž���̵ȴ�.
        }
        return resultNode;
    }

    /// <summary>
    /// �±پ� 8���� Ž�� ���� �����ͼ� ����
    /// </summary>
    /// <param name="currentNode">����ġ</param>
    /// <param name="open">���¸���Ʈ</param>
    /// <param name="close">Ŭ�����Ʈ</param>
    private void OpenListAdd(Tile currentNode , List<Tile> open , List<Tile> close) 
    {
        Tile adjoinTile;
        float sideDistance = 1.0f;
        float diagonalDistance = 1.414f;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (currentNode.Width + x < 0 || currentNode.Width + x > mapData.sizeX - 1 || // ���̵� �˻� 
                    currentNode.Length + y < 0 || currentNode.Length + y > mapData.sizeY - 1) //���̵� �˻�
                    continue;

                adjoinTile = mapData.GetTile(currentNode.Width + x, currentNode.Length + y);    // ������ Ÿ�� ��������

                if (adjoinTile == currentNode)                                          // ������ Ÿ���� (0, 0)�� ���
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None)                // ������ Ÿ���� None�� �ƴ� ��
                    continue;
                if (close.Exists((inClose) => inClose == adjoinTile))             // close����Ʈ�� ���� ��
                    continue;

                bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
                    (mapData.IsWall(currentNode.Width + x, currentNode.Length) ||
                    mapData.IsWall(currentNode.Width, currentNode.Length + y)))
                    continue;

                float distance;
                if (isDiagonal)
                {
                    distance = diagonalDistance;
                }
                else
                {
                    distance = sideDistance;
                }

                if (adjoinTile.G > currentNode.G + distance)
                {
                    open.Add(adjoinTile);
                    adjoinTile.G = currentNode.G + distance;
                }
            }
        }

    }
    
}
