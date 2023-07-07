using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// 특정 게임오브젝트 밑에 버튼을 생성할때 사용할수있는 스크립트 
/// </summary>
public class ButtonCreateTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    ///  버튼이 여러개 생성되야할때 이넘을 선언 
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
    /// UI이동체크용 
    /// </summary>
    bool isMove = false;
    /// <summary>
    /// UI 화면 감추기 애니메이션
    /// </summary>
    Animator hideAnim;
    /// <summary>
    /// 애니메이션 변수값
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
        Array enumList = Enum.GetValues(typeof(ButtonList)); //이넘에 선언한 리스트목록을 읽어온다
        Action[] buttonClickListener = new Action[enumList.Length];//이넘리스트목록만큼 크기를잡는다

        for (int i = 0; i < enumList.Length; i++)//읽어온리스트목록만큼 돌린다
        {
            switch (enumList.GetValue(i)) //이넘위치를 검색
            {
                case ButtonList.Attack:
                    buttonClickListener[i] += Attack; //원하는함수를 넣는다.
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
        //이건 UI 용  버튼상위오브젝트 크기 설정 
        RectTransform rt = transform.GetChild(0).GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(ButtonCreateBase<ButtonList>.width, ButtonCreateBase<ButtonList>.height * enumList.Length); //이넘만큼 크기잡는다
        ButtonCreateBase<ButtonList>.SettingNoneScriptButton(transform.GetChild(0).GetChild(0), ButtonList.Attack, buttonClickListener); //버튼생성호출
    }

    private void Attack() {
        Debug.Log("Attack 함수실행");
    }
    private void Defence() { 
    
        Debug.Log("Defence 함수실행");
    }
    private void Skill()
    {
        Debug.Log("Skill 함수실행");
    }
    private void Options() 
    {
        Debug.Log("Options 함수실행");
    }
}
