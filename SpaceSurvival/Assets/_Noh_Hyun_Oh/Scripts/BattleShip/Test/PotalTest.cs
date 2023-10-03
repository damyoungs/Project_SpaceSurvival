using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
        }
    }
}
