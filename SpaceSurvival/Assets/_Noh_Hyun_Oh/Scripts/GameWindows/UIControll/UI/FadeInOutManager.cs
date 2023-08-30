using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �� ���̵��� ���̵� �ƿ� ��� ��� ���۳�Ʈ
/// </summary>
public class FadeInOutManager  :TestBase
{
    /// <summary>
    /// ȭ�� ����ũ�� ���صα�
    /// </summary>
    readonly float screenWidth = Screen.width;
    /// <summary>
    /// ȭ�� ����ũ�� ���صα�
    /// </summary>
    readonly float screenHeight = Screen.height;

    /// <summary>
    /// UI �̵� �ӵ�
    /// </summary>
    [SerializeField]
    private float moveSpeed = 5.0f;

    /// <summary>
    /// �̵��� ��ġ�ӽ÷� ������ ����
    /// </summary>
    private Vector2 tempMin;
    private Vector2 tempMax;

    /// <summary>
    /// ���൵ 
    /// </summary>
    private float timeElapsed = 0.0f;

    private bool isMove = false;

    RectTransform rt;

    protected override void Awake()
    {
        base.Awake();
        rt = GetComponent<RectTransform>();
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        TurnStartView(rt, 0);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        TurnEndView(rt, screenWidth);
    }
    public void TurnStartView(RectTransform rt , float arriveValue = 0.0f) 
    {
        StopAllCoroutines();
        StartCoroutine(MoveToLeftFromRight(rt, arriveValue));
    }

    public void TurnEndView(RectTransform rt , float arriveValue = 0.0f) 
    {
        StopAllCoroutines();
        StartCoroutine( MoveToRightFromLeft(rt, arriveValue)); 
    }
    /// <summary>
    /// ������ â ũ�⸸ŭ ���ʿ��� ���������� �����̰� �ϴ·���
    /// </summary>
    /// <param name="arriveValue">������ ��ġ�� ���Է¾����� ������ġ�� ���ư���. </param>
    IEnumerator MoveToLeftFromRight(RectTransform rt , float arriveValue = 0.0f) 
    {
        isMove = true;
        arriveValue = -arriveValue; //������ġ���� �������̸� -�� ����

        timeElapsed = 0.0f; //�ð������� ������ �ʱ�ȭ
        while (true)
        {
            timeElapsed += Time.deltaTime * moveSpeed; // �ӵ���ŭ �ð�����
            
            SetLeftRightValue(rt,
                         rt.offsetMin.x - timeElapsed //���ʿ��� ���������� �������� ��갪
                        );
            yield return null;
            if (rt.offsetMin.x <= arriveValue) //��������
            {
                SetLeftRightValue(rt, arriveValue); //�ӵ����������� ������� ������������ ������
                break;
            }
        }
        isMove = false;
    }
    /// <summary>
    /// ������ â ũ�⸸ŭ �����ʿ��� �������� �����̰��ϴ·���
    /// </summary>
    /// <param name="arriveValue">������ ��ġ�� ���Է¾����� ������ġ�� ���ư���. </param>
    IEnumerator MoveToRightFromLeft(RectTransform rt,  float arriveValue = 0.0f)
    {

        timeElapsed = 0.0f;
        while (true)
        {
            timeElapsed += Time.deltaTime * moveSpeed;
            SetLeftRightValue(rt,
                rt.offsetMin.x + timeElapsed //�����ʿ��� �������� �������� ��갪
                );
            yield return null;
            if (rt.offsetMin.x >= arriveValue) //��������
            {
                SetLeftRightValue(rt, arriveValue); //�ӵ��������� ������� ��ġ�� ����
                break;
            }
        }
    }

    /// <summary>
    /// UI ũ�⸦ ������ä 
    /// ����, ���������� �̵���Ű�� �Լ� 
    /// </summary>
    /// <param name="value">�̵��� ��ġ��</param>
    ///<param name="rt">�̵��� ������Ʈ</param>
    private void SetLeftRightValue(RectTransform rt , float value)
    {
        tempMin = rt.offsetMin; //Left
        tempMax = rt.offsetMax; //Right
        tempMin.x = value; //Left ���� ����� �״�� ����
        tempMax.x = value; //Right ���� ����� ������ ���������� ��ȯ�Ǿ� ����.
        rt.offsetMin = tempMin; //Left
        rt.offsetMax = tempMax; //Right

    }

    /// <summary>
    ///  UI ũ�⸦ ������ä ���Ʒ��� �̵���Ű�� �Լ�
    ///<param name="value">��ġ��</param>
    ///<param name="rt">�̵��� ������Ʈ</param>
    /// </summary>
    private void SetTopBottomValue(RectTransform rt, float value)
    {
        tempMin = rt.offsetMin; //bottom
        tempMax = rt.offsetMax; //top
        tempMin.y = value; //bottom ���� ����� �״�� ����
        tempMax.y = value; //top ���� ����� ������ ���������� ��ȯ�Ǿ� ����.
        rt.offsetMin = tempMin; //bottom
        rt.offsetMax = tempMax; //top
    }

}
