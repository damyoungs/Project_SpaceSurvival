using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tracking_UI_Test : TestBase
{
    [SerializeField]
    GameObject obj;
    //[SerializeField]
    //CinemachineVirtualCamera camera;

    [SerializeField]
    Camera mainCamera;
    /// <summary>
    /// �Ÿ� 10 ���� 1����    20�Ÿ�   0.5����    �Ÿ� 5 = 2��
    /// </summary>
    private void Start()
    {
        //mainCamera = Camera.main;
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        Debug.Log(mainCamera.WorldToScreenPoint(obj.transform.position));
        Debug.Log(mainCamera.ScreenToWorldPoint(obj.transform.position));
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
    }


}
