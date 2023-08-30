using System;
using UnityEngine;
[Serializable]
public struct ISaveData_Default 
{
    [SerializeField]
    float m_Hp,         //현재 체력 
        m_MaxHp ,       //최대 체력
        m_Stamina,      //현재 스태미나
        m_MaxStamina,   //최대 스태미나
        m_Strength,     //근력 스탯
        m_Dexterity,    //민첩 스탯
        m_Luck,         //행운 스탯
        m_Attack,       //공격력
        m_Defence,      //방어력
        m_Critical,     //크리티컬 확율
        m_Spirit,       //정신력
        m_Buring,       //화상
        m_Posion,       //독
        m_Fear;         //공포
    [SerializeField]
    int m_Sight;        //시야 
  

}
