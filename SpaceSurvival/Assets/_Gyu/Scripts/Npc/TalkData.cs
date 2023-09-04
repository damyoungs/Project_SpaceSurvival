using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "안녕 나는 외계인이라고 해 \n만나서 반가워!!!" });
        talkData.Add(2000, new string[] { "오늘따라 너무 더워. \n언제쯤 시원해질까" });

    }

    public string GetTalk(int id)
    {
        return talkData[id][0];
    }
}
