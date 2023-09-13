using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_01 : SkillData
{
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(11).GetComponent<Button>();
        button.onClick.AddListener(Skill_LevelUp);
    }
}
