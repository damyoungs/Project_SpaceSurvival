using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class WeatherLightCameraBase : MonoBehaviour
{
    [Header("����ī�޶� ���� ����")]

    /// <summary>
    /// PCĳ���� ������Ʈ ��ġ�� ã������ �����´�. 
    /// </summary>
    [SerializeField]
    private GameObject player = null;

    /// <summary>
    /// ����ī�޶� ������Ʈ 
    /// </summary>
    [SerializeField]
    protected GameObject mainCamera = null;
    
    /// <summary>
    /// �¾� ��  ������Ʈ
    /// </summary>
    [SerializeField]
    protected GameObject DirectionalLight = null;

    /// <summary>
    /// ĳ���Ϳ��� ���� ����
    /// </summary>
    [SerializeField]
    private float cameraIntervalY = 10.0f;
    
    public float CameraIntervalY
    {
        get => cameraIntervalY;

        protected set
        {
            cameraIntervalY = value;
        }
    }
    /// <summary>
    /// ī�޶� ���ͺ並 �����Ҽ��ְ��ϴ� ī�޶� ����
    /// </summary>
    [SerializeField]
    private float cameraAngle = 40.0f;
    public float CameraAngle
    {
        get => cameraAngle;
        protected set
        {
            cameraAngle = value;
        }
    }
    /// <summary>
    /// ī�޶��  ĳ���Ϳ��� �Ÿ� ����
    /// </summary>
    [SerializeField]
    private float cameraIntervalZ = -10.0f;
    public float CameraIntervalZ
    {
        get => cameraIntervalZ;
        protected set
        {
            cameraIntervalZ = value;
        }
    }
   
    //[Header("�¾�� ���� ����")]
    //private float test1 = 0.0f;

    //[Header("���� ���� ����")]
    //[SerializeField]
    //private float test2 = 0.0f;
    /// <summary>
    /// ������Ʈ������ �±׷� ã�ƺ���.
    /// ����Ǵ��ڵ�.
    /// </summary>
    private void Awake()
    {
        if (player == null) { 
            player = GameObject.FindWithTag("Player");
        }
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindWithTag("MainCamera").gameObject;
        }
        if (DirectionalLight == null) 
        { 
            DirectionalLight = GameObject.FindWithTag("Light").gameObject;
        }
        Debug.Log($"player = {player} \n mainCamera = {mainCamera}  DirectionalLight = {DirectionalLight}");
        
    }
    /// <summary>
    /// �÷��̾ ������ �÷��̾� �������� ī�޶���ġ��ų� 
    /// ������ ������ġ�� ��´�.
    /// </summary>
    private void Start()
    {
        if (player != null)
        {
            mainCamera.transform.Rotate(new Vector3(CameraAngle, 0, 0)); 
            MoveActionChangeWLC(player.transform.position);
        }
        else
        {
            DirectionalLight.transform.Translate(new Vector3(0, cameraIntervalY, 0));
            //ī�޶��� �������� �����ʿ䰡 �ִ�.
            mainCamera.transform.Translate(new Vector3(0, cameraIntervalY, cameraIntervalZ));
            mainCamera.transform.Rotate(new Vector3(40, 0, 0));
            //DirectionalLight.transform.Rotate(new Vector3(40, 0, 0));
        }

    }

    public virtual void CameraAngleSetting(GameObject mainCamera) {
        
    }

    /// <summary>
    /// ĳ������ �������� ���������� ī�޶� ���󰣴�
    /// ĳ���Ͱ� �ٶ󺸴� ������ ��������� ã���ʿ䰡����
    /// ī�޶� �̵��� �ڿ������� �̵���Ű�� �����ʿ�
    /// </summary>
    /// <param name="playerPosition">ĳ������ ��ġ</param>
    public virtual void MoveActionChangeWLC(Vector3 playerPosition) {
        //ĳ���� �������� �󸶳� �������� ����ʿ��� �ٶ󺸰��ִ��������� �������� �ش�Ŭ�����ȿ��� �����Ѵ�.
        mainCamera.transform.position = playerPosition + new Vector3(
                                               0,
                                               cameraIntervalY,
                                               cameraIntervalZ
                                               );
        Mathf.Sin(transform.position.x);
        //DirectionalLight.transform.Rotate();
        Debug.Log($"main : {mainCamera.transform.position}");
    }
}
