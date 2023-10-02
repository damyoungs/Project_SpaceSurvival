using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleMapEnemy : BattleMapEnemyBase, IBattle
{

    /// <summary>
    /// 이동버그가 존재해서 체크하는 변수
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
    /// 공격범위안에 있으면 공격하는 함수 
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
        //데이터 셋팅
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
        isMoveCheck = true; //이동 중인지 체크하기 
        Vector3 targetPos = currentTile.transform.position; //길이없는경우 현재 타일위치 고정
        //unitAnimator.SetBool(isWalkingHash, true); //이동애니메이션 재생 시작
        foreach (Tile tile in path)  // 길이있는경우 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //새로운 위치잡고 
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //해당방향 바라보고 
            this.currentTile.ExistType = Tile.TileExistType.None;
            //Debug.Log($"{this.currentTile.Index}타일 오브젝트 이동중에 타일 데이터일단 move로변경");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}타일 이 데이터가 변경되야된다 charcter 로 ");
            tile.ExistType = Tile.TileExistType.Monster; 

            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //이동시작
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        //unitAnimator.SetBool(isWalkingHash, false);

        //charcterData.Stamina -= this.currentTile.MoveCheckG; //최종이동한 거리만큼 스태미나를 깍는다.

        isMoveCheck = false; //이동끝낫는지 체크
        IsAttackAction(); //공격 범위안에있는지 체크
    }
}
