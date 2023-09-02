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
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���� 
    /// </summary>
    Tile[,] battleMapDoubleArray;
    public Tile[,] BattleMapDoubleArray
    {
        get
        {
            //if (battleMapDoubleArray == null) //��Ʋ���� ���̾����� 
            //{
            //    battleMapDoubleArray = GetBattleMapTilesDataDoubleArray?.Invoke(); //��������Ʈ ��û�� ���� �޾ƿ������Ѵ�.
            //    //��Ʋ���� �ƴѰ�� ��������Ʈ�� ���� ���ø��ϴ� null �� ���� �ɼ����ִ�.
            //}
            battleMapDoubleArray ??= GetBattleMapTilesDataDoubleArray?.Invoke(); //���� �ּ��� ���� �����̶�� �Ѵ� . (������)
            Debug.Log(battleMapDoubleArray);
            Debug.Log(battleMapDoubleArray.Length);
            return battleMapDoubleArray;

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

    /// <summary>
    /// ��Ʋ�� �����͸� �޾ƿ������� Func ��������Ʈ
    /// </summary>
    public Func<Tile[,]> GetBattleMapTilesDataDoubleArray;

    /*
     �κ��丮�� �ϳ��� ����Ұ�� ���⿡ �߰��� �ʿ��ϴ� .
     */
    protected override void Awake()
    {
        base.Awake();

    }
    
}
