using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]                         // ������Ʈ Ŭ�� �� �ڽ� ������Ʈ�� �ƴ� �� Ŭ������ ����ִ� ������Ʈ�� Ŭ���ǵ��� ����� ��Ʈ����Ʈ
public class Tile : MonoBehaviour
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
    public enum ExistType
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
    public ExistType existType = 0;
    public ExistType ExistTypes
    {
        get => existType;
        set
        {
            existType = value;
        }
    }

    // Ÿ���� ���� �ε���
    int width = 0;
    public int Width
    {
        get => width;
        set
        {
            width = value;
        }
    }

    // Ÿ���� ���� �ε���
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
