using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Code : MonoBehaviour
{
    List<int> temp;

    private void Awake()
    {
        Debug.Log(temp);
        temp = new List<int>();
        Debug.Log(temp);
        temp.Add(1);
        temp.RemoveAt(0);
        Debug.Log(temp);
    }
}
