using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        Debug.Log("�̵�Ŭ��");
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
