using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���콺�� ��ũ�� �ǳ��� ��ġ��Ű�� ī�޶� �̵���Ű�� ����
/// �ó׸ӽſ� 
/// ī�޶� �̵� ���� üũ�� 
/// </summary>
public class MouseEnterEventDriven : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// �̵��� ī�޶� ���� Ŭ����
    /// </summary>
    Camera_Move moveAction;
    /// <summary>
    /// ȭ�� ������ ��ǥ Ȯ�ο� ����
    /// </summary>
    readonly float leftCheck = Screen.width * 0.05f;
    readonly float rightCheck = Screen.width * 0.95f;
    readonly float topCheck = Screen.height * 0.95f;
    readonly float bottomCheck = Screen.height * 0.05f;

    private void Awake()
    {
        moveAction = transform.parent.GetComponent<Camera_Move>();
    }
    /// <summary>
    /// ���콺 �� ȭ�� ���� ��ġ�ϴ��� üũ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 dir = eventData.position;
        if (dir.x > rightCheck)  //������ ��
        {
            dir.x = 1.0f;
        }
        else if (dir.x < leftCheck) //���� ��
        {
            dir.x = -1.0f;
        }
        else  // �̵� �����ƴ� �߰��϶� 
        {
            dir.x = 0.0f;
        }

        if (dir.y > topCheck) //���� ��
        {
            dir.y = 1.0f;
        }
        else if (dir.y < bottomCheck) //�Ʒ��� �� 
        {
            dir.y = -1.0f;
        }
        else  //�̵� �����ƴ� �߰��϶� 
        {
            dir.y = 0.0f;
        }
        //Debug.Log(dir);
        moveAction.moveCamera?.Invoke(dir); //������ ����
    }

    /// <summary>
    /// ���콺 �߰� �϶� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        moveAction.moveCamera?.Invoke(Vector2.zero); //�ʱⰪ ����
    }

}
