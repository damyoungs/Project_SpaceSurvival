using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ÿ ���̵��ÿ��� �������� ���Ƿο�����Ʈ
/// </summary>
public class EtcObjects : Singleton<EtcObjects>
{
    UICamera[] teamCharcterView; //ĳ���� ��û����ʿ� ����ī�޶� 3��
    public UICamera TeamCharcterView => cameraQueue.Dequeue(); //ī�޶� �ٶ� ť���� �������ش�.
    Queue<UICamera> cameraQueue; //ī�޶� ������ ť
    protected override void Awake()
    {
        base.Awake();
        WindowList windowList = FindObjectOfType<WindowList>();
        TeamBorderManager teamBorderManager = windowList.GetComponentInChildren<TeamBorderManager>(true);
        RawImage[] rawImages =  teamBorderManager.GetComponentsInChildren<RawImage>(true);
        Debug.Log($"{windowList}{teamBorderManager}{rawImages.Length}");
        teamCharcterView = GetComponentsInChildren<UICamera>(true); //EtcObject �ؿ��� �׽� 3���� ���� ��ġ�ٲ���ã������ �� �̷��� ã�´�.
        cameraQueue = new Queue<UICamera>(teamCharcterView.Length); //ã�� ������ ť�����ΰ� 
        int i = 0;
        foreach (UICamera camera in teamCharcterView) //���鼭
        {
            rawImages[i].texture =  camera.FollowCamera.activeTexture; //�ý��� ����ֱ�
            camera.resetData = () =>  //���µɶ� 
            {
                cameraQueue.Enqueue(camera);//�ٽ� �����ְ� �������ش�.
                //Debug.Log(camera.GetHashCode()); //camera ������ ����ִ°��� ����� ������� ���� Ȯ�οϷ� 
            };
            i++;
            camera.gameObject.SetActive(false);
        }
    }
}
