using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상점 상태 제어용 이넘값
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
    /// 상인이 판매중인 아이템목록 
    /// </summary>
    [SerializeField]
    ItemData[] merchantItemArray;
    public ItemData[] MerchantItemArray => merchantItemArray;

    /// <summary>
    /// 판매중인 목록의 아이템 갯수 
    /// merchantItemArray 와 인덱스가 같게 셋팅할예정이다 
    /// </summary>
    int[] merchantItemCountArray;
    public int[] MerchantItemCountArray => merchantItemCountArray;

    /// <summary>
    /// 상인이 해당물품을 얼마나 싸게 혹은 비싸게 셋팅할지에대한 최종값을 저장할 배열
    /// merchantItemArray 와 인덱스가 같게 셋팅할 예정 
    /// </summary>
    int[] merchantItemCoinValue;
    public int[] MerchantItemCoinValue => merchantItemCoinValue;

    /// <summary>
    /// 상인이 구입한 아이템 목록 
    /// 잘못판매한거 다시사는용도로 사용 
    /// </summary>
    List<ItemData> sellsItemArray;
    public List<ItemData> SellsItemArray => sellsItemArray;

    /// <summary>
    /// 재구입 용 아이템 갯수 
    /// sellsItemArray 와 인덱스를 같게 셋팅할 예정이다.
    /// </summary>
    List<int> merchantSellCountArray;
    public List<int> MerchantSellCountArray => merchantSellCountArray;



}
