using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/*
    �ʱⰪ �����ϴ� �Լ� ����Ʈ ũ�� �̸����صα� �����ؼ� 
 */
/// <summary>
/// A* �� �ϳ��� ���� �Ŵ��� Ŭ���� 
/// ��� �߰�:  ������������ �ش���ǥ���� �޸���ƽ ���� �����ϴ� ����� �߰��ؾ��Ѵ�.
/// </summary>
public static class AstarProccess 
{
    /// <summary>
    /// ���� ������ ���°�
    /// </summary>
    public enum NodeState 
    {
        None = 0,           //�ʱⰪ
        Nomal   ,           //���ٰ���   
        Inaccessible,       //���ٺҰ���
    }

    /// <summary>
    /// Ÿ�� ���� �ֺ��� �̵��������������� üũ�ϱ����� �̳Ѱ�
    /// </summary>
    [Flags]
    public enum Four_Way_Access_Area_Check :byte
    {
        NONE = 0,                           // 0000     �ֺ�4���⿡ ���ڽŰ� �ٸ����� ���������ʴ´�
        UP = 1,                             // 0001     
        DOWN = 2,                           // 0010     
        LEFT = 4,                           // 0100     
        RIGHT = 8,                          // 1000     
        ALL = UP | DOWN | LEFT | RIGHT ,    // 1111     �ֺ�4���⿡ ���ڽŰ� �ٸ����� �����Ѵ� 

    }


    /// <summary>
    /// ���� �� ��� ����
    /// </summary>
    private static Astar_Node startNode;
    
    /// <summary>
    /// ���� �� ��� ����
    /// </summary>
    private static Astar_Node endNode;

    /// <summary>
    /// ���� �˻����� ������� 
    /// </summary>
    private static Astar_Node currentNode;

    /// <summary>
    /// A* ��帮��Ʈ
    /// </summary>
    private static Astar_Node[,] nodes;
    /// <summary>
    /// ��� ���� ����Ʈ 
    /// </summary>
    private static List<Astar_Node> openList = new List<Astar_Node>();

    /// <summary>
    /// ��� Ŭ���� ����Ʈ
    /// </summary>
    private static List<Astar_Node> closeList = new List<Astar_Node>();

    /// <summary>
    /// ���簢���� ���� �� 
    /// </summary>
    const float nomalLine = 1.0f;

    /// <summary>
    /// ���簢���� �밢�� �� 1.414 �� 
    /// ���簢���̶� ������ �������ʴ´�.
    /// </summary>
    //readonly float diagonalLine = Mathf.Sqrt((nomalLine*nomalLine) + (nomalLine * nomalLine)); //�밢�� �����ϱ� 1.414 �Է��ص� �Ǳ��ϴ�.
    readonly static float diagonalLine = 1.414f;

    public static Action<Astar_Node> onTileCreate;

    /// <summary>
    /// A* ������ ������ �迭�� ���´�.
    /// </summary>
    /// <param name="horizontalSize">���� ����</param>
    /// <param name="verticalSize"> ���� ����</param>
    /// <param name="obstaclesArray">���������� �ε��� �迭</param>
    /// <returns>�������������� �������� ��������迭 ��ȯ</returns>
    public static Astar_Node[,] InitData(int horizontalSize, int verticalSize, int[] obstaclesArray = null)
    {
        openList.Clear(); //���� ����Ʈ �ʱ�ȭ 

        closeList.Clear(); // Ŭ���� ����Ʈ �ʱ�ȭ 

        nodes = CreateNodeArray(horizontalSize, verticalSize); // �� �迭����� 

        if (nodes != null && obstaclesArray != null) //������ ������������
        {
            SetPlaceObstacles(horizontalSize, verticalSize, obstaclesArray); //���������� ����Ѵ�.
        }
        return nodes;
    }


    /// <summary>
    /// ���� �Ұ����� ���� ���� 
    /// obstaclesArray ���� ��ǥ�� �������� �������� ������ �Ȼ��¿��� �����۵��̵ȴ�.
    /// ����߰� : ��忡�ٰ� ���� ����(���ٰ���,�Ұ���)�� �ٸ� ���� �����ϸ� üũ�ϴ� ���� �߰�
    /// </summary>
    /// <param name="horizontalSize">���� ũ��</param>
    /// <param name="verticalSize">���� ũ��</param>
    /// <param name="obstaclesArray">���ٺҰ����� ���� �ε��� �����ص� �迭</param>
    private static void SetPlaceObstacles(int horizontalSize, int verticalSize, int[] obstaclesArray = null)
    {
        int index = 0; // �ε��� �� üũ�� ���� 
        int obstacleIndex = 0; //��ֹ� �ε����� üũ�� ����
        
        for (int y = 0; y < verticalSize; y++) // ��ü�� ���ΰ�����ŭ ����
        {
            for (int x = 0; x < horizontalSize; x++) // ��ü�� ���ΰ�����ŭ ���Ƽ� �ٵ�����.
            {
                if (obstaclesArray.Length > obstacleIndex && //�ƿ�����ٿ��� ���� �ɷ������
                    obstaclesArray[obstacleIndex] == index) //�ε������� üũ�ؼ� 
                {
                    nodes[y, x].State = NodeState.Inaccessible; //���� �ε������� �־��ش�.
                    obstacleIndex++;//���� ��ֹ� �ε����� ã������ �ε��� ����
                }
                else 
                {
                    nodes[y, x].State = NodeState.Nomal; //���� �ε������� �־��ش�.
                }
                index++;//��ü �ε��� �� ���� 
            }
        }
        // �������� ���� ������ �̸�����
        int startIndex = 0;
        int endIndex = 0;
        foreach (Astar_Node node in nodes) //���� �Ұ����� ��忡 ��ó�� �����ִ� �����ִ��� üũ�ϱ�
        {
            //���� üũ ����

            //���Ʒ� üũ
            startIndex = node.Y - 1 < 0 ? 1 : - 1;  //�� �Ʒ��� ��� �϶� ���� ��常 �˻��ϸ� �ǰ� 
            endIndex = node.Y + 2 > verticalSize    ? 0 : 2; //�� ���� ����϶� �Ʒ��� ��常 �˻��ϸ� �ȴ�.
            for (int y  = startIndex; y < endIndex; y += 2) // -1�� 1�� �ݾ������� üũ�Ѵ�.
            {
                //Debug.Log($" Y = {node.Y} , {y},{startIndex},{endIndex}");
                if (node.State != nodes[node.Y + y, node.X].State)//���Ʒ��� üũ
                {
                    // ����������� üũ�� �̸��Ҽ����������� �����ȿ��� �Ѵ�.
                    if (y > 0) //�����̳�
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.UP;
                        //������� �����ִ��� üũ
                    }
                    else if (y < 0) //�Ʒ����̳�
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.DOWN;
                        //�ƴϸ� ������� �Ʒ��� �ִ��� üũ
                    }
                }
            }

            //�¿� üũ
            startIndex = node.X - 1 < 0 ? 1 : -1;  //�� ���ʳ�� �϶� ������ ��常 �˻��ϸ� �ǰ� 
            endIndex = node.X + 2 > horizontalSize  ? 0 : 2; //�� ������ ��� �϶� ���� ��常 �˻��ϸ� �ȴ�.
            for (int x = startIndex; x < endIndex; x += 2) // -1 �� 1 �� �ݾ� ���� üũ�Ѵ�.
            {
                //Debug.Log($"x = {node.X} , {x},{startIndex},{endIndex}");
                if (node.State != nodes[node.Y, node.X + x].State)//�¿츸 üũ
                {
                    if (0 > x) //�����̳�?
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.LEFT;
                        //������� ���ʿ� �ִ��� üũ
                    }
                    else if (0 < x) //�������̳�?
                    {
                        node.FourWayCheck |= Four_Way_Access_Area_Check.RIGHT;
                        //�ƴϸ� ����� �����ʿ� �ִ��� üũ
                    }
                }
            }
        }
    }


    
    /// <summary>
    /// ��ġ�� �ΰ��� ������ ���� ����� ��θ� Ž���Ѵ�.
    /// ������ġ�� ���� ��ġ�� ��ȿ���� üũ���ؼ� ��ȿ���ϸ� ��ó�� �ٲ����.
    /// </summary>
    /// <param name="startIndex">������ġ �ε���</param>
    /// <param name="endIndex">������ġ �ε���</param>
    public static Astar_Node GetShortPath(int startIndex, int endIndex)
    {
        ResetValue();                     // ��ο� G , H ���� ���� ��Ų��.
        int lastX = nodes.GetLength(0);         //x ��ǥ�� �ִ밪
        int lastY = nodes.GetLength(1);         //y ��ǥ�� �ִ밪


        int startX = startIndex == 0 ? 0 : startIndex % lastX;  //������ġ �� x��ǥ�� 
        int startY = startIndex == 0 ? 0 : startIndex / lastY;  //������ġ �� y��ǥ��

        int endY = endIndex == 0 ? 0 : endIndex / lastY;    //������ġ �� y��ǥ��
        int endX = endIndex == 0 ? 0 : endIndex % lastX;    //������ġ �� x��ǥ��

        //Debug.Log($"{nodes.GetLength(0)} , {nodes.GetLength(1)} , {startIndex} ,{endIndex}");
        //Debug.Log($"start : {startX},{startY} end : {endX},{endY}");
        startNode = nodes[startY, startX];  // ������ġ��
        endNode = nodes[endY, endX];        // ������ġ�� �����ϰ� 
        SetHeuristicsValue(endNode); //���� ���� ��带 �̿��� ������ �޸���ƽ ������ �����Ѵ�.

        return PathFinding(); //��ã�⸦ �����Ѵ�.
    }


 




    /// <summary>
    /// ���� ��ġ�������� �ൿ�� ���� �̵������� ���� �� ��ǥ����Ʈ�� ������������ �Լ�
    /// </summary>
    /// <param name="currentNode">���� �����ġ </param>
    /// <param name="moveCheck">�ൿ�� ��</param>
    /// <returns>ĳ���Ͱ� �̵������� ��帮��Ʈ</returns>
    public static List<Astar_Node> SetMoveSize(Astar_Node currentNode, float moveCheck)
    {
        List<Astar_Node> resultNode = new List<Astar_Node>(); 
        openList.Clear();   // Ž���� �ʿ��� ��� ����Ʈ 
        closeList.Clear();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 

        foreach (Astar_Node node in nodes) 
        {
            node.ResetMoveCheckValue(); // H ���� 1�� ������Ű�� G ���� �ʱ�ȭ�Ͽ� ��� �� G�����θ� �Ҽ��ְ� �Ѵ�.
        }

        openList.Add(currentNode);

        currentNode.G = 0.0f; //������ ���ʱ�ȭ �ϰ��������� ó������ 0���� �ٽü���

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
    /// ���õȰ��� ���������� �����Ҽ��ִ��� ���θ� Ȯ���ϴ� ����
    /// </summary>
    /// <returns>���������� true ������ false</returns>
    private static bool IsComplite() 
    {
        while (openList.Count > 0)  //Ž�� ����
        {
            currentNode = openList[0];
            if (currentNode == endNode) //Ž���� ��尡 ���� ��ġ�� 
            {
                Debug.Log($"({currentNode.X},{currentNode.Y}) == ({endNode.X},{endNode.Y})�� ������"); //���ִٰ� �ϰ� 
                return true; //��������
            }
            else if(!closeList.Contains(currentNode)) //Ŭ�����Ʈ�� ����� 
            {
                closeList.Add(currentNode);//�߰��Ѵ�.
                Debug.Log($"({currentNode.X},{currentNode.Y}) == ({endNode.X},{endNode.Y})�� ã����"); //���ִٰ� �ϰ� 
            }
            openList.Remove(currentNode);

            OpenListAdd(currentNode);
        }
        Debug.Log($"�˻����������� ��� ��ǥ��({currentNode.X},{currentNode.Y}) �̰� ���̾���");
        return false;
    }
    /// <summary>
    /// �������� ���ٺҰ����� ���� ������ �ް� 
    /// ���� ��ġ������ ���� ��ġ������ ��θ� ã�ƺ��� �����ϸ� �ƹ��������ϰ� 
    /// ��������������� ���ٺҰ������� �ٿ����鼭 ��θ� Ž���Ѵ�.
    /// </summary>
    /// <param name="obstacleIndexArray"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public static Astar_Node FindLineCheck(int startIndex, int endIndex)
    {


        openList.Clear(); //Ž���ϱ����� üũ�� ����Ʈ �ΰ� �ʱ�ȭ �ϰ� 

        closeList.Clear();

        currentNode = AstarProccess.GetNode(startIndex); //������ġ�� ��尪 �����ϰ�

        currentNode.G = 0.0f; //���������� 0���� ���ҽ� ���ã�´�.

        endNode =  AstarProccess.GetNode(endIndex); //������ġ�� ��尪 ���� �ϰ�

        openList.Add(currentNode);//���� ��� ��Ƽ� 

        SetHeuristicsValue(endNode); //�޸���ƽ ���� ���ְ� ã�� 

        int debugCount = 0;
        while(!IsComplite()) //���� ã���� ���� ��� A Star �˻� �õ�  
        {
            closeList.Sort(ListCompareTo);  //�޸���ƽ�� �������� ������ ��Ų��.
            //randomCount = UnityEngine.Random.Range(0, closeList.Count); //�������� �̱�
            NextCheckNode(closeList[0]); //����������� ���� �������� ���¸���Ʈ�� ������������ ��´�.
            debugCount++;
            if (debugCount > 1000) 
            {
                Debug.Log($"��ǥ�� ���Ⱑ���̾� ({currentNode.X},{currentNode.Y}) ��ã�Ѿ� �ᱹ ");
                break;
            }
        }

        return endNode;
    }

    /// <summary>
    /// ���Ĺ�� �����ϱ� 
    /// </summary>
    /// <param name="thisNode">�������</param>
    /// <param name="otherNode">�񱳳��</param>
    /// <returns>���ı��ذ�</returns>
    public static int ListCompareTo(Astar_Node thisNode, Astar_Node otherNode)
    {
        // ������ 0���� �۴�(-1)  : ����(����) �۴�(this < other)
        // ������ 0�̴�           : ���� ��밡 ����( this == other )
        // ������ 0���� ũ��(+1)  : ����(����) ũ��(this > other)
        return thisNode.H == otherNode.H ? 0 : thisNode.H < otherNode.H ? -1 : 1;    // H ���� �������� ũ�⸦ �����ض�.
    }
    
    /// <summary>
    /// ���̸����ִ°�� 
    /// ���������� ���������� ���� ã�Ƽ� ���� ��带 �˻��ؿ´�.
    /// �׸��� ���¸���Ʈ�� �ִ´�
    /// </summary>
    /// <param name="prevNode">�˻�������ġ</param>
    /// <returns></returns>
    private static void NextCheckNode(Astar_Node prevNode) 
    {
        Astar_Node node; //�̵��� ��ġ�� ���� ��ü���� ����
        Four_Way_Access_Area_Check tempWay = Four_Way_Access_Area_Check.NONE; // ����ʿ��� �Դ��� üũ�ϱ����Ѻ���

        int x = prevNode.X;     //����� ���� �ΰ� ����
        int y = prevNode.Y;

        int random = UnityEngine.Random.Range(0, 2);
        //���������� ������ ���������� �̵��Ұ��� �������� ����
     
        if (random < 1) //���ο켱 �������� ���� 
        {
            //���������� ���������� ��ǥ�� ����
            if (endNode.Y > prevNode.Y)
            {
                y += 1;
                tempWay = Four_Way_Access_Area_Check.DOWN; //�������� ���ϱ� �Ʒ����� �����Ѵ�
            }
            else if (endNode.Y < prevNode.Y)
            {
                y -= 1;
                tempWay = Four_Way_Access_Area_Check.UP; //�Ʒ������� ���ϱ� ������ �����Ѵ�
            }

            node = nodes[y,prevNode.X]; 
        }
        else  // ���ο켱 
        {
            //���������� ���������� ��ǥ�� ����
            if (endNode.X > prevNode.X)
            {
                x += 1;
                tempWay = Four_Way_Access_Area_Check.LEFT; //���������� ���ϱ� ������ �����Ѵ�
            }
            else if (endNode.X < prevNode.X)
            {
                x -= 1;
                tempWay = Four_Way_Access_Area_Check.RIGHT; //�������� ���ϱ� �������� �����Ѵ�
            }
            node = nodes[prevNode.Y,x];
        }

        if (node.State == NodeState.Inaccessible) //���� �Ұ� �����̸�  
        {

            //���� ����
            node.FourWayCheck &= ~tempWay;  // ���� ������ ���⿡�� ���� ������ ���� ��Ų��.
            node.FourWayCheck = ~node.FourWayCheck & Four_Way_Access_Area_Check.ALL; // �Ӽ��� �ٲ������� ��ü�� �����´� .
            
            // �ش������� �̵������������� ���� 
            node.State = NodeState.Nomal; // ���ٺҰ����������� ���ٰ����������� �����Ų�� 
            node.G = prevNode.G + nomalLine; //G�� ����  �밢���� �ȵ����� ������ �������� �� �����ָ�ȴ�.
            node.PrevNode = prevNode; //����� ���� ��尪 �Է�
            onTileCreate?.Invoke(node); //Ÿ�ϻ����϶�� ȣ��
            Debug.Log($" ������ :{prevNode.State}({prevNode.X}{prevNode.Y}) ��ǥ���� {node.X}{node.Y} = {prevNode.State}");
        }
        if (!openList.Contains(node)) //���¸���Ʈ�� �������� 
        {
            openList.Add(node); //��´�.
        }
    }

    

    /// <summary>
    /// 1. Ư������� �ֺ���带 �˻��ؼ� ���� ����Ʈ�� ���
    /// 2. �ֺ������ G���� �����Ѵ�.
    /// </summary>
    /// <param name="currentNode">�����̵Ǵ� ���</param>
    private static void OpenListAdd(Astar_Node currentNode)
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
    private static Astar_Node PathFinding()
    {
        openList.Clear();   // Ž���� �ʿ��� ��� ����Ʈ 
        closeList.Clear();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 

        startNode.G = 0.0f;      // �� ó�� ���������� G ���� 0�̷��� �ȴ�.
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
    private static Astar_Node[,] CreateNodeArray(int horizontalSize, int verticalSize)
    {
        Astar_Node[,] nodes = new Astar_Node[verticalSize, horizontalSize]; //��üũ�⸸ŭ �迭��� 
        int i = 0; // �ε��� ����� ���������ϰ� 
        for (int y = 0; y < verticalSize; y++) //���� ��ŭ
        {
            for (int x = 0; x < horizontalSize; x++)//���θ�ŭ
            {
                nodes[y, x] = new Astar_Node(i , x, y); //2���� �迭 �����  ///Ǯ�� �����ʿ� --
                i++; //�ε��� ����
            }
        }
        return nodes;
    }

    /// <summary>
    /// ������ G,H �� �� �ʱ�ȭ �ϴ� �Լ�
    /// ��� ��Ž���� �ʿ���
    /// </summary>
    private static void ResetValue()
    {
        foreach (var node in nodes)
        {
            node.AstarDataReset();
        }
    }

    /// <summary>
    /// ��ü ����  ���� ���õ� �������Ѵ�.
    /// ���� ��ġ �������� ��帮��Ʈ�� �޸���ƽ ���� �����ϴ� �Լ� 
    /// </summary>
    /// <param name="endNode">���� ��� ��</param>
    private static void SetHeuristicsValue(Astar_Node endNode) 
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
    public static List<Vector3Int> GetPath(Astar_Node node)
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
    public static Astar_Node GetNode(int startIndex)
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
    public static Astar_Node GetNode( int x , int y , int z = 1)
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




    public static void TestGLog()
    {
        string str = "";
        foreach (Astar_Node node in nodes)
        {
            str += $"��ǥ({node.X},{node.Y}) : G �� : {node.G} \r\n";
        }
        Debug.Log(str);
    }

   

}