using System;
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
public class SkillData : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler, IPointerMoveHandler,IPointerExitHandler
{
    string skillName;                        public string SkillName { get => skillName; protected set { skillName = value; } }
    AnimationClip animClip;                  public AnimationClip AnimClip { get => animClip; protected set {  animClip = value; } }
    AudioClip audioClip;                     public AudioClip AudioClip { get => audioClip; protected set { audioClip = value; } }
    int attackRange;                         public int AttackRange { get => attackRange; protected set { attackRange = value; } }
    int require_Stamina_For_UsingSkill;      public int Require_Stamina_For_UsingSkill { get => require_Stamina_For_UsingSkill; protected set { require_Stamina_For_UsingSkill = value; } }
    public Sprite skill_sprite;

    uint require_Force_For_skillLevelUp;   
    public uint Require_Force_For_skillLevelUp
    {
        get => require_Force_For_skillLevelUp; 
        protected set
        {
            require_Force_For_skillLevelUp = value; 
            require_Force_Text.text = require_Force_For_skillLevelUp.ToString();
        } 
    }
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
    SkillBox_Description skillBox_Description;
    SkillType skillType; public SkillType SkillType { get => skillType; protected set { skillType = value; } }
    //UI
    protected Image skill_Icon;
    protected Button button;
    protected TextMeshProUGUI skillLevel_Text;
    TextMeshProUGUI require_Force_Text;
    //#EndUI

    public Action<SkillData> on_PointerEnter;
    public Action on_PointerExit;
    public Action on_PointerMove;
    public Action<SkillData> on_PointerClick;

    void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        player = GameManager.Player__;
        skillBox_Description = transform.parent.GetChild(12).GetComponent<SkillBox_Description>();
        skill_Icon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        skillLevel_Text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        require_Force_Text = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        player.on_DarkForce_Change += () => button.interactable = Require_Force_For_skillLevelUp < player.DarkForce;
        on_PointerEnter += skillBox_Description.Open;
        on_PointerExit += skillBox_Description.Close;
        on_PointerMove += skillBox_Description.MovePosition;
        skill_Icon.sprite = skill_sprite;
    }
    protected virtual void Skill_LevelUp()
    {
        if (Require_Force_For_skillLevelUp < player.DarkForce)
        {
            player.DarkForce -= Require_Force_For_skillLevelUp;
            SkillLevel++;
        }
    }
    protected virtual void Calculate_NextLevel_Info()
    {
        //stamina 사용량
        // 공격력
    }
    protected virtual void LevelUp_Skill_Info()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        on_PointerClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        on_PointerEnter?.Invoke(this);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        on_PointerMove?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        on_PointerExit?.Invoke();
    }
}
