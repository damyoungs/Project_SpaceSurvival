using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        Debug.Log("이동클릭");
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
