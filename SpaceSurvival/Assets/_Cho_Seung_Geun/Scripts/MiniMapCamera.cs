using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    Transform player;
    public Transform Player 
    {
        set 
        {
            player = value;
        }
    }
    Vector3 offset = Vector3.zero;
    private void OnEnable()
    {
        Vector3 playerPos = player.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + 50.0f, playerPos.z);
        offset = transform.position - player.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, player.position + offset, 2.0f);
    }
}
