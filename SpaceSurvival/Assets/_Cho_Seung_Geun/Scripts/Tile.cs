using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]                         // ������Ʈ Ŭ�� �� �ڽ� ������Ʈ�� �ƴ� �� Ŭ������ ����ִ� ������Ʈ�� Ŭ���ǵ��� ����� ��Ʈ����Ʈ
public class Tile : MonoBehaviour, IComparable<Tile>
{
    /// <summary>
    /// Ÿ���� Ÿ��
    /// </summary>
    public enum MapTileType
    {
        centerTile = 0,
        sideTile,
        vertexTile
    }

    /// <summary>
    /// �� Ÿ�� ���� ������ �ִ� ��ü�� Ÿ��
    /// </summary>
    public enum TileExistType
    {
        None = 0,
        monster,
        item,
        prop
    }

    // Ÿ�� Ÿ��
    public MapTileType tileType = 0;
    public MapTileType TileType
    {
        get => tileType;
        set
        {
            tileType = value;
        }
    }

    // Ÿ�� �� ����, ������ �� Ÿ�� ���� ����
    public TileExistType existType = 0;
    public TileExistType ExistType
    {
        get => existType;
        set
        {
            existType = value;
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

    public int Index = 0;

    public float G;

    public float H;

    public float F => G + H;

    public Tile parent;

    /// <summary>
    /// A*�� ���� ���� �ʱ�ȭ
    /// </summary>
    public void Clear()
    {
        G = float.MaxValue;
        H = float.MaxValue;
        parent = null;
    }

    public int CompareTo(Tile other)
    {
        if (other == null)
            return 1;
        return F.CompareTo(other.F);
    }
}
