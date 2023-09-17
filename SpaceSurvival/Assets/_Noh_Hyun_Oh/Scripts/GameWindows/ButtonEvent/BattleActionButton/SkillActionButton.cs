using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        Debug.Log("스킬클릭");
    }


    //protected override void OnMouseEnter()
    //{
    //    uiController.ViewButtons();
    //}
    //protected override void OnMouseExit()
    //{
    //    uiController.ResetButtons();
    //}
}
