using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody rigid;

    public float moveSpeed = 3.0f;
    int hp;
    int mp;
    int fatigue;
    int darkForce;
    int att;
    int dp;
    int level;

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }
    public int Fatigue { get { return fatigue; } set { fatigue = value; } }
    public int DarkForce { get { return darkForce; } set { darkForce = value; } }

    Vector3 dir;

    [SerializeField]
    GameObject inven;

    public Action onPlayerDie;
    private void Awake()
    {
        playerInput = new PlayerInput();
        rigid = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.OpenInven.started += OpenInventory;
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector3>();
    }
    //private void FixedUpdate()
    //{
    //    transform.position += (Time.fixedDeltaTime * moveSpeed * dir);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
    //}
    private void OnDisable()
    {
        
    }
    public void OpenInventory(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        GameManager.Inventory.Open_Inventory();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
        }
    }
}
