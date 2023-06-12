
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        inputSystem.Mouse.VirtualMouse.performed += OnClickTest;
    }

    private void OnDisable()
    {
        inputSystem.Mouse.VirtualMouse.performed -= OnClickTest;
        inputSystem.Disable();
    }

    private void OnClickTest(InputAction.CallbackContext context)
    {
        Debug.Log(context);

    }

    /// <summary>
    /// Ŭ���� ��ġ�� �����͸� ������������ ���
    /// </summary>
    /// <param name="eventData">Ŭ�������� ���� ����������</param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {

        if (eventData.pointerEnter.CompareTag("Button"))
        {
            if (eventData.pointerEnter.gameObject.name.Equals("NewGame"))
            {
                LoadingScean.LoadScean(EnumList.SceanName.Title);
            }
            else if (eventData.pointerEnter.gameObject.name.Equals("Continue"))
            {

                LoadingScean.LoadScean(EnumList.SceanName.World);

            }
            else if (eventData.pointerEnter.gameObject.name.Equals("Options"))
            {
                //��Ȱ��ȭ�Ȱ� ��ã�� 
                //optionsWindow = GameObject.FindGameObjectWithTag("Window");
                //Debug.Log(optionsWindow);
                //optionsWindow = GameObject.FindWithTag("Window");
                //Debug.Log(optionsWindow);

                //��Ȱ��ȭ�� ������Ʈ�� ã�����ؼ��� Ȱ��ȭ�� �θ� �ʿ��ϰ� �׾ȿ� �־����
                GameObject optionsWindow = GameObject.FindGameObjectWithTag("Window").transform.GetChild(0).gameObject;
                if (optionsWindow != null)
                {
                    optionsWindow.SetActive(true);
                }
            }
            else if (eventData.pointerEnter.gameObject.name.Equals("Exit")) 
            { 
                LoadingScean.LoadScean(EnumList.SceanName.Ending);
            }
        }

    }
}
