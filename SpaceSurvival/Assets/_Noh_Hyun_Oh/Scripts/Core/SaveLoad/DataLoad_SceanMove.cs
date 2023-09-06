using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoad_SceanMove : MonoBehaviour
{
    SlotManager slotManager;
    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>(true);
    }
    private void Start()
    {
        SaveLoadManager.Instance.loadedSceanMove = FileLoadAction;
    }
    /// <summary>
    /// 로드 눌렀을때 화면이동과 데이터 셋팅 함수
    /// </summary>
    /// <param name="data">로드된 데이터</param>
    private void FileLoadAction(JsonGameData data)
    {
        //여기에 파싱작업이필요하다 실제로사용되는 작업
        if (data != null)
        {
            setData(data);
            Debug.Log($"{data} 파일이 정상로드됬습니다 , {data.SceanName} 파싱작업후 맵이동 작성을 해야하니 맵이 필요합니다.");
            if (SpaceSurvival_GameManager.Instance.GetBattleMapInit != null) //배틀맵데이터가 셋팅되있으면
            {
                SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //초기화 하기
            }
            LoadingScean.SceanLoading(data.SceanName);

        }
    }
    public void setData(JsonGameData data) 
    {
        //slotManager.Initialize();
        Slot temp = null;
        ItemData_Enhancable tempEnchan;

        List<Slot> slots = slotManager.slots[Current_Inventory_State.Equip];

        for (int slotIndex = 0; slotIndex  < data.EquipSlotLength; slotIndex ++)
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