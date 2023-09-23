using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// ���忡�ʿ��ѵ����ʹ� Ŭ���� Ŀ��Ʈ �ΰ���.
/// </summary>
public class PlayerQuest_Gyu : MonoBehaviour
{
    /// <summary>
    /// �����Ҽ��ִ� ����Ʈ�� ���� 
    /// </summary>
    [SerializeField]
    int questMaxLength = 10;
    public int QuestMaxLength => questMaxLength;

    /// <summary>
    /// ĳ���Ͱ� �������� ����Ʈ ���
    /// </summary>
    [SerializeField]
    List<Gyu_QuestBaseData> currentQuests;
    public readonly List<Gyu_QuestBaseData> CurrentQuests;

    /// <summary>
    /// ĳ���Ͱ� �Ϸ��� ����Ʈ ��� 
    /// </summary>
    [SerializeField]
    List<Gyu_QuestBaseData> clearQuestList;


    private void Awake()
    {
        currentQuests = new List<Gyu_QuestBaseData>(questMaxLength);
        clearQuestList = new List<Gyu_QuestBaseData>(questMaxLength);
    }

    /// <summary>
    /// ����Ʈ ������ ����Ʈ�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="addQuest">�߰��� ����Ʈ</param>
    public void AppendQuest(Gyu_QuestBaseData addQuest)
    {
        if (questMaxLength > currentQuests.Count) 
        {
            addQuest.Quest_state = Quest_State.Quest_Start;
            currentQuests.Add(addQuest);
            return;
        }
        Debug.Log("���̻� ����Ʈ�� �����Ҽ� �����ϴ�.");
    }

    /// <summary>
    /// ����Ʈ ��ҽ� ó���� ����
    /// </summary>
    /// <param name="cancelQuest">����� ����Ʈ</param>
    public void CancelQuest(Gyu_QuestBaseData cancelQuest) 
    {
        if (currentQuests.Contains(cancelQuest))
        {
            cancelQuest.Quest_state = Quest_State.None;
            currentQuests.Remove(cancelQuest);
            return;
        }
        Debug.LogWarning($"����� ����Ʈ {cancelQuest} �� ĳ���Ͱ� �����������ʽ��ϴ�.");
    }

    /// <summary>
    /// ����Ʈ �Ϸ�� ������ó���Լ�
    /// </summary>
    /// <param name="clearQuest">�Ϸ��� ����Ʈ</param>
    public void ClearQuest(Gyu_QuestBaseData clearQuest)
    {
        if (currentQuests.Contains(clearQuest))
        {
            clearQuest.Quest_state = Quest_State.Quest_Complete;
            clearQuestList.Add(clearQuest);
            currentQuests.Remove(clearQuest);
            RewardDataSetting(clearQuest);
            return;
        }
        Debug.LogWarning($"Ŭ������ ����Ʈ�� : {clearQuest} , ĳ���Ͱ� �������ִ�����Ʈ�� �ƴմϴ�.");
    }
    /// <summary>
    /// ����ó���� �Լ�
    /// </summary>
    /// <param name="clearData">Ŭ������ ����Ʈ</param>
    private void RewardDataSetting(Gyu_QuestBaseData clearData)
    {
        // ����ó�������� ĳ���� �κ��丮�� �������ؼ� ó���ؾ��Ѵ�.
    }
    public void ResetData() 
    {
        currentQuests.Clear();
        clearQuestList.Clear();
    }

    public void SetDataParsing() 
    {
        
    }


}
