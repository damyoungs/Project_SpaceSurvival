using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill_05 : SkillData
{
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(7).GetComponent<Button>();
        button.onClick.AddListener(Skill_LevelUp);
        SkillName = "�Ϲ� ����";
        SkillLevel = 0;
        //AnimClip
        //audioClip
        AttackRange = 1;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillType = SkillType.Normal;
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        Require_Stamina_For_UsingSkill = (int)(SkillLevel * 0.33f);
        Debug.Log("������ ����");
    }
    //����Ŭ���� ��ų�ߵ� ������ ����
}
