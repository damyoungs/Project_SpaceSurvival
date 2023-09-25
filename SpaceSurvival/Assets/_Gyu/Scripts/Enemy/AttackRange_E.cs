using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange_E : AttackRange
{
    public BattleMapPlayerBase[] GetPlayerArray()
    {
        if (activeAttackTiles.Count > 0)
        {

            BattleMapPlayerBase[] playerArray = SpaceSurvival_GameManager.Instance.PlayerTeam; //��Ʋ���� �������� ���� ��� 

            int playerSize = playerArray.Length;      // ��Ʋ�ʿ� �����ִ� ������ ���� ��������

            List<BattleMapPlayerBase> resultEnemyList = new List<BattleMapPlayerBase>(playerSize); //�ִ�ũ��� ���� ����Ʈ���� Ŭ���������� �׳� �ִ������

            foreach (Tile attackTile in activeAttackTiles) //���ݹ�����ŭ �˻��ϰ�
            {
                for (int i = 0; i < playerSize; i++) //������ �˻��� ���� 
                {
                    if (playerArray[i].CurrentTile.width == attackTile.width &&
                        playerArray[i].CurrentTile.length == attackTile.length) //Ÿ���� ������ 
                    {
                        resultEnemyList.Add(playerArray[i]); //����Ʈ�� �߰�
                        break;//����Ÿ�ϰ˻������� ��������
                    }
                }
            }
            return resultEnemyList.ToArray();
        }
        return null;
    }
}
