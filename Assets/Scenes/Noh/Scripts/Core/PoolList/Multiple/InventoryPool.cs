using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPool : MultipleObjectPool<InventoryObjectIsPool>
{
    
    protected override void StartInitialize()
    {
        setPosition = WindowList.Instance.InvenWindow.transform. //�κ� �˾�âã�� 
            GetChild(0).// ��������
            GetChild(0).// Base �˾� Contents ������Ʈ ã��
            GetChild(0).// ��ũ�Ѻ信 �����ϱ����� ��������
            GetChild(0).// ����Ʈ ������ ��������
            GetChild(0);// ������ ������Ʈ�� ã�ư���.. ���. 
        base.StartInitialize();

    }

    /// <summary>
    /// ��Ȱ��ȭ�� �θ� �߰��� �⺻������ ��Ȱ��ȭ ���ֱ⶧���� Ȱ��ȭ�۾��� �����Ͽ���.
    /// Ȱ��ȭ�Ҷ� ť���� �����͸� �����־� ����� �����ϱ⶧���� �̸������д�.
    /// ȭ����ȯ�ø��� �߻�
    /// </summary>
    protected override void EndInitialize()
    {
        base.EndInitialize();

        //foreach (InventoryObjectIsPool obj in pool) //ȭ����ȯ�� �ʱ�ȭ�ϰ� �ٽò��������� �߰��Ͽ��� ó�����۽ÿ��� ��Ȱ��ȭ�� �ѹ�������.
        //{
        //    obj.gameObject.SetActive(false);    // ���� ��Ȱ��ȭ
        //}
        //int queueCount = readyQueue.Count; //ť���� ������ �ִ°�� �����ͼ� ������������.
        //for (int i = 0; i < queueCount; i++) //�����ִ�ť �ʱ�ȭ
        //{
        //    InventoryObjectIsPool comp = readyQueue.Dequeue();      // �ϳ� ������
        //    comp.gameObject.SetActive(true);                // Ȱ��ȭ��Ų��
        //}


    }


}
