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
    int[] merchantItemCountArray;
    public int[] MerchantItemCountArray => merchantItemCountArray;

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
