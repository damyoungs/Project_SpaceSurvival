using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkMaster : MonoBehaviour
{
    /// <summary>
    /// ��Ī�� ��ȭ �迭 <����id, ��ȭ ����>
    /// </summary>
    Dictionary<int, string[]> talkData;


    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    // ��ȭ ���� �߰�
    void GenerateData()     // talkData.Add(���� id, new string[]{"��ȭ ����", "�� ���� ��ȭ ����"});
    {
        // talkData.Add(���� id, new string[]{"��ȭ ����", "�� ���� ��ȭ ����"});
        talkData.Add(1000, new string[] { "��! ���ο� �������ΰ�", "\n������ �ݰ���!! �� �ܰ����̶�� �ϳ�" });
        talkData.Add(2000, new string[] { "�����", " ������ �ʿ���..." });
        talkData.Add(100, new string[] { "�̰� �׸� ����", "\n���� �׸� ����� ����"});


        // talkData.Add(���� id + ����Ʈ id + ���� ����Ʈ�� ������ index, new string[]{"����Ʈ ����"});
        talkData.Add(1000 + 10, new string[] { "���� �̾��ѵ�.. ���� ã���ִ°� �ְŵ�..", "\nȤ�� �����ټ� ������?" });
        talkData.Add(2000 + 10 + 1, new string[] { "����?", "\n���ڶ�� ���� �����ִ°� ���ϴ°Ű�����?" });
    }

    /// <summary>
    /// ��ȭ ������ ���������� ������ ��ȯ ��
    /// </summary>
    /// <param name="id">���� id</param>
    /// <param name="talkIndex">��ȭ�� ���� ã�� �ε���</param>
    /// <returns>��ȯ�� ����</returns>
    public string GetTalk(int id, int talkIndex)
    {
        if(talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
