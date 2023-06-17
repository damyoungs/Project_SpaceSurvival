
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// ����� �׽�Ʈ�ڵ�
/// </summary>
public class ClickTest : MonoBehaviour , IPointerClickHandler
{
    InputKeyMouse inputSystem;
    
    private void Awake()
    {
        inputSystem = new InputKeyMouse();
       
    }
    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Mouse.MouseClick.performed += OnClickTest;
        inputSystem.Mouse.TestClick.performed += OnClickTest;
    }

    private void OnDisable()
    {
        inputSystem.Mouse.TestClick.performed -= OnClickTest;
        inputSystem.Mouse.MouseClick.performed -= OnClickTest;
        inputSystem.Disable();
    }


    private void OnClickTest(InputAction.CallbackContext context)
    {
       

    }

    /// <summary>
    /// Ŭ���� ��ġ�� �����͸� ������������ ���
    /// </summary>
    /// <param name="eventData">Ŭ�������� ���� ����������</param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {



        if (eventData.pointerEnter.CompareTag("TitleButton"))
        {
            if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.NewGame.ToString()))
            {
              
            }
            else if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.Continue.ToString()))
            {

                LoadingScean.SceanLoading(EnumList.SceanName.World);

            }
            else if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.Options.ToString()))
            {
                GameObject optionsWindow = GameObject.FindGameObjectWithTag("WindowList"). //Ȱ��ȭ ������Ʈ
                    transform.GetChild(0).gameObject; // ��Ȱ��ȭ ������Ʈ ����

                if (optionsWindow != null)
                {
                    optionsWindow.SetActive(true);
                }
            }
            else if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.Exit.ToString())) 
            { 
                LoadingScean.SceanLoading(EnumList.SceanName.Ending);
            }
        }

    }
}

                //��Ȱ��ȭ�Ȱ� ��ã�� 
                //optionsWindow = GameObject.FindGameObjectWithTag("Window");
                //Debug.Log(optionsWindow);
                //optionsWindow = GameObject.FindWithTag("Window");
                //Debug.Log(optionsWindow);

                //��Ȱ��ȭ�� ������Ʈ�� ã�����ؼ��� Ȱ��ȭ�� �θ� �ʿ��ϰ� �׾ȿ� �־����
                //GameObject optionsWindow = GameObject.FindGameObjectWithTag("Window").transform.GetChild(0).gameObject;