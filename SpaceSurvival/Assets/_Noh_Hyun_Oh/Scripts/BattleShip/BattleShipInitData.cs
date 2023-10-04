using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShipInitData : MonoBehaviour
{
    Gyu_QuestManager questManager;
    Merchant_Manager merchantManager;

    private void Start()
    {
        questManager = FindObjectOfType<Gyu_QuestManager>(true);
        merchantManager = FindObjectOfType<Merchant_Manager>(true);
        questManager.InitDataSetting();
        merchantManager.InitDataSetting();
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
