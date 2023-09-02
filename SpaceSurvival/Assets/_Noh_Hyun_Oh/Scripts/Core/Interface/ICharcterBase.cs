using System;
using UnityEngine;
/// <summary>
/// ��Ʋ �ʿ��� ���ֵ��� ������ �־���� �������̽� �ʿ��ϸ� �߰�����
/// </summary>
public interface ICharcterBase 
{
    public Transform transform { get; }
    /// <summary>
    /// ������ UI ĳ�̿� ������Ƽ
    /// </summary>
    TrackingBattleUI BattleUI { get; set; }

    /// <summary>
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform BattleUICanvas { get;  }

    /// <summary>
    /// ���� ĳ���Ͱ� �ִ� Ÿ�� 
    /// </summary>
    Tile CurrentTile { get; }

   
    /// <summary>
    /// �������� ������� �ʱ�ȭ�� �Լ�
    /// </summary>
    public void ResetData();
}
