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
        MoveRange moverange = SpaceSurvival_GameManager.Instance.MoveRange;
        Debug.Log($"������ : {curruentUnit} , �̵������� :{moverange}");
        moverange.MoveSizeDoubleView(curruentUnit.CurrentTile,curruentUnit.MoveSize);//�̵�����ǥ�����ֱ� 
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
