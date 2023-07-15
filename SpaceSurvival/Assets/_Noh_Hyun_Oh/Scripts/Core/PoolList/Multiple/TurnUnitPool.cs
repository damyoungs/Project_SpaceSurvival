using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUnitPool : MultipleObjectPool<TurnUnitObjectIsPool>
{
    protected override void StartInitialize()
    {
        
        Transform turnGaugeManger = FindObjectOfType<WindowList>().GetComponentInChildren<TurnGaugeManager>(true).transform;
        setPosition = turnGaugeManger.GetChild(0);  
    }
}
