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
        
        setPosition = FindObjectOfType<WindowList>().MainWindow.transform.
                                                    GetChild(0). //ContentParent
                                                    GetChild(0). //Contents
                                                    GetChild(0). //SaveLoadWindow
                                                    GetChild(1). //PageListAndButton
                                                    GetChild(1); //PageNumber
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
