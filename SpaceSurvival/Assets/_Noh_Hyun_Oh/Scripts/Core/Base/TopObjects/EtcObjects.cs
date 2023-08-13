using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ÿ ���̵��ÿ��� �������� ���Ƿο�����Ʈ
/// </summary>
public class EtcObjects : Singleton<EtcObjects>
{
    UICamera[] teamCharcterView;
    public UICamera TeamCharcterView => cameraQueue.Dequeue();
    Queue<UICamera> cameraQueue;
    protected override void Awake()
    {
        base.Awake();
        teamCharcterView = GetComponentsInChildren<UICamera>(true);
        cameraQueue = new Queue<UICamera>(teamCharcterView.Length);
        foreach (UICamera camera in teamCharcterView) 
        {
            cameraQueue.Enqueue(camera);
            camera.resetData = () => 
            {
                cameraQueue.Enqueue(camera);
                Debug.Log(camera.GetHashCode());
            };
        }
    }
}
