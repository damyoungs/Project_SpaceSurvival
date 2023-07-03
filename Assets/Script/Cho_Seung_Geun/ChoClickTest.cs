using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoClickTest : MonoBehaviour
{
    Camera mainCamera;
    float speed = 4.0f;
    CharacterController characterController;
    BoxCollider target = null;

    private void OnEnable()
    {
    }

    private void Start()
    {
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        //Mouse.current.position.ReadValue();
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                target = (BoxCollider)hitInfo.collider;
                Tile tile = target.gameObject.GetComponent<Tile>();
                Debug.Log($"타일 위치 : {tile.Width}, {tile.Length}");
            }

        }

        if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.01f)
        {
            transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
        }
    }
}
