
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
    KeyMouseInputSystem inputSystem;
    
    private void Awake()
    {
        inputSystem = new KeyMouseInputSystem();
       
    }
    private void OnEnable()
    {
        inputSystem.Enable();
        //inputSystem.Mouse.MouseClick.performed += OnClickTest;
        //inputSystem.Mouse.TestClick.performed += OnClickTest;
    }

    private void OnDisable()
    {
        //inputSystem.Mouse.TestClick.performed -= OnClickTest;
        //inputSystem.Mouse.MouseClick.performed -= OnClickTest;
        inputSystem.Disable();
    }


    string filePath = "Assets/ImportPack/SoundFiles";
    string fileName = "Piano Instrumental 1.wav";
    private void OnClickTest(InputAction.CallbackContext context)
    {
        string sb = filePath + fileName; 
        //UIBehaviour testVal = context.ReadValue<UIBehaviour>();
        if (File.Exists(sb)) {
            Debug.Log($"{sb} ������ ������");
        }
        //Debug.Log(testVal);

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
                LoadingScean.SceanLoading(EnumList.SceanName.Title);
            }
            else if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.Continue.ToString()))
            {

                LoadingScean.SceanLoading(EnumList.SceanName.Item_Test);

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