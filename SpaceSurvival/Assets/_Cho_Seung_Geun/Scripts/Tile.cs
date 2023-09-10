using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]                         // 오브젝트 클릭 시 자식 오브젝트가 아닌 이 클래스가 들어있는 오브젝트가 클릭되도록 만드는 어트리뷰트
public class Tile : MonoBehaviour, IComparable<Tile>
{
    /// <summary>
    /// 타일의 타입
    /// </summary>
    public enum MapTileType
    {
        centerTile = 0,
        sideTile,
        vertexTile
    }

    /// <summary>
    /// 이 타일 위에 놓여져 있는 물체의 타입
    /// </summary>
    public enum TileExistType
    {
        None = 0,
        Monster,
        Item,
        Prop,
        Move,
        Charcter

    }

    // 타일 타입
    public MapTileType tileType = 0;
    public MapTileType TileType
    {
        get => tileType;
        set
        {
            tileType = value;
        }
    }

    // 타일 위 몬스터, 아이템 등 타입 존재 여부
    public TileExistType existType = 0;
    public TileExistType ExistType
    {
        get => existType;
        set
        {
            if (existType != value) 
            {
                
                existType = value;
                switch (value)
                {
                    case TileExistType.None:
                        lineRenderer.enabled = false;
                        break;
                    case TileExistType.Monster:
                        lineRenderer.enabled = false;
                        break;
                    case TileExistType.Item:
                        break;
                    case TileExistType.Prop:
                        lineRenderer.enabled = false;
                        break;
                    case TileExistType.Move:
                        lineRenderer.enabled = true;
                        break;
                    case TileExistType.Charcter:
                        GameManager.Player__.Stamina--;
                        lineRenderer.enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // 타일의 가로 인덱스
    public int width = 0;
    public int Width
    {
        get => width;
        set
        {
            width = value;
        }
    }

    // 타일의 세로 인덱스
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

    public float MoveCheckG = 1000.0f;

    public float H;

    public float F => G + H;

    public Tile parent;


    LineRenderer lineRenderer;

    public Action on_Decrease_Player_Stamina;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// A*에 관한 변수 초기화
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
