using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestClear_Gyu
{
    /// <summary>
    /// Ŭ���� Npcid
    /// </summary>
    public int ClearNpcid;

    /// <summary>
    /// ����Ʈ Ÿ��
    /// </summary>
    public QuestManager_Gyu.QuestType clearType;

    /// <summary>
    /// Ŭ������� �ʿ��� ī��Ʈ(����)
    /// </summary>
    public int requiredCount;

    /// <summary>
    /// ���� ī��Ʈ(����)
    /// </summary>
    public int currentCount;

    /// <summary>
    /// ����Ʈ���� �޼�����
    /// </summary>
    public bool isSucess => requiredCount - 1 < currentCount;


    /// <summary>
    /// �������
    /// </summary>
    public void QuestCounting()
    {
        currentCount++;
    }
}
