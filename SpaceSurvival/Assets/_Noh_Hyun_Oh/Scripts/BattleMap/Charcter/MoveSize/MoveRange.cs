using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̵� ������ ���� ǥ������ ������Ʈ
/// </summary>
public class MoveRange : MonoBehaviour
{

    /// <summary>
    /// ������ Ÿ���� ��Ƶ� �迭����
    /// </summary>
    Tile[] mapTiles;


    /// <summary>
    /// ������ Ÿ���� ���� ����
    /// </summary>
    int tileSizeX;

    /// <summary>
    /// ������ Ÿ���� ���� ����
    /// </summary>
    int tileSizeY;

    /// <summary>
    /// �̵� ������ Ÿ�� ��Ƶ� ����Ʈ
    /// </summary>
    List<Tile> activeMoveTiles;

    private void Awake()
    {
        SpaceSurvival_GameManager.Instance.GetMoveRangeComp = () => this; //������ �����ϱ� 
        activeMoveTiles = new List<Tile>(); //�̵������� Ÿ�� ����Ʈ �ʱ�ȭ 
    }

    /// <summary>
    /// �ʿ����� ���� �޾ƿͼ� �ʱ�ȭ �ϱ�
    /// </summary>
    /// <param name="mapTiles">������</param>
    /// <param name="tileSizeX">��Ÿ�� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ�� ���ΰ���</param>
    public void InitDataSetting(Tile[] mapTiles, int tileSizeX, int tileSizeY)
    {
        this.mapTiles = mapTiles;
        this.tileSizeX = tileSizeX;
        this.tileSizeY = tileSizeY;
    }
   

    /// <summary>
    /// ������ �ʱ�ȭ
    /// </summary>
    public void ResetData()
    {
        this.mapTiles = null;
        this.tileSizeX = 0;
        this.tileSizeY = 0;
    }

    /// <summary>
    /// �ٴڿ� �̵������� ������ ǥ���ϴ� ���� 
    /// </summary>
    /// <param name="mapTiles">��Ÿ�� ���� </param>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="moveSize">�̵������� �Ÿ� ��</param>
    /// <param name="tileSizeX">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ���� �ִ� ���ΰ���</param>
    public void MoveSizeView(Tile currentNode, float moveSize)
    {
        if (mapTiles == null) return; //�ʱ�ȭ �ȉ����� ����ȵǰ� ��ȯ

        ClearLineRenderer();
        SetMoveSize(currentNode, moveSize); //�̵� ���� ����Ʈ ��������
        OpenLineRenderer();
    }

    /// <summary>
    /// ���� �̵������� ���� �ʱ�ȭ �ϱ�
    /// </summary>
    private void ClearLineRenderer() //���� ���η����� ����
    {
        foreach (Tile tile in activeMoveTiles) 
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>(); // Ÿ�Ͽ��ٰ� �ش������� �̸���Ƶθ� ������.. �������ʿ��ϴ�.
            lineRenderer.enabled = false;
            tile.ExistType = Tile.TileExistType.None; //�ʱⰪ���� ���� 
        }
    }

    /// <summary>
    /// �����ִ����� ǥ���ϱ� 
    /// </summary>
    /// <param name="moveTiles">��Ÿ�� ���� </param>
    private void OpenLineRenderer()
    {
        foreach (Tile tile in activeMoveTiles)
        {
            LineRenderer lineRenderer = tile.GetComponent<LineRenderer>(); // Ÿ�Ͽ��ٰ� �ش������� �̸���Ƶθ� ������.. �������ʿ��ϴ�.
            lineRenderer.enabled = true;
            tile.ExistType = Tile.TileExistType.Move; //�̵� �����ϵ��� ����  
        }
    }


    /// <summary>
    /// ���� ��ġ�������� �ൿ�� ���� �̵������� ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// </summary>
    /// <param name="mapTiles">��Ÿ�� ���� </param>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="moveCheck">�̵������� �Ÿ� ��</param>
    /// <param name="tileSizeX">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ���� �ִ� ���ΰ���</param>
    private void SetMoveSize(Tile currentNode, float moveCheck)
    {
        
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
                activeMoveTiles.Add(currentNode); //��ȯ ��ų ����Ʈ�� �߰��Ѵ�.
            }

            OpenListAdd(currentNode, openList, closeList); //�ֺ� 8������ ��带 ã�Ƽ� G�� �����ϰ�  ���¸���Ʈ�� ������������ ��´�.
            openList.Sort();            //ã�� G���� ���� ���������� ��Ž���̵ȴ�.
        }
    }

    /// <summary>
    /// �±پ� 8���� Ž�� ���� �����ͼ� ����
    /// Ÿ���� none �ΰ͸� �����´� 
    /// </summary>
    /// <param name="mapTiles">��Ÿ�� ���� </param>
    /// <param name="tileSizeX">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="open">A* ���¸���Ʈ</param>
    /// <param name="close">A* Ŭ���� ����Ʈ</param>

    private void OpenListAdd(Tile currentNode, List<Tile> open, List<Tile> close)
    {
        Tile adjoinTile;
        float sideDistance = 1.0f;
        float diagonalDistance = 1.414f;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // ���̵� �˻� 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //���̵� �˻�
                    continue;

                adjoinTile = mapTiles[(currentNode.Width + x) * tileSizeY + currentNode.Length + y];    // ������ Ÿ�� ��������

                if (adjoinTile == currentNode)                                          // ������ Ÿ���� (0, 0)�� ���
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None)                // ������ Ÿ���� None�� �ƴ� ��
                    continue;
                if (close.Exists((inClose) => inClose == adjoinTile))             // close����Ʈ�� ���� ��
                    continue;

                bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
                    mapTiles[(currentNode.Width + x) * tileSizeY + currentNode.Length].ExistType == Tile.TileExistType.Prop ||
                    mapTiles[(currentNode.Width) * tileSizeY + currentNode.Length + y].ExistType == Tile.TileExistType.Prop
                    )
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










    /// <summary>
    /// ������ Ÿ���� ��Ƶ� �迭����
    /// </summary>
    Tile[,] mapTilesDoubleArray;
    List<Tile> moveTiles;
    /// <summary>
    /// û�� �������� �ٽð˻��Ҽ��ְ� üũ�ϴº���
    /// </summary>
    bool isClear = false;
    public void InitDataSetting(Tile[,] mapTiles, int tileSizeX, int tileSizeY)
    {
        this.mapTilesDoubleArray = mapTiles;
        this.tileSizeX = tileSizeX;
        this.tileSizeY = tileSizeY;
    }
    /// <summary>
    /// ������ �ʱ�ȭ
    /// </summary>
    public void ResetDoubleData()
    {
        this.mapTilesDoubleArray = null;
        this.tileSizeX = 0;
        this.tileSizeY = 0;
    }

    /// <summary>
    /// �ٴڿ� �̵������� ������ ǥ���ϴ� ���� 
    /// </summary>
    /// <param name="mapTiles">��Ÿ�� ���� </param>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="moveSize">�̵������� �Ÿ� ��</param>
    /// <param name="tileSizeX">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ���� �ִ� ���ΰ���</param>
    public void MoveSizeDoubleView(Tile currentNode, float moveSize)
    {
        if (!isClear)
        {
            SetMoveDoubleSize(currentNode, moveSize); //�̵� ���� ����Ʈ �����ϱ�
            OpenDoubleLineRenderer(currentNode);
        }
    }
    /// <summary>
    /// �����ִ����� �ʱ�ȭ �ϱ� ����ġ���� �ʱ�ȭ�Ǳ⶧���� ����ġ�� ���ܵд�.
    /// </summary>
    public void ClearDoubleLineRenderer(Tile currentTile) //���� ���η����� ����
    {
        if (!isClear) 
        {
            isClear = true;
            if (activeMoveTiles.Count > 0) //�ʱ�ȭ �� Ÿ������������  
            {
                Tile.TileExistType currentTileType = currentTile.ExistType; //�������� �Ź� üũ�����ʱ����� ���δ�� 
                foreach (Tile tile in activeMoveTiles)
                {
                    tile.ExistType = Tile.TileExistType.None;
                }
                currentTile.ExistType = currentTileType; //������������ �����ص״� ���� ��´�.
                activeMoveTiles.Clear();//�ʱ�ȭ�������� ���� ����
            }
            isClear = false;
        }
    }

    /// <summary>
    /// �����ִ����� ǥ���ϱ� 
    /// </summary>
    private void OpenDoubleLineRenderer(Tile currentTile) //�̵������ѹ��� �� ���η����� Ű��
    {
        Tile.TileExistType currentTileType = currentTile.ExistType; //�������� �Ź� üũ�����ʱ����� ���δ�� 
        foreach (Tile tile in activeMoveTiles)
        {
            tile.ExistType = Tile.TileExistType.Move;
        }
        currentTile.ExistType = currentTileType; //������������ �����ص״� ���� ��´�.
    }
    /// <summary>
    /// ���� ��ġ�������� �ൿ�� ���� �̵������� ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// </summary>
    /// <param name="mapTiles">��Ÿ�� ���� </param>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="moveCheck">�̵������� �Ÿ� ��</param>
    /// <param name="tileSizeX">��Ÿ���� �ִ� ���ΰ���</param>
    /// <param name="tileSizeY">��Ÿ���� �ִ� ���ΰ���</param>
    /// <returns>ĳ���Ͱ� �̵������� ��帮��Ʈ</returns>
    private void SetMoveDoubleSize(Tile currentNode, float moveCheck)
    {
        List<Tile> openList = new List<Tile>();   // Ž���� �ʿ��� ��� ����Ʈ 
        List<Tile> closeList = new List<Tile>();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 

        foreach (Tile node in mapTilesDoubleArray)
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
                activeMoveTiles.Add(currentNode); //��ȯ ��ų ����Ʈ�� �߰��Ѵ�.
            }

            OpenDoubleListAdd(currentNode, openList, closeList); //�ֺ� 8������ ��带 ã�Ƽ� G�� �����ϰ�  ���¸���Ʈ�� ������������ ��´�.
            openList.Sort();            //ã�� G���� ���� ���������� ��Ž���̵ȴ�.
        }
    }
    private void OpenDoubleListAdd(Tile currentNode, List<Tile> open, List<Tile> close)
    {
        Tile adjoinTile;
        float sideDistance = 1.0f;
        float diagonalDistance = 1.414f;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // ���̵� �˻� 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //���̵� �˻�
                    continue;

                adjoinTile = mapTilesDoubleArray[currentNode.Length + y, currentNode.Width + x];    // ������ Ÿ�� ��������

                if (adjoinTile == currentNode)                                          // ������ Ÿ���� (0, 0)�� ���
                    continue;
                if (adjoinTile.ExistType != Tile.TileExistType.None)                // ������ Ÿ���� None�� �ƴ� ��
                    continue;
                if (close.Exists((inClose) => inClose == adjoinTile))             // close����Ʈ�� ���� ��
                    continue;

                bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
                    mapTilesDoubleArray[currentNode.Length, currentNode.Width + x].ExistType == Tile.TileExistType.Prop ||
                    mapTilesDoubleArray[currentNode.Length + y, currentNode.Width].ExistType == Tile.TileExistType.Prop
                    )
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

    /// <summary>
    /// �������� ���� ������(none�ΰ�) �� ã�Ƽ� ��ȯ�ϴ� �Լ�  ��ã���� null ��ȯ
    /// </summary>
    /// <param name="tileType">Ÿ������ �ö� Ÿ��</param>
    /// <returns>�߰������� Ÿ������ȯ �ϰų� �������� null ��ȯ</returns>
    public Tile GetRandomTile(Tile.TileExistType tileType) 
    {
        int x = mapTilesDoubleArray.GetLength(1)-1;
        int y = mapTilesDoubleArray.GetLength(0)-1;
        Tile result = null;
        int maxCount = 100; //�ִ� 100��������.
        int count = 0;
        //Debug.Log($"{x},{y}");
        while (count < maxCount) //���� ���� ������ 
        {
            
            result = mapTilesDoubleArray[Random.Range(0, y), Random.Range(0, x)];
            if (result.ExistType == Tile.TileExistType.None)//�����ִ°��̸� 
            {
                result.ExistType = tileType; //�����Ǿߵ� Ÿ������ �ٲ۵� 
                return result;//����������.
            }
            else 
            {
                result = null; //������ �����̸� �ʱ�ȭ ��Ų��.   
            }

            count++; //���ѷ��� ������
        }
        Debug.Log($"{count} ��ŭ ���Ҵµ� �ٲ����ִ��� ��ã�Ҿ� ");
        return null;
    }

}
