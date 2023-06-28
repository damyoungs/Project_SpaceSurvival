using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePageButtonPool : MultipleObjectPool<SavePageButtonIsPool>
{
    /// <summary>
    /// Ǯ�� ������ �θ���ġ�� �ٲٱ����� �߰���
    /// �Լ��� Ǯ�� �ʱ�ȭ �ϰ����� �����۾����ʿ��Ұ�� ����ϸ�ȴ�.
    /// </summary>
    protected override void StartInitialize()
    {
        setPosition = SaveLoadManager.Instance.SaveLoadPagingWindow.transform; //�⺻������ Ǯ�Ʒ��� ���������� ���ϴ¿�����Ʈ�Ʒ��� �����ǰ� �����Ͽ���.
    }


    /// <summary>
    /// ��Ȱ��ȭ�� �θ� �߰��� �⺻������ ��Ȱ��ȭ ���ֱ⶧���� Ȱ��ȭ�۾��� �����Ͽ���.
    /// Ȱ��ȭ�Ҷ� ť���� �����͸� �����־� ����� �����ϱ⶧���� �̸������д�.
    /// ȭ����ȯ�ø��� �߻�
    /// </summary>
    protected override void EndInitialize()
    {
        base.EndInitialize();

        foreach (SavePageButtonIsPool obj in pool) //ȭ����ȯ�� �ʱ�ȭ�ϰ� �ٽò��������� �߰��Ͽ��� ó�����۽ÿ��� ��Ȱ��ȭ�� �ѹ�������.
        {
            obj.gameObject.SetActive(false);    // ���� ��Ȱ��ȭ
        }
        int queueCount = readyQueue.Count; //ť���� ������ �ִ°�� �����ͼ� ������������.
        for (int i = 0; i < queueCount; i++) //�����ִ�ť �ʱ�ȭ
        {
            SavePageButtonIsPool comp = readyQueue.Dequeue();      // �ϳ� ������
            comp.gameObject.SetActive(true);                // Ȱ��ȭ��Ų��
        }


    }
}
