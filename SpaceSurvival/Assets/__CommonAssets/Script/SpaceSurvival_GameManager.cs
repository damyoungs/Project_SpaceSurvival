using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ű �̺�Ʈ ��Ʈ���� �̳� ���� 
/// </summary>
[Flags]
public enum HotKey_Use : byte
{
    None = 0,            // ó�����۽� �⺻�� : ������, Ÿ��Ʋ, �ε�â �� �Է��� �Ǹ�ȵǴ°��� ���� �÷��� 
    BattleMap = 1,            // ��Ʋ�� ó�� ������Ȳ�� ���� �÷���
    TownMap = 2,            // ���� ó�� ���������� ��Ȳ�ϰ�� ���� �÷���                   
    OptionView = 4,            // �ɼ�â�� �������� üũ�� �÷���  - �����̳� ����� �κ�â ���� ���� ������ ����ɸ��Ѱ� ������ȵǴ� üũ�ʿ�
}

/// <summary>
/// ���ӿ��� �ʿ��� ������ �� ����� ����� ���� �޴��� Ŭ���� 
/// </summary>
public class SpaceSurvival_GameManager : Singleton<SpaceSurvival_GameManager>
{
    HotKey_Use inputkeyCheck;
    public HotKey_Use InputKeyCheck 
    {
        get => inputkeyCheck;
        set 
        {
            if (inputkeyCheck != value) 
            {
                inputkeyCheck = value;
                switch (value)
                {
                    case HotKey_Use.None:

                        break;
                    case HotKey_Use.BattleMap:
                        break;
                    case HotKey_Use.TownMap:
                        break;
                    case HotKey_Use.OptionView:
                        break;
                    default:
                        break;
                }
            }
        
        }
    }
    /// <summary>
    /// ����Ű ��뿩�� 
    /// </summary>
    public bool Is_Use_HotKey = false;

    /// <summary>
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���� 
    /// </summary>
    Tile[] battleMap;
    public Tile[] BattleMap 
    {
        get 
        {
            //if (battleMap == null) //��Ʋ���� ���̾����� 
            //{
            //    battleMap = GetBattleMapTilesData?.Invoke(); //��������Ʈ ��û�� ���� �޾ƿ������Ѵ�.
            //    //��Ʋ���� �ƴѰ�� ��������Ʈ�� ���� ���ø��ϴ� null �� ���� �ɼ����ִ�.
            //}
            battleMap ??= GetBattleMapTilesData?.Invoke(); //���� �ּ��� ���� �����̶�� �Ѵ� . (������)
            return battleMap;

        }
    }
    /// <summary>
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���ΰ��� 
    /// </summary>
    int mapSizeX = -1;
    public int MapSizeX
    {
        get
        {
            if (mapSizeX < 0 && GetMapTileX != null) //�ʱⰪ�̸� 
            {
                mapSizeX = GetMapTileX();
            }
            return mapSizeX;
        }
    }
    /// <summary>
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���ΰ��� 
    /// </summary>
    int mapSizeY = -1;
    public int MapSizeY
    {
        get
        {
            if (mapSizeY < 0 && GetMapTileY != null) //�ʱⰪ�̸� 
            {
                mapSizeY = GetMapTileY();
            }
            return mapSizeY;
        }
    }

    /// <summary>
    /// �÷��̾��� ���� ����� �����صд�.
    /// </summary>
    ICharcterBase[] playerTeam;
    public ICharcterBase[] PlayerTeam 
    {
        get 
        {
            //if (playerTeam == null) //������� ������ 
            //{
            //    playerTeam = GetPlayerTeam?.Invoke(); // ������ ��û�ؼ� �޾ƿ´�
            //}
            playerTeam ??= GetPlayerTeam?.Invoke(); // ���� �ּ� ����� ����(������)
            return playerTeam;
        }
    }
    /// <summary>
    /// �÷��̾��� ���� ����� �޾ƿ������� Func ��������Ʈ 
    /// �̰� ���� �����ͻ����� ��Ե����𸣴� �ϴ� Ʋ�� ��Ƴ��� ���߿� ����.
    /// </summary>
    public Func<ICharcterBase[]> GetPlayerTeam;

    /// <summary>
    /// ��Ʋ�� �����͸� �޾ƿ������� Func ��������Ʈ
    /// </summary>
    public Func<Tile[]> GetBattleMapTilesData;
    public Func<int> GetMapTileX;
    public Func<int> GetMapTileY;


    /*
     �κ��丮�� �ϳ��� ����Ұ�� ���⿡ �߰��� �ʿ��ϴ� .
     */

    /// <summary>
    /// �̵����� ǥ���ϴ� ������Ʈ �����´�.
    /// </summary>
    MoveRange moveRange;
    public MoveRange MoveRange 
    {
        get 
        {
            if (moveRange == null) 
            {
                moveRange = GetMoveRangeComp?.Invoke();
            }
            return moveRange;
        }
    
    }
    /// <summary>
    /// �̵� ����ǥ���ϴ� ���� �޾ƿ������� ��������Ʈ
    /// </summary>
    public Func<MoveRange> GetMoveRangeComp;


    /// <summary>
    /// ������ ���� ���콺 �̺�Ʈ �������� ���� 
    /// </summary>
    public bool IsUICheck = false;


    /// <summary>
    /// ��Ʋ���� �ʱ�ȭ �Լ��� �̱����������� ���ٴ��� �ʱ⶧���� 
    /// �������� ������ ������ �׽õ����ʴ´� �׷��� Get �Ҷ� üũ �ʿ� 
    /// </summary>
    InitCharcterSetting battleMapInitClass;
    public InitCharcterSetting BattleMapInitClass 
    {
        get 
        {
            if (battleMapInitClass == null) 
            {
                battleMapInitClass = GetBattleMapInit?.Invoke();
            }
            return battleMapInitClass;
        }
    }
    public Func<InitCharcterSetting> GetBattleMapInit;

    
}
