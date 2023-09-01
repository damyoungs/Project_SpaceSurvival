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
        
        Debug.Log($"{node.UnitBattleIndex}번째 옵젝 : 값 :{node.TurnActionValue} 함수가등록되있냐? : {node.TurnEndAction}");
        
        //테스트코드 
        node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //턴 진행 시 행동력 감소치 대충 때려넣는다.

        //턴종료를 실행한다 .
        node.TurnEndAction(node); //턴완료 를 알린다.
    }

}
