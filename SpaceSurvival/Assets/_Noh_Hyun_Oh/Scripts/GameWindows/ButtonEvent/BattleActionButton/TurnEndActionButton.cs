using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndActionButton : BattleActionButtonBase
{
    TurnManager turnManager;
    private void Start()
    {
        turnManager = WindowList.Instance.TurnManager;
    }
    protected override void OnClick()
    {
        ITurnBaseData node = turnManager.GetNode(); //원래는 캐릭터 쪽에서 턴완료 버튼 호출해야하는데 캐릭터 가없음으로  테스트코드로 찾아온다.
        if (node == null)
        {
            Debug.Log("왜못찾냐?");
            return;
        }
        Debug.Log($"{node.UnitBattleIndex}번째 옵젝 : 값 :{node.TurnActionValue} 함수가등록되있냐? : {node.TurnEndAction}");
        node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //턴 진행 시 행동력 감소치 대충 때려넣는다.

        node.TurnEndAction(node); //턴완료 를 알린다.
    }

}
