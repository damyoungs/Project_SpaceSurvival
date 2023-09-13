using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// ���콺�� ȭ�� �����ڸ��� ��� ���ϴ��������� ������
/// </summary>
public enum Screen_Side_Mouse_Direction : byte
{
    None = 0,
    Left = 1,
    Right = 2,
    Top = 4,
    Bottom = 8,
}

/// <summary>
///  ��Ʋ��  �ó׸ӽ� ����� ī�޶� �̵� Ŭ����
/// </summary>
public class Camera_Move : MonoBehaviour
{
    /// <summary>
    /// ĳ���� ����ٴϴ� �⺻ī�޶�
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera originCam;
    /// <summary>
    /// ȭ�� ���� ���콺 �÷����� �̵��� ī�޶�
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera moveCam;
    /// <summary>
    /// ȭ������� ����� ��ġ���� ����� �극�� ī�޶�
    /// </summary>
    [SerializeField]
    CinemachineBrain brain;
    public CinemachineBrain Brain 
    {
        get 
        {
            if (brain == null) //ȣ��������� ������ 
            {
                brain = GetCineBrainCam?.Invoke(); //�ѹ� ã�´� .
            }
            return brain;
        }
    }
    /// <summary>
    /// �극�� ī�޶� ã�ƿ��� ��������Ʈ 
    /// �ʱ�ȭ ���۳�Ʈ InitCharcterSetting ���� ������
    /// </summary>
    public Func<CinemachineBrain> GetCineBrainCam;
    /// <summary>
    ///  ȭ���̵��Ҷ� �켱���� ������������ �������� ����
    /// </summary>
    readonly int viewIndex = 1000;
    /// <summary>
    /// ȭ�� �̵��������� �켱���� ��ȯ�� ������ ����
    /// </summary>
    readonly int closeIndex = 0;

    /// <summary>
    /// ī�޶� �̵��ӵ� 
    /// </summary>
    [SerializeField]
    float moveSpeed = 10.0f;

    /// <summary>
    /// ī�޶��� Ÿ���� ȸ�� ������ ��������
    /// </summary>
    [SerializeField]
    Vector3 originTargetDefaultForward;

    /// <summary>
    /// ī�޶��̵��� ��ũ���� ���Ʒ� �¿� ���� ���� �޴µ� x y ��ǥ�� �޴°���  
    /// ȸ�������� ���������� �� �ϱ� ���� ���̴�. 
    /// </summary>
    float originRotateValue ; 

    /// <summary>
    /// ī�޶� �̵����� �޾ƿ� ��������Ʈ
    /// </summary>
    public Action<Screen_Side_Mouse_Direction> moveCamera;

    [SerializeField]
    /// <summary>
    /// �̵��� ���� ���� ����
    /// </summary>
    Vector3 tempMoveDir = Vector3.zero;

    private void Awake()
    {
        //���� ī�޶� �������°� ���⼭ �������� 
        //���߿� ī�޶� �Ƚ��Ǹ� ���⿡ ã�·����߰�
        moveCamera += OnMove;

        //Follow �������� awake ������ Vcam �� ����ε� ȸ������ ������ �ȵǱ⶧���̴�
        originTargetDefaultForward = originCam.Follow.forward;

        originRotateValue = originCam.Follow.eulerAngles.y; //���� ȸ���� ��ġ�� �����صα� ���߿� üũ������ �ʿ� 
    }
    private void OnEnable()
    {
        OnInitPos();
    }
    public void OnInitPos() 
    {
        moveCam.transform.position = originCam.transform.position; //�ϴ� ��ġ �⺻ī�޶��
        moveCam.transform.rotation = originCam.transform.rotation; //ī�޶��̵��� �̻������ʰ� ���������
    
    }
    /// <summary>
    /// ī�޶� �̵� ���� ������
    /// </summary>
    /// <param name="dir">���콺�� ��ġ�� ��ũ�� ����</param>
    private void OnMove(Screen_Side_Mouse_Direction dir)
    {
        // ȸ�������̵Ǵ� ī�޶��� ȸ������ �����´�
        float angle = originCam.transform.eulerAngles.y; //Vcam ����

        switch (dir)
        {
            case Screen_Side_Mouse_Direction.None:
                tempMoveDir = Vector3.zero;
                moveCam.Priority = closeIndex; //����ī�޶�� �켱�����ѱ��
                break;
            case Screen_Side_Mouse_Direction.Left:
                moveCam.Priority = viewIndex; //�켱���� ��������
                break;
            case Screen_Side_Mouse_Direction.Right:
                moveCam.Priority = viewIndex; //�켱���� ��������
                break;
            case Screen_Side_Mouse_Direction.Top:
                moveCam.Priority = viewIndex; //�켱���� ��������
                break;
            case Screen_Side_Mouse_Direction.Bottom:
                moveCam.Priority = viewIndex; //�켱���� ��������
                break;
            default:
                break;
        }

        if (originRotateValue > 0)
        {
            //tempMoveDir.x -= (tempMoveDir.x * originTargetDefaultForward.x);
            //tempMoveDir.z -= (tempMoveDir.z * originTargetDefaultForward.z);
        }
        
        moveCam.transform.position = Brain.transform.position;
        
        
        //ī�޶� ȸ���̵Ǹ� �̵������� Ʋ�������� ȸ��üũ�ؼ� �̵��� ����

        //if (angle > originRotateValue - 1.0f && angle < originRotateValue + 1.0f) //0�� ȸ��
        //{
        //    //Debug.Log("0��");
        //    tempMoveDir.x = dir.x;
        //    tempMoveDir.z = dir.y;
        //}
        //else if (angle > originRotateValue + 89.0f && angle < originRotateValue + 91.0f) //90�� ȸ��
        //{

        //    tempMoveDir.x = dir.y;
        //    tempMoveDir.z = -dir.x;
        //}
        //else if (angle > originRotateValue + 179.0f && angle < originRotateValue + 181.0f) //180�� ȸ��
        //{

        //    tempMoveDir.x = -dir.x;
        //    tempMoveDir.z = -dir.y;
        //    //Debug.Log("180��");
        //}
        //else if (angle > originRotateValue + 269.0f && angle < originRotateValue + 271.0f) //270�� ȸ��
        //{
        //    tempMoveDir.x = -dir.y;
        //    tempMoveDir.z = dir.x;
        //}
     

        //Debug.Log(dir);
       

        //�⺻������ ���� �����̱⶧���� ȭ������� ����°��� �����ϱ����� �극���� ����ٴϰ� �����Ѵ�.

    }
    //�̵��� ȸ�� ����
    private void Update()
    {
        moveCam.transform.rotation = originCam.transform.rotation;
        moveCam.transform.position += Time.deltaTime * moveSpeed * tempMoveDir;
    }

}
