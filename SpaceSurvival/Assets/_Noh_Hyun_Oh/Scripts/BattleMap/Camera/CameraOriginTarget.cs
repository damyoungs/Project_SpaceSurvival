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
    Transform target;
    public Transform Target 
    {
        set => target = value;
    }
    Vector3 screenHalfPosition = Vector3.zero;
    public Action<Transform> onCameraOriginMove;

    Vector3 offSet = Vector3.zero;
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
        offSet = target.position;
        transform.position = Vector3.Lerp(transform.position,offSet,followSpeed * Time.deltaTime);
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
    /// ȸ�����⿡���� õõ�� ȸ����Ű��
    /// </summary>
    /// <param name="rotateValue">ȸ�� ����� ����(90,-90)</param>
    /// <returns></returns>
    IEnumerator RotateCourutine(float rotateValue) 
    {
        isRotate = true;//ȸ������������ �Էµ��͵� ���¿�
        bool isLeft = rotateValue > 0; //-�� + ��  ���� ������ üũ
        //Debug.Log(transform.rotation.eulerAngles.y);
        float time = transform.rotation.eulerAngles.y; //���۰� ����
        rotateValue += time; //������ ����
        while (CheckValue(ref time, rotateValue, isLeft))//üŷ
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, time, transform.rotation.z);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, rotateValue, transform.rotation.z);
        isRotate = false;//ȸ���������� üũ
        
       
    }

    /// <summary>
    /// ȸ�����⿡���� ȸ�� �ى���� üũ
    /// </summary>
    /// <param name="checkValue">���� ȸ����</param>
    /// <param name="rotateValue">��ǥ ȸ����</param>
    /// <param name="isLeft">����ȸ������ ������ȸ������ üũ�Ұ� - ����� +�� ������</param>
    /// <returns></returns>
    private bool CheckValue(ref float checkValue, float rotateValue, bool isLeft)
    {
        //Debug.Log($"{checkValue}  ====  {rotateValue} ");
        if (isLeft) //+���� ������ ���� +�ؼ� üũ 
        {
            checkValue += Time.deltaTime * rotateSpeed;
            return checkValue < rotateValue; //������ȸ��
        }
        else // - ���� ������ -�ؼ� üũ
        {
            checkValue -= Time.deltaTime * rotateSpeed;
            return checkValue > rotateValue; //����ȸ��
        }
    }
}

