
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �˾� â �������϶� Ŭ���� ������� ȭ�� �������� ���ϼ��ְ� üũ�ϴ� �̺�Ʈ 
/// </summary>
public interface IPopupSortWindow 
{
    public GameObject gameObject { get; }
    public Action<IPopupSortWindow> PopupSorting { set; }
}
