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
    int testPlayerLength = 10;
    /// <summary>
    /// 캐릭터 데이터는 외부에서 셋팅하기때문에 해당 델리게이트 연결해줘야함
    /// </summary>
    public Func<BattleMapEnemyBase[]> initEnemy;

    public Action turnStart;

    BattleMap_Player_Controller bpc;

    /// <summary>
    /// 몬스터 다죽으면 맵초기화시키기위해 찾아오기
    /// </summary>
    InitCharcterSetting battleMapEndAction;

    [SerializeField]
    GameObject bossPrefab;

    public CameraOriginTarget cot;

    /// <summary>
    /// 데이터 초기화 함수 
    /// </summary>
    public override void InitData()
    {
        cot = FindObjectOfType<CameraOriginTarget>(true);
        BattleMapEnemyBase[] enemyList = initEnemy?.Invoke(); //외부에서 몬스터 배열이 들어왔는지 체크
        battleMapEndAction = FindObjectOfType<InitCharcterSetting>();
        bpc = FindObjectOfType<BattleMap_Player_Controller>();  
        if (enemyList == null || enemyList.Length == 0) //몬스터 초기화가 안되있으면 
        {
            int enumStartValue = (int)EnumList.MultipleFactoryObjectList.SIZE_S_HUMAN_ENEMY_POOL;
            int enumEndValue = (int)EnumList.MultipleFactoryObjectList.SIZE_L_ROBOT_ENEMY_POOL+1;
            int randValue = 0;
            //테스트 데이터 생성
            for (int i = 0; i < testPlayerLength; i++)//캐릭터들 생성해서 셋팅 
            {
                randValue = UnityEngine.Random.Range(enumStartValue, enumEndValue);
                BattleMapEnemyBase go = (BattleMapEnemyBase)Multiple_Factory.Instance.GetObject(
                    (EnumList.MultipleFactoryObjectList)randValue);
                
                charcterList.Add(go);
                
                go.name = $"Enemy_{i}";
                go.EnemyData.wType = go.EnemyData.wType;
                go.EnemyData.mType = go.EnemyData.mType;
                go.EnemyData.AttackRange = 1;
                
                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //데이터 연결 
                go.transform.position = go.CurrentTile.transform.position; //셋팅된 타일위치로 이동시킨다.
                go.onDie += (unit) => { 
                    charcterList.Remove(unit);
                    PlayerQuest_Gyu playerQuest = SpaceSurvival_GameManager.Instance.PlayerQuest;
                    foreach (var quest in playerQuest.CurrentQuests) 
                    {
                        if (quest.QuestType == QuestType.Killcount) 
                        {
                            int forSize = quest.QuestMosters.Length; 
                            for (int i = 0; i < forSize; i++)
                            {
                                if (unit.EnemyData.mType == quest.QuestMosters[i]) 
                                {
                                    quest.CurrentCount[i]++;
                                } 

                            }
                        }
                    }
                    
                    if (charcterList.Count < 1)
                    {
                        Debug.Log("유닛전멸 마을로이동하든 뭘하든 처리");
                        //기존 작업중인것들 코루틴들이 전부 실행다된후에 초기화 로직이 실행되야한다.

                        StartCoroutine(BattleMapEnd()) ;
                    }

                };
            }
            //보스 추가시 밑에 기능연결
            if (SpaceSurvival_GameManager.Instance.IsBoss)
            {
                BattleMapEnemyBase go = Instantiate(bossPrefab).GetComponent<BattleMapEnemyBase>(); 
                charcterList.Add(go);
                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //데이터 연결 
                go.transform.position = go.CurrentTile.transform.position; //셋팅된 타일위치로 이동시킨다.
                go.onDie += (unit) =>
                {
                    charcterList.Remove(unit);
                    PlayerQuest_Gyu playerQuest = SpaceSurvival_GameManager.Instance.PlayerQuest;
                    foreach (var quest in playerQuest.CurrentQuests)
                    {
                        if (quest.QuestType == QuestType.Story)
                        {
                            int forSize = quest.QuestMosters.Length;
                            for (int i = 0; i < forSize; i++)
                            {
                                if (unit.EnemyData.mType == quest.QuestMosters[i])
                                {
                                    quest.CurrentCount[i]++;
                                }

                            }
                        }
                    }
                    StartCoroutine(BattleMapEnd());
                };
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

    IEnumerator BattleMapEnd()
    {
        yield return null;
        battleMapEndAction.TestReset();
        LoadingScene.SceneLoading(EnumList.SceneName.SpaceShip);
    }

    Tile PlayerTileIndex;
    public override void TurnStartAction()
    {
        PlayerTileIndex = SpaceSurvival_GameManager.Instance.PlayerTeam[0].currentTile;
        BattleMapEnemyBase Ene;
        int forSize = charcterList.Count;
        for (int i = 0; i < forSize; i++)
        {
            Ene = (BattleMapEnemyBase)charcterList[i];
            Ene.EnemyData.Stamina += TurnActionValue;
            cot.Target = Ene.transform;
            Ene.EnemyTurnAction(PlayerTileIndex);
        }
        TurnActionValue -= UnityEngine.Random.Range(5.0f, 10.0f);// 행동력 소모후 테스트 용 
        Debug.Log($"적군턴끝 행동력 :{TurnActionValue}");
        
        TurnEndAction();
    }
}
