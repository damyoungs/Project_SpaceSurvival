using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Normal_Attack_Line : MonoBehaviour
{


    Camera mainCam;
    Vector3 mouse_Screen_Pos;
    Vector3 mouse_World_Pos;
    
    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        mouse_Screen_Pos = Mouse.current.position.ReadValue();
        mouse_World_Pos = mainCam.ScreenToWorldPoint(mouse_Screen_Pos);
        mouse_World_Pos.z = mouse_World_Pos.y;
        mouse_World_Pos.y = 0;
        transform.position = mouse_World_Pos;
    }
}
