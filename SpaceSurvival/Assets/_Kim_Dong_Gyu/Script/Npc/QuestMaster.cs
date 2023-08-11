using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMaster : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    /// <summary>
    /// 매칭할 퀘스트 배열<퀘스트id, 퀘스트 내용>
    /// </summary>
    Dictionary<int, QuestData> questList;

    /// <summary>
    /// 퀘스트 진행에 필요한 아이템 오브젝트
    /// </summary>
    public GameObject[] questObject;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData> ();
        GenerateQuest();
    }

    // 퀘스트 제목 추가
    void GenerateQuest()
    {
        // questList.Add(퀘스트 id, newQuestData("퀘스트 제목, new int[] { 퀘스트를 진행해야 하는 순서(현재 진행중인 id, 다음 진행될 id) }"));
        questList.Add(10, new QuestData("잃어버린 박스를 찾아라", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("상자가 저기있다!", new int[] { 2000, 300 }));
        
    }


    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    
    public int GetQuestTalkIndex(int id)    // id를 받아 퀘스트 번호를 반환하는 함수
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        if(id == questList[questId].NpdId[questActionIndex])
        {
            questActionIndex++;
        }

        ControlObject();

        if(questActionIndex == questList[questId].NpdId.Length)
        {
            NextQuest();
        }


        return questList[questId].questName;
    }

    void ControlObject()
    {
        switch(questId)
        {
            case 10:
                if(questActionIndex == 2)
                {
                    questObject[0].SetActive(true);
                }
                break;

            case 20:
                if(questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                }
                break;
        }
    }
}
