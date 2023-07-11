
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 팝업 창 여러개일때 클릭한 순서대로 화면 앞쪽으로 보일수있게 체크하는 이벤트 
/// </summary>
public interface IPopupSortWindow 
{
    public GameObject gameObject { get; }
    public Action<IPopupSortWindow> PopupSorting { set; }
}
