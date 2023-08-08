using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    [SerializeField]
    private EnumList.SceneName scene = EnumList.SceneName.TITLE;
    private void Start()
    {
        LoadingScean.SceanLoading(scene);
    }

}
