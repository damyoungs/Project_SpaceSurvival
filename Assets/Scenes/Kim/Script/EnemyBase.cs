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
    public float hpMax = 10.0f;

    /// <summary>
    /// 현재 활동력
    /// </summary>
    public float stamina = 1.0f;
    
    /// <summary>
    /// 근력 스탯
    /// </summary>
    public float str = 1.0f;

    /// <summary>
    /// 민첩 스탯
    /// </summary>
    public float dex = 1.0f;

    /// <summary>
    /// 행운 스텟
    /// </summary>
    public float lux = 1.0f;

    /// <summary>
    /// 화상
    /// </summary>
    public float burning = 1.0f;

    /// <summary>
    /// 독
    /// </summary>
    public float poison = 1.0f;

    /// <summary>
    /// 감전
    /// </summary>
    public float eletric = 1.0f;

    /// <summary>
    /// 공포
    /// </summary>
    public float fear = 1.0f;

    // 애니메이터 파라메터 해쉬's
    readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");            // Fl.이동 상태
    readonly int WeaponType_R = Animator.StringToHash("Weapon_Riffle");     // Fl.무기 - 총
    readonly int WeaponType_S = Animator.StringToHash("Weapon_Sword");      // Fl.무기 - 칼
    readonly int AC_Attack = Animator.StringToHash("Attack");               // Tr.공격 상태
    readonly int AC_Defens = Animator.StringToHash("Defens");               // Tr.방어 상태

    private void Awake()
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
            //if(hp <= 0)
            //{
                
            //}
        }
    }

    // 상태이상
    public void abnStatus()
    {

    }

    //protected virtual void OnMove()
    //{

    //}

    ///// <summary>
    ///// 사망 이벤트
    ///// </summary>
    //public void Die()
    //{

    //}

    
}
