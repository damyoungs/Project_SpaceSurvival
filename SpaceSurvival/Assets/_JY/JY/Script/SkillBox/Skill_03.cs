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
    }
}
