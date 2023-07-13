using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MapTileType
{
    centerTile = 0,
    sideTile,
    vertexTile
}

enum ExistType
{
    monster = 0,
    item,
    prop
}

[SelectionBase]
public class Tile : MonoBehaviour
{
    // Ÿ�� Ÿ��
    MapTileType tileType = MapTileType.centerTile;
    public int TileType
    {
        get => (int)tileType;
        set
        {
            tileType = (MapTileType)value;
        }
    }

    // Ÿ�� �� ����, ������ �� Ÿ�� ���� ����
    ExistType existType = 0;
    public int ExistType
    {
        get => (int)existType;
        set
        {
            existType = (ExistType)value;
        }
    }

    // Ÿ���� ���� �ε���
    public int width = 0;
    public int Width
    {
        get => width;
        set
        {
            width = value;
        }
    }

    // Ÿ���� ���� �ε���
    public int length = 0;
    public int Length
    {
        get => length;
        set
        {
            length = value;
        }
    }
}
