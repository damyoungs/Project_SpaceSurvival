using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ������Ʈ���� ���� id(int), isNpc(Bool)�� ������ �� �ΰ����� ��ũ������(��ȭ), ����Ʈ�����Ϳ� �ִ� ��ȭ������ ��Ī�Ͽ� ȭ�� �����
public class GameMaster : MonoBehaviour
{
    public QuestMaster questMaster;
    public TalkMaster talkMaster;
    public int talkIndex;

    /// <summary>
    /// ��ȭâ Panel
    /// </summary>
    public GameObject TalkPanel;

    /// <summary>
    /// �̸�â.Text
    /// </summary>
    public TextMeshProUGUI NameBox;
    
    /// <summary>
    /// ��ȭâ.Text
    /// </summary>
    public TextMeshProUGUI TalkBox;

    /// <summary>
    /// ����Ʈâ.Text
    /// </summary>
    public TextMeshProUGUI QuestBox;

    /// <summary>
    /// ��ȭ�� ������ ������Ʈ
    /// </summary>
    public GameObject ObjectName;

    /// <summary>
    /// ��ȭâ�� ����Ǵ��� Ȯ��
    /// </summary>
    public bool isAction;

    /// <summary>
    /// �̸��� �����ϴ� �ӽ� ����
    /// </summary>
    private string NameTag;

    /// <summary>
    /// ��ȭ�� �����Ű�� �Լ�
    /// </summary>
    /// <param name="objName">�Է¹��� ������Ʈ</param>
    public void Action(GameObject objName)  // �Է¹��� ������Ʈ�� ȭ�鿡 ���
    {
        ObjectName = objName;
        NameTag = objName.name;
        ObjData objData = ObjectName.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        TalkPanel.SetActive(isAction);
    }

    /// <summary>
    /// �����͸� �޾Ƽ� Panel�� �����ִ� �Լ�
    /// </summary>
    /// <param name="id">��ȭ ��ȣ</param>
    /// <param name="isNpc">Npc ����</param>
    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = questMaster.GetQuestTalkIndex(id); // �Է¹��� ������Ʈ�� id�� ����Ʈ �������� id�� ��ȯ
        string talkData = talkMaster.GetTalk(id+questTalkIndex, talkIndex); // ������Ʈ id�� ����Ʈ id�� ���Ͽ� ������Ʈ�� ���� ����Ʈ�� ��ȯ

        if (talkData == null)   // ������ ��ȭ�� ������ �ʱ�ȭ
        {
            isAction = false;
            talkIndex = 0;
            QuestBox.text = questMaster.CheckQuest(id);
            NameBox.text = null;
            TalkBox.text = null;
            return;
        }
        if (isNpc)  // Npc�� ���
        {
            NameBox.text = NameTag;
            StartCoroutine(Typing(talkData));
        }
        else  // �繰�� ���
        {
            NameBox.text = NameTag;
            StartCoroutine(Typing(talkData));
            //TalkBox.text = talkData;
        }

        isAction = true;
        talkIndex++;
    }

    /// <summary>
    /// Ÿ���� ȿ��(0.05�ʸ��� �ؽ�Ʈ �ڽ��� �ѱ��ھ� �߰����ִ� �ڷ�ƾ)
    /// </summary>
    /// <param name="text">��ȭ����</param>
    /// <returns>�ڷ�ƾ ���</returns>
    IEnumerator Typing(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            TalkBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
