using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SkillType
{
    Sniping,
    Penetrate,
    rampage,
    Blessing,
    Normal

}
public class SkillData : MonoBehaviour, IPointerClickHandler
{
    string skillName;                        public string SkillName { get => skillName; protected set { skillName = value; } }
    AnimationClip animClip;                  public AnimationClip AnimClip { get => animClip; protected set {  animClip = value; } }
    AudioClip audioClip;                     public AudioClip AudioClip { get => audioClip; protected set { audioClip = value; } }
    int attackRange;                         public int AttackRange { get => attackRange; protected set { attackRange = value; } }
    uint require_Force_For_skillLevelUp;      public uint Require_Force_For_skillLevelUp { get => require_Force_For_skillLevelUp; protected set { require_Force_For_skillLevelUp = value; } }
    int require_Stamina_For_UsingSkill;      public int Require_Stamina_For_UsingSkill { get => require_Stamina_For_UsingSkill; protected set { require_Stamina_For_UsingSkill = value; } }

    int skillLevel;                         
    public int SkillLevel
    {
        get => skillLevel; 
        protected set
        {
            skillLevel = value;
            LevelUp_Skill_Info();
            skillLevel_Text.text = skillLevel.ToString();
        }
    }

    Player_ player;
    SkillType skillType; public SkillType SkillType { get => skillType; protected set { skillType = value; } }
    //UI
    protected Image itemicon;
    protected Button button;
    protected TextMeshProUGUI skillLevel_Text;
    //#EndUI

    void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        player = GameManager.Player__;
        itemicon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        button = transform.GetChild(2).GetComponent<Button>();
        skillLevel_Text = transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        button.onClick.AddListener(Skill_LevelUp);
        player.on_DarkForce_Change += () => button.interactable = Require_Force_For_skillLevelUp < player.DarkForce;
    }
    void Skill_LevelUp()
    {
        if (Require_Force_For_skillLevelUp < player.DarkForce)
        {
            player.DarkForce -= Require_Force_For_skillLevelUp;
            SkillLevel++;
        }
    }
    protected virtual void LevelUp_Skill_Info()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.skillType);
    }
}
