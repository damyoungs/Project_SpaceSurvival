using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipBox : MonoBehaviour
{
    EquipBox_Slot[] equipBox_Slots;
    EquipBox_Description description;

    public Action<ItemData, ItemData> on_Update_Status_For_EquipOrSwap;
    public Action<ItemData> on_Update_Status_For_UnEquip;
    public EquipBox_Description Description => description;
    public EquipBox_Slot this[EquipType type] => equipBox_Slots[(int) type - 1];//0��° �ε��� = None 
    public Transform[] equip_Parent_Transform;

    PlayerDummy player;

    CanvasGroup canvasGroup;
    public bool IsOpen => canvasGroup.alpha > 0.9f;
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

    }
    public void Set_ItemData_For_Drag(ItemData itemData)// ������ ���� ó����
    {
        //itemdata �� hat, Weapon, Suit, Jewel ���� Ȯ���ϰ� ������ Ÿ�԰� ���� ������ ���Ͻ�Ű��
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

                    GameManager.SlotManager.Just_ChangeSlot.ItemData = null;// ������ ������ ���̹Ƿ� �κ��丮�� ���� ����
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
    //equipSlot Clear�ϴ� ��������Ʈ ������ ���� 
    void UnEquip_Item(ItemData itemData)
    {
        on_Update_Status_For_UnEquip?.Invoke(itemData);
        Remove_Prefab(itemData);
        EquipBox_Slot slot = Find_Slot_By_Type(itemData);
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
            on_Update_Status_For_EquipOrSwap?.Invoke(slot.ItemData, itemData);//������� �ƴ� ���� ù��° �Ķ���Ͱ� null �� ���� �ȴ�. // �÷��̾� ���ݷ�, ���� ����
            if (itemData.code == ItemCode.Space_Armor)
            {
                player.ArmorType_ = PlayerDummy.ArmorType.SpaceArmor;// enum ������ player ���� �˸��� ���ʸ� Ȱ��ȭ�ϰ� �ٸ� ������ ��Ȱ��ȭ
            }
            else if (itemData.code == ItemCode.Big_Space_Armor)
            {
                player.ArmorType_ = PlayerDummy.ArmorType.BigArmor;
            }
            else
            {
                Attach_Prefab(itemData);//������ ����
            }
            slot.SetItemData(itemData);//��񽽷� UI������Ʈ
        }

    }
    void Attach_Prefab(ItemData data)
    {
        Transform parentTransform = GetParentTransform(data);
        if (parentTransform.transform.childCount > 0)// �̹� �����Ǿ��ִ� �������� ������ ���� �� ����
        {
            GameObject itemPrefab = parentTransform.GetChild(0).gameObject;
            Destroy(itemPrefab);
            GameObject newItemPrefab = Instantiate(data.modelPrefab, parentTransform);
            newItemPrefab.transform.localPosition = Vector3.zero;
            newItemPrefab.transform.localRotation = Quaternion.identity;
        }
        else
        {
            GameObject itemPrefab = Instantiate(data.modelPrefab, parentTransform);
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
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
