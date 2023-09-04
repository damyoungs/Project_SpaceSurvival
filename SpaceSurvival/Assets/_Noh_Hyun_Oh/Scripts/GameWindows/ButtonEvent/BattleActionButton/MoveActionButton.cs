using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActionButton : BattleActionButtonBase
{

    protected override void OnClick()
    {
        ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // 턴 오브젝트찾아서 
        ICharcterBase curruentUnit = turnObj.CurrentUnit;           // 현재 행동중인 유닛 찾고 
        if (curruentUnit == null)
        {
            Debug.LogWarning("선택한 유닛이없습니다");
            return;
        }
        if (!curruentUnit.IsMoveCheck) //이동중이 아닌경우만  
        {
            SpaceSurvival_GameManager.Instance.MoveRange.ClearDoubleLineRenderer(curruentUnit.CurrentTile);
            SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeDoubleView(curruentUnit.CurrentTile,curruentUnit.MoveSize);//이동범위표시해주기 
        }
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
