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
    /// �׽�Ʈ�� ���� 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 10;
    /// <summary>
    /// ĳ���� �����ʹ� �ܺο��� �����ϱ⶧���� �ش� ��������Ʈ �����������
    /// </summary>
    public Func<BattleMapEnemyBase[]> initEnemy;

    public Action turnStart;

    BattleMap_Player_Controller bpc;

    /// <summary>
    /// ���� �������� ���ʱ�ȭ��Ű������ ã�ƿ���
    /// </summary>
    InitCharcterSetting battleMapEndAction;

    [SerializeField]
    GameObject bossPrefab;

    public CameraOriginTarget cot;

    /// <summary>
    /// ������ �ʱ�ȭ �Լ� 
    /// </summary>
    public override void InitData()
    {
        cot = FindObjectOfType<CameraOriginTarget>(true);
        BattleMapEnemyBase[] enemyList = initEnemy?.Invoke(); //�ܺο��� ���� �迭�� ���Դ��� üũ
        battleMapEndAction = FindObjectOfType<InitCharcterSetting>();
        bpc = FindObjectOfType<BattleMap_Player_Controller>();  
        if (enemyList == null || enemyList.Length == 0) //���� �ʱ�ȭ�� �ȵ������� 
        {
            int enumStartValue = (int)EnumList.MultipleFactoryObjectList.SIZE_S_HUMAN_ENEMY_POOL;
            int enumEndValue = (int)EnumList.MultipleFactoryObjectList.SIZE_L_ROBOT_ENEMY_POOL+1;
            int randValue = 0;
            //�׽�Ʈ ������ ����
            for (int i = 0; i < testPlayerLength; i++)//ĳ���͵� �����ؼ� ���� 
            {
                randValue = UnityEngine.Random.Range(enumStartValue, enumEndValue);
                BattleMapEnemyBase go = (BattleMapEnemyBase)Multiple_Factory.Instance.GetObject(
                    (EnumList.MultipleFactoryObjectList)randValue);
                
                charcterList.Add(go);
                
                go.name = $"Enemy_{i}";
                go.EnemyData.wType = go.EnemyData.wType;
                go.EnemyData.mType = go.EnemyData.mType;
                go.EnemyData.AttackRange = 1;
                
                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //������ ���� 
                go.transform.position = go.CurrentTile.transform.position; //���õ� Ÿ����ġ�� �̵���Ų��.
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
                        Debug.Log("�������� �������̵��ϵ� ���ϵ� ó��");
                        //���� �۾����ΰ͵� �ڷ�ƾ���� ���� ����ٵ��Ŀ� �ʱ�ȭ ������ ����Ǿ��Ѵ�.

                        StartCoroutine(BattleMapEnd()) ;
                    }

                };
            }
            //���� �߰��� �ؿ� ��ɿ���
            if (SpaceSurvival_GameManager.Instance.IsBoss)
            {
                BattleMapEnemyBase go = Instantiate(bossPrefab).GetComponent<BattleMapEnemyBase>(); 
                charcterList.Add(go);
                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //������ ���� 
                go.transform.position = go.CurrentTile.transform.position; //���õ� Ÿ����ġ�� �̵���Ų��.
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
        else // �ܺο��� �����Ͱ� ���������  �̰�찡 �������ΰ���  ���� ���̼� ���þ��Ұ��̱⶧����...
        {
            foreach (BattleMapEnemyBase enemy in enemyList)
            {
                charcterList.Add(enemy); //�ϰ����� �� ����
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
        TurnActionValue -= UnityEngine.Random.Range(5.0f, 10.0f);// �ൿ�� �Ҹ��� �׽�Ʈ �� 
        Debug.Log($"�����ϳ� �ൿ�� :{TurnActionValue}");
        
        TurnEndAction();
    }
}
