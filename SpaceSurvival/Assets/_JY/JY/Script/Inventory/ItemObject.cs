using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    SphereCollider sp;

    private void Awake()
    {
        sp = GetComponent<SphereCollider>();
    }
    public ItemData itemData = null;
    public ItemData ItemData
    {
        get => itemData;
        set
        {
            if (itemData == null)
            {
                itemData = value;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.SlotManager.AddItem(itemData.code);
            Destroy(this.gameObject);
        }
    }


}
