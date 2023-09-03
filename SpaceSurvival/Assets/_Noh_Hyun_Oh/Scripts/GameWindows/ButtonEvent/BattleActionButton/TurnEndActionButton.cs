using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        ITurnBaseData node = TurnManager.Instance.CurrentTurn; //현재 턴인 유닛을 가져와서 
        if (node == null) // 없으면 실행안하고 
        {
            Debug.Log("왜못찾냐?");
            return;
        }
        ICharcterBase unit = node.CurrentUnit;
        //테스트코드 

        //턴종료를 실행한다 .
        if (!unit.IsMoveCheck) //이동버그가 있어서 수정중  
        {
            Debug.Log("턴종료라고");
            node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //턴 진행 시 행동력 감소치 대충 때려넣는다.
            node.IsTurn = false;
            SpaceSurvival_GameManager.Instance.MoveRange.ClearDoubleLineRenderer(node.CurrentUnit.CurrentTile); //이동범위 리셋시킨다.
            node.TurnEndAction(); //턴완료 델리게이트를 실행한다 .
        }
        else 
        {
            Debug.Log("아직 이동중");
        }
    }

}
