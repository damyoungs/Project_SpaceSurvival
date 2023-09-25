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
    /// 관리할 윈도우 중 옵션관련 윈도우
    /// </summary>
    SaveWindowManager mainWindow;
    public SaveWindowManager MainWindow => mainWindow;

    /// <summary>
    /// 인벤토리 오브젝트
    /// </summary>
    Inventory invenWindow;
    public Inventory InvenWindow => invenWindow;

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
    /// 턴 게이지 UI 위치
    /// </summary>
    TurnGaugeOnOff turnGaugeUI;
    public TurnGaugeOnOff TurnGaugeUI => turnGaugeUI;

    /// <summary>
    /// 배틀맵 액션 버튼 
    /// </summary>
    Transform battleActionButtons;
    public Transform BattleActionButtons => battleActionButtons;
    
    /// <summary>
    /// 배틀맵에서 캐릭터 상시정보를 보여줄 컴포넌트 위치
    /// </summary>
    TeamBorderManager teamBorderManager;
    public TeamBorderManager TeamBorderManager => teamBorderManager;
    
    /// <summary>
    /// 퀘스트 매니저
    /// </summary>
    Gyu_QuestManager gyu_QuestManager;
    public Gyu_QuestManager Gyu_QuestManager => gyu_QuestManager;

    /// <summary>
    /// 윈도우리스트는 항상가지고다니는것이기때문에 여기에서 이벤트처리를 진행.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        //오브젝트 순서 계속바껴서 걍무겁더라도 GetComponentInChildren<Type>(true) 으로 찾아둘란다.. 매번 이거때매 고치기귀찮.
        defencePanel = transform.GetComponentInChildren<DefenceEvent>(true);
        invenWindow = transform.GetComponentInChildren<Inventory>(true);
        saveLoadPopupWindow = transform.GetComponentInChildren<SaveLoadPopupWindow>(true);
        mainWindow = transform.GetComponentInChildren<SaveWindowManager>(true);
        popupManager = transform.GetComponentInChildren<PopupSortManager>(true);
        optionsPopupWindow = transform.GetComponentInChildren<OptionsPopupWindow>(true);
        turnGaugeUI = transform.GetComponentInChildren<TurnGaugeOnOff>(true);
        teamBorderManager = transform.GetComponentInChildren<TeamBorderManager>(true);
        gyu_QuestManager = transform.GetComponentInChildren<Gyu_QuestManager>(true);
        battleActionButtons = transform.GetChild(0).GetChild(1); //나중에 수정필요 

    }
    private void Start()
    {
        InputSystemController.Instance.On_Options_Options += OnOffWindowOption; //옵션창 열고 닫기 
        InputSystemController.Instance.On_Common_Esc += OffPopupWindow;          //키입력시 순서대로 닫히게만들기
        
        //InputSystemController.Instance.OnInput_Action_NoneGame_MouseClick+= ()=> { }; //뭘연결시킬까나.   
        //InputSystemController.Instance.OnInput_Action_NoneGame_Esc+= ()=> { };        //오프닝에사용할건데 바꿀수도있음  
        
        mainWindow.Oninitialize();
    }

    
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
    private void OnOffWindowOption()
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScene.IsLoading){ 
            popupOnOff(mainWindow);
        }
    }

    /// <summary>
    /// 인벤 창 온오프
    /// </summary>
    /// <param name="context"></param>
    //private void OnOffInventory(InputAction.CallbackContext context)
    //{
    //    //씬로딩이아닌경우만 실행한다. 
    //    if (!LoadingScean.IsLoading)
    //    {
    //        if (context.performed)
    //        {
    //            invenWindow.Open_Inventory();
    //        }

    //    }
    //}

    /// <summary>
    /// 스텟 창 온오프
    /// </summary>
    /// <param name="context"></param>
    //private void OnOffStateWindow(InputAction.CallbackContext context)
    private void OnOffStateWindow()
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScene.IsLoading)
        {
            popupOnOff(optionsPopupWindow);

        }
    }

    private void OffPopupWindow()
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScene.IsLoading)
        {
            popupManager.PopupClose();
        }
    }
}
