
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
/// 참고용 테스트코드
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
    /// 클릭한 위치의 데이터를 가져오기위해 사용
    /// </summary>
    /// <param name="eventData">클릭지점에 대한 데이터정보</param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {



        if (eventData.pointerEnter.CompareTag("TitleButton"))
        {
            if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.NEWGAME.ToString()))
            {
              
            }
            else if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.CONTINUE.ToString()))
            {

                LoadingScean.SceanLoading(EnumList.SceanName.SAVELOADTEST);

            }
            else if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.OPTIONS.ToString()))
            {
                GameObject optionsWindow = GameObject.FindGameObjectWithTag("WindowList"). //활성화 오브젝트
                    transform.GetChild(0).gameObject; // 비활성화 오브젝트 접근

                if (optionsWindow != null)
                {
                    optionsWindow.SetActive(true);
                }
            }
            else if (eventData.pointerEnter.gameObject.name.Equals(EnumList.TitleMenu.EXIT.ToString())) 
            { 
                LoadingScean.SceanLoading(EnumList.SceanName.ENDING);
            }
        }

    }
}

                //비활성화된것 못찾음 
                //optionsWindow = GameObject.FindGameObjectWithTag("Window");
                //Debug.Log(optionsWindow);
                //optionsWindow = GameObject.FindWithTag("Window");
                //Debug.Log(optionsWindow);

                //비활성화된 오브젝트를 찾기위해서는 활성화된 부모가 필요하고 그안에 넣어야함
                //GameObject optionsWindow = GameObject.FindGameObjectWithTag("Window").transform.GetChild(0).gameObject;