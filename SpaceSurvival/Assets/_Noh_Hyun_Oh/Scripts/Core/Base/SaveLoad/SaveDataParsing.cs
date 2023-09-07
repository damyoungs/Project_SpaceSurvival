using StructList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataParsing : MonoBehaviour
{
    SlotManager slotManager;
    JsonGameData saveData; //저장할데이터 파싱할동안 담아둘 변수

    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>();
    }

    /// <summary>
    /// 저장 하기전에 저장데이터 만드는 함수
    /// </summary>
    public void SaveParsing()
    {
        saveData = new JsonGameData();                              //저장할 객체 생성

        SaveInvenDataParsing();                                     //인벤토리 에서 데이터 가져오기 

        SaveLoadManager.Instance.GameSaveData = saveData;           //저장로직에사용될 객체에 담기
    }
    /// <summary>
    /// 로드 시 데이터 셋팅하는 함수
    /// </summary>
    /// <param name="data"></param>
    public void LoadParsing(JsonGameData data)
    {
        LoadInvenDataParsing(data);

    }

    /// <summary>
    /// 게임저장시 인벤토리내용 접근해서 데이터로 만드는 작업 
    /// </summary>
    private void SaveInvenDataParsing() 
    {

        int defaultSlotLength = 10; //초기화시 기본슬롯수
        
        List<Slot> temp = slotManager.slots[Current_Inventory_State.Equip]; //저장탭 슬롯내용 가져와서
        
        List<CharcterItems> tempList = new(); // 저장데이터 만들고 

        CharcterItems tempData = new CharcterItems(); // 슬롯하나당 데이터 셋팅할 객체만들고 

        ItemData_Enhancable enhanceItem; // 강화된 장비인지 체크할 변수만들고 

        foreach (Slot slot in temp) //슬롯 내용 싹다 돌면서 
        {
            if (slot.ItemData != null) //아이템이 있는것들은 
            {
                enhanceItem = slot.ItemData as ItemData_Enhancable; //강화 되는건지 체크해서 

                if (enhanceItem != null) //강화되는거면 
                {
                    tempData.ItemEnhanceValue = enhanceItem.itemLevel; //강화된 레벨을 담아두고  0강 = 1  1강은 = 2 값
                }

                tempData.ItemIndex = slot.ItemData.code;            // 공통된부분 아이템이무엇인지 코드로 담고

                tempData.Values = slot.ItemCount;                   // 저장된 갯수도 담고
                
                tempData.SlotIndex = slot.Index;                    // 저장된 슬롯 위치도 담고
                
                tempList.Add(tempData);                             //데이터 셋팅됬으면 리스트에 추가를 해둔다
            }
        }

        saveData.EquipSlotLength = temp.Count - defaultSlotLength; // 장비창이 슬롯추가됬는지 확인하기위해 현재 갯수와 초기값을 빼서 저장해둔다

        saveData.EquipData = tempList.ToArray();                    //리스트는 jsonUtil에서 권장하지않음으로 배열로 다시 바꿔서 저장될 클래스에 입력

        tempList.Clear();                                           //리스트 내용 비우고 재사용 

        temp = slotManager.slots[Current_Inventory_State.Consume];  //소비창 데이터

        SetTempData(temp,tempList);                                 //데이터 가져와서 담아두기

        saveData.ConsumeSlotLength = temp.Count - defaultSlotLength;//슬롯 갯수 변동값 저장하기 

        saveData.ConsumeData = tempList.ToArray();                  //최종 저장할데이터 배열로 변환

        tempList.Clear();                                           //리스트 내용 비우고 재사용 


        temp = slotManager.slots[Current_Inventory_State.Etc];      //기타창 데이터

        SetTempData(temp,tempList);                                 //데이터 가져와서 담아두기
        
        saveData.EtcSlotLength = temp.Count - defaultSlotLength;    //슬롯 갯수 변동값 저장하기 
        
        saveData.EtcData = tempList.ToArray();                      //최종 저장할데이터 배열로 변환
        
        tempList.Clear();                                           //리스트 내용 비우고 재사용 



        temp = slotManager.slots[Current_Inventory_State.Craft];    //조합 데이터
        
        SetTempData(temp,tempList);                                 //데이터 가져와서 담아두기
        
        saveData.CraftSlotLength = temp.Count - defaultSlotLength;  //슬롯 갯수 변동값 저장하기 
        
        saveData.CraftData = tempList.ToArray();                    //최종 저장할데이터 배열로 변환
        
    }

    /// <summary>
    /// 임시 리스트에 가져온데이터 담는 함수
    /// </summary>
    /// <param name="invenTabData">인벤창 슬롯에대한 정보 탭별로</param>
    /// <param name="saveData">데이터가 저장될 리스트</param>
    private void SetTempData(List<Slot> invenTabData, List<CharcterItems> saveData) 
    {
        CharcterItems tempData = new CharcterItems(); // 슬롯하나당 데이터 셋팅할 객체만들고 
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
