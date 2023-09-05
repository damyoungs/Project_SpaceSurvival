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
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(node.CurrentUnit.CurrentTile); //�̵����� ���½�Ų��.
            node.CurrentUnit = null;
        }
        if (!node.IsMove) 
        {
            node.IsTurn = false;
            Debug.Log($"�Ʊ��� ���� �����ൿ�� :{node.TurnActionValue}");
            
            node.TurnEndAction(); //�ϿϷ� ��������Ʈ�� �����Ѵ� .
        
        }
    }

}
