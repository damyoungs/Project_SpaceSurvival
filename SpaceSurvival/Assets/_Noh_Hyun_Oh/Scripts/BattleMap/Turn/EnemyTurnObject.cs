using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.OpenVR;
using UnityEngine;

public class EnemyTurnObject : TurnBaseObject
{
    /// <summary>
    /// �׽�Ʈ�� ���� 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 2;
    /// <summary>
    /// ĳ���� �����ʹ� �ܺο��� �����ϱ⶧���� �ش� ��������Ʈ �����������
    /// </summary>
    public Func<BattleMapEnemyBase[]> initEnemy;

    public Action turnStart; 

    /// <summary>
    /// ������ �ʱ�ȭ �Լ� 
    /// </summary>
    public override void InitData()
    {
        BattleMapEnemyBase[] enemyList = initEnemy?.Invoke(); //�ܺο��� ���� �迭�� ���Դ��� üũ
        if (enemyList == null || enemyList.Length == 0) //���� �ʱ�ȭ�� �ȵ������� 
        {
            //�׽�Ʈ ������ ����
            for (int i = 0; i < testPlayerLength; i++)//ĳ���͵� �����ؼ� ���� 
            {
                BattleMapEnemy go = (BattleMapEnemy)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_ENEMY_POOL);
                
                charcterList.Add(go);
                
                go.name = $"Enemy_{i}";
                //go.EnemyNum = i;
                
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
                                if (unit.EnemyType == quest.QuestMosters[i]) 
                                {
                                    quest.RequestCount[i]++;
                                } 

                            }
                        }
                    }
                    if (charcterList.Count < 1)
                    {
                        Debug.Log("�������� �������̵��ϵ� ���ϵ� ó��");
                        LoadingScene.SceneLoading(EnumList.SceneName.BattleShip);
                    }

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
    public Tile des;
    float AttackRange;
    public override void TurnStartAction()
    {
        turnStart?.Invoke();
        des = SpaceSurvival_GameManager.Instance.PlayerTeam[0].currentTile;
        Debug.Log(charcterList.Count);
        foreach (var enemy in charcterList) 
        {
            enemy.CharcterMove(des);
        }

        TurnActionValue -= UnityEngine.Random.Range(5.0f, 10.0f);// �ൿ�� �Ҹ��� �׽�Ʈ �� 
        Debug.Log($"�����ϳ� �ൿ�� :{TurnActionValue}");
        TurnEndAction();
    }
}
