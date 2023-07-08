using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TaskBase : MonoBehaviour
{
    /*
     Task 배열 생성 및 초기화 
     Task 프로퍼티 접근해서 함수를 집어 넣는다.
     파일을 확인 
     파일을 읽기
     파일을 닫기
     파일을 쓰기
     파일을 삭제
     */
    [SerializeField]
    int threadSize = 4;

    int threadIndex = 0; //큐에서 관리할 순번 

    Task[] file_IOThread;
    Queue<Task> file_IOQueue;
    public Task File_IOThread {//외부에서 Task 에다가 함수를 연결할때 사용 
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
        file_IOThread = new Task[threadSize]; //쓰레드 미리 할당하기
        file_IOQueue = new Queue<Task>(threadSize); //큐도 미리 생성
    }




}
