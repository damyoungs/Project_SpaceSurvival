using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleShipInitData : MonoBehaviour
{
    Gyu_QuestManager questManager;
    Merchant_Manager merchantManager;

    Cho_PlayerMove player;

    BattleMapPlayerBase PlayerDumy;

    private void Awake()
    {
        player = FindObjectOfType<Cho_PlayerMove>();
    }
    private void Start()
    {
        CharcterMove(SpaceSurvival_GameManager.Instance.ShipStartPos);
        SpaceSurvival_GameManager.Instance.PlayerStartPos = player.transform;
        questManager = FindObjectOfType<Gyu_QuestManager>(true);
        merchantManager = FindObjectOfType<Merchant_Manager>(true);
        questManager.InitDataSetting();
        merchantManager.InitDataSetting();
        InputSystemController.InputSystem.Player.Esc.performed += EscClick; 
        InputSystemController.InputSystem.UI_Inven.InvenKey.performed += OnCursorOn;
        InputSystemController.InputSystem.UI_Inven.StateKey.performed += OnCursorOn;
        InputSystemController.InputSystem.UI_Inven.SkillBox_Open.performed += OnCursorOn;
        InputSystemController.InputSystem.UI_Inven.EquipBox_Open.performed += OnCursorOn;
        InputSystemController.InputSystem.Options.Options.performed += OnCursorOn;
        InputSystemController.InputSystem.Player.Action.performed += OnCursorOn;


        PlayerDumy = (BattleMapPlayerBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL);
        PlayerDumy.transform.position = new Vector3(0.0f,100.0f,0.0f); //안보이게 멀리보낸다
        PlayerDumy.BattleUI.SetInVisibleUI();

        if (SpaceSurvival_GameManager.Instance.IsBattleMapClear) 
        {
            SpaceSurvival_GameManager.Instance.StageClear |= SpaceSurvival_GameManager.Instance.CurrentStage;
            SpaceSurvival_GameManager.Instance.CurrentStage &= StageList.None;
            SpaceSurvival_GameManager.Instance.IsBattleMapClear = false;
            if (SpaceSurvival_GameManager.Instance.StageClear == StageList.All)
            {

                //배틀맵에서 돌아왔을때 
                //전부클리어 됬으면 처리할 내용 
            }
        }

    }

    private void OnCursorOn(InputAction.CallbackContext context) 
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void EscClick(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        questManager.QuestUIManager.initialize();
        merchantManager.NpcTalkController.ResetData();
    }

    private void OnDisable()
    {
        InputSystemController.InputSystem.Player.Action.performed -= OnCursorOn;
        InputSystemController.InputSystem.UI_Inven.EquipBox_Open.performed -= OnCursorOn;
        InputSystemController.InputSystem.UI_Inven.StateKey.performed -= OnCursorOn;
        InputSystemController.InputSystem.UI_Inven.SkillBox_Open.performed -= OnCursorOn;
        InputSystemController.InputSystem.Options.Options.performed -= OnCursorOn;
        InputSystemController.InputSystem.UI_Inven.InvenKey.performed -= OnCursorOn;
        InputSystemController.InputSystem.Player.Esc.performed -= EscClick;
        PlayerDumy.BattleUI.SetVisibleUI();
        PlayerDumy.gameObject.SetActive(false);
    }
    public void CharcterMove(Vector3 startPos) 
    {
        if (startPos != Vector3.zero)
        {
            player.Controller.enabled = false;
            player.transform.position = startPos;
            player.Controller.enabled = true;
        }
    }
    //private void OnEnable()
    //{
    //    InputSystemController.InputSystem.Common.Esc.performed += (_)=> EscOnAction();
    //    InputSystemController.InputSystem.Mouse.MouseClick.performed += (_)=> EscOffAction();
    //}


    //private void OnDestroy()
    //{
    //    InputSystemController.InputSystem.Mouse.MouseClick.performed -= (_)=> EscOffAction();
    //    InputSystemController.InputSystem.Common.Esc.performed -= (_)=> EscOnAction();
    //}
    //private void EscOnAction()
    //{
    //    Cursor.lockState = CursorLockMode.None;
    //}
    //private void EscOffAction()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

}
