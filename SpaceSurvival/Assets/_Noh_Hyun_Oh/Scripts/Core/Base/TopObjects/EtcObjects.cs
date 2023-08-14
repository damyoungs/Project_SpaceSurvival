using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        teamCharcterView = GetComponentsInChildren<UICamera>(true); //EtcObject �ؿ��� �׽� 3���� ���� ��ġ�ٲ���ã������ �� �̷��� ã�´�.
        cameraQueue = new Queue<UICamera>(teamCharcterView.Length); //ã�� ������ ť�����ΰ� 
        foreach (UICamera camera in teamCharcterView) //���鼭
        {
            cameraQueue.Enqueue(camera); //ť�� �ϳ��� ����ְ�
            camera.resetData = () =>  //���µɶ� 
            {
                cameraQueue.Enqueue(camera);//�ٽ� �����ְ� �������ش�.
                //Debug.Log(camera.GetHashCode()); //camera ������ ����ִ°��� ����� ������� ���� Ȯ�οϷ� 
            };
        }
    }
}
