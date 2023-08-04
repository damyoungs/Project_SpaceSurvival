using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Enhancable", menuName = "Scriptable Object/Item Data/ItemData - Enhancable", order = 3)]
public class ItemData_Enhancable : ItemData, IEnhancable
{
    public EnhanceType enhanceType;
    public byte itemLevel;
    byte priviousLevel;
    public byte Itemlevel
    {
        get => itemLevel;
        private set
        {
            if (itemLevel != value)
            {
                itemLevel = value;
            }
            if (itemLevel > priviousLevel)
            {
                LevelUpItemStatus();
            }
            else if (itemLevel < priviousLevel)
            {
                LevelDownItemStatus();
            }
            priviousLevel = value;
        }
    }
    public uint attackPoint;
    public uint defencePoint;
    public bool LevelUp(uint darkForceCount)
    {
        bool result = false;
        float SuccessRate = CalculateSuccessRate(darkForceCount);
        if (Random.Range(0, 100) < SuccessRate)
        {
            Itemlevel++;
            result = true;
        }
        else
        {
            if (Itemlevel > 7)
            Itemlevel--;
        }
        return result;
    }
    public float CalculateSuccessRate(uint darkForceCount)
    {
        float finalSuccessRate = 0.0f;
        float forceBoost = darkForceCount * 0.2f;
        float levelBonus = 100 - (Itemlevel * 10);

        finalSuccessRate = Mathf.Clamp(levelBonus + forceBoost, 0.0f, 100.0f);

        return finalSuccessRate;
    }
    void LevelUpItemStatus()
    {
        //ó������ TempSlot���� �ű涧 �ش� ������ �ε����� �����ߴٰ� ��ȭ�� �����ϸ� �ش� �ε����� ������ �����ϰ� �� ItemData�� �Ҵ��غ�����?
        //���Ը޴����� �� �����۰� ���� �������� �ִٸ� �����ϵ��� ��������Ʈ?
        Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        //������ new?
        this.attackPoint = resultAttackPoint;
        this.defencePoint = resultDefencePoint;
        this.itemName = itemname;
        ItemData_Enhancable Pistol = new ItemData_Enhancable();
        GameManager.SlotManager.RemoveItem(this, GameManager.SlotManager.IndexForEnhancer);
        GameManager.SlotManager.AddItem(Pistol.code);
       // Debug.Log(GameManager.SlotManager.slots[Current_Inventory_State.Equip][GameManager.SlotManager.IndexForEnhancer].ItemData);
        //�� �������� Slot�� �� ������ �����͸� Assign ������Ѵ�.
    }
 
    public void Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemName)
    {
        float increaseRatio = 0.3f;
        uint increaseAttackValue = (uint)(this.attackPoint * increaseRatio * (Itemlevel * 0.5f));
        uint increaseDefenceValue = (uint)(this.defencePoint * increaseRatio * (Itemlevel * 0.5f));

        if (this.itemLevel == 2)
        {
            itemName = $"{this.itemName} ��";
        }
        else
        {
            itemName = $"{this.itemName+"��"}";
        }
        resultAttackPoint = this.attackPoint + increaseAttackValue;
        resultDefencePoint = this.defencePoint + increaseDefenceValue;
    }
    void LevelDownItemStatus()
    {
        float decreaseRatio = 0.3f; // ������ �� ����� ������ ����
        uint decreaseAttackValue = (uint)(attackPoint * decreaseRatio * (Itemlevel * 0.5f));
        uint decreaseDefenceValue = (uint)(defencePoint * decreaseRatio * (Itemlevel * 0.5f));

        // ���ҷ��� ���� �������� ũ�� �ʵ��� �Ѵ�.
        decreaseAttackValue = (uint)Mathf.Min(attackPoint, decreaseAttackValue);
        decreaseDefenceValue = (uint)Mathf.Min(defencePoint, decreaseDefenceValue);

        attackPoint -= decreaseAttackValue;
        defencePoint -= decreaseDefenceValue;
    }
 
}
