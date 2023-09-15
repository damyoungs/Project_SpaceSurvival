using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_03 : SkillData
{
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(9).GetComponent<Button>();
        button.onClick.AddListener(Skill_LevelUp);
        SkillName = "난사";
        SkillLevel = 1;
        //AnimClip
        //audioClip
        AttackRange = 1;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillType = SkillType.rampage;
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        Require_Stamina_For_UsingSkill = (int)(SkillLevel * 0.33f);
        Debug.Log("레벨업 실행");
    }
    protected override void SetCurrentLevel_Description_Info(out string info)
    {
        info = "dddd";
    }
    protected override void SetNextLevel_Description_Info(out string info)
    {
        info = "aaaaa";
    }
}
