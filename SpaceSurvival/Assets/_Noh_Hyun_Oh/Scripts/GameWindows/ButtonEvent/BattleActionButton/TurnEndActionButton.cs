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
        ICharcterBase unit = node.CurrentUnit;
        //�׽�Ʈ�ڵ� 
        if (node.CurrentUnit != null) 
        {
            SpaceSurvival_GameManager.Instance.MoveRange.ClearDoubleLineRenderer(node.CurrentUnit.CurrentTile); //�̵����� ���½�Ų��.
        }
        node.TurnActionValue -= 0.8f;//UnityEngine.Random.Range(0.05f, 0.7f); //�� ���� �� �ൿ�� ����ġ ���� �����ִ´�.
        node.IsTurn = false;

        node.TurnEndAction(); //�ϿϷ� ��������Ʈ�� �����Ѵ� .
    }

}
