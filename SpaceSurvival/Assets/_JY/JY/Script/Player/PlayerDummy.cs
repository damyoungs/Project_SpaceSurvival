using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerDummy : MonoBehaviour
{
    InputKeyMouse inputActions;



    Vector3 dir = Vector3.zero;

    Quaternion lookDir = Quaternion.identity;
    public float moveSpeed = 0.0f;
    public float rotateSpeed = 0.0f;

    public float pickupRange = 3.0f;
    private void Awake()
    {
        inputActions = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ChangeVectorDir;
     //   inputActions.Player.Move.canceled += MakeDirZero;
    }
    private void Update()
    {
        Move();
        Rotate();
    }
    void Move()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);
    }
    void Rotate()
    {
        //Vector3 newDir = transform.position + dir;
        // transform.LookAt(transform.position + dir);
        transform.LookAt(transform.position + dir);
    }
    private void ChangeVectorDir(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            Vector3 value = context.ReadValue<Vector3>();
            dir = value;
            Debug.Log(dir);
        }
    }

    void ItemPickUp()
    {

    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, pickupRange, 2.0f);
    }
#endif
}
