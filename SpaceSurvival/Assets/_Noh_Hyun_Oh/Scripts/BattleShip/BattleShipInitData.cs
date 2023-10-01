using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShipInitData : MonoBehaviour
{
    Gyu_QuestManager questManager;
    Merchant_Manager merchantManager;

    private void Start()
    {
        questManager = FindObjectOfType<Gyu_QuestManager>(true);
        merchantManager = FindObjectOfType<Merchant_Manager>(true);
        questManager.InitDataSetting();
        merchantManager.InitDataSetting();
    }
}
