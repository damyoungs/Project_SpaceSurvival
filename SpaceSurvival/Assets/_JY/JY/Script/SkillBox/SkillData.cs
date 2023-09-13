using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    string skillName;   public string SkillName { get => skillName; protected set { skillName = value; } }
    byte skillLevel;    public byte SkillLevel { get => skillLevel; protected set { skillLevel = value; } }
    AnimationClip animClip;  public AnimationClip AnimClip { get => animClip; protected set {  animClip = value; } }
    AudioClip audioClip;    public AudioClip AudioClip { get => audioClip; protected set { audioClip = value; } }
    byte attackRange;
    short require_Force_For_skillLevelUp;
    byte require_Stamina_For_UsingSkill;

    protected virtual void Init()
    {

    }
}
