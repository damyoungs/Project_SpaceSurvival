using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActionButton : BattleActionButtonBase
{

    protected override void OnClick()
    {
        ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // �� ������Ʈã�Ƽ� 
        ICharcterBase curruentUnit = turnObj.CurrentUnit;           // ���� �ൿ���� ���� ã�� 
        if (curruentUnit == null)
        {
            Debug.LogWarning("������ �����̾����ϴ�");
            return;
        }
        if (!curruentUnit.IsMoveCheck) //�̵����� �ƴѰ�츸  
        {
            SpaceSurvival_GameManager.Instance.MoveRange.ClearDoubleLineRenderer(curruentUnit.CurrentTile);
            SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeDoubleView(curruentUnit.CurrentTile,curruentUnit.MoveSize);//�̵�����ǥ�����ֱ� 
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
