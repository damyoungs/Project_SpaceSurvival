using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    public string questName;    // 퀘스트 이름
    public int[] NpdId;         // Npc id

    // 생성자
    public QuestData(string name, int[] npcid)
    {
        questName = name;
        NpdId = npcid;
    }
}
