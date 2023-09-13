using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_05 : SkillData
{
    protected override void Init()
    {
        base.Init();
        SkillName = "일반 공격";
        SkillLevel = 0;
        //AnimClip
        //audioClip
        AttackRange = 1;
        Require_Force_For_skillLevelUp = 2;
        Require_Stamina_For_UsingSkill = 2;
    }
}
