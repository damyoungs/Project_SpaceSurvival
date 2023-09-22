using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    List<Gyu_QuestBaseData> currentQuests;
    public List<Gyu_QuestBaseData> CurrentQuests => currentQuests; 

    /// <summary>
    /// ĳ���Ͱ� �Ϸ��� ����Ʈ ��� 
    /// </summary>
    List<Gyu_QuestBaseData> clearQuestList;
    public List<Gyu_QuestBaseData> ClearQuestList => clearQuestList;
    

    private void Awake()
    {
        currentQuests = new List<Gyu_QuestBaseData>(questMaxLength);
        clearQuestList = new List<Gyu_QuestBaseData>(questMaxLength);
    }
    
    public void ResetData() 
    {
        currentQuests.Clear();
        clearQuestList.Clear();
    }

}
