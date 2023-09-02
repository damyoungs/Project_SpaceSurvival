using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� �� 
/// </summary>
public class PlayerTurnObject : TurnBaseObject 
{
   
    /// <summary>
    /// �׽�Ʈ�� ���� 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 1;

    [SerializeField]
    BattleMap_Player_Controller bpc;
    
    
    /// <summary>
    /// ĳ���� �����ʹ� �ܺο��� �����ϱ⶧���� �ش� ��������Ʈ �����������
    /// </summary>
    public Func<ICharcterBase[]> initPlayer;


    
    /// <summary>
    /// ������ �ʱ�ȭ �Լ�
    /// </summary>
    public override void InitData() 
    {
        bpc = FindObjectOfType<BattleMap_Player_Controller>();
        bpc.onClickMonster += OnClickUnit;
        TurnActionValue = 0.0f; //�׼ǰ� �ʱ�ȭ 
        ICharcterBase[] playerList = initPlayer?.Invoke(); //�ܺο��� ĳ���͹迭�� ���Դ��� üũ
        if (playerList == null || playerList.Length == 0) //ĳ���� �ʱ�ȭ�� �ȵ������� 
        {
            //�׽�Ʈ ������ ����
            for (int i = 0; i < testPlayerLength; i++)//ĳ���͵� �����ؼ� ���� 
            {
                BattleMapPlayerBase go = (BattleMapPlayerBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL);
                charcterList.Add(go);
                go.name = $"Player_{i}";
                bpc.onMoveActive += (tile) => {
                    if (isTurn && go.isControll) //���� ���λ��°� , ĳ������ ��Ʈ���� ������������  
                    {
                        go.CharcterMove(tile); //�̵��Ѵ�.
                    }
                };
                go.SetTile(SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster));
                go.transform.position = go.CurrentTile.transform.position; //���õ� Ÿ����ġ�� �̵���Ų��.
            }
            
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(testPlayerLength); //�� ��� ������ �����ֱ� 
        }
        else // �ܺο��� �����Ͱ� ���������  �̰�찡 �������ΰ���  ���� ���̼� ���þ��Ұ��̱⶧����...
        {
            foreach (ICharcterBase player in playerList)
            {
                charcterList.Add(player); //�ϰ����� ĳ���ͷ� ����
                player.GetCurrentTile = () => SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster); //Ÿ�� ���ÿ���
                player.transform.position = player.CurrentTile.transform.position;//���õ� Ÿ����ġ�� �̵���Ų��.
            }
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(playerList.Length);//�� ��� ������ �����ֱ� 
        }
    }
    public override void TurnStartAction()
    {
        isTurn = true;
        Debug.Log($"{name} ������Ʈ�� ���� ���۵Ǿ���");
    }

    /// <summary>
    /// ���ϰ�� Ŭ���̺�Ʈ ó�� 
    /// </summary>
    /// <param name="tile"></param>
    public void OnClickUnit(Tile tile)
    {
        if (isTurn) //���ϰ�츸 �۵��Ѵ�.
        {
            ///�Ʊ� �� Ŭ�������� ����Ҽ��ִ� . ������ ���͵� �����־ �ȵ� 
            foreach (ICharcterBase playerUnit in charcterList) //�÷��̾� ������ 
            {
                if (tile.width == playerUnit.CurrentTile.width &&
                    tile.length == playerUnit.CurrentTile.length) //Ŭ���� Ÿ���� �÷��̾� ���� ��ġ�� 
                {
                    currentUnit = playerUnit; //Ŀ��Ʈ �����ϰ� ����������
                    BattleMapPlayerBase unit = (BattleMapPlayerBase)playerUnit;
                    unit.isControll = true;
                    break;
                }
                else 
                {
                    currentUnit = playerUnit; //Ŀ��Ʈ �����ϰ� ����������
                    BattleMapPlayerBase unit = (BattleMapPlayerBase)playerUnit;
                    unit.isControll = false;
                }
            }

            Debug.Log($"{currentUnit} �̰� ���������� ");
            if (currentUnit != null) //�ൿ ������ ���� ��������� ����  
            {
                
            }

        }
    }
}
