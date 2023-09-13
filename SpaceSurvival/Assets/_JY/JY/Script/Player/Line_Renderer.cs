using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attack_State
{
    DeSelect,
    Normal_Attack,
    Skill
}
public class Line_Renderer : MonoBehaviour
{
    [SerializeField]
    GameObject normal_Attack_Line;

    Attack_State state = Attack_State.DeSelect;
    public Attack_State State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case Attack_State.DeSelect:
                        Destroy(normal_Attack_Line);
                        break;
                    case Attack_State.Normal_Attack:
                        Instantiate(normal_Attack_Line);
                        break;
                    case Attack_State.Skill:
                        break;
                }
            }
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
