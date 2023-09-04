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

        //�����Ḧ �����Ѵ� .
        if (!unit.IsMoveCheck) //�̵����װ� �־ ������  
        {
            Debug.Log("��������");
            node.TurnActionValue -= UnityEngine.Random.Range(0.05f, 0.7f); //�� ���� �� �ൿ�� ����ġ ���� �����ִ´�.
            node.IsTurn = false;
            SpaceSurvival_GameManager.Instance.MoveRange.ClearDoubleLineRenderer(node.CurrentUnit.CurrentTile); //�̵����� ���½�Ų��.
            node.TurnEndAction(); //�ϿϷ� ��������Ʈ�� �����Ѵ� .
        }
        else 
        {
            Debug.Log("���� �̵���");
        }
    }

}
