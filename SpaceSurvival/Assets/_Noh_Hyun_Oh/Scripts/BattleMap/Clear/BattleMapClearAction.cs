using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMapClearAction : PopupWindowBase
{
    Button confimBtn;

    TextMeshProUGUI rewordText;

    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(0).GetChild(0);
        rewordText = child.GetChild(2).GetComponent<TextMeshProUGUI>();
        confimBtn = child.GetChild(3).GetComponent<Button>();
        confimBtn.onClick.AddListener(() => {
            LoadingScene.SceneLoading(EnumList.SceneName.SpaceShip);
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 일단 그냥 하드코딩이다..
    /// </summary>
    /// <param name="rewordItemText"></param>
    public void SetRewordText(string rewordItemText) 
    {
        rewordText.text = rewordItemText;
        GameManager.SlotManager.AddItem(ItemCode.DarkCrystal); //다크크리스탈 보상으로 주기 
    }
}
