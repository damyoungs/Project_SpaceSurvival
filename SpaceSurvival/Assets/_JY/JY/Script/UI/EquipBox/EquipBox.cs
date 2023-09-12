using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipBox : MonoBehaviour, IPopupSortWindow, IPointerClickHandler
{
    EquipBox_Slot[] equipBox_Slots;
    EquipBox_Description description;

    public Action<Transform> on_Pass_Item_Transform;
    public Action<ItemData, ItemData> on_Update_Status_For_EquipOrSwap;
    public Action<ItemData> on_Update_Status_For_UnEquip;
    public EquipBox_Description Description => description;
    public EquipBox_Slot this[EquipType type] => equipBox_Slots[(int)type - 1];//0??¬¸ ?????= None 
    Transform[] equip_Parent_Transform;

    Player_ player;

    CanvasGroup canvasGroup;
    public bool IsOpen => canvasGroup.alpha > 0.9f;

    public Action<IPopupSortWindow> PopupSorting { get; set; }
    //InputKeyMouse player_Input_Action;

    private void Awake()
    {
        //player_Input_Action = new InputKeyMouse();
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
    private void Start()
    {
        InputSystemController.Instance.OnUI_Inven_EquipBox_Open = On_EquipBox_Open;
        GameManager.SlotManager.on_UnEquip_Item += UnEquip_Item;
        Close(); //????ªê??ªë???? ×²ô¯?????ñ©ü·??? ?ªí????ýä???? íõ????
        StartCoroutine(Get_Player_Reference());
    }
    //private void OnEnable()
    //{

    //player_Input_Action.KeyBoard.Enable();
    //player_Input_Action.KeyBoard.EquipBox_Open.performed += On_EquipBox_Open;
    //}

    //private void On_EquipBox_Open(InputAction.CallbackContext _)
    private void On_EquipBox_Open()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }


    IEnumerator Get_Player_Reference()
    {
        yield return null;
        player = GameManager.Player__;
        equip_Parent_Transform = new Transform[4];
        equip_Parent_Transform[0] = player.Hat_Parent_Transform;
        equip_Parent_Transform[1] = player.Weapon_Parent_Transform;
        equip_Parent_Transform[2] = player.Suit_Parent_Transform;
        equip_Parent_Transform[3] = player.Jewel_Parent_Transform;
    }
    public void Set_ItemData_For_Drag(ItemData itemData)// ??¥Ì???«ìäß ??¥Ì??
    {
        //itemdata íõ? hat, Weapon, Suit, Jewel ??? ?????? ??¡¯??????ãí ??? ?????????????
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

                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;// ?«ìäß???????????????????¥Ì????¡¯ ?ý³???
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
    //equipSlot Clear??? ??¥Ì??????÷ûÚÜ????? 
    void UnEquip_Item(ItemData itemData)
    {
        on_Update_Status_For_UnEquip?.Invoke(itemData);
        Remove_Prefab(itemData);
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        Set_Edditional_State(itemData, false);//ÏÑ???? Î² Øù? ?Ñ¤? òÚÕô
        slot.ItemData = null;
    }
    void Remove_Prefab(ItemData data)
    {
        Transform parentTransform = GetParentTransform(data);
        GameObject itemPrefab = parentTransform.GetChild(0).gameObject;
        if (data.code == ItemCode.Space_Armor)
        {
            player.ArmorType_ = Player_.ArmorType.None;
        }
        else if (data.code == ItemCode.Big_Space_Armor)
        {
            player.ArmorType_ = Player_.ArmorType.None;
        }
        Destroy(itemPrefab);
    }
    public void Set_ItemData_For_DoubleClick(ItemData itemData)
    {
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        if (slot != null)
        {
            GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
            on_Update_Status_For_EquipOrSwap?.Invoke(slot.ItemData, itemData);//?«ë???? ??ê± ??? ?þë?ýä???????íõ? null ????? ???. // ????ñ©? ?ïÁÚÏ?? ?¦â??????
            if (itemData.code == ItemCode.Space_Armor)
            {
                player.ArmorType_ = Player_.ArmorType.SpaceArmor;// enum ?ªì???player ??? ????? íõ?äÅ???ô¯???????ªë«¸ íõ?äÅ?? ????øú?
            }
            else if (itemData.code == ItemCode.Big_Space_Armor)
            {
                player.ArmorType_ = Player_.ArmorType.BigArmor;
            }
            else
            {
                Attach_Prefab(itemData);//??¥Ì??????
            }
            slot.SetItemData(itemData);//?«ë???¡¯ UI??????
        }
        //  Set_Edditional_State(itemData, true);//ÏÑ???? Î² Øù? ?Ñ¤? ××?
    }
    bool Set_Edditional_State(ItemData data, bool equip)
    {
        bool result = false;
        switch (data.code)
        {
            case ItemCode.Enhancable_Pistol:
                if (equip)
                {
                    player.Weapon_Type = Player_.WeaponType.Pistol;
                }
                else
                {
                    player.Weapon_Type = Player_.WeaponType.None;
                }
                result = true;
                break;
            case ItemCode.Enhancable_Rifle:
                if (equip)
                {
                    player.Weapon_Type = Player_.WeaponType.Rifle;
                }
                else
                {
                    player.Weapon_Type = Player_.WeaponType.None;
                }
                result = true;
                break;
            case ItemCode.Enhancable_shotGun:
                if (equip)
                {
                    player.Weapon_Type = Player_.WeaponType.ShotGun;
                }
                else
                {
                    player.Weapon_Type = Player_.WeaponType.None;
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
        if (parentTransform.transform.childCount > 0)// ?ðÂ? ???¦á??ñ©????ý³??ô¯? ??????üÑÝä ???«ìäß
        {
            GameObject itemPrefab = parentTransform.GetChild(0).gameObject;
            Destroy(itemPrefab);
            GameObject newItemPrefab = Instantiate(data.modelPrefab, parentTransform);
            if (Set_Edditional_State(data, true))// Íëø·øë ÖÕ ?Ý«
            {
                on_Pass_Item_Transform?.Invoke(newItemPrefab.transform);// Í»???ñÕ ???Ñ© ?õË ShootPoint Ðã??
            }
            ItemRotater rotater = newItemPrefab.GetComponentInChildren<ItemRotater>();
            Destroy(rotater);
            newItemPrefab.transform.localPosition = Vector3.zero;
            newItemPrefab.transform.localRotation = Quaternion.identity;
        }
        else
        {
            GameObject itemPrefab = Instantiate(data.modelPrefab, parentTransform);
            if (Set_Edditional_State(data, true))// Íëø·øë ÖÕ ?Ý«
            {
                on_Pass_Item_Transform?.Invoke(itemPrefab.transform);// Í»???ñÕ ???Ñ© ?õË ShootPoint Ðã??
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
        foreach (var slot in equipBox_Slots)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupSorting?.Invoke(this);
    }
}