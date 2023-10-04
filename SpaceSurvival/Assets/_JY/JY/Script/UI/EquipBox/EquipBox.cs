using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class Equipments_Data_Server//플레이어가 원하는 타이밍에 언제든 현재 장비된 장비들로 인해 추가된 공격력과 방어력을 받아오기위한 클래스
{
    EquipBox equipBox_;
    uint total_ATT;
    public uint Total_ATT => total_ATT;
    uint total_DP;
    public uint Total_DP => total_DP;
    public uint Total_STR;
    public uint Total_INT;
    public uint Total_LUK;
    public uint Total_DEX;
    public float Total_CriticalRate;
    public float Total_DodgeRate;

    public ItemData[] itemDatas;
    public Equipments_Data_Server(EquipBox equipBox)
    {
        equipBox_ = equipBox;
        itemDatas = new ItemData[4];
    }

    public Equipments_Data_Server GetEquipments_Total_ATT_DP()
    {
        Equipments_Data_Server result = this;
        IEquippable itemData;
        this.total_ATT = 0;
        this.total_DP = 0;
        Total_STR = 0;
        Total_INT = 0;
        Total_LUK = 0;
        Total_DEX = 0;
        Total_CriticalRate = 0;
        Total_DodgeRate = 0;
        foreach (var equipSlot in equipBox_.EquipBox_Slots)
        {
            itemData = equipSlot.ItemData as IEquippable;
            if (itemData != null)
            {
                total_ATT += itemData.ATT;
                total_DP += itemData.DP;
                Total_STR += itemData.STR;
                Total_INT += itemData.INT;
                Total_DEX += itemData.DEX;
                Total_LUK += itemData.LUK;
                Total_CriticalRate += itemData.Critical_Rate;
                Total_DodgeRate += itemData.Dodge_Rate;
            }
        }

        return result;
    }
}

public class EquipBox : MonoBehaviour, IPopupSortWindow, IPointerClickHandler
{
    EquipBox_Slot[] equipBox_Slots;
    EquipBox_Description description;

    public Action<Transform> on_Pass_Item_Transform;
    public Action on_Update_Status;
    public EquipBox_Description Description => description;
    public EquipBox_Slot this[EquipType type] => equipBox_Slots[(int) type - 1];//0踰덉㎏ ?몃뜳??= None 
    public EquipBox_Slot[] EquipBox_Slots => equipBox_Slots;
    Transform[] equip_Parent_Transform;

    Player_ player;

    CanvasGroup canvasGroup;
    public bool IsOpen => canvasGroup.alpha > 0.9f;

    public Action<IPopupSortWindow> PopupSorting { get ; set ; }
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
        Close(); //??긽?ㅺ퀬?ㅻ땲?붾뜲 耳쒖졇?덉쑝硫댁븞?섎땲 ?ㅽ??몃쭏吏�留됱뿉 媛먯텣??
        StartCoroutine(Get_Player_Reference());
    }
    
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
        player = GameManager.Player_;
        equip_Parent_Transform = new Transform[4];
        equip_Parent_Transform[0] = player.Hat_Parent_Transform;
        equip_Parent_Transform[1] = player.Weapon_Parent_Transform;
        equip_Parent_Transform[2] = player.Suit_Parent_Transform;
        equip_Parent_Transform[3] = player.Jewel_Parent_Transform;
    }
    public void Set_ItemData_For_Drag(ItemData itemData)// ?꾨━???μ갑 泥섎━以?
    {
        //itemdata 媛� hat, Weapon, Suit, Jewel ?몄? ?뺤씤?섍퀬 ?щ’???�?낃낵 留욎? ?딆쑝硫?由ы꽩?쒗궎湲?
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

                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;// ?μ갑???깃났??寃껋씠誘�濡??몃깽?좊━???щ’ 鍮꾩슦湲?
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
    //equipSlot Clear?섎뒗 ?몃━寃뚯씠???곌껐??李⑤? 
    void UnEquip_Item(ItemData itemData)
    {
        Remove_Prefab(itemData);
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        Set_Edditional_State(itemData, false);//애니메이션 및 추가 이펙트 해제
        slot.ItemData = null;
        on_Update_Status?.Invoke();
    }
    void Remove_Prefab(ItemData data)
    {
        if (data.code == ItemCode.Space_Armor)
        {
            player.ArmorType_ = Player_.ArmorType.None;
        }
        else if (data.code == ItemCode.Big_Space_Armor)
        {
            player.ArmorType_ = Player_.ArmorType.None;
        }
        else
        {
            Transform parentTransform = GetParentTransform(data);
            GameObject itemPrefab = parentTransform.GetChild(0).gameObject;
            Destroy(itemPrefab);
        }
    }
    public void Set_ItemData_For_DoubleClick(ItemData itemData)
    {
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
        if (slot != null)
        {
            GameManager.SlotManager.Just_ChangeSlot.ItemData = null;
            if (itemData.code == ItemCode.Space_Armor)
            {
                player.ArmorType_ = Player_.ArmorType.SpaceArmor;// enum ?ㅼ젙??player ?먯꽌 ?뚮쭪?� 媛묒샆留??쒖꽦?뷀븯怨??ㅻⅨ 媛묒샆?� 鍮꾪솢?깊솕
            }
            else if (itemData.code == ItemCode.Big_Space_Armor)
            {
                player.ArmorType_ = Player_.ArmorType.BigArmor;
            }
            else
            {
                Attach_Prefab(itemData);//?꾨━??遺�李?
            }
            slot.SetItemData(itemData);//?λ퉬?щ’ UI?낅뜲?댄듃
            on_Update_Status?.Invoke();//?λ퉬以묒씠 ?꾨땺 ?뚮뒗 泥ル쾲吏??뚮씪誘명꽣媛� null ???꾨떖 ?쒕떎. // ?뚮젅?댁뼱 怨듦꺽?? 諛⑹뼱???뗮똿
        }
      //  Set_Edditional_State(itemData, true);//애니메이션 및 추가 이펙트 적용
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
        if (parentTransform.transform.childCount > 0)// ?대? 遺�李⑸릺?댁엳???꾩씠?쒖씠 ?덉쑝硫??쒓굅 ???μ갑
        {
            GameObject itemPrefab = parentTransform.GetChild(0).gameObject;
            Destroy(itemPrefab);
            GameObject newItemPrefab = Instantiate(data.modelPrefab, parentTransform);
            if (Set_Edditional_State(data, true))// 무기류 일 때만
            {
                on_Pass_Item_Transform?.Invoke(newItemPrefab.transform);// 플레이어에 트랜스폼 전달 ShootPoint 설정용
            }
            ItemRotater rotater = newItemPrefab.GetComponentInChildren<ItemRotater>();
            Destroy(rotater);
            newItemPrefab.transform.localPosition = Vector3.zero;
            newItemPrefab.transform.localRotation = Quaternion.identity;
        }
        else
        {
            GameObject itemPrefab = Instantiate(data.modelPrefab, parentTransform);
            if (Set_Edditional_State(data, true))// 무기류 일 때만
            {
                on_Pass_Item_Transform?.Invoke(itemPrefab.transform);// 플레이어에 트랜스폼 전달 ShootPoint 설정용
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
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        PopupSorting?.Invoke(this);
    }
    public void Close()
    {
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
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

    public Equipments_Data_Server Save_EquipmentsData()
    {
        Equipments_Data_Server data_Server = player.Player_Status.Equipments_Data;
        for(int  i = 0; i < equipBox_Slots.Length; i++)
        {
            data_Server.itemDatas[i] = equipBox_Slots[i].ItemData;
        }
        return data_Server;
    }
}
