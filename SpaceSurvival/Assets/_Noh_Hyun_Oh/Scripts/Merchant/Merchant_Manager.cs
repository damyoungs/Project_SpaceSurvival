using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ����� �̳Ѱ�
/// </summary>
public enum Merchant_Selected
{
    None,
    Buy,
    Sell,

}
public class Merchant_Manager : MonoBehaviour
{
    Merchant_Selected selected;
    public Merchant_Selected Selected
    {
        get => selected;
        set
        {
            if (selected != value)
            {
                selected = value;
                switch (selected)
                {
                    case Merchant_Selected.None:

                        break;
                    case Merchant_Selected.Buy:
                        
                        break;
                    case Merchant_Selected.Sell:
                        
                        break;

                    default:
                        break;
                }
            }
        }
    }

    Current_Inventory_State merchant_State;
    public Current_Inventory_State Merchant_State 
    {
        get => merchant_State;
        set 
        {
            if (merchant_State != value)
            {
                merchant_State = value;
                switch (merchant_State)
                {
                    case Current_Inventory_State.None:
                        break;
                    case Current_Inventory_State.Equip:
                        break;
                    case Current_Inventory_State.Consume:
                        break;
                    case Current_Inventory_State.Etc:
                        break;
                    case Current_Inventory_State.Craft:
                        break;
                    default:
                        break;
                }
            }
        }
    }


    /// <summary>
    /// ������ �Ǹ����� �����۸�� 
    /// </summary>
    [SerializeField]
    ItemData[] merchantItemArray;
    public ItemData[] MerchantItemArray => merchantItemArray;

    /// <summary>
    /// �Ǹ����� ����� ������ ���� 
    /// merchantItemArray �� �ε����� ���� �����ҿ����̴� 
    /// </summary>
    uint[] merchantItemCountArray;
    public uint[] MerchantItemCountArray => merchantItemCountArray;

    /// <summary>
    /// ������ �ش繰ǰ�� �󸶳� �ΰ� Ȥ�� ��ΰ� �������������� �������� ������ �迭
    /// merchantItemArray �� �ε����� ���� ������ ���� 
    /// </summary>
    int[] merchantItemCoinValue;
    public int[] MerchantItemCoinValue => merchantItemCoinValue;

    /// <summary>
    /// ������ ������ ������ ��� 
    /// �߸��Ǹ��Ѱ� �ٽû�¿뵵�� ��� 
    /// </summary>
    List<ItemData> sellsItemArray;
    public List<ItemData> SellsItemArray => sellsItemArray;

    /// <summary>
    /// �籸�� �� ������ ���� 
    /// sellsItemArray �� �ε����� ���� ������ �����̴�.
    /// </summary>
    List<int> merchantSellCountArray;
    public List<int> MerchantSellCountArray => merchantSellCountArray;

    

}
