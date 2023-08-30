using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Script : TestBase
{
    public GameMaster gameMaster;
    public GameObject GO;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        gameMaster.Action(GO);
    }
}
