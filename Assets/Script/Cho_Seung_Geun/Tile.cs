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
    // 타일 타입
    MapTileType tileType = MapTileType.centerTile;
    public int TileType
    {
        get => (int)tileType;
        set
        {
            tileType = (MapTileType)value;
        }
    }

    // 타일 위 몬스터, 아이템 등 타입 존재 여부
    ExistType existType = 0;
    public int ExistType
    {
        get => (int)existType;
        set
        {
            existType = (ExistType)value;
        }
    }

    // 타일의 가로 인덱스
    int width = 0;
    public int Width
    {
        get => width;
        set
        {
            width = value;
        }
    }

    // 타일의 세로 인덱스
    int length = 0;
    public int Length
    {
        get => length;
        set
        {
            length = value;
        }
    }
}
