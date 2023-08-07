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
        ITurnBaseData node = turnManager.GetNode(); //������ ĳ���� �ʿ��� �ϿϷ� ��ư ȣ���ؾ��ϴµ� ĳ���� ����������  �׽�Ʈ�ڵ�� ã�ƿ´�.
        if (node == null)
        {
            Debug.Log("�ָ�ã��?");
            return;
        }
        Debug.Log($"{node.UnitBattleIndex}��° ���� : �� :{node.TurnActionValue} �Լ�����ϵ��ֳ�? : {node.TurnEndAction}");
        node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //�� ���� �� �ൿ�� ����ġ ���� �����ִ´�.

        node.TurnEndAction(node); //�ϿϷ� �� �˸���.
    }

}
