using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUnitObjectIsPool : ObjectIsPool
{
    private void Awake()
    {
        isPositionReset = false;
    }
}
