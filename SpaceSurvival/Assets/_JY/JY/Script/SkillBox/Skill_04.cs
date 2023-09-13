using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_04 : SkillData
{
    protected override void Init()
    {
        base.Init();
        SkillName = "스나이핑";
        SkillLevel = 0;
        //AnimClip
        //audioClip
        AttackRange = 1;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillType = SkillType.Sniping;
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        Require_Stamina_For_UsingSkill = (int)(SkillLevel * 0.33f);
        Debug.Log("레벨업 실행");
    }
}
