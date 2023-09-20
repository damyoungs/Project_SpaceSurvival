using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Gyu
{
    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    string title;
    public string Title 
    {
        get => title;
        set => title = value;
    }

    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    string description;
    public string Description 
    {
        get => description;
        set => description = value;
    }

    /// <summary>
    /// ����Ʈ ��ǥ
    /// </summary>
    string clearObjectives;
    public string ClearObjectives 
    {
        get => clearObjectives; 
        set => clearObjectives = value;

    }

    /// <summary>
    /// ����Ʈ id
    /// </summary>
    int questId;
    public int QuestId => questId;
   
    /// <summary>
    /// Ŭ���� ���� Ȯ��
    /// </summary>
    public bool IsSucess => processValue > 0.99f;

    /// <summary>
    /// ����Ʈ ���� ��
    /// </summary>
    public float processValue = 0.00f;
 

    /// <summary>
    /// ����Ʈ Ŭ���� ����
    /// </summary>
    public QuestReward_Gyu questReward;
    
    /// <summary>
    /// ����Ʈ Ŭ���� ����
    /// </summary>
    public QuestClear_Gyu questClear;

    public Quest_Gyu(int index) 
    {
        questId = index;
        questReward = new();
        questClear = new();
    }

}
