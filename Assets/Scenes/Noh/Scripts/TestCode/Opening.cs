using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    [SerializeField]
    private EnumList.SceanName scean = EnumList.SceanName.TITLE;
    [SerializeField]
    private bool battleCheck = false;
    private void Start()
    {
        LoadingScean.SceanLoading(scean, battleCheck);
    }

}
