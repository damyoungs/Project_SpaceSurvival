
using System;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// �˾� â �������϶� Ŭ���� ������� ȭ�� �������� ���ϼ��ְ� üũ�ϴ� �̺�Ʈ 
/// </summary>
public interface IPopupSortWindow
{

    /// <summary>
    /// �������̽��� �ִ� ������ ������ ��ӹ��������� �����ؾ��ϳ� 
    /// GameObject gameObject { get;}  �� ���´� Component ���� �̹� ������ �Ǿ��־ 
    /// �������̽� ��ӹ��� Ŭ�������� MonoBehaviour �� ��ӹ޾��ִ� �����̸� �߰� ������ ���ص� ������ �ȳ���.
    /// �̸��� �������̽��� �Լ��� �ٸ� ���Ŭ���� �Լ��͵� �����Ҽ��� �ִٴ°��̴�.
    /// </summary>
    public GameObject gameObject { get; }
    /// <summary>
    /// �˾�â Ŭ���� ��ȣ�� �޾ƿ������� �߰� IPointerDownHandler ��ӹް� �޼ҵ忡 �����ϴ��ڵ带�������ֽø�˴ϴ�.
    /// </summary>
    public Action<IPopupSortWindow> PopupSorting { set; }
}