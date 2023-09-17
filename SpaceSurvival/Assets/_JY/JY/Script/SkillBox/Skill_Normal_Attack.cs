using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill_Normal_Attack : SkillData
{
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(7).GetComponent<Button>();
        button.onClick.AddListener(Skill_LevelUp);
        SkillName = "�Ϲ� ����";
        SkillLevel = 1;
        //AnimClip
        //audioClip
        AttackRange = 1;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillPower = 1.0f;
        SkillType = SkillType.Normal;
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        Require_Stamina_For_UsingSkill = (int)(SkillLevel * 0.33f);
        Debug.Log("������ ����");
    }
    protected override void SetCurrentLevel_Description_Info(out string info)
    {
        info = "dddd";
    }
    protected override void SetNextLevel_Description_Info(out string info)
    {
        info = "aaaaa";
    }
    //����Ŭ���� ��ų�ߵ� ������ ����
}
