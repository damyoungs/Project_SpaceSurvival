using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShipInitData : MonoBehaviour
{
    Gyu_QuestManager questManager;
    Merchant_Manager merchantManager;

    Cho_PlayerMove player;

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
