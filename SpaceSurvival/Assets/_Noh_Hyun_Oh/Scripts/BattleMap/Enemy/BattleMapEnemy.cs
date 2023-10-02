using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleMapEnemy : BattleMapEnemyBase, IBattle
{

    /// <summary>
    /// �̵����װ� �����ؼ� üũ�ϴ� ����
    /// </summary>
    bool isMoveCheck = false;
    public override bool IsMoveCheck => isMoveCheck;

    Monster_Type enemyType = Monster_Type.Base;
    public Monster_Type EnemyType 
    {
        get => enemyType;
        set 
        {
            if (enemyType != value)
            {
                enemyType = value;

            }

        }
    }



    float hp = 100;
    float HP 
    {
        get => hp;
        set 
        {
            hp = value;
            if (hp < 0) 
            {
                OnDie();
            }
        }
    }


    float maxHp = 100;

    float attData = 0;

    float defData = 0;

    int attRange = 3;
    

   
    /// <summary>
    /// ���ݹ����ȿ� ������ �����ϴ� �Լ� 
    /// </summary>
    private void IsAttackAction()
    {
        Tile attackTile = Cho_BattleMap_Enemy_AStar.SetEnemyAttackSize(currentTile, attRange);
        Debug.Log(SpaceSurvival_GameManager.Instance.PlayerTeam[0]);

        if (attackTile != null) 
        {
            Attack_Enemy(SpaceSurvival_GameManager.Instance.PlayerTeam[0].CharcterData);
        }
    }

    public void InitData() 
    {
        //������ ����
    }

  
    public void Attack_Enemy(IBattle target)
    {
        target.Defence(attData);
    }

    public void Defence(float damage, bool isCritical = false)
    {
        HP -= Mathf.Max(0, damage - defData);
        GameManager.EffectPool.GetObject(damage, transform, isCritical);
    }

    public void OnDie() 
    {
        ResetData();
        onDie?.Invoke(this);
    }


    public override void CharcterMove(Tile selectedTile)
    {
        List<Tile> path = Cho_BattleMap_Enemy_AStar.PathFind(
                                                           SpaceSurvival_GameManager.Instance.BattleMap,
                                                           SpaceSurvival_GameManager.Instance.MapSizeX,
                                                           SpaceSurvival_GameManager.Instance.MapSizeY,
                                                           this.currentTile,
                                                           selectedTile,
                                                           moveSize
                                                           );
        EnemyMove(path);
    }
    //[SerializeField]
    //Animator unitAnimator;
    //int isWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField]
    float moveSpeed = 3.0f;
    private void EnemyMove(List<Tile> path)
    {
        isMoveCheck = true; //�̵� ������ üũ�ϱ� 
        Vector3 targetPos = currentTile.transform.position; //���̾��°�� ���� Ÿ����ġ ����
        //unitAnimator.SetBool(isWalkingHash, true); //�̵��ִϸ��̼� ��� ����
        foreach (Tile tile in path)  // �����ִ°�� 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //���ο� ��ġ��� 
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //�ش���� �ٶ󺸰� 
            this.currentTile.ExistType = Tile.TileExistType.None;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� ������Ʈ �̵��߿� Ÿ�� �������ϴ� move�κ���");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� �� �����Ͱ� ����Ǿߵȴ� charcter �� ");
            tile.ExistType = Tile.TileExistType.Monster; 

            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //�̵�����
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        //unitAnimator.SetBool(isWalkingHash, false);

        //charcterData.Stamina -= this.currentTile.MoveCheckG; //�����̵��� �Ÿ���ŭ ���¹̳��� ��´�.

        isMoveCheck = false; //�̵��������� üũ
        IsAttackAction(); //���� �����ȿ��ִ��� üũ
    }
}
