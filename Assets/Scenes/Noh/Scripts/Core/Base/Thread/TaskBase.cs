using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TaskBase : MonoBehaviour
{
    /*
     Task �迭 ���� �� �ʱ�ȭ 
     Task ������Ƽ �����ؼ� �Լ��� ���� �ִ´�.
     ������ Ȯ�� 
     ������ �б�
     ������ �ݱ�
     ������ ����
     ������ ����
     */
    [SerializeField]
    int threadSize = 4;

    int threadIndex = 0; //ť���� ������ ���� 

    Task[] file_IOThread;
    Queue<Task> file_IOQueue;
    public Task File_IOThread {//�ܺο��� Task ���ٰ� �Լ��� �����Ҷ� ��� 
        set 
        {
            if (file_IOThread != null) { 
                if (threadIndex < threadSize) {  
                    file_IOThread[threadIndex] = value;
                    file_IOQueue.Enqueue(value);
                    threadIndex++;
                }

            }
        } 
    } 



    private void Awake()
    {
        file_IOThread = new Task[threadSize]; //������ �̸� �Ҵ��ϱ�
        file_IOQueue = new Queue<Task>(threadSize); //ť�� �̸� ����
    }




}
