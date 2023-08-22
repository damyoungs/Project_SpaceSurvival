using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


/// <summary>
/// A* �� �ϳ��� ���� �Ŵ��� Ŭ���� 
/// ��� �߰�:  ������������ �ش���ǥ���� �޸���ƽ ���� �����ϴ� ����� �߰��ؾ��Ѵ�.
/// </summary>
public class AstarProccess 
{
    public enum NodeState 
    {
        None =0,            //�ʱⰪ
        Nomal,              //�⺻
        Inaccessible,       //���ٺҰ���
    }
    /// <summary>
    /// A* ����� ��� ������ ���� �迭 x y  �����ϱ� ���� 2���迭�� ����
    /// </summary>
    private Astar_Node[,] nodes;
    public Astar_Node[,] Nodes => nodes;

    /// <summary>
    /// ���� �� ��� ����
    /// </summary>
    private Astar_Node startNode;
    
    /// <summary>
    /// ���� �� ��� ����
    /// </summary>
    private Astar_Node endNode;

    /// <summary>
    /// ���� �˻����� ������� 
    /// </summary>
    private Astar_Node currentNode;


    /// <summary>
    /// ��� ���� ����Ʈ 
    /// </summary>
    private List<Astar_Node> openList;

    /// <summary>
    /// ��� Ŭ���� ����Ʈ
    /// </summary>
    private List<Astar_Node> closeList;

    /// <summary>
    /// ���簢���� ���� �� 
    /// </summary>
    const float nomalLine = 1.0f;

    /// <summary>
    /// ���簢���� �밢�� �� 1.414 �� 
    /// ���簢���̶� ������ �������ʴ´�.
    /// </summary>
    //readonly float diagonalLine = Mathf.Sqrt((nomalLine*nomalLine) + (nomalLine * nomalLine)); //�밢�� �����ϱ� 1.414 �Է��ص� �Ǳ��ϴ�.
    readonly float diagonalLine = 1.414f;


    public AstarProccess()
    {
        // ����Ʈ �����صα�
        openList = new List<Astar_Node>(); 
        closeList = new List<Astar_Node>(); 

    }

    /// <summary>
    /// A* ������ ������ �Է��� �ʱ�ȭ ��Ų��.
    /// </summary>
    /// <param name="horizontalSize">���� ����</param>
    /// <param name="verticalSize"> ���� ����</param>
    /// <param name="obstaclesArray">��ֹ� �ε��� �迭</param>
    public void InitData(int horizontalSize, int verticalSize, int[] obstaclesArray = null)
    {
        openList.Clear();

        closeList.Clear();

        CreateNodeArray(horizontalSize, verticalSize); // �� �迭����� 

        if (this.nodes != null && obstaclesArray != null) //������ ������������
        {
            PlaceObstacles(obstaclesArray, horizontalSize, verticalSize); //���������� ����Ѵ�.
        }
    }

    /// <summary>
    /// ���� �Ұ����� ���� ���� 
    /// obstaclesArray ���� ��ǥ�� �������� �������� ������ �Ȼ��¿��� �����۵��̵ȴ�.
    /// </summary>
    /// <param name="obstaclesArray">���ٺҰ����� ���� �ε��� �����ص� �迭</param>
    /// <param name="horizontalSize">���� ũ��</param>
    /// <param name="verticalSize">���� ũ��</param>
    private void PlaceObstacles(int[] obstaclesArray, int horizontalSize, int verticalSize)
    {
        int i = 0; // �ε��� �� üũ�� ���� 
        int obstacleIndex = 0; //��ֹ� �ε����� üũ�� ����
        for (int y = 0; y < verticalSize; y++) // ��ü�� ���ΰ�����ŭ ����
        {
            for (int x = 0; x < horizontalSize; x++) // ��ü�� ���ΰ�����ŭ ���Ƽ� �ٵ�����.
            {
                if (obstaclesArray.Length > obstacleIndex && obstaclesArray[obstacleIndex] == i) //�ε������� ����������� üũ�ؼ� 
                {
                    nodes[y, x].State = NodeState.Inaccessible; //���� �ε������� �־��ش�.
                    obstacleIndex++;//���� ��ֹ� �ε����� ã������ �ε��� ����
                }
                i++;//��ü �ε��� �� ���� 
            }
        }
    }


    /// <summary>
    /// A* ������ ������ �Է��� �ʱ�ȭ ��Ų��.
    /// </summary>
    /// <param name="horizontalSize">���� ����</param>
    /// <param name="verticalSize"> ���� ����</param>
    /// <param name="obstaclesArray">��ֹ� �ε��� �迭</param>
    public void InitData(int horizontalSize, int verticalSize, Vector2Int [] obstaclesArray = null)
    {
        openList.Clear();

        closeList.Clear();

        CreateNodeArray(horizontalSize, verticalSize); // �� �迭����� 

        if (this.nodes != null && obstaclesArray != null) //������ ������������
        {
            PlaceObstacles(obstaclesArray, horizontalSize, verticalSize); //���������� ����Ѵ�.
        }
    }

    /// <summary>
    /// ���� �Ұ����� ���� ���� 
    /// obstaclesArray ���� ��ǥ�� �������� �������� ������ �Ȼ��¿��� �����۵��̵ȴ�.
    /// </summary>
    /// <param name="obstaclesArray">���ٺҰ����� ���� ��ǥ���� �����ص� �迭</param>
    /// <param name="horizontalSize">���� ũ��</param>
    /// <param name="verticalSize">���� ũ��</param>
    private void PlaceObstacles(Vector2Int[] obstaclesArray, int horizontalSize, int verticalSize)
    {
        int i = 0; // �ε��� �� üũ�� ���� 
        int obstacleIndex = 0; //��ֹ� �ε����� üũ�� ����
        for (int y = 0; y < verticalSize; y++) // ��ü�� ���ΰ�����ŭ ����
        {
            for (int x = 0; x < horizontalSize; x++) // ��ü�� ���ΰ�����ŭ ���Ƽ� �ٵ�����.
            {
                if (obstaclesArray.Length > obstacleIndex &&
                    obstaclesArray[obstacleIndex].x == x &&
                    obstaclesArray[obstacleIndex].y == y
                    ) //�ε������� ����������� üũ�ؼ� 
                {
                    nodes[y, x].State = NodeState.Inaccessible; //���� �ε������� �־��ش�.
                    obstacleIndex++;//���� ��ֹ� �ε����� ã������ �ε��� ����
                }
                i++;//��ü �ε��� �� ���� 
            }
        }
    }

    /// <summary>
    /// ��ġ�� �ΰ��� ������ ���� ����� ��θ� Ž���Ѵ�.
    /// ������ġ�� ���� ��ġ�� ��ȿ���� üũ���ؼ� ��ȿ���ϸ� ��ó�� �ٲ����.
    /// </summary>
    /// <param name="startIndex">������ġ �ε���</param>
    /// <param name="endIndex">������ġ �ε���</param>
    public Astar_Node GetShortPath(int startIndex, int endIndex)
    {
        ResetValue();                     // ��ο� G , H ���� ���� ��Ų��.

        int startX = startIndex == 0 ? 0 : startIndex % nodes.GetLength(0);  //������ġ �� x��ǥ�� 
        int startY = startIndex == 0 ? 0 : startIndex / nodes.GetLength(1);  //������ġ �� y��ǥ��

        int endY = endIndex == 0 ? 0 : endIndex / nodes.GetLength(1);    //������ġ �� y��ǥ��
        int endX = endIndex == 0 ? 0 : endIndex % nodes.GetLength(0);    //������ġ �� x��ǥ��

        Debug.Log($"{nodes.GetLength(0)} , {nodes.GetLength(1)} , {startIndex} ,{endIndex}");
        Debug.Log($"start : {startX},{startY} end : {endX},{endY}");
        startNode = nodes[startY, startX];  // ������ġ��
        endNode = nodes[endY, endX];        // ������ġ�� �����ϰ� 
        SetHeuristicsValue(endNode); //���� ���� ��带 �̿��� ������ �޸���ƽ ������ �����Ѵ�.

        return PathFinding(); //��ã�⸦ �����Ѵ�.
    }


    /// <summary>
    /// ��ġ�� �ΰ��� ������ ���� ����� ��θ� Ž���Ѵ�.
    /// ������ġ�� ���� ��ġ�� ��ȿ���� üũ���ؼ� ��ȿ���ϸ� ��ó�� �ٲ����.
    /// </summary>
    /// <param name="startPos">������ġ �ε���</param>
    /// <param name="endPos">������ġ �ε���</param>
    public Astar_Node ShortestdistanceLine(Vector2Int startPos, Vector2Int endPos)
    {
        ResetValue();                     // ��ο� G , H ���� ���� ��Ų��.

        startNode = nodes[startPos.y, startPos.x];  // ������ġ��
        endNode = nodes[endPos.y, endPos.x];        // ������ġ�� �����ϰ� 
        SetHeuristicsValue(endNode); //���� ���� ��带 �̿��� ������ �޸���ƽ ������ �����Ѵ�.

        return PathFinding(); //��ã�⸦ �����Ѵ�.
    }




    /// <summary>
    /// ���� ��ġ�������� �ൿ�� ���� �̵������� ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// </summary>
    /// <param name="currentNode">���� �����ġ </param>
    /// <param name="moveCheck">�ൿ�� ��</param>
    /// <returns>ĳ���Ͱ� �̵������� ��帮��Ʈ</returns>
    public  List<Astar_Node> SetMoveSize(Astar_Node currentNode, float moveCheck)
    {
        List<Astar_Node> resultNode = new List<Astar_Node>(); 
        openList.Clear();   // Ž���� �ʿ��� ��� ����Ʈ 
        closeList.Clear();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 

        foreach (Astar_Node node in nodes) 
        {
            node.ResetMoveCheckValue(); // H ���� 1�� ������Ű�� G ���� �ʱ�ȭ�Ͽ� ��� �� G�����θ� �Ҽ��ְ� �Ѵ�.
        }
        currentNode.G = 0.0f; //������ ���ʱ�ȭ �ϰ��������� ó������ 0���� �ٽü���

        openList.Add(currentNode);


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

            OpenListAdd(currentNode); //�ֺ� 8������ ��带 ã�Ƽ� G�� �����ϰ�  ���¸���Ʈ�� ������������ ��´�.
            openList.Sort();            //ã�� G���� ���� ���������� ��Ž���̵ȴ�.
        }
        return resultNode;
    }






    /// <summary>
    /// 1. Ư������� �ֺ���带 �˻��ؼ� ���� ����Ʈ�� ���
    /// 2. �ֺ������ G���� �����Ѵ�.
    /// </summary>
    /// <param name="currentNode">�����̵Ǵ� ���</param>
    private void OpenListAdd(Astar_Node currentNode)
    {
        int horizontalSize = nodes.GetLength(0);    //���� ���� �������� (2�����迭�� ? �迭�� ���� [y,?]) ������ �����̶� �ް�����.
        int verticalSize = nodes.GetLength(1);      //���� ���� �������� (2�����迭�� ? �迭�� ���� [?,x])

        //����������� üũ�ϱ�
        int horizontalStartIndex = currentNode.X - 1; //���� ��ġ�� ���� �� ��������
        horizontalStartIndex = horizontalStartIndex < 0 ? 0 : horizontalStartIndex; //���ʳ� ���̸� 0���� ����

        int horizontalEndIndex = currentNode.X + 1; //���� ��ġ�� ������ �� ��������
        horizontalEndIndex = horizontalEndIndex == horizontalSize ? // �ǿ������̸�  
            horizontalSize    :                                     
            horizontalEndIndex + 1;                                 // ���� ���������ϱ����� +1�� ���� <= ���ϰ� <�� üũ�ϱ����ؼ� ���߰� 

        int verticalStartIndex = currentNode.Y - 1; //���� ��ġ�� �Ʒ��� �� ��������
        verticalStartIndex = verticalStartIndex < 0 ? 0 : verticalStartIndex; // �ǾƷ� ���̸� 0���� ���� 

        int verticalEndIndex = currentNode.Y + 1;                   //���� ��ġ�� ���� �� ��������
        verticalEndIndex = verticalEndIndex == verticalSize ?       //�� ���� 
                            verticalSize    :                          
                            verticalEndIndex + 1;                       //���� ���������ϱ����� +1�� ���� <= ���ϰ� <�� üũ�ϱ����ؼ� ���߰� 
        //���� üũ ��

        float tempG = 0.0f; //���� G���� ���� �ӽú���

       
        Astar_Node tempNode; // üũ�� ��ü ����(�����ȿ��� �Ź� ���ÿ� �޸𸮾���� �ѹ����ϱ����� �ۿ��� ����)
        for (int y = verticalStartIndex; y < verticalEndIndex; y++) //���� ������ �͸�ŭ ���� ������
        {
            for (int x = horizontalStartIndex; x < horizontalEndIndex; x++) //���� �����Ѹ�ŭ ���� ������
            {
                tempNode = nodes[y, x]; //�ֺ���带 �޾ƿͼ� 
                if (tempNode.State == NodeState.Inaccessible) continue; //ã�ƿ� �ֺ���尡 ������ �����̸� ���� �ݺ������� �̵�
                /// G �� üũ ���� 
                /// 
                //�밢������ �������� üũ
                if ((currentNode.X - x) == 0 || (currentNode.Y - y) == 0) //�����ΰ�� 
                {
                    tempG = currentNode.G + nomalLine;
                    if (tempNode.G  > tempG) //���� ������ ������ �������� ũ��  
                    {
                        tempNode.G = tempG;     //G ���� ����
                        tempNode.PrevNode = currentNode; //G���� �ٲ����� ��𼭿Դ��� ������嵵 ����
                    }
                }
                else //�밢���� ���
                {
                    //(currentNode.X - x) //���� - �� ������ + �� ���� 
                    //(currentNode.Y - y) //���� - �� ����   + �� �Ʒ���

                    //��ֹ� üũ
                    if (nodes[
                            currentNode.Y - (currentNode.Y - y ),   // ������ġ���� �˻��ϴ� ��ġ�� ���� ���� +�� -�� ���������� - �� ó�� 
                            currentNode.X                           // ex) ���� (1,1) �϶�  (2,2)Ȯ�ν� (1,2) , (2,1)��Ȯ���ؾߵ� �����̵Ƿ��� +ó���ξȵȴ�.
                            ].State == NodeState.Inaccessible || //���κκп� ������������ �ְų�
                        nodes[
                            currentNode.Y, 
                            currentNode.X - (currentNode.X -  x)
                            ].State == NodeState.Inaccessible)   //���κκп� ������������ ������
                    {
                        Debug.Log($"������ġ({currentNode.X},{currentNode.Y}) ��ֹ� ��ġ ({x},{y})");
                        continue;//ó�����ϰ� ���� ��带 ã�´�.
                    }

                    tempG = currentNode.G + diagonalLine; //���� ���ŵ� G�� ���� �밢��������
                    if (tempNode.G > tempG) //���� ������ ������ �������� ũ��  
                    {
                        tempNode.G = tempG;   //G ���� ����
                        tempNode.PrevNode = currentNode; //G���� �ٲ����� ��𼭿Դ��� ������嵵 ����
                    }
                }

                //G�� üũ ��

                if (!closeList.Contains(tempNode) && !openList.Contains(tempNode))// Ž���� ������ ���� ��� �̰� ���¸���Ʈ���� ����� 
                {
                   openList.Add(tempNode); //���¸���Ʈ�� ��Ƶд�.
                }
            }
        }
    }

    /// <summary>
    /// ������ ��θ� ã�´�.
    /// </summary>
    /// <returns>��ΰ� ����� ��带 ��ȯ�Ѵ�.</returns>
    private Astar_Node PathFinding()
    {
        openList.Clear();   // Ž���� �ʿ��� ��� ����Ʈ 
        closeList.Clear();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 

        startNode.G = 0.0f; //���� ��尪�� G�� �׻� 0 �̷����Ѵ�.
        openList.Add(startNode); // �� ó������ ���� ���� ����ؼ� A* ���� ����
  
        while (openList.Count > 0) //���¸���Ʈ�� ���� ������ ��� Ž�� 
        {
            currentNode = openList[0]; //���� Ž�� ���� ��带 ��Ƶд�.

            if (currentNode == endNode) // �������� �����ߴ��� üũ�Ѵ�.
            {
                return currentNode;     //���������� ��ΰ� ��� ��带 ��ȯ�Ѵ�.
            }

            openList.Remove(currentNode); // Ž�������� ��Ͽ��� ���� Ž������ ����� �����ϰ� 
            closeList.Add(currentNode);   // Ž�������� ����Ʈ�� ���� ����� ��´�.

            OpenListAdd(currentNode); //������ġ���� �ֺ� ��带 ã�� G ���� ������Ű�� ���¸���Ʈ�� ��´�.

            //�������� ���� 
            openList.Sort();
        }
        return null; //��θ�ã���� ���� �����Ѵ�.
    }

    /// <summary>
    /// A* �� ����� ���߹迭�� �����Ѵ�.
    /// </summary>
    /// <param name="horizontalSize">���� ũ��</param>
    /// <param name="verticalSize">���� ũ��</param>
    private void CreateNodeArray(int horizontalSize, int verticalSize)
    {
        this.nodes = new Astar_Node[verticalSize, horizontalSize]; //��üũ�⸸ŭ �迭��� 
        int i = 0; // �ε��� ����� ���������ϰ� 
        for (int y = 0; y < verticalSize; y++) //���� ��ŭ
        {
            for (int x = 0; x < horizontalSize; x++)//���θ�ŭ
            {
                this.nodes[y, x] = new Astar_Node(i , x, y); //2���� �迭 ����� 
                i++; //�ε��� ����
            }
        }
    }

    /// <summary>
    /// ������ G,H �� �� �ʱ�ȭ �ϴ� �Լ�
    /// ��� ��Ž���� �ʿ���
    /// </summary>
    private void ResetValue()
    {
        foreach (var node in nodes)
        {
            node.G =float.MaxValue;
            node.H =float.MaxValue;
            node.PrevNode = null;
        }
    }
    /// <summary>
    /// ���� ��ġ �������� ��帮��Ʈ�� �޸���ƽ ���� �����ϴ� �Լ� 
    /// </summary>
    /// <param name="endNode">���� ��� ��</param>
    private void SetHeuristicsValue(Astar_Node endNode) 
    {
        float tempX = 0.0f;    //�밢�� ��������� �ӽ÷� ����� ���� 
        float tempY = 0.0f;    //�밢�� ��������� �ӽ÷� ����� ���� 
        float tempLine = 0.0f; //�밢�� ��������� �ӽ÷� ����� ���� 

        foreach (Astar_Node node in nodes) //��ü ����Ʈ�� ã�Ƽ�
        {
            node.H = float.MaxValue; //�޸���ƽ���� �ٽü����Ҷ� H ���� �ʱ�ȭ��Ų��.

            if (node.X != endNode.X && node.Y != endNode.Y) //�밢���ΰ�� ( x���̳� ,y ���� ������ ���������ΰ��� �̿��� üũ)
            {
                // x �� y �� ������ ���� ���� ������ �밢���� �߰� �ΰ��� ���̸�ŭ ���ϸ� �� (��/ �̷����)

                tempX = Math.Abs(endNode.X - node.X); //���� ������ �Ÿ����� ���ϰ� 

                tempY = Math.Abs(endNode.Y - node.Y); //���� ������ �Ÿ����� ���ϰ�

                tempLine = tempX == tempY ?                                         // �Ÿ����� ������ ���簢�������� 
                            tempX * diagonalLine                                    // �Ѻ����ٰ� �밢������ ���ϸ鳡
                            : tempX > tempY ?                                       // ���� �Ÿ����� ��ũ�� 
                            (tempY * diagonalLine) + ((tempX - tempY) * nomalLine)  // ���� ���̸�ŭ �밢�����̸� ���ϰ� ���� ���� �Ÿ��� �������̷� ���Ѵ�
                            :                                                       // ���� �Ÿ����� ��ũ�� 
                            (tempX * diagonalLine) + ((tempY - tempX) * nomalLine); // ���� ���̸�ŭ �밢�����̸� ���ϰ� ���� ���� �Ÿ��� �������̷� ���Ѵ�.

                node.H = tempLine; //���� ���� ����ִ´�.
            }
            else if (node.X == endNode.X) //���� ���� ���ο� �����ϸ�
            {
                node.H = Math.Abs(endNode.Y - node.Y) * nomalLine; //���� ���̸�ŭ�� ����ϸ������ �������   
            }
            else if (node.Y == endNode.Y) //���� ���� ���ο� �����ϸ� 
            {
                node.H = Math.Abs(endNode.X - node.X) * nomalLine;  //���� ���̸�ŭ�� �������
            }
            else 
            {
                Debug.Log($"��������? ������ �����Ű�����  : node({node.X},{node.Y}) _ curruntNode({endNode.X},{endNode.Y})");
            }
            Debug.Log($"�޸���ƽ({node.X},{node.Y}) : {node.H} ");
        }
    }

    /// <summary>
    /// ��尡 �������ִ� ��θ� ã�ƿ´�.
    /// </summary>
    /// <param name="node">��� ã�� ���</param>
    /// <returns>������ ������ ��� </returns>
    public List<Vector3Int> GetPath(Astar_Node node)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        path.Add(new Vector3Int(node.X, node.Y, node.Z));
        Astar_Node prevNode = node.PrevNode;

        while (prevNode != null) //������ΰ� ���������� ����!
        {
            path.Add(new Vector3Int(prevNode.X, prevNode.Y, prevNode.Z)); //��� ������ ��´�.
            prevNode = prevNode.PrevNode; //������� ���� ã�ƿ���
        }
        path.Reverse(); //�� ���� ���ٷ� �����´�.
        return path;
    }
    /// <summary>
    /// �ε����� �ش��ϴ� ��尪 ��������
    /// </summary>
    /// <param name="startIndex">�ε�����</param>
    /// <returns>�ε����� �ش��ϴ� �������</returns>
    public Astar_Node GetNode(int startIndex)
    {
        if (nodes != null && nodes.Length > 0 && startIndex < nodes.Length)
        {
            if (startIndex < 1)
            {
                return nodes[0, 0];
            }
            int startX = startIndex == 0 ? 0 : startIndex % nodes.GetLength(0);  //������ġ �� x��ǥ�� 
            int startY = startIndex == 0 ? 0 : startIndex / nodes.GetLength(1);  //������ġ �� y��ǥ��

            return nodes[startY, startX];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// �ε����� �ش��ϴ� ��尪 ��������
    /// <param name="x">x�ε��� ��ǥ</param>
    /// <param name="y">y�ε��� ��ǥ</param>
    /// <param name="z">z�ε��� ��ǥ? ������</param>
    /// <returns>�ε����� �ش��ϴ� �������</returns>
    public Astar_Node GetNode(int x , int y , int z = 1)
    {
        if (x  < 0 || y < 0  || x > nodes.GetLength(0) || y >  nodes.GetLength(1))
        {
            return nodes[x, y];
        }
        else
        {
            return null;
        }
    }

    //----------- �׽�Ʈ�� �Լ�




    public void TestGLog()
    {
        string str = "";
        foreach (Astar_Node node in nodes)
        {
            str += $"��ǥ({node.X},{node.Y}) : G �� : {node.G} \r\n";
        }
        Debug.Log(str);
    }

   

}