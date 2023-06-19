using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class SaveDataSort : MonoBehaviour
{
    /// <summary>
    /// ���� �������ѹ�
    /// �ش簪�� �⺻������ 1�� �������Է�ó���� ������ ���������� �����;��Ѵ�.
    /// �������ѹ��� 0���� �����ؾ��Ѵ�.
    /// </summary>
    [SerializeField]
    int pageIndex = 0;          // ���� �������ѹ� ù��° 0��
    public int PageIndex 
    {
        get => pageIndex;
        set {
            pageIndex = value;

            if( pageIndex < 0) //���������� 0�� ù��°�̴� �׺�������������
            {
                pageIndex = 0;
            }else if (value > lastPageIndex) // �������� �ִ����������ٸ��Ե�����
            { 
                pageIndex = lastPageIndex; //�������������� �����ش�.
            }
               
        }
    }
    /// <summary>
    /// ������ ������Ͽ� ������ �������� ��Ƶд�.
    /// </summary>
    int lastPageIndex = -1;

    /// <summary>
    /// �������� �������� ������ ������Ʈ ����
    /// </summary>
    int lastPageFileViewSize = -1;

    /// <summary>
    /// ���������� ���� ������Ʈ ����
    /// </summary>
    [SerializeField]
    int pageMaxSize = 8;        // ���������� ���� ������Ʈ����

    /// <summary>
    /// ������������ ������Ʈ ����ũ��
    /// </summary>
    [SerializeField]
    float saveDataHeight = 150.0f;

    /// <summary>
    /// ���������� ��ġ�� �޾ƿ���
    /// </summary>
    private GameObject saveWindowObject;

    /// <summary>
    /// �б��������� �����͹޾ƿ���
    /// </summary>
    private JsonGameData[] saveDatas;
   
    /// <summary>
    /// ���������� ������Ʈ�� ���̺굥���� �̸������ؼ� ��������
    /// ������, �������, ������ �ߵ��� ȭ�� ���ε� ����� ����ϱ����� �׼��� �������ش�.
    /// ȭ����ȯ�� �ٽùߵ��ȵ�
    /// </summary>
    private void Start()
    {
        saveWindowObject = SaveLoadManager.Instance.SaveLoadWindow; //�������Ͽ�����Ʈ���� ��� ������Ʈ ��ġ //Awake ���� ��ã�´�.
        saveDatas = SaveLoadManager.Instance.SaveDataList; //���嵥���Ͱ�������
        // ����� ��������ġ�� ������Ʈ�� �����ѳ������� ��ȯ����������� ���ε�
        SaveLoadManager.Instance.readAgain += reLoad;

        Debug.Log("���̵��� ����?�ǳ�9?");
        InitSaveObjects(); //����ȭ�� �ʱⰪ����
        SetLastPageIndex();//���������� ������ ������ �������� ������Ʈ ���� ����
    }

    
    /// <summary>
    /// Ȱ��ȭ�� Ǯ���� ������ ������ ������Ʈ�� ����±���߰�
    /// </summary>
    private void OnEnable()
    {
        if (saveWindowObject != null) { //��ó�� �ʱ�ȭ�Ҷ��� �ڵ����� ����Ǵ� ���� Ȱ��ȭ�ÿ��� üũ����.
            SetPoolBug(); 
        }
    }

    /// <summary>
    /// �����ִ밹�� �� ���� �������� ���̴� ������Ʈ���� �� ������ �������������� �������������� ������ ������Ʈ������ �����´�.
    /// </summary>
    private void SetLastPageIndex() {
        lastPageIndex = (SaveLoadManager.Instance.MaxSaveDataLength / pageMaxSize) + 1;  //������ ���� ��������
        lastPageFileViewSize = SaveLoadManager.Instance.MaxSaveDataLength % pageMaxSize; //������ �������� ������ ������Ʈ���� ��������
    }


    /// <summary>
    /// ȭ�鿡 ������� ������Ʈ���� ��ġ�� �⺻������ �ʱ�ȭ �Լ�
    /// </summary>
    private void InitSaveObjects() {
        
        if (saveWindowObject.transform.childCount > 0) 
        { //���������� �ʱ�ȭ������ Ǯ���� �����Ͽ� 0���̻��̵ȴ�.
            if (saveWindowObject.transform.childCount < pageMaxSize)
            {// �ʱ�ȭ�� ������ ������Ʈ������ ������ �ִ���� ������ ũ�� 
                for (int i = saveWindowObject.transform.childCount; i < pageMaxSize; i++) //�����Ѹ�ŭ �����´�.
                {
                    AddSaveDataObject();
                }

            }
        }
        for (int i = 0; i < pageMaxSize; i++)//����������ŭ�� ������
        {
            saveWindowObject.transform.GetChild(i).localPosition = new Vector3(0, -(saveDataHeight * i), 0);// â��ġ ����ֱ� 
            SetGameObject(null, i);//�ʱⵥ���� ���� (*�ʱⰪ �����̶� ������ 0���������� �����ϱ⶧���� ��������.*)
        }
        //Ʈ�������� �����غ����� �����Ÿ���� ���������ιٲ��� �����Ÿ�� �����Ͽ���.
        saveWindowObject.GetComponent<RectTransform>().sizeDelta = // �����Ÿ�� Vector2�� ���� ������ ����� �����ȴ�. 
                new Vector2(saveWindowObject.GetComponent<RectTransform>().rect.width, //�⺻������ 
                saveDataHeight * pageMaxSize); //�������� �� ���� ������ũ�� ���ϱ�
        SetGameObjectList(); //������ ����
    }

    /// <summary>
    /// ����ȭ�鿡 ���� ������Ʈ �߰� 
    /// Ǯ�� ����Ͽ� �����ð� ������ �ڵ����� �����Ѵ�.
    /// </summary>
    private void AddSaveDataObject() {
        MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.SaveDataPool); // Ǯ�� �ø������� �ٻ��  �ڵ�Ǯ����
    }


    /// <summary>
    /// �����ϰų� �����ϰų� �����ϰų� �Ҷ� Ư��������Ʈ ���ε� ��� 
    /// </summary>
    /// <param name="saveData">���嵥����</param>
    /// <param name="index">ȭ���ε���</param>
    private void reLoad(JsonGameData saveData,  int index)
    {
        SetGameObject(saveData, index);//ȭ�鰻�����ֱ�
    }


    /// <summary>
    /// ���������� ���̴� ������Ʈ���� �����͸� �ٽü����Ѵ�.
    /// </summary>
    public void SetGameObjectList() {
        if (saveDatas == null) { // �о�� �������������°�� ����
            return;
        }
        int startIndex = pageIndex * pageMaxSize; //���������ۿ�����Ʈ��ġ�� ��������
        
        int lastIndex = (pageIndex + 1) * pageMaxSize > SaveLoadManager.Instance.MaxSaveDataLength ? //������ ������ ���� �������� ������ �ε��� ���� ũ��
                            SaveLoadManager.Instance.MaxSaveDataLength : (pageIndex + 1) * pageMaxSize ; //���� �������� ������ �ε����� ����

        for (int i = startIndex; i < lastIndex; i++) { //�����͸� ����������ŭ�� Ȯ���Ѵ�.
            SetGameObject(saveDatas[i],i); // �����͸� �������� 
        }
        
        SetPoolBug();//Ǯ�� ������Ʈ�� 2�辿�ø��µ� �����ϴ°͵��� ��Ȱ��ȭ�۾����ʿ��ؼ� �߰��ߴ�.

    }
    


    /// <summary>
    /// ������Ʈ ��ġã�Ƽ� ������ ����Ѵ�.
    /// </summary>
    /// <param name="index">�������� �ε���</param>
    public void SetGameObject(JsonGameData saveData, int index) {

        int viewObjectNumber = index - (pageIndex * pageMaxSize); //�������� ������Ʈ ��ġã��

        SaveDataObject sd = saveWindowObject.transform.GetChild(viewObjectNumber).GetComponent<SaveDataObject>(); //������Ʈ �����ͼ�

        InitObjectIndex(sd,viewObjectNumber);

        if (saveData != null) { //���嵥���Ͱ� �ִ��� üũ
            
            //�����δ� ������ ���� ������Ƽ Set �Լ��� ȭ�鿡 �����ִ� ��ɳ־����.
            //���̴±�ɼ����� SaveDataObject Ŭ��������.
            sd.FileIndex = saveData.DataIndex; 
            sd.name = saveData.CharcterInfo.CharcterName; //������Ʈ�� �ٲ���� Ȯ�ο�
            sd.CreateTime = saveData.SaveTime;
            sd.Money = saveData.CharcterInfo.Money;
            sd.SceanName = saveData.SceanName;
        }
        else
        {
            sd.FileIndex = index;
            sd.name = $"SaveList  :: {index} :: ��ü����";
            sd.CreateTime = "";
            sd.Money = 0;
            sd.SceanName = "";
            //�ʱ�ȭ����
        }
    }
    /// <summary>
    /// ��ü�� ������Ʈ��ȣ�� �ο��Ѵ� . ó���ѹ���
    /// </summary>
    /// <param name="sdo">��ü��ġ</param>
    /// <param name="index">��ȣ</param>
    private void InitObjectIndex(SaveDataObject sdo, int index) {
        if (sdo.ObjectIndex < 0) { //�ʱⰪ�� -1�̱⶧���� �ʱⰪ�϶��� ���� �����Ѵ�.
            sdo.ObjectIndex = index;  // ������Ʈ�� �����־����
        }
    }
    /// <summary>
    /// Ǯ���� 2�辿�ø��⶧���� �Ⱦ��������� ����� �װ͵��� ��Ȱ��ȭ �ϴ� �۾�.
    /// </summary>
    private void SetPoolBug() {
        for (int i = pageMaxSize; i < saveWindowObject.transform.childCount; i++)//�Ⱦ��� ���ϸ�ŭ�� ������.
        {
            saveWindowObject.transform.GetChild(i).gameObject.SetActive(false); //�Ⱦ������� ����� 
            Debug.Log("���ܶ�+");
        }
    }
}
