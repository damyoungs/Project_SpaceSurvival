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
<<<<<<< HEAD
    public enum ExistType
=======
    public enum TileExistType
>>>>>>> Cho2
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
<<<<<<< HEAD
    public ExistType existType = 0;
    public ExistType ExistTypes
=======
    public TileExistType existType = 0;
    public TileExistType ExistType
>>>>>>> Cho2
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
}
