using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTest : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public CinemachineBrain brain;
    private void Awake()
    {
    }

    private void Start()
    {
        Debug.Log(brain);
        Debug.Log(vcam);
    
    }
}
