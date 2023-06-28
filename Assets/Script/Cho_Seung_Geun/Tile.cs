using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileType
{
    centerTile = 0,
    sideTile,
    vertexTile
}

[SelectionBase]
public class Tile : MonoBehaviour
{
    

    TileType type = 0;
    //public int Type
    //{
    //    get => type;
    //    set
    //    {
    //        type = value;
    //    }

    //}
    public int Type
    {
        get => (int)type;
        set
        {
            type = (TileType)value;
        }

    }
}
