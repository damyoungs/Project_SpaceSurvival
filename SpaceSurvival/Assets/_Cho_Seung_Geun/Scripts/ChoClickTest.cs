using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoClickTest : MonoBehaviour
{
    /// <summary>
    /// ȭ���� ��� ī�޶�
    /// </summary>
    Camera mainCamera;
    /// <summary>
    /// ���� ��ġ�� �̵��� ĳ���� �ӵ�
    /// </summary>
    float speed = 4.0f;
    /// <summary>
    /// ȭ�鿡�� ��� ���� �ε����� �ڽ��ݶ��̴�
    /// </summary>
    BoxCollider target = null;
    /// <summary>
    /// ��ǲ�ý��� Ŭ��
    /// </summary>
    InputKeyMouse inputClick;


    private void Awake()
    {
        inputClick = new InputKeyMouse();
    }
    private void OnEnable()
    {
        inputClick.Mouse.Enable();
        inputClick.Mouse.MouseClick.performed += onClick;
        inputClick.Player.Enable();
        inputClick.Player.LeftRotate.performed += onLeftRotate;
        inputClick.Player.RightRotate.performed += onRightRotate;
    }

    private void OnDisable()
    {
        inputClick.Player.RightRotate.performed -= onRightRotate;
        inputClick.Player.LeftRotate.performed -= onLeftRotate;
        inputClick.Player.Disable();
        inputClick.Mouse.MouseClick.performed -= onClick;
        inputClick.Mouse.Disable();
    }

    /// <summary>
    /// ���콺�� Ŭ������ �� �Ͼ �Լ�
    /// </summary>
    /// <param name="context"></param>
    private void onClick(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());      // ȭ�鿡�� ���� ���콺�� ��ġ�� ��� ��
        Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red, 1.0f);              // ����׿� ������

        if (Physics.Raycast(ray, out RaycastHit hitInfo))                       // ���� ���� �ε�����
        {
            if (hitInfo.transform.gameObject.CompareTag("Tile"))                // �±� "Ÿ��"�� �浹�ϸ�
            {
                target = (BoxCollider)hitInfo.collider;                         // Ÿ���� �ڽ��ݶ��̴� ��ȯ
                Tile tile = target.gameObject.GetComponent<Tile>();             // �Ʒ��� ����׸� ���� Ÿ�� ��ȯ(����� �� �ҽ� ��� ��)
                //Debug.Log($"Ÿ�� ��ġ : {tile.Width}, {tile.Length}");
            }
        }
    }

    private void onLeftRotate(InputAction.CallbackContext _)
    {
        Quaternion rotate = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(-90.0f, Vector3.up), 1.0f);

        transform.rotation *= rotate;
        
    }

    private void onRightRotate(InputAction.CallbackContext _)
    {
        Quaternion rotate = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90.0f, Vector3.up), 1.0f);

        transform.rotation *= rotate;
    }

    private void Start()
    {
        mainCamera = Camera.main;           //  ��� �ִ� ī�޶� ��������
    }

    private void FixedUpdate()
    {
        // Ÿ���� ������Ʈ�� �ƴϰ� Ÿ���� �������� �ʾ��� �� �̵�
        if (target != null && (target.gameObject.transform.position - transform.position).sqrMagnitude > 0.01f)
        {
            transform.Translate(Time.fixedDeltaTime * speed * (target.gameObject.transform.position - transform.position).normalized);
        }
    }
}
