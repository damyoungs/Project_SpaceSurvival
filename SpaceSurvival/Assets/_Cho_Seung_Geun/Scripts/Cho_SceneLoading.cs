using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cho_SceneLoading : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
    }
}