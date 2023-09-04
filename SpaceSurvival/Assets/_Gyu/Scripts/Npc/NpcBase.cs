using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    public GameObject Button;
    BoxCollider boxCollider;

    Transform C;

    private void Awake()
    {
        C = transform.GetChild(0);
        Button = C.transform.GetChild(3).gameObject;
        Button.gameObject.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            QuestManager.instance.initialize();
            Button.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Button.gameObject.SetActive(false);
            QuestManager.instance.initialize();
        }
    }
}
