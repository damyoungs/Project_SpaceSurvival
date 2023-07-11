using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class EnemyAlien : EnemyBase
{
    // 공격 목표
    Transform target;

    // 무기 장착 여부
    enum WeaponType
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

    // Enemy 턴
    private void enemyTrun()
    {
        if(isTrun == true)
        {
            if(target !=  null)
            {
                // 스킬
                if(Random.value < 0.25f) // 25%확률로 스킬 사용
                {

                }
                // 공격
                else
                {

                }
            }

            else
            {

            }
        }
        
    }
}
