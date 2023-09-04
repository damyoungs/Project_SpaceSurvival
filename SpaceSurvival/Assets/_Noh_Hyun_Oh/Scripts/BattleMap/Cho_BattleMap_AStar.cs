using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �±پ� AStar �ణ �պ� ���� 
/// ������ �����͸� �޾Ƽ� ó���ϵ��� ���� 
/// ���� �ٸ� ������Ʈ�� ������ �̿��ؾߵǼ� ���������� ����Ҽ�������� ����.
/// </summary>
public static class Cho_BattleMap_AStar
{
    public static List<Tile> PathFind(Tile[] map, Tile start, Tile end ,int sizeX , int sizeY)
    {
        const float sideDistance = 1.0f;
        const float diagonalDistance = 1.414f;

        List<Tile> path = null;
        
        List<Tile> open = new List<Tile>();
        List<Tile> close = new List<Tile>();

        for (int i = 0; i < map.Length; i++)
        {
            map[i].Clear();
        }

        Tile current = start;
        current.G = 0;
        current.H = GetHeuristic(current, end);
        open.Add(current);

        Tile adjoinTile;                            // ������ Ÿ���� ����

        while (open.Count > 0)
        {
            open.Sort();
            current = open[0];
            open.RemoveAt(0);

            if (current != end)
            {
                close.Add(current);

                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        if (current.Width + x < 0 || current.Width + x > sizeX - 1 ||
                            current.Length + y < 0 || current.Length + y > sizeY - 1)
                            continue;
                        
                        adjoinTile = map[(current.Width + x) * sizeY + current.Length + y];    // ������ Ÿ�� ��������

                        if (adjoinTile == current)                                          // ������ Ÿ���� (0, 0)�� ���
                            continue;
                        if (adjoinTile.ExistType != Tile.TileExistType.None)                // ������ Ÿ���� None�� �ƴ� ��
                            continue;
                        if (close.Exists( (inClose) => inClose == adjoinTile ))             // close����Ʈ�� ���� ��
                            continue;

                        bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                        if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
                            (
                             map[(current.Width + x) * sizeY + current.Length].ExistType == Tile.TileExistType.Prop ||
                             map[(current.Width) * sizeY + current.Length + y].ExistType == Tile.TileExistType.Prop
                            ))
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

                        if (adjoinTile.G > current.G + distance)
                        {
                            if (adjoinTile.parent == null)
                            {
                                adjoinTile.H = GetHeuristic(adjoinTile, end);
                                open.Add(adjoinTile);
                            }
                            adjoinTile.G = current.G + distance;
                            adjoinTile.parent = current;
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }

        if (current == end)
        {
            path = new List<Tile>();
            while (current.parent != null)
            {
                path.Add(current);
                current = current.parent;
            }

            path.Reverse();
        }

        return path;
    }

    private static float GetHeuristic(Tile current, Tile end)
    {
        return Mathf.Abs(end.Width - current.Width) + Mathf.Abs(end.Length - current.Length);
    }








   

}
