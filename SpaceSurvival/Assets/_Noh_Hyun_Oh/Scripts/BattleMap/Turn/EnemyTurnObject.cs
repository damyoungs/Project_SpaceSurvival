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
    int testPlayerLength = 5;
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
                BattleMapEnemyBase go = (BattleMapEnemyBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_ENEMY_POOL);
                
                charcterList.Add(go);
                
                go.name = $"Enemy_{i}";
                go.Enemy.wType = go.Enemy.wType;
                go.Enemy.mType = go.Enemy.mType;
                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //������ ���� 
                go.transform.position = go.CurrentTile.transform.position; //���õ� Ÿ����ġ�� �̵���Ų��.
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
    float AttackRange;

    Tile PlayerTileIndex;
    
    public override void TurnStartAction()
    {
        turnStart?.Invoke();
        TurnManager.Instance.CurrentTurn.TurnActionValue = 20.0f;
        PlayerTileIndex = SpaceSurvival_GameManager.Instance.PlayerTeam[0].currentTile;
        BattleMapEnemyBase Ene;
        
        for (int i = 0; i < testPlayerLength; i++)
        {
            Ene = (BattleMapEnemyBase)charcterList[i];
            Ene.EnemyAi(PlayerTileIndex);
        }
        //TurnActionValue -= UnityEngine.Random.Range(5.0f, 10.0f);

        Debug.Log($"�����ϳ� �ൿ�� :{TurnActionValue}");
        //TurnEndAction();
    }
}
