using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;

public class EnemyTurnObject : TurnBaseObject
{
    /// <summary>
    /// 테스트용 변수 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 2;
    /// <summary>
    /// 캐릭터 데이터는 외부에서 셋팅하기때문에 해당 델리게이트 연결해줘야함
    /// </summary>
    public Func<BattleMapEnemyBase[]> initEnemy;

    public Action turnStart; 

    /// <summary>
    /// 데이터 초기화 함수 
    /// </summary>
    public override void InitData()
    {
        BattleMapEnemyBase[] enemyList = initEnemy?.Invoke(); //외부에서 몬스터 배열이 들어왔는지 체크
        if (enemyList == null || enemyList.Length == 0) //몬스터 초기화가 안되있으면 
        {
            //테스트 데이터 생성
            for (int i = 0; i < testPlayerLength; i++)//캐릭터들 생성해서 셋팅 
            {
                BattleMapEnemyBase go = (BattleMapEnemyBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_ENEMY_POOL);
                
                charcterList.Add(go);
                
                go.name = $"Enemy_{i}";
                go.EnemyNum = i;
                
                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //데이터 연결 
                go.transform.position = go.CurrentTile.transform.position; //셋팅된 타일위치로 이동시킨다.
            }
        }
        else // 외부에서 데이터가 들어왔을경우  이경우가 정상적인경우다  내가 데이서 셋팅안할것이기때문에...
        {
            foreach (BattleMapEnemyBase enemy in enemyList)
            {
                charcterList.Add(enemy); //턴관리할 몹 셋팅
            }
        }

        SpaceSurvival_GameManager.Instance.GetEnemeyTeam = () => charcterList.OfType<BattleMapEnemyBase>().ToArray();
    }
    public Tile Des;
    float AttackRange;
    //public BattleMapEnemyBase Ene;
    
    public override void TurnStartAction()
    {
        turnStart?.Invoke();
        Des = SpaceSurvival_GameManager.Instance.PlayerTeam[0].currentTile;
        //for (int i = 0; i < testPlayerLength; i++)
        //{
        //    Debug.Log("123");
        //}
            BattleMapEnemyBase Ene = (BattleMapEnemyBase)charcterList[0];
            Ene.EnemyAi();
        TurnActionValue -= UnityEngine.Random.Range(2.0f, 3.0f);

        Debug.Log($"적군턴끝 행동력 :{TurnActionValue}");
    }
}
