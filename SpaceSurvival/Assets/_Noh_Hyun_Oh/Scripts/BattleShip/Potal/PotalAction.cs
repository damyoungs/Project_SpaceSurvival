using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalAction : MonoBehaviour
{
    Potal_SceneChange_UI uiComp;

    private void Awake()
    {
        uiComp = FindObjectOfType<Potal_SceneChange_UI>(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiComp.OpenWindow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiComp.CloseWindow();
        }
    }
}
