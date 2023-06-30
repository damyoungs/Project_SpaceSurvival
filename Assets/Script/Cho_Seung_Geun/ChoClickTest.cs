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
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                target = (BoxCollider)hitInfo.collider;
            }

        }

        if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.01f)
        {
            transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
        }
    }
}
