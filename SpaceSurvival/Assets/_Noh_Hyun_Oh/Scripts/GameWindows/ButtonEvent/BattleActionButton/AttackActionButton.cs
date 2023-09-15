using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
    }
    protected override void OnMouseEnter()
    {
        uiController.ViewButtons();
    }
    protected override void OnMouseExit()
    {
        uiController.ResetButtons();
    }
}
