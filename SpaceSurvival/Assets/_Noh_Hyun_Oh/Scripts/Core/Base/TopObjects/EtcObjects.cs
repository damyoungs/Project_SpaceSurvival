using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기타 맵이동시에도 공통으로 사용되로오브젝트
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
