using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// �̺�Ʈ���γ����� �۾��ؾ��ҵ�
/// </summary>
public class GameOptionsWindow : GameOptionsBase, IPointerClickHandler
{
   
    /// <summary>
    /// ���� ��ũ��Ʈ ������Ʈ ���Թ� ���� ������Ʈ�� Ŭ�� ����ġ�� ������ �ڵ鷯
    /// </summary>
    /// <param name="eventData">���콺Ŭ�� �߻���ġ������ �����������Ե��ִ�.</param>

    //���������¾ʴ� ���콺�� ���̵��ÿ�������
    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //��ũ��Ʈ�� �� ������Ʈ �ڽ��� Ŭ���� ��ġ�� ã�� �ڵ鷯 
        Debug.Log(eventData.pointerEnter);
        //�ݱ� ��ưŬ���� ������ ��Ȱ��ȭ
        if (eventData.pointerEnter.name.Equals("CloseButton")) // ������ ���ٹ���� ������ ���ٴ�.
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //������ ������ ��������� 
            //�ǽð��ϰ�� ���⿡ �÷������� ������ ����δ� ��ɵ��ʿ���.
        }
    }

}
