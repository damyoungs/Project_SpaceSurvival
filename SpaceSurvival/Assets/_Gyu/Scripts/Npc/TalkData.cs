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
        talkData.Add(1000, new string[] { "�ȳ� ���� �ܰ����̶�� �� \n������ �ݰ���!!!" });
        talkData.Add(2000, new string[] { "���õ��� �ʹ� ����. \n������ �ÿ�������" });

    }

    public string GetTalk(int id)
    {
        return talkData[id][0];
    }
}
