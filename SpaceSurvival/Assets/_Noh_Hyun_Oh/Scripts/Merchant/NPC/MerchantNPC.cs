using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� ��л���
/// </summary>
public enum Merchant_State
{
    Nomal = 0,          // �������
    High,               // ��������� ���� �ǸŹ�ǰ�� ������ �϶��ϰ� �����Ҷ� ��ΰ� �����Ѵ�
    Low                 // ��г��ܶ� ���� �ǸŹ�ǰ�� ������ ����ϰ� �����Ҷ� ���ΰ� �����Ѵ�
}

public class MerchantNPC : MonoBehaviour
{
    /// <summary>
    /// ���� ���Ǿ� ���� ��л���
    /// </summary>
    Merchant_State merchant_State;
    public Merchant_State Merchant_State => merchant_State;


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


    private void Awake()
    {

    }
}
