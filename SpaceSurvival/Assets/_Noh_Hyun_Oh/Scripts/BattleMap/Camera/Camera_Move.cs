using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
///  ��Ʋ�� ī�޶� �̵� Ŭ����
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
    public CinemachineBrain Brain { set => brain = value; }
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
    /// ī�޶� �̵����� �޾ƿ� ��������Ʈ
    /// </summary>
    public Action<Vector2> moveCamera;

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
    /// <param name="dir"></param>
    private void OnMove(Vector2 dir)
    {
        // ȸ�������̵Ǵ� ī�޶��� ȸ������ �����´�
        float angle = originCam.transform.eulerAngles.y;

        //Debug.Log(tempMoveDir);
        if (angle > -1.0f && angle < 1.0f) //0�� ȸ��
        {
            Debug.Log("0��");
            tempMoveDir.x = dir.x;
            tempMoveDir.z = dir.y;
        }
        else
        if (angle > 89.0f && angle < 91.0f) //90�� ȸ��
        {

            tempMoveDir.x = dir.y;
            tempMoveDir.z = -dir.x;
        }
        else
        if (angle > 179.0f && angle < 181.0f) //180�� ȸ��
        {

            tempMoveDir.x = -dir.x;
            tempMoveDir.z = -dir.y;
            Debug.Log("180��");
        }
        else
        if (angle > 269.0f && angle < 271.0f) //270�� ȸ��
        {
            tempMoveDir.x = -dir.y;
            tempMoveDir.z = dir.x;
        }
        //ȸ���̵Ǹ� �̵������� Ʋ�������� �������ʿ� 

        //Debug.Log(dir);
        if (dir == Vector2.zero) //ȭ�� �̵��������� zero �� üũ��
        {
            moveCam.Priority = closeIndex; //����ī�޶�� �켱�����ѱ��
        }
        else //ȭ�� �̵����̸� 
        {
            moveCam.Priority = viewIndex; //�켱���� ��������
        }

        //�⺻������ ���� �����̱⶧���� ȭ������� ����°��� �����ϱ����� �극���� ����ٴϰ� �����Ѵ�.
        moveCam.transform.position = brain.transform.position;

    }
    //�̵��� ȸ�� ����
    private void Update()
    {
        moveCam.transform.rotation = originCam.transform.rotation;
        moveCam.transform.position += Time.deltaTime * moveSpeed * tempMoveDir;
    }

}
