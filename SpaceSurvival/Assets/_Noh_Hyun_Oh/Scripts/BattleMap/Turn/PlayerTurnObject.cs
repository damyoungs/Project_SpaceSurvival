using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                bpc.onMoveActive = (tile) => {
                    go.CharcterMove(tile);
                };
                go.SetTile(SpaceSurvival_GameManager.Instance.BattleMapDoubleArray[0,0]);
            }
            
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(testPlayerLength); //�� ��� ������ �����ֱ� 
        }
        else // �ܺο��� �����Ͱ� ���������  �̰�찡 �������ΰ���  ���� ���̼� ���þ��Ұ��̱⶧����...
        {
            foreach (ICharcterBase player in playerList)
            {
                charcterList.Add(player); //�ϰ����� ĳ���ͷ� ����
            }
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(playerList.Length);//�� ��� ������ �����ֱ� 
        }
    }
}
