using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOriginTarget : MonoBehaviour
{
    InputKeyMouse inputAction;
    [SerializeField]
    float rotateSpeed = 100.0f;

    bool isRotate = false;

    [SerializeField]
    GameObject target;
    public GameObject Target 
    {
        set => target = value;
    }
    Vector3 screenHalfPosition = Vector3.zero;

    Camera mainCam;

    private void Awake()
    {
        inputAction = new();
        screenHalfPosition.x = Screen.width * 0.5f;
        screenHalfPosition.z = 0.0f;
        screenHalfPosition.y = Screen.height * 0.5f;
    }
    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        transform.position = target.transform.position;
    }

    private void OnEnable()
    {
        inputAction.Camera.Enable();
        inputAction.Camera.RightRotate.performed += OnRightRotate;
        inputAction.Camera.LeftRotate.performed += OnLeftRotate;
    }
    
    private void OnDisable()
    {
        
        inputAction.Camera.LeftRotate.performed -= OnLeftRotate;
        inputAction.Camera.RightRotate.performed -= OnRightRotate;
        inputAction.Camera.Disable();
    }
    private void OnLeftRotate(InputAction.CallbackContext context)
    {
        if (!isRotate)
        {
            StartCoroutine(RotateCourutine(-90.0f));

        }
    }

    private void OnRightRotate(InputAction.CallbackContext context)
    {
        if (!isRotate)
        {
            StartCoroutine(RotateCourutine(90.0f));

        }
    }
    /// <summary>
    /// 회전방향에따라 천천히 회전시키기
    /// </summary>
    /// <param name="rotateValue">회전 방향및 각도(90,-90)</param>
    /// <returns></returns>
    IEnumerator RotateCourutine(float rotateValue) 
    {
        isRotate = true;//회전끝날때까지 입력들어와도 막는용
        bool isLeft = rotateValue > 0; //-값 + 값  왼쪽 오른쪽 체크
        //Debug.Log(transform.rotation.eulerAngles.y);
        float time = transform.rotation.eulerAngles.y; //시작값 셋팅
        rotateValue += time; //도착값 셋팅
        while (CheckValue(ref time, rotateValue, isLeft))//체킹
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, time, transform.rotation.z);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, rotateValue, transform.rotation.z);
        isRotate = false;//회전끝난것을 체크
        
       
    }

    /// <summary>
    /// 회전방향에따라 회전 다됬는지 체크
    /// </summary>
    /// <param name="checkValue">현재 회전값</param>
    /// <param name="rotateValue">목표 회전값</param>
    /// <param name="isLeft">왼쪽회전인지 오른쪽회전인지 체크할값 - 면왼쪽 +면 오른쪽</param>
    /// <returns></returns>
    private bool CheckValue(ref float checkValue, float rotateValue, bool isLeft)
    {
        //Debug.Log($"{checkValue}  ====  {rotateValue} ");
        if (isLeft) //+값이 들어오면 값을 +해서 체크 
        {
            checkValue += Time.deltaTime * rotateSpeed;
            return checkValue < rotateValue; //오른쪽회전
        }
        else // - 값이 들어오면 -해서 체크
        {
            checkValue -= Time.deltaTime * rotateSpeed;
            return checkValue > rotateValue; //왼쪽회전
        }
    }
}

