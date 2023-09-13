using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillData : MonoBehaviour
{
    string skillName;                        public string SkillName { get => skillName; protected set { skillName = value; } }
    int skillLevel;                          public int SkillLevel { get => skillLevel; protected set { skillLevel = value; } }
    AnimationClip animClip;                  public AnimationClip AnimClip { get => animClip; protected set {  animClip = value; } }
    AudioClip audioClip;                     public AudioClip AudioClip { get => audioClip; protected set { audioClip = value; } }
    int attackRange;                         public int AttackRange { get => attackRange; protected set { attackRange = value; } }
    int require_Force_For_skillLevelUp;      public int Require_Force_For_skillLevelUp { get => require_Force_For_skillLevelUp; protected set { require_Force_For_skillLevelUp = value; } }
    int require_Stamina_For_UsingSkill;      public int Require_Stamina_For_UsingSkill { get => require_Stamina_For_UsingSkill; protected set { require_Stamina_For_UsingSkill = value; } }


    //UI
    protected Image itemicon;
    protected Button button;
    protected TextMeshProUGUI skillLevel_Text;
    //#EndUI
    protected virtual void Init()
    {
        itemicon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        button = transform.GetChild(2).GetComponent<Button>();
        skillLevel_Text = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }
}
