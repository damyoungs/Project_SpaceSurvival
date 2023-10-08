using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    Collider sp;
    public bool isAttached { get; set; } = false;
    private void Awake()
    {
        sp = GetComponent<Collider>();
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
        if (isAttached)
        {
            sp.enabled = false;
            return;
        }
        if (other.CompareTag("Player"))
        {
            GameManager.SlotManager.AddItem(itemData.code);
            Destroy(this.gameObject);
        }
    }


}
