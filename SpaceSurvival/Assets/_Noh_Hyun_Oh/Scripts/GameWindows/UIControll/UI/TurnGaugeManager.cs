using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
/// <summary>
/// 턴게이지 에보일 유닛의 추가 및 삭제 관련해서 관리해줄 메니저 
/// </summary>
public class TurnGaugeManager : MonoBehaviour
{
    /// <summary>
    /// 턴게이지 관리용 리스트
    /// </summary>
    List<GaugeUnit> gaugeList;

    /// <summary>
    /// 턴게이지 위치
    /// </summary>
    Transform turnGaugeParent;

    private void Awake()
    {
        turnGaugeParent = transform.GetChild(0).GetChild(0);
        gaugeList = new List<GaugeUnit>(); 
    }
    
    /// <summary>
    /// 턴메니저에서 관리할수있게 델리게이트에 함수등록
    /// </summary>
    private void Start()
    {
        WindowList.Instance.TurnManager.addTurnObjectGauge += UITurnUnitAdd; //턴 유닛 추가시 호출할 함수 등록
        WindowList.Instance.TurnManager.removeTurnObjectGauge += UITurnUnitDelete; //턴 유닛 삭제시 호출할 함수 등록
    }
   
    /// <summary>
    /// 턴 게이지 유닛 하나 추가
    /// </summary>
    /// <param name="addData">추가될 유닛 데이터</param>
    private void UITurnUnitAdd(ITurnBaseData addData) 
    {
        GameObject obj = MultipleObjectsFactory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL);// 풀에서 데이터 가져온다.

        GaugeUnit gaugeUnit = obj.GetComponent<GaugeUnit>(); //게이지 컴퍼넌트 찾아서
        gaugeUnit.ProgressValue = addData.TurnActionValue; //초기값 셋팅
        gaugeList.Add(gaugeUnit); // 관리용리스트에 추가
        addData.GaugeUnit = gaugeUnit; //턴 유닛에 게이지 유닛 캐싱 
        obj.transform.parent = turnGaugeParent; //부모위치 설정
    }

    /// <summary>
    /// 턴 게이지 유닛 하나 삭제 
    /// </summary>
    /// <param name="deleteData">턴에서 삭제될 유닛데이터</param>
    private void UITurnUnitDelete(ITurnBaseData deleteData) 
    {
        GaugeUnit gaugeUnit = deleteData.GaugeUnit;
        gaugeUnit.ProgressValue = -1.0f; //값 리셋시키고
        gaugeUnit.gameObject.SetActive(false); // 큐로 돌리고 
        gaugeUnit.transform.SetParent(gaugeUnit.PoolTransform); // 풀로 돌린다.
        gaugeList.Remove(gaugeUnit);//관리용 리스트에서 지운다
        deleteData.GaugeUnit = null; // 게이지유닛 참조해제
    }


    /// <summary>
    /// 턴게이지 리스트 초기화시키기
    /// </summary>
    public void ResetTurnGaugeList(LinkedList<ITurnBaseData> turnList) 
    {
        foreach (ITurnBaseData node in turnList)
        {
             UITurnUnitDelete(node); //턴리스트에 있는 내용가지고 정리하기
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
