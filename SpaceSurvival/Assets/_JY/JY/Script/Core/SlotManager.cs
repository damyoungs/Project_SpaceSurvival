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
    public IsMovingChange isMovingChange; // Slot�� isMoving�� �ٲٴ� �Լ� ȣ��

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
        UpdateSlot(item, slotList);
    }
    public void DropItem()
    {
        //�̹��� emptySlot���� �ٲٱ�
        //slot.CurrentItem null;
        selectedSlot = null;
    }
    private void UpdateSlot(ItemBase item, List<GameObject> slotList)
    {
        if (item.IsStackable)//�� ĭ�� ������ ���� ������ �������� ��� 
        {
            foreach(GameObject slotObject in slotList) //����Ʈ�� ��ȸ�ϸ鼭 ���� �������� ������ Count�� ������Ű�� return;
            {
                Slot slot = slotObject.GetComponent<Slot>();
                if (item.name == slot.CurrentItem)
                {
                    slot.ItemCount++;
                    return;
                }
            }
        }
        foreach (GameObject slotObject in slotList)
        {
            Slot slot = slotObject.GetComponent<Slot>();
            if (slot.IsEmpty)
            {
                Image slotImage = slotObject.transform.GetChild(0).GetComponent<Image>();
                string spriteName = Enum.GetName(typeof(ItemImagePath), item.ItemImagePath);          
                foreach (Sprite s in sprite)
                {
                    if (s.name == spriteName)
                    {
                        slotImage.sprite = s;
                        slot.Item = item;
                        break;
                    }
                }
                slot.IsEmpty = false;
                slot.CurrentItem = item.name;
                break;
            }
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
    void ResetImageAlpha()// �̹��� ���İ� �ʱ�ȭ 
    {
        if (firstClickImage != null)
        {
            var color = firstClickImage.color;
            color.a = 1.0f;
            firstClickImage.color = color;
        }
    }
    void SwapItems(Slot firstSlot, Slot secondSlot)
    {
        // �� ������ ���� ����Ʈ ��������
        List<GameObject> SlotList = null;

        foreach (var slotList in slots)
        {
            if (slotList.Value.Contains(firstSlot.gameObject))
            {
                SlotList = slotList.Value;
            }
        }

        if (SlotList == null)
        {
            Debug.LogError("Slot lists not found.");
            return;
        }

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
        var color = firstClickImage.color;
        color.a = 0.5f;//���ú��� color�� ���İ��� �����ϴ°� ����������  clickedItemImage.color.a = 0.5f; �̷��� ���� ���� �����ϴ°� �б������̶� �ȵȴ�
        firstClickImage.color = color;
        while (selectedSlot != null)
        {
            imageTransform.position = Input.mousePosition;
            yield return null;
        }
        imageTransform.anchoredPosition = Vector2.zero;

        yield break;
    }
}
