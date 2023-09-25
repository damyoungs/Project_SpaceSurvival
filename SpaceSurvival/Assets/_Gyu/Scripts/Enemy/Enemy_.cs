using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class Enemy_ : MonoBehaviour, IBattle
{
    public enum Monster_Type
    {
        Base = 0,
        Size_S,
        Size_M,
        Size_L,
        Boss,
    }
    Monster_Type type = Monster_Type.Base;

    public Monster_Type Type
    {
        get => type;
        set
        {
            type = value;
        }
    }


    public enum WeaponType
    {
        None = 0,
        Riffle,
    }
    WeaponType weaponType = WeaponType.None;
    Animator Anima;

    public Action AC_Attack;
    public Action<float> on_Enemy_Stamina_Change;
    public Action<float> on_Enemy_HP_Change;
    public Action onDie { get; set; }

    int Go_Attack = Animator.StringToHash("Attack");
    int OnHit = Animator.StringToHash("Hit");
    
    float hp = 200;
    public float maxHP = 200;
    public float MaxHp => maxHP;
    public float HP
    {
        get => hp;
        set
        {
            if(hp != value)
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
            if(stamina != value)
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
            if(attackPower != value)
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

    private void Awake()
    {
        Anima = GetComponent<Animator>();
    }

    private void Attack()
    {
        stamina--;
        Anima.SetTrigger(Go_Attack);

    }

    public void Attack_Enemy(IBattle target)
    {
        Attack();
        if(target != null)
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
