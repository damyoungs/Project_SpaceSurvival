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
    MapTileType tileType = MapTileType.centerTile;
    ExistType existType = 0;

    //public int Type
    //{
    //    get => type;
    //    set
    //    {
    //        type = value;
    //    }
    //}

    public int TileType
    {
        get => (int)tileType;
        set
        {
            tileType = (MapTileType)value;
        }
    }

    public int ExistType
    {
        get => (int)existType;
        set
        {
            existType = (ExistType)value;
        }
    }
}
