using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 세이브 데이터만 따로 처리할수있게 로직변경
/// </summary>
public class SaveDataIsPool : ObjectIsPool
{
    private void Awake()
    {

        isPositionReset = false; //활성화시 로컬포지션 로테이션 초기화를하지않는다.
        
    }
    
}
