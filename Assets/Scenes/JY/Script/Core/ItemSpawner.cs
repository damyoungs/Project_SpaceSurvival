using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSpawner : MonoBehaviour
{
    PlayerInput inputAction;
    private void Awake()
    {
        inputAction = new PlayerInput();
    }
    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Test1.
    }

}
