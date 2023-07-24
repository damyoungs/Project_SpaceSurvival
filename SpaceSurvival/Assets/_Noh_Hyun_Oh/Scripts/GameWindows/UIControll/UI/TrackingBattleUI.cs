using EnumList;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ UI ��� Ŭ���� 
/// �����̻� �� ü�� ���� UI ��� ����ִ� ���۳�Ʈ
/// </summary>
public class TrackingBattleUI : TrackingBattleUIObjectIsPool 
{
#if UNITY_EDITOR
    public bool isDebug = false; //����� ǥ������ üũ�ڽ� �����Ϳ� �߰�����
#endif

    /// <summary>
    /// ������ ������Ʈ ����
    /// </summary>
    [SerializeField]
    private ITurnBaseData player = null;
    public ITurnBaseData Player
    {
        get => player;
        set
        {
            player = value;
        }

    }

    /// <summary>
    /// ���� �����ְ��ִ� ī�޶� �Ÿ����������� ���ȴ� 
    /// </summary>
    [SerializeField]
    private Camera mainCamera;

    /// <summary>
    /// ī�޶���� �Ÿ� �ּҰ� �̰Ÿ� ���ϰ� �Ǹ� �Ⱥ�����ߵȴ�.
    /// </summary>
    float minDirection = 3.0f;

    /// <summary>
    /// ī�޶���� �Ÿ� �ִ밪 �̰Ÿ� �̻��� �Ǹ� �Ⱥ�����ߵȴ�
    /// </summary>
    float maxDirection = 20.0f;

    /// <summary>
    /// ī�޶�� ĳ���Ͱ��� �⺻�Ÿ��� ī�޶� Ÿ�������� ����ִ� ĳ���Ͱ��� �Ÿ� 
    /// </summary>
    float defaultDir = 10.0f;

    /// <summary>
    /// �����̻� ���ٿ� �� ����
    /// </summary>
    const int gridWidthLength = 4;

    /// <summary>
    /// 1�������� ���ٿ� ���� ������ ������
    /// �������� �ѹ����ϱ����� �ļ� 
    /// </summary>
    readonly float gridWidthScale = 1.0f / gridWidthLength;


    /// <summary>
    /// �����̻��� �� �׸��巹�̾ƿ� �׷�
    /// </summary>
    GridLayoutGroup glg;

    /// <summary>
    /// ���������� �����̻� ��ġ��  
    /// </summary>
    RectTransform rtTop;

    /// <summary>
    /// �Ʒ��� ������ ü�¹� ���¹̳� ������ ��ġ��
    /// </summary>
    RectTransform rtBottom;

    /// <summary>
    /// �����̻� ��ġ�� ���� ����
    /// </summary>
    Vector2 topDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// ü�¹� ���¹̳� ��ġ�� ���� ����
    /// </summary>
    Vector2 bottomDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// �����̻� ũ�������� ����
    /// </summary>
    Vector2 topDefaultDeltaSize = Vector2.zero;

    /// <summary>
    /// ü�� �� ���¹̳� ũ�������� ����
    /// </summary>
    Vector2 bottomDefaultSize = Vector2.zero;

    /// <summary>
    /// �����̻��� ũ�⺯���� ���� ���Ǵ� ����
    /// </summary>
    Vector2 topGroupCellSize = Vector2.zero;

    /// <summary>
    /// �����̻��� �þ��� �׸�����ġ���������� ���Ǵ� ����
    /// </summary>
    Vector2 gridPos = Vector2.zero;

    /// <summary>
    /// �����̻� UI �� �׸���׷� ��ġ��
    /// </summary>
    Transform stateGroup;

    /// <summary>
    /// �����̻��� �ɷ��� �߰� �Ǵ� ��� ȣ��
    /// </summary>
    public Action<StateType> statusAbnormalities;

    /// <summary>
    /// ���ӽð��� �����ų� �������� ����ؼ� ���°� ������ ��� ȣ��
    /// </summary>
    public Action<IStateData> releaseStatus;

    /// <summary>
    /// �����̻� �ִ밹�� 
    /// </summary>
    int stateSize = 4;
    int StateSize {
        get => stateSize;
        set
        {
            if (value > stateSize) //ũ�Ⱑ �⺻�������ũ�� 
            {
                stateSize *= 2; //�ϴ� �ι�� �ø���
                IStateData[] temp = states; //���������� ����صд�.
                states = new IStateData[stateSize]; // �ø������ŭ ���� �迭 �����. 
                for (int i = 0; i < temp.Length; i++)
                {
                     states[i] = temp[i]; //���������� �ٽ� ��´�.
                }
            }
        }
        
    }

    /// <summary>
    /// ���� �����̻� ���� ��Ƶ� �迭
    /// </summary>
    private IStateData[] states;
    public IStateData[] States => states;

    /// <summary>
    /// �ʱⰪ���� �����صд� ���߿� �Ÿ������� ������������ ����Ұ�
    /// </summary>
    private void Awake()
    {
        stateGroup = transform.GetChild(0);
        glg = stateGroup.GetComponent<GridLayoutGroup>();
        rtTop = stateGroup.GetComponent<RectTransform>();
        rtBottom = transform.GetChild(1).GetComponent<RectTransform>();

        //��ó�� ������� ������(�⺻)���� �����صд�.
        topGroupCellSize = glg.cellSize;
        //�����̻�
        topDefaultAnchoredPosition = rtTop.anchoredPosition;
        topDefaultDeltaSize = rtTop.sizeDelta;
        //ü�� ���¹̳�
        bottomDefaultAnchoredPosition = rtBottom.anchoredPosition;
        bottomDefaultSize = rtBottom.sizeDelta;


        statusAbnormalities += AddOfStatus; // �����̻��� �߰��ɶ� ȣ��� �Լ����

        states = new IStateData[stateSize]; // �����̻��� �迭ũ�⸦ ��Ƶд�.
        
        isPositionReset = false; //��ġ�� �ʱ�ȭ ����
    }

    /// <summary>
    /// Ȱ��ȭ�� �����ϱ�
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        InitTracking();
    }
   
    /// <summary>
    /// ��Ʋ�� ���۽� �ʱ�ȭ�� ���� ���� 
    /// </summary>
    private void InitTracking() 
    {
        mainCamera = FindObjectOfType<Camera>(); //ī�޶� ã�Ƽ� �����Ѵ�. - �ó׸ӽ����� ī�޶� ��ȯ�� �ش簪�� ��ü�ϴ� ������ �߰��� �ʿ�
        //mainCamera = Camera.main; //����Ƽ���� �������ִ°� ����غ��� awake������ �������´�. OnEnable ó��ȣ�⿡�����������´�.
        StopAllCoroutines();//���������ϴ��������� ���߰� 
        StartCoroutine(StartTracking()); //������ô
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    IEnumerator StartTracking() 
    {
        while (true)
        {
            SetTrackingUI(); //ĳ���Ͱ� �����϶����� UI ũ��� ��ġ�� �����������Ѵ�. 
            yield return null;

        }
    }
    /// <summary>
    /// ī�޶�� �÷��̾�� �Ÿ��� �缭 ������ UI ũ�⸦ ������Ű�� �Լ�
    /// </summary>
    private void SetTrackingUI()
    {
        if (Player != null) //�÷��̾ �����ϸ�
        {
            Vector3 playerPosition = mainCamera.WorldToScreenPoint(Player.transform.position); //�÷��̾� ��ũ����ǥ�� �о�´�.
            transform.position = playerPosition; //������ ������Ʈ�� ��ġ�� �i�ư���.
#if UNITY_EDITOR
            if (isDebug) 
            {
                Debug.Log(playerPosition);
            }
#endif
            //UI���̴� �Ÿ� ������ Ȯ��
            if (playerPosition.z < maxDirection && playerPosition.z > minDirection) //�ش�����ȿ����� �����ش�. z�������θ���������� �߰����ָ�ȴ�.
            {
                //�������� ��������� ������ �����ϰ� �־����� ������ �����ϴ� ������ �ϰ������ ���ϱ�� �Ϸ��� if���� ��û���� 
                float scale = defaultDir / playerPosition.z; //������ �̱����� ���ϱ⿬������ ������;����� ����� ��ã�Ѵ�.
                                                             //�ʱⰪ���� ������ ���ؼ� ���������Ѵ�.
                                                             ////�������� ����������
                rtBottom.anchoredPosition = bottomDefaultAnchoredPosition * scale;
                rtBottom.sizeDelta = bottomDefaultSize * scale;

                ////�����̻�
                glg.cellSize = topGroupCellSize * scale; //������� �����ϰ� 
                gridPos = topDefaultAnchoredPosition * scale; //�����̻�⺻��ġ�� �����ѵ� 
                //�����̻��� ���ٿ��� ���� , ���ٿ��� ���� ���� ����ɶ��� ���� ������Ѵ�.
                gridPos.y += (glg.cellSize.y * //�������ŭ ������ �߰��Ѵ� 
                                (int)((transform.GetChild(0).childCount - 1) * gridWidthScale)); //������ �Ѿ�� 

                rtTop.anchoredPosition = gridPos; //������ ���� ����       
                
                rtTop.sizeDelta = topDefaultDeltaSize * scale; //�����̻��� ũ�⸦ ����

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true); //ȭ�鿡 �����ش�.
                }

            }
            else //����� 
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false); //�Ⱥ����ش�.
                }
            }
        }
    }
   
    /// <summary>
    /// ���ϴ� �����̻��� ���൵�� ���ҽ�Ű�� �Լ�  (�����ϴ��Լ�)
    /// �ϸ޴������� �ϰ�����  - �׽�Ʈ ������
    /// </summary>
    public void TrunActionStateChange() 
    {
        for (int i=0; i<  states.Length; i++) 
        {
            if (states[i] == null) continue; //���̸� �������� 
            states[i].CurrentDuration += states[i].ReducedDuration; //���� �����ϸ� �����̻� ����
            if (states[i].CurrentDuration < 0.0f) // �����̻� ���ӽð��� �������� 
            {
                releaseStatus?.Invoke(states[i]);//����������ٰ� ��ȣ�� ������.
                states[i] = null; //�迭���� ����
            } 
        }

    }

    /// <summary>
    /// ���º�ȭ �߻�(�߰�)�� UI �߰� 
    /// �����ʿ��� ȣ�� �ϴ� �Լ� 
    /// </summary>
    /// <param name="stateData">�����̻� �� Ÿ��</param>
    public void AddOfStatus(EnumList.StateType type)
    {
        IStateData stateData = SettingStateUI(type); //Ǯ���� ��ü�����ͼ� UI����
        AddStateArray(stateData); //�����ͼ���
        
    }

    /// <summary>
    /// ���� �������� �����̻� ������ ����
    /// �迭�� �߰��ϰ� �����Ѵ�.
    /// </summary>
    /// <param name="addData">�����̻��� ����</param>
    private void AddStateArray(IStateData addData) 
    {
        for(int i=0; i< StateSize; i++)//��ü�˻��غ��� 
        {
            if (states[i] == null) //������ִ°�� 
            {
                states[i] = addData;//�߰��ϰ�
                return;//����������.
            }
        }
        StateSize++; //���¸���Ʈ ���������� �迭 ������ø��� 
        AddStateArray(addData); // �Լ��� �ٽ�ȣ���ؼ� �迭�� �߰���Ų��.
    }

    /// <summary>
    /// �����̻��� �ߵ��� ȣ��� �Լ� 
    /// �����̻󺰷� UIó������ ������ �߰��ҿ���
    /// </summary>
    /// <param name="type">�߰��� �����̻�Ÿ��</param>
    /// <returns>�����̻��� ������ �����ؼ� ��ȯ</returns>
    private IStateData SettingStateUI(EnumList.StateType type)
    {
        GameObject obj = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.STATE_POOL); //Ǯ���� ������
        StateObjectIsPool poolObj = obj.GetComponent<StateObjectIsPool>(); //������Ʈ �����о�ͼ� 
        poolObj.transform.SetParent(stateGroup);// �θ� �����ϰ� 

        ///�ؿ��� �̹��� ���� 
        switch (type)
        {
            case EnumList.StateType.ElectricShock:
                //�̹�������
                break;
            case EnumList.StateType.Poison:

                break;
            case EnumList.StateType.Freeze:

                break;
            case EnumList.StateType.Burns:

                break;
            case EnumList.StateType.Fear:

                break;
            default:
                break;
        }
        return poolObj;
    }

    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// ���ð� �ʱ�ȭ�� Ǯ�� ������ɷ��ִ� �����̻� �� ���� �ʱ�ȭ 
    /// ���������� ť�� �߰��ϰ� Ǯ�� ����������.
    /// </summary>
    public  void ResetData() 
    {
        //��������Ʈ �ʱ�ȭ 
        statusAbnormalities = null;
        releaseStatus = null;
        //�Ÿ�������� ī�޶�� �������̵� �÷��̾� ������ ���� 
        mainCamera = null;
        Player = null;

        for (int i = 0; i < StateSize; i++) //�����̻� ������ ����
        {
            if (states[i] != null) //���� ��� �ִ°͵� ã�Ƽ�
            {
                states[i].ResetData(); //���ΰ��� �ʱ�ȭ�� Ǯ�� �������۾� �ϰ�
                states[i] = null; // ������ ����
            }
        }
       
        transform.SetParent(PoolTransform);//Ǯ�� ������
        gameObject.SetActive(false); //ť�� ������ 
        //�ʱ�ȭ �������� Ǯ�� ������.

        
    }


    


}
