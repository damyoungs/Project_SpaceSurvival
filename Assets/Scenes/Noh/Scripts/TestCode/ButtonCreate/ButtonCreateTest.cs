using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Ư�� ���ӿ�����Ʈ �ؿ� ��ư�� �����Ҷ� ����Ҽ��ִ� ��ũ��Ʈ 
/// </summary>
public class ButtonCreateTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    ///  ��ư�� ������ �����Ǿ��Ҷ� �̳��� ���� 
    /// </summary>
    enum ButtonList {
        Attack = 0,
        Defence,
        Skill,
        Options,
        attack,
        defence,
        skill,
        options
    }
    /// <summary>
    /// UI�̵�üũ�� 
    /// </summary>
    bool isMove = false;
    /// <summary>
    /// UI ȭ�� ���߱� �ִϸ��̼�
    /// </summary>
    Animator hideAnim;
    /// <summary>
    /// �ִϸ��̼� ������
    /// </summary>
    int str_OnViewButton;
    private void Awake()
    {

        str_OnViewButton = Animator.StringToHash("OnViewButton");

        hideAnim = transform.GetComponent<Animator>();

        Initialize();

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMove)
        {
            hideAnim.SetBool(str_OnViewButton, true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideAnim.SetBool(str_OnViewButton, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMove = true;
        transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMove = false;
    }

    private void Initialize() {
        Array enumList = Enum.GetValues(typeof(ButtonList)); //�̳ѿ� ������ ����Ʈ����� �о�´�
        Action[] buttonClickListener = new Action[enumList.Length];//�̳Ѹ���Ʈ��ϸ�ŭ ũ�⸦��´�

        for (int i = 0; i < enumList.Length; i++)//�о�¸���Ʈ��ϸ�ŭ ������
        {
            switch (enumList.GetValue(i)) //�̳���ġ�� �˻�
            {
                case ButtonList.Attack:
                    buttonClickListener[i] += Attack; //���ϴ��Լ��� �ִ´�.
                    break;
                case ButtonList.Defence:
                    buttonClickListener[i] += Defence;
                    break;
                case ButtonList.Skill:
                    buttonClickListener[i] += Skill;
                    break;
                case ButtonList.Options:
                    buttonClickListener[i] += Options;
                    break;
                case ButtonList.attack:
                    buttonClickListener[i] += Attack;
                    break;
                case ButtonList.defence:
                    buttonClickListener[i] += Defence;
                    break;
                case ButtonList.skill:
                    buttonClickListener[i] += Skill;
                    break;
                case ButtonList.options:
                    buttonClickListener[i] += Options;
                    break;
                default:
                    break;
            }
        }
        //�̰� UI ��  ��ư����������Ʈ ũ�� ���� 
        RectTransform rt = transform.GetChild(0).GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(ButtonCreateBase<ButtonList>.width, ButtonCreateBase<ButtonList>.height * enumList.Length); //�̳Ѹ�ŭ ũ����´�
        ButtonCreateBase<ButtonList>.SettingNoneScriptButton(transform.GetChild(0).GetChild(0), ButtonList.Attack, buttonClickListener); //��ư����ȣ��
    }

    private void Attack() {
        Debug.Log("Attack �Լ�����");
    }
    private void Defence() { 
    
        Debug.Log("Defence �Լ�����");
    }
    private void Skill()
    {
        Debug.Log("Skill �Լ�����");
    }
    private void Options() 
    {
        Debug.Log("Options �Լ�����");
    }
}
