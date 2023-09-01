using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 배틀맵에서 최종적으로 컨트롤 할 컴포넌트  
/// 턴메니저에서 해당 컴포넌트를 관리할 계획
/// </summary>
public class BattleMap_Player_Controller : MonoBehaviour
{

    /// <summary>
    /// 레이케스트에서 충돌 처리할 레이어 
    /// 이동 체크를 위해 사용
    /// </summary>
    int layer_Ray_Tile;

    /// <summary>
    /// 레이가 최대로 체크할 거리
    /// </summary>
    [SerializeField]
    float ray_Range = 15.0f;

    /// <summary>
    /// 키입력 처리가져오기
    /// </summary>
    InputKeyMouse inputSystem;

    /// <summary>
    /// 이동가능한 타일 클릭시 신호를넘겨준다
    /// </summary>
    public Action<Tile> onMoveActive;
    /// <summary>
    /// 타일에 몬스터가 있고 클릭했을때 신호를 넘겨준다
    /// </summary>
    public Action<Tile> onClickMonster;
    /// <summary>
    /// 타일에 아이템이 있고 클릭햇을때 신호를 넘겨준다
    /// </summary>
    public Action<Tile> onClickItem;

    private void Awake()
    {
        inputSystem = new();
        layer_Ray_Tile = LayerMask.GetMask("Ground");
    }

    private void OnEnable()
    {
        inputSystem.BattleMap_Player.Enable();
        inputSystem.BattleMap_Player.UnitMove.performed += OnMove;
    }


    private void OnDisable()
    {
        inputSystem.BattleMap_Player.UnitMove.performed -= OnMove;
        inputSystem.BattleMap_Player.Disable();

    }
    
    /// <summary>
    /// 클릭했을때 타일인지 체크하는 로직 
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());      // 화면에서 현재 마우스의 위치로 쏘는 빛
        Debug.DrawRay(ray.origin, ray.direction * ray_Range, Color.red, 1.0f);              // 디버그용 레이저

        if (Physics.Raycast(ray, out RaycastHit hitInfo, ray_Range, layer_Ray_Tile))       // 만약 빛이 부딪히고 타일이면
        {
            Tile targetTile = hitInfo.transform.GetComponent<Tile>();
            if (targetTile != null) //타일이 클릭 됬을경우 
            {
                switch (targetTile.ExistType) //타일 상태확인하고 
                {
                    case Tile.TileExistType.None:
                        break;
                    case Tile.TileExistType.Monster:
                        onClickMonster?.Invoke(targetTile);
                        //몬스터 클릭시 몬스터에대한 정보가 나오던 뭔가 액션이필요
                        Debug.Log($"이동불가 몬스터: 레이타겟{hitInfo.transform.name} , 위치 : {hitInfo.transform.position}");
                        break;
                    case Tile.TileExistType.Item:
                        onClickItem?.Invoke(targetTile);
                        // 아이템이 타일에있는경우 아이템 에대한 정보를 띄우던 뭔가을 액션 
                        break;
                    case Tile.TileExistType.Prop:
                        Debug.Log($"이동불가 장애물 : 레이타겟{hitInfo.transform.name} , 위치 : {hitInfo.transform.position}");
                        break;
                    case Tile.TileExistType.Move:
                        Debug.Log(targetTile);
                        onMoveActive?.Invoke(targetTile);//이동로직 실행
                        Debug.Log($"이동가능 : 레이타겟{hitInfo.transform.name} , 위치 : {hitInfo.transform.position}");
                        break;
                    default:
                        Debug.Log($"접근되면 안된다.");
                        break;
                }
            }
        }
    }


}
  
