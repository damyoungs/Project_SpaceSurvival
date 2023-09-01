using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        ITurnBaseData node = TurnManager.Instance.CurrentTurn; //���� ���� ������ �����ͼ� 
        if (node == null) // ������ ������ϰ� 
        {
            Debug.Log("�ָ�ã��?");
            return;
        }
        
        Debug.Log($"{node.UnitBattleIndex}��° ���� : �� :{node.TurnActionValue} �Լ�����ϵ��ֳ�? : {node.TurnEndAction}");
        
        //�׽�Ʈ�ڵ� 
        node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //�� ���� �� �ൿ�� ����ġ ���� �����ִ´�.

        //�����Ḧ �����Ѵ� .
        node.TurnEndAction(node); //�ϿϷ� �� �˸���.
    }

}
