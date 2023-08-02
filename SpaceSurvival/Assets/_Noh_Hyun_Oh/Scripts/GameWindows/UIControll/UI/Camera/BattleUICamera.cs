using UnityEngine;


/// <summary>
/// �� ��������ġ�� ������ ī�޶� ��ġ����
/// </summary>
public class BattleUICamera : MonoBehaviour
{

    [SerializeField]
    int index = 0;
    public int Index => index;
    /// <summary>
    /// ĳ���� ���� ī�޶�
    /// </summary>
    Camera actionCam;
   
    /// <summary>
    /// ����ī�޶� ��ġ�� 
    /// </summary>
    Vector3 camPosition = Vector3.zero;

    /// <summary>
    /// �������� ������Ʈ
    /// </summary>
    private Transform target = null;
    public Transform TObj 
    {
        get => target;
        set 
        {
            target = value;
        }
    }

    /// <summary>
    /// ī�޶�� ��ǥ���� ����
    /// </summary>
    [SerializeField]
    [Range(-1.0f,1.0f)]
    float distance = 1.0f;
    
    /// <summary>
    /// ī�޶� ��ǥ�� �ٶ󺸴� ����
    /// </summary>
    Vector3 dir;
    private void Awake()
    {
        actionCam = GetComponent<Camera>();
    }

   
    //IEnumerator FollowCamera()
    //{
    //    while (true)
    //    {

    //        if (TargetObject != null) 
    //        {
    //            TrackingValueSetting();
    //        }
    //        yield return null;

    //    }

    //}
    private void Update()
    {
       
        if (TObj != null) TrackingValueSetting();
    }
    /// <summary>
    /// ī�޶� Ư�� ��ǥ�� ���� x,y ������ z ���������� �ٶ󺸰Ը����.
    /// </summary>
    private void TrackingValueSetting() 
    {
        camPosition = target.position; //Ÿ�� ��ġ��������
        camPosition.z += distance; // �Ÿ������ϰ� 
        actionCam.transform.position = camPosition; //Ÿ�� ��ġ�� �Ÿ������Ѹ�ŭ ��� 

        dir = target.position - transform.position; //Ÿ�� ���⺤�� ���ϰ�
        dir.Normalize(); //���Ⱚ�� �����ϰ� 
        actionCam.transform.rotation = Quaternion.LookRotation(dir,Vector3.up); //������ͷ� �ٶ󺸰� ����� y��������� ������.
    }

}
