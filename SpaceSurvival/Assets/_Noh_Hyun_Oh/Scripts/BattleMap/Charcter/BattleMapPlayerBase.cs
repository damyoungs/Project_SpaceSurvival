using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapPlayerBase : PlayerBase_PoolObj , ICharcterBase
{
    /// <summary>
    /// ������ UI 
    /// </summary>
    private TrackingBattleUI battleUI = null;
    public TrackingBattleUI BattleUI
    {
        get => battleUI;
        set => battleUI = value;

    }

    /// <summary>
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;


    UICamera viewPlayerCamera;


    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI ���� ĵ������ġ
        InitUI();//��ó�� 
    }

    protected override void OnEnable()
    {
        if (battleUICanvas != null)  //ĵ���� ��ġ�� ã�Ƴ�����
        {
            InitUI();//�ʱ�ȭ
        }
    }

    /// <summary>
    /// ������ UI �ʱ�ȭ �Լ� ����
    /// </summary>
    public void InitUI()
    {
        if (BattleUI != null) //���� ������
        {
            BattleUI.gameObject.SetActive(true); //Ȱ��ȭ�� ��Ų��
        }
        else //������ UI�� ���þȵ������� �����Ѵ�
        {
            GameObject obj = Multiple_Factory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // ����ó�� �ʱ�ȭ�Ҷ� ��Ʋ UI �����ϰ� 
            obj.gameObject.name = $"{name} _ Tracking"; //�̸�Ȯ�ο�
            obj.transform.SetParent(battleUICanvas);//Ǯ�� ĵ���� �ؿ����⶧���� ��Ʋ��UI�� ������ ĵ���� ��ġ ������ �̵���Ų��.
            obj.gameObject.SetActive(true); //Ȱ��ȭ ��Ų��.
            BattleUI = obj.GetComponent<TrackingBattleUI>(); //UI ���۳�Ʈ ã�ƿ´�.
            BattleUI.Player = transform.GetChild(0);     //UI �� ���ְ� 1:1 ��ġ�� ���־�� ������ ��Ƶд�.
        }
        if (viewPlayerCamera == null)  //ī�޶� ���þȵ������� 
        {
            viewPlayerCamera = EtcObjects.Instance.TeamCharcterView;// EtcObject �� �̸� ������ ���ӿ�����Ʈ �������� ť�� �������̴� 
            Transform cameraTarget = transform.GetChild(0); //ĳ������ġ
            viewPlayerCamera.TargetObject = cameraTarget.GetChild(cameraTarget.childCount-1); //ĳ���;ȿ� �ǹؿ� ī�޶� Ÿ���� �����־ߦi�ƴٴѴ�.
            viewPlayerCamera.gameObject.SetActive(true); //���ó������� Ȱ��ȭ��Ű��
        }
    }

    /// <summary>
    /// �������� ������ ������
    /// ���� �ʱ�ȭ ��Ű�� Ǯ�� ������ ť�� ������.
    /// </summary>
    public void ResetData()
    {
        if (BattleUI != null) //��Ʋ UI�� ���õ������� 
        {
            BattleUI.ResetData();// ������ UI �ʱ�ȭ 
            BattleUI = null; // ����
        }
        if (viewPlayerCamera != null)
        {
            viewPlayerCamera.TargetObject = null; //Ÿ�� �����
            viewPlayerCamera.gameObject.SetActive(false); // ��Ȱ��ȭ ��Ű�� ���������� ť�� ������.
            viewPlayerCamera = null; //���� �����
        }
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(poolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
    }

}
