using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Cash : ItemData, IConsumable
{
    public void Consume(GameObject target)
    {
        PlayerDummy player = target.GetComponent<PlayerDummy>();
        if (player != null)
        {
            player.Money += (int)price;
        }
    }


}
