using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 저장데이터화면에 보여줄 오브젝트 생산클래스
/// 오브젝트풀에 없다고 동작안하는것은 아니다.
/// </summary>
public class SaveDataPool : MultipleObjectPool<SaveDataIsPool>
{

    /// <summary>
    /// 풀이 생성될 부모위치를 바꾸기위해 추가함
    /// 함수는 풀이 초기화 하가전에 사전작업이필요할경우 사용하면된다.
    /// </summary>
    protected override void StartInitialize()
    {
        setPosition = FindObjectOfType<WindowList>().MainWindow.transform.
                                                    GetChild(0). //ContentParent
                                                    GetChild(0). //Contents
                                                    GetChild(0). //SaveLoadWindow
                                                    GetChild(0). //SaveFileList
                                                    GetChild(0). //Scroll View
                                                    GetChild(0). //Viewport
                                                    GetChild(0);//Content 

    }


    /// <summary>
    /// 비활성화된 부모에 추가시 기본적으로 비활성화 되있기때문에 활성화작업을 진행하였다.
    /// 활성화할때 큐에서 데이터를 꺼내둬야 제대로 동작하기때문에 미리꺼내둔다.
    /// 화면전환시마다 발생
    /// </summary>
    protected override void EndInitialize()
    {
        base.EndInitialize();

        //foreach (SaveDataIsPool obj in pool) //화면전환시 초기화하고 다시꺼내기위해 추가하였다 처음시작시에도 비활성화를 한번더진행.
        //{
        //    obj.gameObject.SetActive(false);    // 전부 비활성화
        //}
        //int queueCount = readyQueue.Count; //큐값이 변동이 있는경우 가져와서 포문을돌린다.
        //for (int i = 0; i < queueCount; i++) //남아있는큐 초기화
        //{
        //    SaveDataIsPool comp = readyQueue.Dequeue();      // 하나 꺼내고
        //    comp.gameObject.SetActive(true);                // 활성화시킨다
        //}


    }


}
