using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase_Gyu : MonoBehaviour
{
    /// <summary>
    /// ���Ǿ��� ������ �ε���
    /// </summary>
    public int Npcid;

    /// <summary>
    /// ���� �������� ����Ʈ
    /// </summary>
    Quest_Gyu currentQuest;
    public Quest_Gyu currentQuestGyu
    {
        get => currentQuest;
        set
        {
            if (currentQuest != value)
            {
                currentQuest = value;

            }
        }
    }

    /// <summary>
    /// �̿��Ǿ��� �������ִ� ����Ʈ ���� ��ũ���ͺ�� �־��ش�.
    /// </summary>
    [SerializeField]
    Quest_Gyu[] questArray;

    /// <summary>
    /// ���Ǿ��� ���� ��ȭ����
    /// </summary>
    QuestManager_Gyu.TalkType talkType;
    public QuestManager_Gyu.TalkType TalkType => talkType;

    /// <summary>
    /// ��ó���ͼ� ��ȭ�����ϵ��� �����Ű�� ��������Ʈ
    /// </summary>
    public Action<int> onTalkEnableButton;

    /// <summary>
    /// �ֺ��� ����� ��ȣ���� ��������Ʈ
    /// </summary>
    public Action onTalkDisableButton;

    public void SettingQuest(int index) 
    {
        currentQuest = new Quest_Gyu(index);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onTalkEnableButton?.Invoke(Npcid);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onTalkDisableButton?.Invoke();
        }
    }
}
