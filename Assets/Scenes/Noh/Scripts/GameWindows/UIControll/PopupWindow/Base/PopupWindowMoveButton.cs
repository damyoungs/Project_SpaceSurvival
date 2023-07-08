using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �˾�â �̵��� ȭ������� ����� �ʰ� ó���ϴ� �κ�
/// </summary>
public class PopupWindowMoveButton : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    /// <summary>
    /// ȭ�� ���� ��������
    /// </summary>
    float windowWidth = Screen.width;

    /// <summary>
    /// ȭ�� ���� ��������
    /// </summary>
    float windowHeight = Screen.height;

    /// <summary>
    /// �̵��� ������â ��ġ�� ��ƮƮ�������� �����´�
    /// </summary>
    private RectTransform parentWindow;

    /// <summary>
    /// �巡�׽�����ġ������ ����
    /// </summary>
    private Vector2 startPosition;

    /// <summary>
    /// �巡�׽��۽� �̺�Ʈ��ġ���� ���� ����
    /// </summary>
    private Vector2 movePosition;

    /// <summary>
    /// â�̵��� �ӽ÷� ��ġ��Ƶ� ����
    /// </summary>
    private Vector2 moveOffSet;
    private void Awake()
    {
        parentWindow = transform.parent.parent.GetComponent<RectTransform>();
    }

    /// <summary>
    /// �巡�׽����Ҷ� �ѹ����ߵ� 
    /// </summary>
    /// <param name="eventData">�̺�Ʈ��ġ������</param>
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)//�������̽��ΰ�������ϱ⶧���� ��������� �ۼ�
    {
            startPosition = parentWindow.anchoredPosition;//�巡�׽����Ҷ� ��ġ������
            movePosition = eventData.position; //�巡�׽����Ҷ��� �̵�ó���� �����ǰ� ����
    }

    /// <summary>
    /// �巡�� ������ �϶� ȭ������� ������� üũ�ϰ� �ȹ������ �̵���Ų��
    /// </summary>
    /// <param name="eventData">�̺�Ʈ��ġ������</param>
    void IDragHandler.OnDrag(PointerEventData eventData) //�������̽��ΰ�������ϱ⶧���� ��������� �ۼ�
    {
        
        
        //�̵��� �巡�׸�ŭ ���� �����ش�.
        //�������� ���� �̵��ѰŸ�(�̵��Ѱ����� ó����������)�� ���Ѵ�.
        if (CheckOutOfWindow(eventData)) { //â������ ����� �ʾҴ��� üũ 
            parentWindow.anchoredPosition = moveOffSet;  //����� �ʾ����� �̵���Ų��.
        }
    }

    /// <summary>
    /// ����ȭ�鿡�� ����� �ʾҴ��� üũ�Ѵ�
    /// </summary>
    /// <returns>ȭ�鿡�� ������� false</returns>
    bool CheckOutOfWindow(PointerEventData eventData)
    {
        moveOffSet = startPosition + (eventData.position - movePosition); //�̵��� ��
        //���ʰ� �� üũ
        if (moveOffSet.x < 0 || moveOffSet.y > 0) { 
            return false;
        }

        //������ üũ
        float x = moveOffSet.x + parentWindow.rect.width; //���������ι���°���üũ�ϱ����� âũ�����ǥ�� ��ģ��
        if (x > windowWidth) { //��ģ���� â���� ũ�� ������� �ƿ�!
            //moveOffSet.x = windowWidth;
            return false; 
        }
         
        //�Ʒ�üũ
        float y = moveOffSet.y - parentWindow.rect.height; // �Ʒ������̱⶧���� ���ϴ°Ծƴ϶� ���ش�. moveOffSet.y�� �׻� �����̴�.
        if (-y > windowHeight) {//�Ʒ������̱⶧���� ���갪�� -���ؼ� ����� �ٲ��ش�.
            //moveOffSet.y = windowHeight;
            return false;
        }

        return true;
    }
}