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
            StopAllCoroutines();
            StartCoroutine(Open());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
    }

    IEnumerator Open()
    {
        doorCollider.enabled = false;
        while (door.localRotation.eulerAngles.y < 120.0f)
        {
            door.rotation *= Quaternion.Euler(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
        doorCollider.enabled = true;
    }

    IEnumerator Close()
    {
        doorCollider.enabled = false;
        float rotate = Time.deltaTime * speed;

        while (door.localRotation.eulerAngles.y > rotate)
        {
            door.rotation *= Quaternion.Euler(0.0f, -rotate, 0.0f);
            yield return null;
        }
        door.localRotation = Quaternion.Euler(door.localRotation.x, 0.0f, door.localRotation.z);
        doorCollider.enabled = true;
    }
}
