using System.Collections;
using System.Collections.Generic;
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
                case EnumList.PopupList.SAVELOADPOPUP:
                    defencePanel.SetActive(true);
                    break;
                case EnumList.PopupList.NONE:
                    defencePanel.SetActive(false);
                    break;
            }
            activePopup = value;
        } 
    }
    /// <summary>
    /// 팝업창이뜨면 디펜스 패널을 띄워 인풋이벤트를 막는다.
    /// </summary>
    GameObject defencePanel;

    /// <summary>
    /// 키입력 이벤트
    /// </summary>
    InputKeyMouse inputKeyEvent;
    public InputKeyMouse InputKeyEvent => inputKeyEvent;
    /// <summary>
    /// 관리할 윈도우 중 옵션관련 윈도우
    /// </summary>
    GameObject optionsWindow;
    public GameObject OptionsWindow => optionsWindow;
    /// <summary>
    /// 윈도우리스트는 항상가지고다니는것이기때문에 여기에서 이벤트처리를 진행.
    /// </summary>

    protected override void Awake()
    {
        base.Awake();
        inputKeyEvent = new InputKeyMouse();
        defencePanel = transform.GetComponentInChildren<DefenceEvent>(true).gameObject; //이건순서가 제일 마지막이래야되서 계속바뀐다.
        optionsWindow = transform.GetChild(0).gameObject; //옵션윈도우는 0번째다
    }
    /// <summary>
    /// 키입력및 마우스 입력처리도 추가하자
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        inputKeyEvent.Enable();
        inputKeyEvent.KeyBorad.System.performed += OnOffWindowOption; //키입력시 옵션창 온오프
        inputKeyEvent.KeyBorad.OptionKey.performed += OnOffWindowOption; // 위에것과 동일
        inputKeyEvent.KeyBorad.InvenKey.performed += OnOffInventory; // 아직 인벤창을 안만듬 
        inputKeyEvent.Mouse.MouseClick.performed += OnLeftClick; //화면에서 클릭했을때 처리할 이벤트 

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
    /// 메뉴 창 온오프 
    /// </summary>
    /// <param name="context">입력정보</param>
    protected virtual void OnOffWindowOption(InputAction.CallbackContext context)
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScean.IsLoading){ 
            if (context.performed)
            {
                optionsWindow.SetActive(!optionsWindow.activeSelf);//옵션윈도우 열고 닫고 
                
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
                Debug.Log("인벤창로직");

            }

        }
    }
    /// <summary>
    /// 화면클릭시 처리할이벤트
    /// 제약이많을거같다 클릭한오브젝트정보를 가져올수없어서 처리가 난감하다.
    /// 필요없을시 삭제
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnLeftClick(InputAction.CallbackContext context)
    {
        //씬로딩이아닌경우만 실행한다. 
        if (!LoadingScean.IsLoading)
        {
            if (context.performed)
            {
                //Debug.Log("클릭했지롱");

            }

        }
    }

}
