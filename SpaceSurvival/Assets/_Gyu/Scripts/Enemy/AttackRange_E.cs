using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange_E : AttackRange
{
    public BattleMapPlayerBase[] GetPlayerArray()
    {
        if (activeAttackTiles.Count > 0)
        {

            BattleMapPlayerBase[] playerArray = SpaceSurvival_GameManager.Instance.PlayerTeam; //배틀맵의 몹정보를 전부 들고 

            int playerSize = playerArray.Length;      // 배틀맵에 나와있는 몬스터의 갯수 가져오고

            List<BattleMapPlayerBase> resultEnemyList = new List<BattleMapPlayerBase>(playerSize); //최대크기는 몬스터 리스트보다 클수없음으로 그냥 최대로잡자

            foreach (Tile attackTile in activeAttackTiles) //공격범위만큼 검색하고
            {
                for (int i = 0; i < playerSize; i++) //적들을 검색을 진행 
                {
                    if (playerArray[i].CurrentTile.width == attackTile.width &&
                        playerArray[i].CurrentTile.length == attackTile.length) //타일이 같으면 
                    {
                        resultEnemyList.Add(playerArray[i]); //리스트에 추가
                        break;//다음타일검색을위해 빠져나감
                    }
                }
            }
            return resultEnemyList.ToArray();
        }
        return null;
    }
}
