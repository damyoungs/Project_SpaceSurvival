using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI �������� ��ũ��Ʈ���� 
/// </summary>
public class BattleActionButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //��ư�� �����ܿ� �ϳ��ۿ��������
    Button bt;
    //��Ʈ�ѷ��� �ϳ�������
    protected BattleActionUIController uiController;
    protected virtual void Awake()
    {
        Transform parent = transform.parent;
        uiController = parent.GetComponentInChildren<BattleActionUIController>(true);
        bt = GetComponentInChildren<Button>(true);
        bt.onClick.AddListener(OnClick);
    }
    protected virtual void OnClick() 
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    protected virtual void OnMouseEnter()
    {
        
    }
    protected virtual void OnMouseExit() 
    {
    
    }

}
