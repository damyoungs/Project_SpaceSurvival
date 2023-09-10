using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkAction : MonoBehaviour
{
    public GameObject PlayerCharacter;

    private void Awake()
    {
        PlayerCharacter = GameObject.Find("Player");
    }

    public void TalkNpc()
    {

    }
}
