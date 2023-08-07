using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
///  배틀맵 카메라 이동 클래스
/// </summary>
public class Camera_Move : MonoBehaviour
{
    /// <summary>
    /// 캐릭터 따라다니는 기본카메라
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera originCam;
    /// <summary>
    /// 화면 밖을 마우스 올렸을때 이동할 카메라
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera moveCam;
    /// <summary>
    /// 화면밖으로 벗어날때 위치값을 잡아줄 브레인 카메라
    /// </summary>
    [SerializeField]
    CinemachineBrain brain;
    public CinemachineBrain Brain { set => brain = value; }
    /// <summary>
    ///  화면이동할때 우선순위 가져오기위해 높은값을 지정
    /// </summary>
    readonly int viewIndex = 1000;
    /// <summary>
    /// 화면 이동끝났을때 우선순위 반환할 낮은값 지정
    /// </summary>
    readonly int closeIndex = 0;

    /// <summary>
    /// 카메라 이동속도 
    /// </summary>
    [SerializeField]
    float moveSpeed = 10.0f;

    /// <summary>
    /// 카메라 이동감지 받아올 델리게이트
    /// </summary>
    public Action<Vector2> moveCamera;

    [SerializeField]
    /// <summary>
    /// 이동값 방향 담을 백터
    /// </summary>
    Vector3 tempMoveDir = Vector3.zero;

    private void Awake()
    {
        //위에 카메라 가져오는거 여기서 가져오기 
        //나중에 카메라 픽스되면 여기에 찾는로직추가
        moveCamera += OnMove;
    }
    private void OnEnable()
    {
        OnInitPos();
    }
    public void OnInitPos() 
    {
        moveCam.transform.position = originCam.transform.position; //일단 위치 기본카메라로
        moveCam.transform.rotation = originCam.transform.rotation; //카메라이동시 이상하지않게 시작점잡기
    
    }
    /// <summary>
    /// 카메라 이동 관련 값셋팅
    /// </summary>
    /// <param name="dir"></param>
    private void OnMove(Vector2 dir)
    {
        // 회전기준이되는 카메라의 회전값을 가져온다
        float angle = originCam.transform.eulerAngles.y;

        //Debug.Log(tempMoveDir);
        if (angle > -1.0f && angle < 1.0f) //0도 회전
        {
            Debug.Log("0도");
            tempMoveDir.x = dir.x;
            tempMoveDir.z = dir.y;
        }
        else
        if (angle > 89.0f && angle < 91.0f) //90도 회전
        {

            tempMoveDir.x = dir.y;
            tempMoveDir.z = -dir.x;
        }
        else
        if (angle > 179.0f && angle < 181.0f) //180도 회전
        {

            tempMoveDir.x = -dir.x;
            tempMoveDir.z = -dir.y;
            Debug.Log("180도");
        }
        else
        if (angle > 269.0f && angle < 271.0f) //270도 회전
        {
            tempMoveDir.x = -dir.y;
            tempMoveDir.z = dir.x;
        }
        //회전이되면 이동방향이 틀어짐으로 조절이필요 

        //Debug.Log(dir);
        if (dir == Vector2.zero) //화면 이동끝난것을 zero 로 체크중
        {
            moveCam.Priority = closeIndex; //메인카메라로 우선순위넘기기
        }
        else //화면 이동중이면 
        {
            moveCam.Priority = viewIndex; //우선순위 가져오기
        }

        //기본적으로 같이 움직이기때문에 화면밖으로 벗어나는것을 방지하기위해 브레인을 따라다니게 셋팅한다.
        moveCam.transform.position = brain.transform.position;

    }
    //이동및 회전 셋팅
    private void Update()
    {
        moveCam.transform.rotation = originCam.transform.rotation;
        moveCam.transform.position += Time.deltaTime * moveSpeed * tempMoveDir;
    }

}
