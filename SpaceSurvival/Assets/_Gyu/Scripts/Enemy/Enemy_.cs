using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class Enemy_ : MonoBehaviour, IBattle
{
    Animator Anima;
    public AnimatorController EnemyAc_Basic;
    public AnimatorOverrideController EnemyAc_Riffle;
    public AnimatorOverrideController EnemyAc_Sword;
    int Go_Attack = Animator.StringToHash("Attack");
    int OnHit = Animator.StringToHash("Hit");

    public Transform GrapPosition;
    public Transform Riffle;
    public Transform Sword;

    public enum Monster_Type { Base = 0, Size_S, Size_M, Size_L, Boss, }
    public Monster_Type type = Monster_Type.Base;
    public Monster_Type mType
    {
        get => type;
        set
        {
            type = value;
            switch (type)
            {
                case Monster_Type.Base:
                    HP = 200;
                    break;
                case Monster_Type.Size_S:
                    HP = 100;
                    break;
                case Monster_Type.Size_M:
                    HP = 200;
                    break;
                case Monster_Type.Size_L:
                    HP = 300;
                    break;
                case Monster_Type.Boss:
                    HP = 500;
                    break;
                default:
                    HP = 200;
                    break;
            }
        }
    }

    public enum WeaponType { None = 0, Riffle, Swrod }
    WeaponType weaponType = WeaponType.None;
    public WeaponType wType
    {
        get => weaponType;
        set
        {
            weaponType = value;
            switch (weaponType)
            {
                case WeaponType.None:
                    Anima.runtimeAnimatorController = EnemyAc_Basic;
                    
                    break;
                case WeaponType.Riffle:
                    Anima.runtimeAnimatorController = EnemyAc_Riffle;

                    break;
                case WeaponType.Swrod:
                    Anima.runtimeAnimatorController = EnemyAc_Sword;

                    break;
                default:
                    Anima.runtimeAnimatorController = EnemyAc_Basic;

                    break;
            }
        }
    }

    public Action AC_Attack;
    public Action<float> on_Enemy_Stamina_Change;
    public Action<float> on_Enemy_HP_Change;

    private void Awake()
    {
        Anima = GetComponent<Animator>();
        
    }

    float hp = 200;
    float maxHP = 200;
    public float MaxHp
    {
        get => maxHP;
        set
        {
            maxHP = value;
        }
    }
    public float HP
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHP);
                on_Enemy_HP_Change(hp);
            }
        }
    }

    float stamina = 10;
    const float maxStamina = 20;
    public float MaxStamina => maxStamina;
    public float Stamina
    {
        get => stamina;
        set
        {
            if (stamina != value)
            {
                stamina = Mathf.Clamp(value, 0, maxStamina);
                on_Enemy_Stamina_Change(stamina);
            }
        }
    }

    uint attackPower;
    public uint AttackPower
    {
        get => attackPower;
        set
        {
            if (attackPower != value)
            {
                attackPower = value;
            }
        }
    }

    uint defencePower;
    public uint DefencePower
    {
        get => defencePower;
        set
        {
            if (defencePower != value)
            {
                defencePower = value;
            }
        }
    }

    private void Attack()
    {
        stamina--;
        Anima.SetTrigger(Go_Attack);

    }

    public void Attack_Enemy(IBattle target)
    {
        Attack();
        if (target != null)
        {
            target.Defence(AttackPower);
        }
    }

    public void Defence(float damage)
    {
        Anima.SetTrigger(OnHit);
        HP -= Mathf.Max(0, damage - defencePower);
    }
}
