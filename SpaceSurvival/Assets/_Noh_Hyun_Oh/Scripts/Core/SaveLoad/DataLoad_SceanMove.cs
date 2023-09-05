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
    /// �ε� �������� ȭ���̵��� ������ ���� �Լ�
    /// </summary>
    /// <param name="data">�ε�� ������</param>
    private void FileLoadAction(JsonGameData data)
    {
        //���⿡ �Ľ��۾����ʿ��ϴ� �����λ��Ǵ� �۾�
        if (data != null)
        {
            setData(data);
            Debug.Log($"{data} ������ ����ε����ϴ� , {data.SceanName} �Ľ��۾��� ���̵� �ۼ��� �ؾ��ϴ� ���� �ʿ��մϴ�.");
            if (SpaceSurvival_GameManager.Instance.GetBattleMapInit != null) //��Ʋ�ʵ����Ͱ� ���õ�������
            {
                SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //�ʱ�ȭ �ϱ�
            }
            LoadingScean.SceanLoading(data.SceanName);

        }
    }
    public void setData(JsonGameData data) 
    {
        List<Slot> slots = slotManager.slots[Current_Inventory_State.Equip];
        int i = 0;
        foreach (Slot slot in slots)
        {
            if (data.EquipData.Length > i)
            {
                slot.ItemData = GameManager.Itemdata[data.EquipData[i].ItemIndex];
                slot.ItemCount = data.EquipData[i].Values;
            }
            else 
            {
                break;
            }
            i++;
        }
        //slotManager.slots[Current_Inventory_State.Equip] = new List<Slot>((Slot[])data.EquipData);
        //slotManager.slots[Current_Inventory_State.Consume] = new List<Slot>((Slot[])data.ConsumeData);
        //slotManager.slots[Current_Inventory_State.Etc] = new List<Slot>((Slot[])data.EtcData);
        //slotManager.slots[Current_Inventory_State.Craft] = new List<Slot>((Slot[])data.CraftData);
    }
}