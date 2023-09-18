using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ : MonoBehaviour, IBattle
{
    public Action AC_Attack;
    public Action<float> on_Enemy_Stamina_Change;
    public Action<float> on_Enemy_HP_Change;
    public Action onDie { get; set; }

    public int EnemyNumber;

    float hp = 200;
    float maxHP = 200;
    public float MaxHp => maxHP;
    public float HP
    {
        get => hp;
        private set
        {
            hp = value;
            if(hp <= 0)
            {
                Die();
            }
            hp = Mathf.Clamp(value, 0, maxHP);
            on_Enemy_HP_Change(hp);
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
        
    }

    private void Attack()
    {
        stamina--;
        AC_Attack();
    }

    public void Die()
    {
        StartCoroutine(EnemyDead());
        onDie?.Invoke();
    }

    IEnumerator EnemyDead()
    {
        yield return null;

        Destroy(this.transform);
    }

    public void Attack_Enemy(IBattle target)
    {
        target.Defence(AttackPower);
        Attack();
    }

    public void Defence(float damage)
    {
        HP -= Mathf.Max(0, damage - defencePower);
    }
}
