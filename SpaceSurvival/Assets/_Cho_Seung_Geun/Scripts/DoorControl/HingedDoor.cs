using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    public float speed = 4.0f;

    Transform door;
    MeshCollider doorCollider;

    private void Awake()
    {
        door = transform.parent.GetChild(0);
        doorCollider = door.GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        doorCollider.enabled = false;
        while (door.rotation.eulerAngles.y < 120.0f)
        {
            door.rotation *= Quaternion.Euler(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
        doorCollider.enabled = true;
    }
}
