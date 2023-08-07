using EnumList;
using System.Collections;
using UnityEngine;


/// <summary>
/// UI ���� ���� ĳ���� ����ٴ� ī�޶� 
/// Texture ������ ī�޶�
/// </summary>
public class UICamera : MonoBehaviour ,ICameraBase
{

    [SerializeField]

    EnumList.CameraTargetType cameraTarget = EnumList.CameraTargetType.Player;
    public CameraTargetType TargetType 
    {
        get => cameraTarget; 
        set => cameraTarget =value; 
    }
    

    /// <summary>
    /// ĳ���� ���� ī�޶�
    /// </summary>
    Camera actionCam = null;
    public Camera FollowCamera 
    { 
        set 
        { 
            actionCam = value; 
        } 
    }
   

    [SerializeField]
    /// <summary>
    /// �������� ������Ʈ
    /// </summary>
    private Transform target = null;
    public Transform TargetObject 
    {
        get => target; 
        set => target = value; 
    }

    public Vector3 Distance 
    { 
        get => distance; 
        set => distance =value; 
    }

    public float FollowSpeed { get => 0.0f; set{ } }

    /// <summary>
    /// ī�޶�� ��ǥ���� ����
    ///  - 1 ĳ���� �󱼺���  1 ĳ���� ����� ����
    /// </summary>
    [SerializeField]
    [Range(-1.0f,1.0f)]
    Vector3 distance = new(0.0f,0.0f,1.0f);
    
   
    private void Awake()
    {
        //gameObject.layer = LayerMask.NameToLayer("UI");///������ ���̾� �˻��ؼ� ��ȣ ��������
        //gameObject.tag = "Respawn"; //�±� �����Ű��
        actionCam = GetComponent<Camera>();
    }


    IEnumerator MoveCamera()
    {
        while (cameraTarget != EnumList.CameraTargetType.None)
        {
            TrackingValueSetting();
            yield return null;

        }
    }
    
    private void OnEnable()
    {
        StartCoroutine(MoveCamera());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// ī�޶� Ư�� ��ǥ�� ���� x,y ������ z ���������� �ٶ󺸰Ը����.
    /// </summary>
    private void TrackingValueSetting() 
    {
        transform.position = target.transform.position // ��ǥ �������� 
                            -(target.transform.forward * distance.z); // ��ǥ�� ������⿡  distance �� ���ѵ�  - �� �ؼ� �ٶ󺸴¹��� �ݴ������� ��ġ ��Ų��.

        actionCam.transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up); //������ͷ� �ٶ󺸰� ����� y��������� ������.
    }

}
