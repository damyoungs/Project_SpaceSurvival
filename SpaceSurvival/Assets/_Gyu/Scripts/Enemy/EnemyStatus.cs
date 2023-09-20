using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IBattle
{

    float hp = 200;
    float maxHP = 200;
    public float MaxHp => maxHP;
    public float HP
    {
        get => hp;
        private set
        {
            if (hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHP);
            }
        }
    }
    float stamina = 10;
    const float max_Stamina = 20;
    public float Max_Stamina => max_Stamina;
    public float Stamina
    {
        get => stamina;
        set
        {
            if (stamina != value)
            {
                stamina = Mathf.Clamp(value, 0, max_Stamina);
            }
        }
    }
    uint att;
    public uint ATT
    {
        get => att;
        set
        {
            if (att != value)
            {
                att = value;
            }
        }
    }
    uint dp;
    public uint DP
    {
        get => dp;
        set
        {
            if (dp != value)
            {
                dp = value;
            }
        }
    }


    public void Attack_Enemy(IBattle target)
    {
        float damage = att;
        if (target != null)
        {
            target.Defence(damage);
        }
    }

    public void Defence(float damage)
    {
        float final_Damage = damage - DP;
        HP -= final_Damage;
    }
}
