using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public GameObject slot;
    public Transform equip_Below;
    public Transform consume_Below;
    public Transform etc_Below;
    public Transform craft_Below;

    Slot selectedSlot;// ó�� Ŭ���� ������ �����ϱ� ���� ����
    Image firstClickImage; //ù��° Ŭ���� ���� ������ ������ �̹���

    Sprite[] sprite; //������ �̹����� �迭
    RectTransform imageTransform; 

    Vector2 firstSlotPosition;
    Vector2 secondSlotPosition;

    public delegate void IsMovingChange();
    public IsMovingChange isMovingChange; // Slot�� isMoving �� �� Ŭ������ IsSlotMoving�� �ٲٴ� �Լ� ȣ��

    public bool IsSlotMoving { get; set; } = false; // �ܺο��� Ŭ������ �� �� ������ true�̸� �������� ������ ���� ����

    public Dictionary<Current_Inventory_State, List<GameObject>> slots;
    private Dictionary<Current_Inventory_State, int> slotCount; //���� ������ ��ȣ�� �ο��ϱ����� Dic
    public void Initialize()//Inventory���� ȣ��
    {
        sprite = Resources.LoadAll<Sprite>($"ItemImage/Items");
        isMovingChange += () =>
        {
            IsSlotMoving = !IsSlotMoving;
        };
        slots = new Dictionary<Current_Inventory_State, List<GameObject>>
        {
            { Current_Inventory_State.Equip, new List<GameObject>() },
            { Current_Inventory_State.Consume, new List<GameObject>() },
            { Current_Inventory_State.Etc, new List<GameObject>() },
            { Current_Inventory_State.Craft, new List<GameObject>() }
        };
        slotCount = new Dictionary<Current_Inventory_State, int> // ���� ������Ʈ�� ��ȣ�� �ο��ϱ� ���� Dic
        {
            { Current_Inventory_State.Equip, 0 },
            { Current_Inventory_State.Consume, 0},
            { Current_Inventory_State.Etc, 0},
            { Current_Inventory_State.Craft, 0}
        };
        GameManager.Inventory.State = Current_Inventory_State.Equip;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Make_Slot();
            }
            switch (GameManager.Inventory.State)
            {
                case Current_Inventory_State.Equip:
                    GameManager.Inventory.State = Current_Inventory_State.Consume;
                    break;
                case Current_Inventory_State.Consume:
                    GameManager.Inventory.State = Current_Inventory_State.Etc;
                    break;
                case Current_Inventory_State.Etc:
                    GameManager.Inventory.State = Current_Inventory_State.Craft;
                    break;
                default:
                    break;
            }
        }
    }

    public void Make_Slot()
    {
        
        GameObject newSlot = Instantiate(slot);
        Transform parentTransform;
        switch (GameManager.Inventory.State)
        {
            case Current_Inventory_State.Equip:
                parentTransform = equip_Below;
                break;
            case Current_Inventory_State.Consume:
                parentTransform = consume_Below;
                break;
            case Current_Inventory_State.Etc:
                parentTransform = etc_Below;
                break;
            case Current_Inventory_State.Craft:
                parentTransform = craft_Below;
                break;
            default:
                parentTransform = null;
                break;
        }
        if (parentTransform != null)
        {
            slotCount[GameManager.Inventory.State]++;
            newSlot.name = $"{GameManager.Inventory.State}_{slotCount[ GameManager.Inventory.State]}";
            newSlot.transform.SetParent(parentTransform.transform, true);
            slots[GameManager.Inventory.State].Add(newSlot);
        } 
    }
  
    public void GetItem(ItemBase item)
    {
        // itemType�� ���� ����Ʈ�� ��������
        List<GameObject> slotList = GetItemTab(item);//item.itemtype�� ���� ����Ʈ(���, �Һ�, ��Ÿ �� ������� ����� ����Ʈ����) �����ϱ�
        UpdateSlot(item, slotList, true);
    }
    public void DropItem()
    {
        //�̹��� emptySlot���� �ٲٱ�
        //slot.CurrentItem null;
        //List���� �����
        ItemBase item = selectedSlot.Item;
        List<GameObject> slotList = GetItemTab(item);
        UpdateSlot(item, slotList, false);
        selectedSlot = null;
    }
    public void OnSlotClicked(Slot clickedSlot)//����Ÿ���� slot����?
    {
        isMovingChange?.Invoke();
        // ù Ŭ��: ������ ���� ����
        if (selectedSlot == null)
        {
            if (!clickedSlot.IsEmpty)
            {
                selectedSlot = clickedSlot;
                firstClickImage = clickedSlot.transform.GetChild(0).GetComponent<Image>(); //�̹����� ���İ��� �ٲٱ� ���� �̹��� ������Ʈ ���İ������ϴ� �Լ��� ���λ��� �ű�°� ������ ����
                firstClickImage.raycastTarget = false;
                firstSlotPosition = clickedSlot.transform.position;
                imageTransform = clickedSlot.transform.GetChild(0).GetComponent<RectTransform>();

                ResetImageAlpha();
                StartCoroutine(ImageMovingCoroutine());//���İ��� moving�ڷ�ƾ���� �ٲ۴�? ���� �̻��ϴ�
            }
        }
        // �� ��° Ŭ��: ������ ��ȯ�ϰ� ������ ���� �ʱ�ȭ
        // �ܺο��� Ŭ��������(������) �� ItemSpawner ���� �Լ��� �����Ų��.
        else
        {
            secondSlotPosition = clickedSlot.transform.position;

            ResetImageAlpha(); // �̵����� ù��° ���� ���İ� ���󺹱�
            SwapItems(selectedSlot, clickedSlot);
            selectedSlot = null;
        }
    }
    private void UpdateSlot(ItemBase item, List<GameObject> slotList, bool getItem)
    {
        if (item.IsStackable)//�� ĭ�� ������ ���� ������ �������� ��� 
        {
            if (getItem)
            {
                foreach (GameObject slotObject in slotList) //����Ʈ�� ��ȸ�ϸ鼭 ���� �������� ������ Count�� ������Ű�� return;
                {
                    Slot slot = slotObject.GetComponent<Slot>();
                    if (item.Name == slot.CurrentItem)
                    {
                        slot.ItemCount++;
                        return;
                    }
                }
            }
            else
            {
                CheckGetOrDrop(item, slotList, getItem);// �̺κп��� ��� �������� �˾��ϰ� ���� CheckGetOrDrop�� ȣ���� �ʿ���� �ٷ� ChangeSprite�� ȣ���ϸ� �ǰڴ�.
                return;
            }
        }
        CheckGetOrDrop(item, slotList, getItem);
    }

    private void CheckGetOrDrop(ItemBase item, List<GameObject> slotList, bool getItem)
    {
        Slot slot;
        if (getItem)//ȹ��
        {
            foreach (GameObject slotObject in slotList)
            {
                slot = slotObject.GetComponent<Slot>();
                if (slot.IsEmpty)
                {
                    ChangeSprite(slot, item);
                    break;
                }         
            }
        }
        else
        {
            ChangeSprite(selectedSlot);         
        }
    }

    private void ChangeSprite(Slot slot, ItemBase item = null)
    {
        Image slotImage = slot.transform.GetChild(0).GetComponent<Image>();
        string spriteName = item == null ? Enum.GetName(typeof(ItemImagePath), ItemImagePath.EmptySlot) : Enum.GetName(typeof(ItemImagePath), item.ItemImagePath);
        foreach (Sprite s in sprite)
        {
            if (s.name == spriteName)
            {
                slotImage.sprite = s;
                slot.Item = item;
                break;
            }
        }
        slot.IsEmpty = !slot.IsEmpty;// ������  false���� true�� �ٲ�
      
        if(!slot.IsEmpty)
        {
            slot.CurrentItem = item.Name;
        }
        else
        {
            slot.CurrentItem = null;
        }
    }

    private List<GameObject> GetItemTab(ItemBase item)
    {
        List<GameObject> slotList;
        switch (item.ItemType)
        {
            case ItemType.Equip:
                slotList = slots[Current_Inventory_State.Equip];
                break;
            case ItemType.Consume:
                slotList = slots[Current_Inventory_State.Consume];
                break;
            case ItemType.Etc:
                slotList = slots[Current_Inventory_State.Etc];
                break;
            case ItemType.Craft:
                slotList = slots[Current_Inventory_State.Craft];
                break;
            default:
                slotList = null;
                break;
        }
        return slotList;
    }
    private List<GameObject> GetItemTab(Slot slot)
    {
        ItemBase item = slot.Item;
        List<GameObject>slotList = GetItemTab(item);
        return slotList;
    }

    void ResetImageAlpha()// �̹��� ���İ� �ʱ�ȭ 
    {
        var color = firstClickImage.color;
        color.a = IsSlotMoving? 0.5f : 1.0f;
        firstClickImage.color = color;
    }
    void SwapItems(Slot firstSlot, Slot secondSlot)
    {
        // �� ������ ���� ����Ʈ ��������
        List<GameObject> SlotList = GetItemTab(firstSlot);

        // �� ������ �ε����� ã��.
        int firstSlotIndex = SlotList.IndexOf(firstSlot.gameObject);
        int secondSlotIndex = SlotList.IndexOf(secondSlot.gameObject);

        // ������ ��� �����ϰ� ����Ʈ���� ����
        GameObject tempFirstSlot = SlotList[firstSlotIndex];
        GameObject tempSecondSlot = SlotList[secondSlotIndex];

        firstSlot.gameObject.transform.position = secondSlotPosition;
        secondSlot.transform.position = firstSlotPosition;


        SlotList.RemoveAt(firstSlotIndex);
        SlotList.Insert(firstSlotIndex, tempSecondSlot);
        SlotList.RemoveAt(secondSlotIndex);
        SlotList.Insert(secondSlotIndex, tempFirstSlot);
        // ������ ��ġ�� �ٸ� ������ �߰��Ͽ� ��ġ�� �ٲ�
    }
    IEnumerator ImageMovingCoroutine()
    {
        while (selectedSlot != null)
        {
            imageTransform.position = Input.mousePosition;
            yield return null;
        }
        imageTransform.anchoredPosition = Vector2.zero;
        IsSlotMoving = false;// ���İ� �ʱ�ȭ ������ ����
        ResetImageAlpha();
        yield break;
    }
}
