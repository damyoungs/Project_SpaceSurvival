using StructList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataParsing : MonoBehaviour
{
    SlotManager slotManager;
    JsonGameData saveData; //�����ҵ����� �Ľ��ҵ��� ��Ƶ� ����

    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>();
    }

    /// <summary>
    /// ���� �ϱ����� ���嵥���� ����� �Լ�
    /// </summary>
    public void SaveParsing()
    {
        saveData = new JsonGameData();                              //������ ��ü ����

        SaveInvenDataParsing();                                     //�κ��丮 ���� ������ �������� 

        SaveLoadManager.Instance.GameSaveData = saveData;           //������������� ��ü�� ���
    }
    /// <summary>
    /// �ε� �� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="data"></param>
    public void LoadParsing(JsonGameData data)
    {
        LoadInvenDataParsing(data);

    }

    /// <summary>
    /// ��������� �κ��丮���� �����ؼ� �����ͷ� ����� �۾� 
    /// </summary>
    private void SaveInvenDataParsing() 
    {

        int defaultSlotLength = 10; //�ʱ�ȭ�� �⺻���Լ�
        
        List<Slot> temp = slotManager.slots[Current_Inventory_State.Equip]; //������ ���Գ��� �����ͼ�
        
        List<CharcterItems> tempList = new(); // ���嵥���� ����� 

        CharcterItems tempData = new CharcterItems(); // �����ϳ��� ������ ������ ��ü����� 

        ItemData_Enhancable enhanceItem; // ��ȭ�� ������� üũ�� ��������� 

        foreach (Slot slot in temp) //���� ���� �ϴ� ���鼭 
        {
            if (slot.ItemData != null) //�������� �ִ°͵��� 
            {
                enhanceItem = slot.ItemData as ItemData_Enhancable; //��ȭ �Ǵ°��� üũ�ؼ� 

                if (enhanceItem != null) //��ȭ�Ǵ°Ÿ� 
                {
                    tempData.ItemEnhanceValue = enhanceItem.itemLevel; //��ȭ�� ������ ��Ƶΰ�  0�� = 1  1���� = 2 ��
                }

                tempData.ItemIndex = slot.ItemData.code;            // ����Ⱥκ� �������̹������� �ڵ�� ���

                tempData.Values = slot.ItemCount;                   // ����� ������ ���
                
                tempData.SlotIndex = slot.Index;                    // ����� ���� ��ġ�� ���
                
                tempList.Add(tempData);                             //������ ���É����� ����Ʈ�� �߰��� �صд�
            }
        }

        saveData.EquipSlotLength = temp.Count - defaultSlotLength; // ���â�� �����߰������ Ȯ���ϱ����� ���� ������ �ʱⰪ�� ���� �����صд�

        saveData.EquipData = tempList.ToArray();                    //����Ʈ�� jsonUtil���� ���������������� �迭�� �ٽ� �ٲ㼭 ����� Ŭ������ �Է�

        tempList.Clear();                                           //����Ʈ ���� ���� ���� 

        temp = slotManager.slots[Current_Inventory_State.Consume];  //�Һ�â ������

        SetTempData(temp,tempList);                                 //������ �����ͼ� ��Ƶα�

        saveData.ConsumeSlotLength = temp.Count - defaultSlotLength;//���� ���� ������ �����ϱ� 

        saveData.ConsumeData = tempList.ToArray();                  //���� �����ҵ����� �迭�� ��ȯ

        tempList.Clear();                                           //����Ʈ ���� ���� ���� 


        temp = slotManager.slots[Current_Inventory_State.Etc];      //��Ÿâ ������

        SetTempData(temp,tempList);                                 //������ �����ͼ� ��Ƶα�
        
        saveData.EtcSlotLength = temp.Count - defaultSlotLength;    //���� ���� ������ �����ϱ� 
        
        saveData.EtcData = tempList.ToArray();                      //���� �����ҵ����� �迭�� ��ȯ
        
        tempList.Clear();                                           //����Ʈ ���� ���� ���� 



        temp = slotManager.slots[Current_Inventory_State.Craft];    //���� ������
        
        SetTempData(temp,tempList);                                 //������ �����ͼ� ��Ƶα�
        
        saveData.CraftSlotLength = temp.Count - defaultSlotLength;  //���� ���� ������ �����ϱ� 
        
        saveData.CraftData = tempList.ToArray();                    //���� �����ҵ����� �迭�� ��ȯ
        
    }

    /// <summary>
    /// �ӽ� ����Ʈ�� �����µ����� ��� �Լ�
    /// </summary>
    /// <param name="invenTabData">�κ�â ���Կ����� ���� �Ǻ���</param>
    /// <param name="saveData">�����Ͱ� ����� ����Ʈ</param>
    private void SetTempData(List<Slot> invenTabData, List<CharcterItems> saveData) 
    {
        CharcterItems tempData = new CharcterItems(); // �����ϳ��� ������ ������ ��ü����� 
        foreach (Slot slot in invenTabData)
        {
            if (slot.ItemData != null)
            {

                tempData.ItemIndex = slot.ItemData.code;
                tempData.Values = slot.ItemCount;
                tempData.SlotIndex = slot.Index;
                saveData.Add(tempData);
            }
        }
    }


    private void LoadInvenDataParsing(JsonGameData data) 
    {
        //slotManager.Initialize();
        Slot temp = null;
        ItemData_Enhancable tempEnchan;

        List<Slot> slots = slotManager.slots[Current_Inventory_State.Equip];

        for (int slotIndex = 0; slotIndex < data.EquipSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Current_Inventory_State.Equip);
        }

        foreach (CharcterItems equipData in data.EquipData)
        {
            temp = slots[(int)equipData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[equipData.ItemIndex];
            temp.ItemCount = equipData.Values;
            if (equipData.ItemEnhanceValue > 0)
            {
                tempEnchan = temp.ItemData as ItemData_Enhancable;
                if (tempEnchan != null)
                {
                    for (int i = 1; i < equipData.ItemEnhanceValue; i++)
                    {
                        tempEnchan.LevelUpItemStatus(temp);
                    }
                }

            }
        }


        slots = slotManager.slots[Current_Inventory_State.Consume];

        for (int slotIndex = 0; slotIndex < data.ConsumeSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Current_Inventory_State.Consume);
        }

        foreach (CharcterItems consumeData in data.ConsumeData)
        {
            temp = slots[(int)consumeData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[consumeData.ItemIndex];
            temp.ItemCount = consumeData.Values;
        }



        slots = slotManager.slots[Current_Inventory_State.Etc];

        for (int slotIndex = 0; slotIndex < data.EtcSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Current_Inventory_State.Etc);
        }

        foreach (CharcterItems etcData in data.EtcData)
        {
            temp = slots[(int)etcData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[etcData.ItemIndex];
            temp.ItemCount = etcData.Values;
        }

        slots = slotManager.slots[Current_Inventory_State.Craft];

        for (int slotIndex = 0; slotIndex < data.CraftSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Current_Inventory_State.Craft);
        }

        foreach (CharcterItems craftData in data.CraftData)
        {
            temp = slots[(int)craftData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[craftData.ItemIndex];
            temp.ItemCount = craftData.Values;
        }


    }




}
