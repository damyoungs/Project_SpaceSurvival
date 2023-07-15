using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �� �������� ���� ���ֿ�����Ʈ
/// </summary>
public class GaugeUnit : TurnUnitObjectIsPool
{
    /// <summary>
    /// �������� �����̴� �ð� �ø����� ������
    /// </summary>
    [SerializeField]
    private float gaugeSpeed = 1.5f;
    /// <summary>
    /// ������ ������ġ �������� �β�
    /// </summary>
    [SerializeField]
    [Range(0.0f, 0.005f)]
    private float boldBar = 0.005f;
    /// <summary>
    /// ���� �������� ���� ������ ������
    /// </summary>
    [SerializeField]
    ITurnBaseData unit;
    public ITurnBaseData Unit 
    {
        get => unit;
        set => unit = value;
    }

    /// <summary>
    /// ���൵ ó���� �� 0.0f ~ 1.0f;
    /// �̰��� �������� ������ ��ġ�� �����Ѵ�.
    /// </summary>
    [SerializeField]
    [Range(0.0f,1.0f)]
    float progressValue = 0.0f;
    public float ProgressValue 
    {
        get => progressValue;
        set 
        {
            progressValue = value;
            if (value > 1.0f) //�ִ밪�� 1.0f�̴�.
            {
                progressValue = 1.0f;
            }
        }
    }

    /// <summary>
    /// ���൵���� ���� �������̹��� ���� �߰�
    /// </summary>
    Image icon;

    /// <summary>
    /// �������� �����̴� ���  ���� �߰�
    /// </summary>
    Animator iconAnim;

    /// <summary>
    /// Position �̵��̾ƴ϶� ��Ŀ���� �����Ұ��̶� �ʿ�
    /// </summary>
    RectTransform moveRect;

    /// <summary>
    /// �����Ȳ ������ ����
    /// anchor�� ���� ����
    /// </summary>
    Vector2 minValue; 
    Vector2 maxValue;

    /// <summary>
    /// ������ȭ���� right left ���� ���� ���� 
    /// </summary>
    Vector2 rectLeft;
    Vector2 rectRight;

    /// <summary>
    /// ���̺�ȯ�̵Ǹ� ������ �����̴� ������ ������� �ڸ�ƾ
    /// ���̵��Ŀ��� ���߱����� �����λ���.
    /// </summary>
    IEnumerator ProgressBar;

    private void Awake()
    {
        icon = GetComponent<Image>();
        moveRect = GetComponent<RectTransform>();
        iconAnim = GetComponent<Animator>();
        ProgressBar = ProgressMove();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine( ProgressBar );
    }
    protected override void OnDisable()
    {
        StopCoroutine( ProgressBar );
        base.OnDisable();
    }

    /// <summary>
    /// progressValue ���� ����Ǹ� ������ �����̴� ������ �ֱ����� �߰� 
    /// ��ü â����� ����ǵ� ������������ �����̴´����� �ֱ����� ��Ŀ�������� ����ߴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProgressMove() 
    {
        if (moveRect != null)
        {
            minValue = moveRect.anchorMin;
            maxValue = moveRect.anchorMax;
           
            rectLeft = new Vector2(0, moveRect.offsetMin.y); //�����ͻ��� left �� ����  �̰� ����� �Էµ�
            rectRight = new Vector2(0, moveRect.offsetMax.y); //�����ͻ��� right �� ���� -�� �ԷµǱ⶧���� ����Է��Ϸ��� -�߰�
            while (true)
            {
                
                minValue.x = Mathf.Lerp(minValue.x, progressValue ,Time.deltaTime * gaugeSpeed);
                //min max ���� ������ ����ٿ��� ����Ⱑ �Ⱥ������� ������ ���̸��ξ���.
                maxValue.x = Mathf.Lerp(maxValue.x , progressValue + boldBar,Time.deltaTime* gaugeSpeed);

                //��Ŀ �����ǰ� ����
                moveRect.anchorMin = minValue;
                moveRect.anchorMax = maxValue;

                //��Ŀ �����ǰ��� �����ϸ� right  left ���� �ڵ����ιٲ�µ� ����� ���̰��Ϸ��� 
                //�ʱⰪ���� �������Ѵ� �׷��� 0������ ����
                moveRect.offsetMin = rectLeft;
                moveRect.offsetMax = rectRight;
                yield return null;
            }
            
        }
        
    }
}
