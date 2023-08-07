using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        Debug.Log("아이템클릭");
    }

}
