using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMaster : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    /// <summary>
    /// ��Ī�� ����Ʈ �迭<����Ʈid, ����Ʈ ����>
    /// </summary>
    Dictionary<int, QuestData> questList;

    /// <summary>
    /// ����Ʈ ���࿡ �ʿ��� ������ ������Ʈ
    /// </summary>
    public GameObject[] questObject;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData> ();
        GenerateQuest();
    }

    // ����Ʈ ���� �߰�
    void GenerateQuest()
    {
        // questList.Add(����Ʈ id, newQuestData("����Ʈ ����, new int[] { ����Ʈ�� �����ؾ� �ϴ� ����(���� �������� id, ���� ����� id) }"));
        questList.Add(10, new QuestData("�Ҿ���� �ڽ��� ã�ƶ�", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("���ڰ� �����ִ�!", new int[] { 2000, 300 }));
        
    }


    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    
    public int GetQuestTalkIndex(int id)    // id�� �޾� ����Ʈ ��ȣ�� ��ȯ�ϴ� �Լ�
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
