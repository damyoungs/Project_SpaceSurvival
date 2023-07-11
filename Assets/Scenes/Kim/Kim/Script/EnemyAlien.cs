using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAlien : EnemyBase
{
    // 공격 목표
    Transform target;

    // 무기 장착 여부
    public enum WeaponType
    {
        None,       // 주먹
        Sword,      // 칼
        Riffle      // 총
    }

    // 방패 장착 여부
    bool equipShield = false;

    // Enemy 턴 확인
    public bool isTrun = false;


    protected override void Awake()
    {
        
    }


    private void Start()
    {
        
    }

    // 몬스터 패턴
    private void enemyTrun()
    {
        if(isTrun == true)
        {
            if(target !=  null) // 타켓이 보이면 공격 행동
            {
                Attack_Roll();
            }
            else // 아니면 이동
            {

            }
        }
        
    }

    /// <summary>
    /// 공격 패턴
    /// </summary>
    private void Attack_Roll()
    {
        // 75% 확률로 기본 공격
        if(Random.value < 0.75f)
        {

        }
        // 25% 확률로 스킬
        else
        {

        }
    }
}
