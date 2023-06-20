using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ConsumeBase
{
    //아이템 베이스 클레스 , 인벤토리 
    //종류별 데이터 게임 시작시 , 개별데이터 동적 할당 //  @습득, 클릭시  아이탬 타입에 따른 동작, 소비, 장비드롭,
    //초기화 타이밍 게임 시작시 각 아이템의 데이터 초기화
    // 아이탬 습득시 list 생성 초기화 -> UI업데이트 -> 사용시 UI업데이트 -> 콘솔창에 표시 
    //무엇을 이용해서 만들어야하는가 struct, list
    // 상속문제
    // 코드 콘솔창으로 테스트
    // 리스트
    protected override void InitializeValue()
    {
        ItemType = ItemType.Consume;
        hpValue = 50;
        mpValue = 0;
        darkForceValue = 0;
        fatigueValue = 0;
    }
    private void Start()
    {
        PrintValue();
    }
    void PrintValue()
    {
        Debug.Log($"{ ItemType} \n {hpValue}\n {mpValue}\n {darkForceValue}\n {fatigueValue}");
    }
}
