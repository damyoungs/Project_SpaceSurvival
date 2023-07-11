using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPool : MultipleObjectPool<InventoryObjectIsPool>
{
    
    protected override void StartInitialize()
    {
        setPosition = WindowList.Instance.InvenWindow.transform. //인벤 팝업창찾고 
            GetChild(0).// 내려가고
            GetChild(0).// Base 팝업 Contents 오브젝트 찾고
            GetChild(0).// 스크롤뷰에 접근하기위해 내려가고
            GetChild(0).// 뷰포트 밑으로 내려가서
            GetChild(0);// 컨텐츠 오브젝트를 찾아간다.. 길다. 
        base.StartInitialize();

    }

    /// <summary>
    /// 비활성화된 부모에 추가시 기본적으로 비활성화 되있기때문에 활성화작업을 진행하였다.
    /// 활성화할때 큐에서 데이터를 꺼내둬야 제대로 동작하기때문에 미리꺼내둔다.
    /// 화면전환시마다 발생
    /// </summary>
    protected override void EndInitialize()
    {
        base.EndInitialize();

        //foreach (InventoryObjectIsPool obj in pool) //화면전환시 초기화하고 다시꺼내기위해 추가하였다 처음시작시에도 비활성화를 한번더진행.
        //{
        //    obj.gameObject.SetActive(false);    // 전부 비활성화
        //}
        //int queueCount = readyQueue.Count; //큐값이 변동이 있는경우 가져와서 포문을돌린다.
        //for (int i = 0; i < queueCount; i++) //남아있는큐 초기화
        //{
        //    InventoryObjectIsPool comp = readyQueue.Dequeue();      // 하나 꺼내고
        //    comp.gameObject.SetActive(true);                // 활성화시킨다
        //}


    }


}
