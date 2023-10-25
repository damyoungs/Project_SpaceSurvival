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

    MapTest map;
    List<Tile> path = null;

    Tile currentPos = null;
    public Tile CurrentPos
    {
        get => currentPos;
        set
        {
            if (currentPos != value)
            {
                if (currentPos != null)
                {
                    currentPos.ExistType = Tile.TileExistType.None;
                }
                else
                {
                    transform.position = value.transform.position;
                }
                currentPos = value;
                currentPos.ExistType = Tile.TileExistType.Monster;
            }
        }
    }

    private void Awake()
    {
        inputClick = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputClick.BattleMap_Player.Enable();
        inputClick.BattleMap_Player.UnitMove.performed += onClick;

        map = FindObjectOfType<MapTest>();
        path = new List<Tile>();
    }

    private void OnDisable()
    {
        inputClick.BattleMap_Player.UnitMove.performed -= onClick;
        inputClick.BattleMap_Player.Disable();

        if (path != null )
            path.Clear();
        path = null;
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
                Tile tile = target.gameObject.GetComponent<Tile>();
                if (tile.ExistType == Tile.TileExistType.None)
                {
                    path = AStar.PathFind(map, CurrentPos, tile);
                }
                //CurrentPos = tile;
                Debug.Log($"타일 위치 : {tile.Width}, {tile.Length}");
            }
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;           //  찍고 있는 카메라 가져오기
    }

    private void FixedUpdate()
    {
        if (path.Count > 0 )
        {
            Tile destPath = path[0];
        
            Vector3 dir = destPath.transform.position - transform.position;
        
            if (dir.sqrMagnitude < 0.01f )
            {
                transform.position = destPath.transform.position;
                CurrentPos = destPath;
                path.RemoveAt(0);
            }
            else
            {
                transform.Translate(Time.fixedDeltaTime * speed * dir.normalized);
            }
        }

    }
}
