using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData_Gyu : MonoBehaviour
{
    /// <summary>
    /// ��ųʸ� int Ű���� ����ũ�ؾߵǸ�  string���߹迭�� ó�� ���� 
    /// �ε����� 0~ ������������ ����ũ�ϰ� ���õǴ� �ش����̳���.
    /// �����͸� ����¿뵵�� ���� �̷������� �Ѵ�.
    /// �ƴϸ� �ܺ� �ؽ�Ʈ �о �Ľ��۾��ؾ���.
    /// ����Ƽ���� scriptable �������� �װ� ����ص��ǰ�.
    /// </summary>
    string[][] talkData = new string[][]
    {
        new string[]{"1000Npc �亯1","1000Npc �亯1" },
        new string[]{"2000NPC �亯2","2000NPC �亯2" },
        new string[]{"3000NPC �亯3","3000NPC �亯3" }
    };
    //List<string[]> talkData = new List<string[]>(); // �����̸����� ����Ʈ�� ����Ѵ� ���� ������ ����ϴ�.
    //Dictionary<int, string[]> talkData = new Dictionary<int, string[]>(); // ��ȭ�����Ͱ� ���� ����� �ؽ����̺�(��ųʸ�) ������.

    /// <summary>
    /// ����� ��ȭ��� ��������
    /// </summary>
    /// <param name="id">���Ǿ� id </param>
    /// <returns>�ε��� ��ġ�� ����� ��ȭ����� String�迭</returns>
    public string[] GetTalk(int id)
    {
        return talkData[id];
    }
}
