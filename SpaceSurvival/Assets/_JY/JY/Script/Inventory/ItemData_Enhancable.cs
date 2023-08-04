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
        //처음부터 TempSlot으로 옮길때 해당 슬롯의 인덱스를 저장했다가 강화가 성공하면 해당 인덱스의 슬롯을 삭제하고 이 ItemData를 할당해보면어떨까?
        //슬롯메니저로 이 아이템과 같은 아이템이 있다면 삭제하도록 델리게이트?
        Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        //생성자 new?
        this.attackPoint = resultAttackPoint;
        this.defencePoint = resultDefencePoint;
        this.itemName = itemname;
        ItemData_Enhancable Pistol = new ItemData_Enhancable();
        GameManager.SlotManager.RemoveItem(this, GameManager.SlotManager.IndexForEnhancer);
        GameManager.SlotManager.AddItem(Pistol.code);
       // Debug.Log(GameManager.SlotManager.slots[Current_Inventory_State.Equip][GameManager.SlotManager.IndexForEnhancer].ItemData);
        //이 시점에서 Slot에 이 아이템 데이터를 Assign 해줘야한다.
    }
 
    public void Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemName)
    {
        float increaseRatio = 0.3f;
        uint increaseAttackValue = (uint)(this.attackPoint * increaseRatio * (Itemlevel * 0.5f));
        uint increaseDefenceValue = (uint)(this.defencePoint * increaseRatio * (Itemlevel * 0.5f));

        if (this.itemLevel == 2)
        {
            itemName = $"{this.itemName} ★";
        }
        else
        {
            itemName = $"{this.itemName+"★"}";
        }
        resultAttackPoint = this.attackPoint + increaseAttackValue;
        resultDefencePoint = this.defencePoint + increaseDefenceValue;
    }
    void LevelDownItemStatus()
    {
        float decreaseRatio = 0.3f; // 레벨업 때 사용한 비율과 동일
        uint decreaseAttackValue = (uint)(attackPoint * decreaseRatio * (Itemlevel * 0.5f));
        uint decreaseDefenceValue = (uint)(defencePoint * decreaseRatio * (Itemlevel * 0.5f));

        // 감소량이 현재 점수보다 크지 않도록 한다.
        decreaseAttackValue = (uint)Mathf.Min(attackPoint, decreaseAttackValue);
        decreaseDefenceValue = (uint)Mathf.Min(defencePoint, decreaseDefenceValue);

        attackPoint -= decreaseAttackValue;
        defencePoint -= decreaseDefenceValue;
    }
 
}
