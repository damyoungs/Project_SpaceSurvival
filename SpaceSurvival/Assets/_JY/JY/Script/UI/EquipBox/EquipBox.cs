using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipBox : MonoBehaviour, IPopupSortWindow
{
    EquipBox_Slot[] equipBox_Slots;
    EquipBox_Description description;

    public Action<Transform> on_Pass_Item_Transform;
    public Action<ItemData, ItemData> on_Update_Status_For_EquipOrSwap;
    public Action<ItemData> on_Update_Status_For_UnEquip;
    public EquipBox_Description Description => description;
    public EquipBox_Slot this[EquipType type] => equipBox_Slots[(int) type - 1];//0번째 인덱스 = None 
    public Transform[] equip_Parent_Transform;

    PlayerDummy player;

    CanvasGroup canvasGroup;
    public bool IsOpen => canvasGroup.alpha > 0.9f;

    public Action<IPopupSortWindow> PopupSorting { get ; set ; }

    private void Awake()
    {
        description = GetComponentInChildren<EquipBox_Description>();
        canvasGroup = GetComponent<CanvasGroup>();
        equipBox_Slots = new EquipBox_Slot[4];
        for (int i = 1; i < transform.childCount - 1; i++)
        {
            equipBox_Slots[i - 1] = transform.GetChild(i).GetComponent<EquipBox_Slot>();
            equipBox_Slots[i - 1].onPointerEnter += description.Open;
            equipBox_Slots[i - 1].onPointerMove += description.MovePosition;
            equipBox_Slots[i - 1].onPointerExit += description.Close;
        }
    }

    void Start()
    {
        player = GameManager.playerDummy;
        player.onEquipItem += Set_ItemData_For_DoubleClick;
        GameManager.SlotManager.on_UnEquip_Item += UnEquip_Item;
        Close(); //항상들고다니는데 켜져있으면안되니 스타트마지막에 감춘다 
    }
    public void Set_ItemData_For_Drag(ItemData itemData)// 프리팹 장착 처리중
    {
        //itemdata 가 hat, Weapon, Suit, Jewel 인지 확인하고 슬롯의 타입과 맞지 않으면 리턴시키기
        ItemData_Armor armor = itemData as ItemData_Armor;
        ItemData_Hat hat = itemData as ItemData_Hat;
        ItemData_Craft jewel = itemData as ItemData_Craft;
        EquipBox_Slot slot = FindSlot_By_Position();
        if (slot != null)
        {
            if (armor != null)
            {
                if (slot.equip_Type == EquipType.Body)
                {
                    Transform parent = equip_Parent_Transform[(int)armor.EquipType];

                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;// 장착에 성공할 것이므로 인벤토리의 슬롯 비우기
                    slot.SetItemData(armor);
                }
            }
            else if (hat != null)
            {
                if (slot.equip_Type == EquipType.Hat)
                {
                    Transform parent = equip_Parent_Transform[(int)armor.EquipType];

                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
                    slot.SetItemData(hat);
                }
            }
            else if (jewel != null)
            {
                if (slot.equip_Type == EquipType.Jewel)
                {
                    Transform parent = equip_Parent_Transform[(int)armor.EquipType];

                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
                    slot.SetItemData(jewel);
                }
            }
            else if (itemData.ItemType == ItemType.Equip)
            {
                if (slot.equip_Type == EquipType.Weapon)
                {
                    Transform parent = equip_Parent_Transform[(int)armor.EquipType];

                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
                    slot.SetItemData(itemData);
                }
            }
        }
    }
    //void Attach_Prefab(ItemData data)
    //{
    //    Transform parent = this[]
    //}
    //equipSlot Clear하는 델리게이트 연결할 차례 
    void UnEquip_Item(ItemData itemData)
    {
        on_Update_Status_For_UnEquip?.Invoke(itemData);
        Remove_Prefab(itemData);
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        Set_Edditional_State(itemData, false);//�ִϸ��̼� �� �߰� ����Ʈ ����
        slot.ItemData = null;
    }
    void Remove_Prefab(ItemData data)
    {
        Transform parentTransform = GetParentTransform(data);
        GameObject itemPrefab = parentTransform.GetChild(0).gameObject;
        if (data.code == ItemCode.Space_Armor)
        {
            player.ArmorType_ = PlayerDummy.ArmorType.None;
        }
        else if (data.code == ItemCode.Big_Space_Armor)
        {
            player.ArmorType_ = PlayerDummy.ArmorType.None;
        }
        Destroy(itemPrefab);
    }
    public void Set_ItemData_For_DoubleClick(ItemData itemData)
    {
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        if (slot != null)
        {
            GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
            on_Update_Status_For_EquipOrSwap?.Invoke(slot.ItemData, itemData);//장비중이 아닐 때는 첫번째 파라미터가 null 이 전달 된다. // 플레이어 공격력, 방어력 셋팅
            if (itemData.code == ItemCode.Space_Armor)
            {
                player.ArmorType_ = PlayerDummy.ArmorType.SpaceArmor;// enum 설정시 player 에서 알맞은 갑옷만 활성화하고 다른 갑옷은 비활성화
            }
            else if (itemData.code == ItemCode.Big_Space_Armor)
            {
                player.ArmorType_ = PlayerDummy.ArmorType.BigArmor;
            }
            else
            {
                Attach_Prefab(itemData);//프리팹 부착
            }
            slot.SetItemData(itemData);//장비슬롯 UI업데이트
        }
      //  Set_Edditional_State(itemData, true);//�ִϸ��̼� �� �߰� ����Ʈ ����
    }
    bool Set_Edditional_State(ItemData data, bool equip)
    {
        bool result = false;
        switch (data.code)
        {
            case ItemCode.Enhancable_Pistol:
                if (equip)
                {
                    player.Weapon_Type = PlayerDummy.WeaponType.Pistol;
                }
                else
                {
                    player.Weapon_Type = PlayerDummy.WeaponType.None;
                }
                result = true;
                break;
            case ItemCode.Enhancable_Rifle:
                if (equip)
                {
                    player.Weapon_Type = PlayerDummy.WeaponType.Rifle;
                }
                else
                {
                    player.Weapon_Type = PlayerDummy.WeaponType.None;
                }
                result = true;
                break;
            case ItemCode.Enhancable_shotGun:
                if (equip)
                {
                    player.Weapon_Type = PlayerDummy.WeaponType.ShotGun;
                }
                else
                {
                    player.Weapon_Type = PlayerDummy.WeaponType.None;
                }
                result = true;
                break;
            default:
                break;
        }
        return result;
    }
    void Attach_Prefab(ItemData data)
    {
        Transform parentTransform = GetParentTransform(data);
        if (parentTransform.transform.childCount > 0)// 이미 부착되어있는 아이템이 있으면 제거 후 장착
        {
            GameObject itemPrefab = parentTransform.GetChild(0).gameObject;
            Destroy(itemPrefab);
            GameObject newItemPrefab = Instantiate(data.modelPrefab, parentTransform);
            if (Set_Edditional_State(data, true))// ����� �� ����
            {
                on_Pass_Item_Transform?.Invoke(newItemPrefab.transform);// �÷��̾ Ʈ������ ���� ShootPoint ������
            }
            ItemRotater rotater = newItemPrefab.GetComponentInChildren<ItemRotater>();
            Destroy(rotater);
            newItemPrefab.transform.localPosition = Vector3.zero;
            newItemPrefab.transform.localRotation = Quaternion.identity;
        }
        else
        {
            GameObject itemPrefab = Instantiate(data.modelPrefab, parentTransform);
            if (Set_Edditional_State(data, true))// ����� �� ����
            {
                on_Pass_Item_Transform?.Invoke(itemPrefab.transform);// �÷��̾ Ʈ������ ���� ShootPoint ������
            }
            ItemRotater rotater = itemPrefab.GetComponentInChildren<ItemRotater>();
            Destroy(rotater);
            itemPrefab.transform.localPosition = Vector3.zero;
            itemPrefab.transform.localRotation = Quaternion.identity;
        }
    }
    EquipBox_Slot Find_Slot_By_Type(ItemData itemData)
    {
        EquipBox_Slot equipSlot = null;
        ItemData_Armor armor = itemData as ItemData_Armor;
        ItemData_Hat hat = itemData as ItemData_Hat;
        ItemData_Craft jewel = itemData as ItemData_Craft;
        ItemData_Enhancable weapon = itemData as ItemData_Enhancable;
       
        if (armor != null)
        {
            equipSlot = this[EquipType.Body];
        }
        else if (hat != null)
        {
            equipSlot = this[EquipType.Hat];
        }
        else if (weapon != null)
        {
            equipSlot = this[EquipType.Weapon];
        }
        else if (jewel != null)
        {
            equipSlot = this[EquipType.Jewel];
        }
        return equipSlot;
    }
    EquipBox_Slot FindSlot_By_Position()
    {
        EquipBox_Slot equipSlot = null;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 distance;
        RectTransform slotRect;
        foreach(var slot in equipBox_Slots)
        {
            slotRect = slot.GetComponent<RectTransform>();
            distance = mousePos - (Vector2)slotRect.position;
            if (slotRect.rect.Contains(distance))
            {
                equipSlot = slot;
                break;
            }
        }
        Debug.Log(equipSlot);
        return equipSlot;
    }
    Transform GetParentTransform(ItemData data)
    {
        Transform result = null;
        ItemData_Armor armor = data as ItemData_Armor;
        ItemData_Hat hat = data as ItemData_Hat;
        ItemData_Craft jewel = data as ItemData_Craft;
        ItemData_Enhancable weapon = data as ItemData_Enhancable;
        if (hat != null)
        {
            result = equip_Parent_Transform[0];
        }
        else if (armor != null)
        {
            result = equip_Parent_Transform[2];
        }
        else if (weapon != null)
        {
            result = equip_Parent_Transform[1];
        }
        else if (jewel != null)
        {
            result = equip_Parent_Transform[3];
        }
        return result;
    }
    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        PopupSorting?.Invoke(this);
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OpenWindow()
    {
        Open();
    }

    public void CloseWindow()
    {
        Close();
    }
    private void OnMouseDown()
    {
        PopupSorting?.Invoke(this);
    }
}
