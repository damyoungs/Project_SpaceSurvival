using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoClickTest : MonoBehaviour
{
    /// <summary>
    /// 화면을 찍는 카메라
    /// </summary>
    Camera mainCamera;
    /// <summary>
    /// 찍은 위치로 이동할 캐릭터 속도
    /// </summary>
    float speed = 4.0f;
    /// <summary>
    /// 화면에서 쏘는 빛이 부딪히는 박스콜라이더
    /// </summary>
    BoxCollider target = null;
    /// <summary>
    /// 인풋시스템 클릭
    /// </summary>
    InputKeyMouse inputClick;


    private void Awake()
    {
        inputClick = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputClick.Mouse.Enable();
        inputClick.Mouse.MouseClick.performed += onClick;
        inputClick.Player.Enable();
        inputClick.Player.LeftRotate.performed += onLeftRotate;
        inputClick.Player.RightRotate.performed += onRightRotate;
    }

    private void OnDisable()
    {
        inputClick.Player.RightRotate.performed -= onRightRotate;
        inputClick.Player.LeftRotate.performed -= onLeftRotate;
        inputClick.Player.Disable();
        inputClick.Mouse.MouseClick.performed -= onClick;
        inputClick.Mouse.Disable();
    }

    /// <summary>
    /// 마우스가 클릭했을 시 일어날 함수
    /// </summary>
    /// <param name="context"></param>
    private void onClick(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());      // 화면에서 현재 마우스의 위치로 쏘는 빛
        Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);              // 디버그용 레이저

        if (Physics.Raycast(ray, out RaycastHit hitInfo))                       // 만약 빛이 부딪히고
        {
            if (hitInfo.transform.gameObject.CompareTag("Tile"))                // 태그 "타일"과 충돌하면
            {
                target = (BoxCollider)hitInfo.collider;                         // 타겟의 박스콜라이더 반환
                Tile tile = target.gameObject.GetComponent<Tile>();             // 아래의 디버그를 위한 타일 반환(디버그 안 할시 없어도 됨)
                //Debug.Log($"타일 위치 : {tile.Width}, {tile.Length}");
            }
        }
    }

    private void onLeftRotate(InputAction.CallbackContext _)
    {
        Quaternion rotate = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(-90.0f, Vector3.up), 1.0f);

        transform.rotation *= rotate;
        
    }

    private void onRightRotate(InputAction.CallbackContext _)
    {
        Quaternion rotate = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90.0f, Vector3.up), 1.0f);

        transform.rotation *= rotate;
    }

    private void Start()
    {
        mainCamera = Camera.main;           //  찍고 있는 카메라 가져오기
    }

    private void FixedUpdate()
    {
        // 타겟이 널포인트가 아니고 타겟이 도착하지 않았을 시 이동
        if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.01f)
        {
            transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
        }
    }
}
