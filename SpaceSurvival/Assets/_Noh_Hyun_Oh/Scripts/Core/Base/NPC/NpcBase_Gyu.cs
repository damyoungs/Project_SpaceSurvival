using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase_Gyu : MonoBehaviour
{
    
    /// <summary>
    /// ���Ǿ��� ������ �ε���
    /// </summary>
    int npcId;
    public int Npcid => npcId;

   
    /// <summary>
    /// ���Ǿ��� ���� ��ȭ����
    /// </summary>
    protected TalkType talkType;
    public TalkType TalkType => talkType;
    
    /// <summary>
    /// ��ȭ ���൵ 
    /// </summary>
    protected int talkDepth = 0;
    public int TalkDepth => talkDepth;



    /// <summary>
    /// ��ó���ͼ� ��ȭ�����ϵ��� �����Ű�� ��������Ʈ
    /// </summary>
    public Action<int> onTalkEnableButton;

    /// <summary>
    /// �ֺ��� ����� ��ȣ���� ��������Ʈ
    /// </summary>
    public Action onTalkDisableButton;

    [SerializeField]
    /// <summary>
    /// NPC ���� ���� ī�޶� 
    /// </summary>
    Camera npcCharcterCamera;

    /// <summary>
    /// UI rawImage �� ������ ������ �ؽ��� 
    /// </summary>
    public RenderTexture GetTexture => npcCharcterCamera.targetTexture;
    protected virtual void Awake()
    {
        npcCharcterCamera = transform.GetComponentInChildren<Camera>();
        npcCharcterCamera.targetTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
        npcCharcterCamera.targetTexture.name = $"{name}_�� �ؽ���";
    }

    /// <summary>
    /// ���Ǿ� �ʱ�ȭ�� �Լ� 
    /// </summary>
    /// <param name="npcIndex">���Ǿ� ��ȣ�ο��� ��</param>
    public virtual void InitData(int npcIndex) 
    {
        npcId = npcIndex;
    }

    // ����Ʈ ����Ʈ�߿� ����� �������� �������� ��ɵ� �ʿ� 
    // Ư�� �����Ȳ������ �Ҽ��ִ� ����Ʈ ����� 
    // �Ϸ�� ����Ʈ ���� �����Ѵٴ��� 
    // ����Ʈ���� �ִµ� ���̻� �����Ҽ����� ����Ʈ �����  ��� ������������ ����


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("����");
            onTalkEnableButton?.Invoke(Npcid);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("����");
            onTalkDisableButton?.Invoke();
        }
    }
}
