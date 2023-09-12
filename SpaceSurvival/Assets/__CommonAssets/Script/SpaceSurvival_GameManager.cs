using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ���ӿ��� �ʿ��� ������ �� ����� ����� ���� �޴��� Ŭ���� 
/// </summary>
public class SpaceSurvival_GameManager : Singleton<SpaceSurvival_GameManager>
{

    /// <summary>
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���� 
    /// </summary>
    [SerializeField]
    Tile[] battleMap;
    public Tile[] BattleMap 
    {
        get 
        {
            if (battleMap == null || battleMap.Length == 0) //��Ʋ���� ���̾����� 
            {
                battleMap = GetBattleMapTilesData?.Invoke(); //��������Ʈ ��û�� ���� �޾ƿ������Ѵ�.
                //��Ʋ���� �ƴѰ�� ��������Ʈ�� ���� ���ø��ϴ� null �� ���� �ɼ����ִ�.
            }
            //battleMap ??= GetBattleMapTilesData?.Invoke(); //���� �ּ��� ���� �����̶�� �Ѵ� . (������)
            return battleMap;

        }
    }
    /// <summary>
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���ΰ��� 
    /// </summary>
    [SerializeField]
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
    [SerializeField]
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

    public void BattleMap_ResetData(bool isLoadedBattleMap = false)
    {
        if (!isLoadedBattleMap) 
        {
            battleMap = null;
            mapSizeX = -1;
            mapSizeY = -1;
            moveRange = null;
            GetMoveRangeComp = null;
            battleMapInitClass = null;
            GetBattleMapInit = null;
        }
        playerTeam = null;
        GetPlayerTeam = null;
    }
}
