using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    public string questName;    // ����Ʈ �̸�
    public int[] NpdId;         // Npc id

    // ������
    public QuestData(string name, int[] npcid)
    {
        questName = name;
        NpdId = npcid;
    }
}
