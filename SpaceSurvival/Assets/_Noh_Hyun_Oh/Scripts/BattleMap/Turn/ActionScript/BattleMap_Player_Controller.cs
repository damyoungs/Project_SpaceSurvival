using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��Ʋ�ʿ��� ���������� ��Ʈ�� �� ������Ʈ  
/// �ϸ޴������� �ش� ������Ʈ�� ������ ��ȹ
/// </summary>
public class BattleMap_Player_Controller : MonoBehaviour
{

    /// <summary>
    /// �����ɽ�Ʈ���� �浹 ó���� ���̾� 
    /// �̵� üũ�� ���� ���
    /// </summary>
    int layer_Ray_Tile;

    /// <summary>
    /// ���̰� �ִ�� üũ�� �Ÿ�
    /// </summary>
    [SerializeField]
    float ray_Range = 15.0f;

    /// <summary>
    /// Ű�Է� ó����������
    /// </summary>
    InputKeyMouse inputSystem;

    /// <summary>
    /// �̵������� Ÿ�� Ŭ���� ��ȣ���Ѱ��ش�
    /// </summary>
    public Action<Tile> onMoveActive;
    /// <summary>
    /// Ÿ�Ͽ� ���Ͱ� �ְ� Ŭ�������� ��ȣ�� �Ѱ��ش�
    /// </summary>
    public Action<Tile> onClickMonster;
    /// <summary>
    /// Ÿ�Ͽ� �������� �ְ� Ŭ�������� ��ȣ�� �Ѱ��ش�
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
    /// Ŭ�������� Ÿ������ üũ�ϴ� ���� 
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());      // ȭ�鿡�� ���� ���콺�� ��ġ�� ��� ��
        Debug.DrawRay(ray.origin, ray.direction * ray_Range, Color.red, 1.0f);              // ����׿� ������

        if (Physics.Raycast(ray, out RaycastHit hitInfo, ray_Range, layer_Ray_Tile))       // ���� ���� �ε����� Ÿ���̸�
        {
            Tile targetTile = hitInfo.transform.GetComponent<Tile>();
            if (targetTile != null) //Ÿ���� Ŭ�� ������� 
            {
                switch (targetTile.ExistType) //Ÿ�� ����Ȯ���ϰ� 
                {
                    case Tile.TileExistType.None:
                        break;
                    case Tile.TileExistType.Monster:
                        onClickMonster?.Invoke(targetTile);
                        //���� Ŭ���� ���Ϳ����� ������ ������ ���� �׼����ʿ�
                        Debug.Log($"�̵��Ұ� ����: ����Ÿ��{hitInfo.transform.name} , ��ġ : {hitInfo.transform.position}");
                        break;
                    case Tile.TileExistType.Item:
                        onClickItem?.Invoke(targetTile);
                        // �������� Ÿ�Ͽ��ִ°�� ������ ������ ������ ���� ������ �׼� 
                        break;
                    case Tile.TileExistType.Prop:
                        Debug.Log($"�̵��Ұ� ��ֹ� : ����Ÿ��{hitInfo.transform.name} , ��ġ : {hitInfo.transform.position}");
                        break;
                    case Tile.TileExistType.Move:
                        Debug.Log(targetTile);
                        onMoveActive?.Invoke(targetTile);//�̵����� ����
                        Debug.Log($"�̵����� : ����Ÿ��{hitInfo.transform.name} , ��ġ : {hitInfo.transform.position}");
                        break;
                    default:
                        Debug.Log($"���ٵǸ� �ȵȴ�.");
                        break;
                }
            }
        }
    }


}
  
