using EnumList;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 팝업 처리용 클래스
/// </summary>
public class SaveLoadPopupWindow : Singleton<SaveLoadPopupWindow>
{
    GameObject saveButton;
    GameObject loadButton;
    GameObject copyButton;
    GameObject deleteButton;
    TextMeshProUGUI windowText;

    Transform proccessPopup;
    public Action<int,bool> focusInChangeFunction;

    /// <summary>
    /// false 면 카피활성화 안된것 true 면 활성화
    /// </summary>
    bool copyCheck = false;
    public bool CopyCheck {
        get => copyCheck;
        set { 
            copyCheck = value;
            if (copyCheck)//카피버튼클릭시
            {
                oldIndex = newIndex;// 카피눌렀을시에 기존선택값은 복사할 항목이된다.
                focusInChangeFunction?.Invoke(newIndex, copyCheck); // 카피눌렀을때 포커싱설정
            }
            else // 카피버튼 해제시 
            {
                focusInChangeFunction?.Invoke(oldIndex, copyCheck);// 카피안눌렀을때
            }
        } 
    }
    /// <summary>
    /// 카피전용 복사할 데이터 인덱스값 -1이면 초기값 
    /// </summary>
    int oldIndex = -1;
    public int OldIndex {
        get => oldIndex; 
        set 
        {
            if (copyCheck) //카피로직일때만 값을 셋팅한다 
            {
                oldIndex = value;
            }
        }
    }
    /// <summary>
    /// 저장 삭제 복사 로드 전부 사용되는 인덱스값
    /// </summary>
    int newIndex = -2;
    public int NewIndex { 
        get => newIndex;
        set
        {
            newIndex = value;
            focusInChangeFunction?.Invoke(newIndex, copyCheck);//바뀐값 수정
        }
    }

    /// <summary>
    /// 저장 로드 카피 삭제 버튼눌렀는지 체크 할변수
    /// </summary>
    EnumList.SaveLoadButtonList buttonType;
    public EnumList.SaveLoadButtonList ButtonType {
        get => buttonType;
        set => buttonType = value;
    }

    /// <summary>
    /// 오브젝트 찾기
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        int savePopupIndex = transform.childCount - 1;
        proccessPopup = transform.GetChild(savePopupIndex);
        saveButton =    proccessPopup.GetChild(1).gameObject;
        loadButton =    proccessPopup.GetChild(2).gameObject;
        copyButton =    proccessPopup.GetChild(3).gameObject;
        deleteButton =  proccessPopup.GetChild(4).gameObject;
        windowText =    proccessPopup.GetChild(6).GetComponent<TextMeshProUGUI>();
    }
    /// <summary>
    /// 처리로직 
    /// </summary>
    /// <param name="type">어떤버튼눌렀는지 그에맞는 팝업창 활성화</param>
    public void OpenPopupAction(EnumList.SaveLoadButtonList type)
    {
        if (newIndex > -1) { 
            buttonType = type;
            switch (type) { 
                case EnumList.SaveLoadButtonList.SAVE:
                    saveButton.SetActive(true);
                    break;
                case EnumList.SaveLoadButtonList.LOAD:
                    loadButton.SetActive(true);
                    break;
                case EnumList.SaveLoadButtonList.COPY:
                    copyButton.SetActive(true);
                    break;
                case EnumList.SaveLoadButtonList.DELETE:
                    deleteButton.SetActive(true);
                    break;
            }
            windowText.text = $"{type} 하시겠습니까 ?";
            proccessPopup.gameObject.SetActive(true); //키이벤트 클릭이벤트 막는 창띄우기
        }
    }

    public void AllClosePopup() { 
        
    }
   
}
