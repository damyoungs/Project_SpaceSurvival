using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 정렬이 필요한 클래스는 이인터페이스를 상속받아서 SortValue 값을 셋팅한다.
/// </summary>
public interface ISortBase 
{
    /// <summary>
    /// 정렬될 기준값
    /// </summary>
    public float SortValue { get; set; }

}
