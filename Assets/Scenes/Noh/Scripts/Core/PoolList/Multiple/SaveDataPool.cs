using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���嵥����ȭ�鿡 ������ ������Ʈ ����Ŭ����
/// ������ƮǮ�� ���ٰ� ���۾��ϴ°��� �ƴϴ�.
/// </summary>
public class SaveDataPool : MultipleObjectPool<SaveDataIsPool>
{

    /// <summary>
    /// Ǯ�� ������ �θ���ġ�� �ٲٱ����� �߰���
    /// �Լ��� Ǯ�� �ʱ�ȭ �ϰ����� �����۾����ʿ��Ұ�� ����ϸ�ȴ�.
    /// </summary>
    protected override void StartInitialize()
    {
        setPosition = SaveLoadManager.Instance.SaveLoadWindow.transform; //�⺻������ Ǯ�Ʒ��� ���������� ���ϴ¿�����Ʈ�Ʒ��� �����ǰ� �����Ͽ���.
    }


    /// <summary>
    /// ��Ȱ��ȭ�� �θ� �߰��� �⺻������ ��Ȱ��ȭ ���ֱ⶧���� Ȱ��ȭ�۾��� �����Ͽ���.
    /// Ȱ��ȭ�Ҷ� ť���� �����͸� �����־� ����� �����ϱ⶧���� �̸������д�.
    /// ȭ����ȯ�ø��� �߻�
    /// </summary>
    protected override void EndInitialize()
    {
        base.EndInitialize();
        Debug.Log("ȭ����ȯ�Ǹ� override void EndInitialize() �Լ��� �ҷ�����");
        foreach (SaveDataIsPool obj in pool) //ȭ����ȯ�� �ʱ�ȭ�ϰ� �ٽò��������� �߰��Ͽ��� ó�����۽ÿ��� ��Ȱ��ȭ�� �ѹ�������.
        {
            obj.gameObject.SetActive(false);    // ���� ��Ȱ��ȭ
        }

        for (int i = 0; i < poolSize; i++)
        {
            SaveDataIsPool comp = readyQueue.Dequeue();      // �ϳ� ������
            comp.gameObject.SetActive(true);                // Ȱ��ȭ��Ų��
        }
    }


}
