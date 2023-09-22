using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : NpcBase_Gyu
{
   

    protected override void SetGatheringQuest(Gyu_QuestBaseData[] gatheringQuestArray)
    {
        foreach (var item in gatheringQuestArray)
        {
            ownQuestList.Add(item);
        }
    }
    protected override void SetKillcountQuest(Gyu_QuestBaseData[] killcountQuestArray)
    {
        foreach (var item in killcountQuestArray)
        {
            ownQuestList.Add(item);
        }
    }
    protected override void SetMainStoryQuest(Gyu_QuestBaseData[] mainStoryQuestArray)
    {
        foreach (var item in mainStoryQuestArray)
        {
            ownQuestList.Add(item);
        }
    }
}
