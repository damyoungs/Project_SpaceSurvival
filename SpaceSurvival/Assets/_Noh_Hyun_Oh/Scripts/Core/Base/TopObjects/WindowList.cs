using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 창관련 최상위 오브젝트에 사용
/// 공통적인 외부입력이벤트처리를 여기서 하면 좋을것같다.
/// 로딩창에서 처리안되게 이벤트함수마다 if (!LoadingScean.IsLoading){ } 안에서 만 실행되게 만들어주세요
/// WindowList , DataFactory  오브젝트는 비활성화를 시키지마세요. 내부오브젝트만 시키도록 로직작성부탁드립니다.
/// </summary>
public class WindowList : Singleton<WindowList> {

    /// <summary>
    /// 활성화 된 팝업이있는경우 defencePanel 셋팅
    /// </summary>
    EnumList.PopupList activePopup;
    public EnumList.PopupList ActivePopup{
        get =>activePopup;
        set 
        {
            switch (value) {
                case EnumList.PopupList.SAVE_LOAD_POPUP:
                    defencePanel.gameObject.SetActive(true);
                    break;
                case EnumList.PopupList.NONE:
                    defencePanel.gameObject.SetActive(false);
                    break;
            }
            activePopup = value;
        } 
    }
    /// <summary>
    /// 팝업창이뜨면 디펜스 패널을 띄워 인풋이벤트를 막는다.
    /// </summary>
    DefenceEvent defencePanel;

    /// <summary>
    /// 키입력 이벤트
    /// </summary>
    InputKeyMouse inputKeyEvent;
    public InputKeyMouse InputKeyEvent => inputKeyEvent;
    
    /// <summary>
    /// 관리할 윈도우 중 옵션관련 윈도우
    /// </summary>
    SaveWindowManager mainWindow;
    public SaveWindowManager MainWindow => mainWindow;

    /// <summary>
    /// 인벤토리 오브젝트
    /// </summary>
    InventoryWindow invenWindow;
    public InventoryWindow InvenWindow => invenWindow;

    /// <summary>
    /// 저장 삭제 복사 로드 실행여부 묻는 창 가져오기
    /// </summary>
    SaveLoadPopupWindow saveLoadPopupWindow;
    public SaveLoadPopupWindow IOPopupWindow => saveLoadPopupWindow;

    /// <summary>
    /// 테스트용 옵션 팝업윈도우
    /// </summary>
    OptionsPopupWindow optionsPopupWindow;
    public OptionsPopupWindow OptionsPopupWindow => optionsPopupWindow;


    /// <summary>
    /// 팝업 창 관리해줄 매니저
    /// </summary>
    PopupSortManager popupManager;
    public PopupSortManager PopupSortManager => popupManager;
    
    /// <summary>
    /// 턴 메니저  
    /// </summary>
    TurnManager turnManager;
    public TurnManager TurnManager => turnManager;

    /// <summary>
    /// 턴 게이지 UI 위치
    /// </summary>
    Transform turnGaugeUI;
    public Transform TurnGaugeUI => turnGaugeUI;

    /// <summary>
    /// 윈도우리스트는 항상가지고다니는것이기때문에 여기에서 이벤트처리를 진행.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        inputKeyEvent = new InputKeyMouse();
        //오브젝트 순서 계속바껴서 걍무겁더라도 GetComponentInChildren<Type>(true) 으로 찾아둘란다.. 매번 이거때매 고치기귀찮.
        defencePanel = transform.GetComponentInChildren<DefenceEvent>(true); 
        invenWindow = transform.GetComponentInChildren<InventoryWindow>(true);
        saveLoadPopupWindow = transform.GetComponentInChildren<SaveLoadPopupWindow>(true);
        mainWindow = transform.GetComponentInChildren<SaveWindowManager>(true);
        popupManager = transform.GetComponentInChildren<PopupSortManager>(true);
        optionsPopupWindow = transform.GetComponentInChildren<OptionsPopupWindow>(true);
        turnGaugeUI = transform.GetChild(0).GetChild(transform.GetChild(0).childCount-1);
        turnManager = FindObjectOfType<DataFactory>().transform.GetComponentInChildren<TurnManager>(true);
    }
    private void Start()
    {
        mainWindow.Oninitialize();
    }
    /// <summary>
    /// 키입력및 마우스 입력처리도 추가하자
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        inputKeyEvent.Enable();
        inputKeyEvent.KeyBoard.System.performed += OffPopupWindow; // esc 입력시 순서대로 창닫기 
        inputKeyEvent.KeyBoard.OptionKey.performed += OnOffWindowOption; // 옵션창 및 세이브창 열고닫기
        inputKeyEvent.KeyBoard.InvenKey.performed += OnOffInventory; // 인벤창 테스트용 
        InputKeyEvent.KeyBoard.StateKey.performed += OnOffStateWindow;// 상태창 테스트용
    }   



    /// <summary>
    /// 비활성화 될일이 게임종료될때만되기때문에 이벤트 삭제함수 처리안해도된다.
    /// 다만 싱글톤 생성시 기존생성된것이 Destroy될때 비활성화를 실행하기때문에 오류가날수있다. - 확인완료
    /// 씬이동시 OnEnable함수가 재호출되진않는다 (확인완료)
    /// </summary>
    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    inputKeyEvent.Mouse.MouseClick.performed -= OnLeftClick;
    //    inputKeyEvent.KeyBorad.OptionKey.performed -= OnOffWindowOption;
    //    inputKeyEvent.KeyBorad.InvenKey.performed -= OnOffInventory;
    //    inputKeyEvent.KeyBorad.System.performed -= OnOffWindowOption;
    //    inputKeyEvent.Disable();
    //}



    /// <summary>
    /// 팝업창 버튼을 눌렀을경우 열렸을경우 닫히고 닫혔을경우 열린다.
    /// <param name="target">열릴 팝업창 객체</param>
    /// </summary>
    public void popupOnOff(IPopupSortWindow target) {
        if (target.gameObject.activeSelf)//창이 열려있으면
        {
            popupManager.PopupClose(target) ;//닫고
        }
        else //아닌경우엔
        { 
            popupManager.PopupOpen(target); //연다
        }
    }

    /// <summary>
    /// 메뉴 창 온오프 
    /// </summary>
    /// <param name="context">입력정보</param>
    private void OnOffWindowOption(InputAction.CallbackContext context)
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScean.IsLoading){ 
            if (context.performed)
            {
                popupOnOff(mainWindow);
            }
        }
    }

    /// <summary>
    /// 인벤 창 온오프
    /// </summary>
    /// <param name="context"></param>
    private void OnOffInventory(InputAction.CallbackContext context)
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                popupOnOff(invenWindow);
            }

        }
    }

    /// <summary>
    /// 스텟 창 온오프
    /// </summary>
    /// <param name="context"></param>
    private void OnOffStateWindow(InputAction.CallbackContext context)
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                popupOnOff(optionsPopupWindow);
            }

        }
    }

    private void OffPopupWindow(InputAction.CallbackContext context)
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                popupManager.PopupClose();
            }

        }
    }
}
