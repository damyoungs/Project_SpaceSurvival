using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOriginTarget : MonoBehaviour
{
    /// <summary>
    /// 키입력 처리할 것들
    /// </summary>
    InputKeyMouse inputAction;
    /// <summary>
    /// 회전속도
    /// </summary>
    [SerializeField]
    float rotateSpeed = 100.0f;

    /// <summary>
    /// 키입력 막기용
    /// </summary>
    bool isRotate = false;

    /// <summary>
    ///  테스트용  해당씬에서만 쓸지 아님 다른곳에서도 쓸지 결정한뒤 접근방법을 바꿀예정
    /// </summary>
    [SerializeField]
    Transform target;
    public Transform Target 
    {
        set => target = value;
    }

    /// <summary>
    /// 화면사이즈(픽셀단위)의 중간 위치값 가져오기 
    /// </summary>
    Vector3 screenHalfPosition = Vector3.zero;

    /// <summary>
    /// 신호받으면 움직이는 코루틴 연결할때 사용 할 델리게이트
    /// </summary>
    //public Action<Transform> onCameraOriginMove;

    /// <summary>
    /// 카메라 이동속도
    /// </summary>
    [SerializeField]
    float followSpeed = 3.0f;
    
    private void Awake()
    {
        inputAction = new();
        screenHalfPosition.x = Screen.width * 0.5f;
        screenHalfPosition.z = 0.0f;
        screenHalfPosition.y = Screen.height * 0.5f;
    }

    private void LateUpdate()
    {
        ///문제가 있을거같지만 일단 동작은 잘하네..?
        transform.position = Vector3.Lerp(transform.position,target.position, followSpeed * Time.deltaTime); // 시작위치가 항상바뀜으로 시간누적 뺏다.
        //transform.Translate(target.transform.position ,Space.World);
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

