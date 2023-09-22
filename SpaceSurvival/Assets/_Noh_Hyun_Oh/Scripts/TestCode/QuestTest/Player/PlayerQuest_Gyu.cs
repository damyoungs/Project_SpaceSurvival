using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest_Gyu : MonoBehaviour
{
    /// <summary>
    /// 수락할수있는 퀘스트의 갯수 
    /// </summary>
    [SerializeField] 
    int questMaxLength = 10; 
    public int QuestMaxLength => questMaxLength;

    /// <summary>
    /// 캐릭터가 수행중인 퀘스트 목록
    /// </summary>
    List<Gyu_QuestBaseData> currentQuests;
    public List<Gyu_QuestBaseData> CurrentQuests => currentQuests; 

    /// <summary>
    /// 캐릭터가 완료한 퀘스트 목록 
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
