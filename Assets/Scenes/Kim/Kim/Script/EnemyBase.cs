using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    /// <summary>
    /// 현재 HP
    /// </summary>
    public float hp = 1.0f;

    /// <summary>
    /// 최대 HP
    /// </summary>
    public float hpMax = 50.0f;

    /// <summary>
    /// 현재 활동력
    /// </summary>
    public float stamina = 1.0f;

    /// <summary>
    /// 최대 활동력
    /// </summary>
    public float staminaMax = 3.0f;

    /// <summary>
    /// 근력 스탯 - 근거리 데미지
    /// </summary>
    public float str = 1.0f;

    /// <summary>
    /// 민첩 스탯 - 원거리 데미지
    /// </summary>
    public float dex = 1.0f;

    /// <summary>
    /// 행운 스텟 - 크리티컬 확률
    /// </summary>
    public float lux = 1.0f;

    /// <summary>
    /// 화상
    /// </summary>
    public int burning = 0;

    /// <summary>
    /// 독
    /// </summary>
    public int poison = 0;

    /// <summary>
    /// 감전
    /// </summary>
    public int eletric = 0;

    /// <summary>
    /// 공포
    /// </summary>
    public int fear = 0;

    // 애니메이터 파라메터 해쉬's
    readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");            // Fl.이동 상태
    readonly int WeaponType_R = Animator.StringToHash("Weapon_Riffle");     // Fl.무기 - 총
    readonly int WeaponType_S = Animator.StringToHash("Weapon_Sword");      // Fl.무기 - 칼
    readonly int AC_Attack = Animator.StringToHash("Attack");               // Tr.공격 상태
    readonly int AC_Defens = Animator.StringToHash("Defense");               // Tr.방어 상태

    protected virtual void Awake()
    {
        hp = hpMax;
    }

    /// <summary>
    /// HP 프로퍼티
    /// </summary>
    public float Hp
    {
        get => hp;
        protected set
        {
            if(hp != value)
            {
                hp = value;
            }
            if(hp <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {

    }

    /// <summary>
    /// 상태이상
    /// </summary>
    public void abnStatus()
    {

    }

    //protected virtual void OnMove()
    //{

    //}

    // 사망 처리
    //public void Die()
    //{

    //}

    /// <summary>
    /// 공격 - 맨손
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Attack_Punch(Transform target)
    {

    }

    /// <summary>
    /// 공격 - 무기(칼)장착
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Attack_Swing(Transform target)
    {
        
    }

    /// <summary>
    /// 공격 - 무기(총)장착
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Attack_Fire(Transform target)
    {

    }

    /// <summary>
    /// 스킬 1번
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Attack_Skill_01(Transform target)
    {

    }

    /// <summary>
    /// 스킬 2번
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Attack_Skill_02(Transform target)
    {

    }

    /// <summary>
    /// 스킬 3번
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Attack_Skill_03(Transform target)
    {

    }

    /// <summary>
    /// 방어 - 방패 장착
    /// </summary>
    protected virtual void Defense()
    {

    }
}
