using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOptionsWindow : GameOptionsBase, IPointerClickHandler
{
   
    /// <summary>
    /// ���� ��ũ��Ʈ ������Ʈ ���Թ� ���� ������Ʈ�� Ŭ�� ����ġ�� ������ �ڵ鷯
    /// </summary>
    /// <param name="eventData">���콺Ŭ�� �߻���ġ������ �����������Ե��ִ�.</param>

    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //��ũ��Ʈ�� �� ������Ʈ �ڽ��� Ŭ���� ��ġ�� ã�� �ڵ鷯 
        Debug.Log(eventData.pointerEnter);
        //�ݱ� ��ưŬ���� ������ ��Ȱ��ȭ
        if (eventData.pointerEnter.name.Equals("CloseButton")) // ������ ���ٹ���� ������ ���ٴ�.
        {
            gameObject.SetActive(false);
            //������ ������ ��������� 
            //�ǽð��ϰ�� ���⿡ �÷������� ������ ����δ� ��ɵ��ʿ���.
        }
    }

}
