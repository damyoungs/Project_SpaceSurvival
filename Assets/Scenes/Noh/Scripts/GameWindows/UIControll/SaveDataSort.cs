using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ȭ�鿡 ���̴°͸� �Ű澲��
/// </summary>
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
            }
            else if (value > lastPageIndex) // �������� �ִ����������ٸ��Ե�����
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
    /// ���������� ������Ʈ�� ���̺굥���� �̸������ؼ� ��������
    /// ������, �������, ������ �ߵ��� ȭ�� ���ε� ����� ����ϱ����� �׼��� �������ش�.
    /// ȭ����ȯ�� �ٽùߵ��ȵ�
    /// </summary>
    private void Start()
    {
        saveWindowObject = SaveLoadManager.Instance.SaveLoadWindow; //�������Ͽ�����Ʈ���� ��� ������Ʈ ��ġ //Awake ���� ��ã�´�.
        
        //saveDatas = SaveLoadManager.Instance.SaveDataList; //���嵥���Ͱ������� �񵿱��ϸ� ��ŸƮŸ�ֿ̹� ��������
        // ����� ��������ġ�� ������Ʈ�� �����ѳ������� ��ȯ����������� ���ε�

        SaveLoadManager.Instance.saveObjectReflash += SetGameObject;
        SaveLoadManager.Instance.isDoneDataLoaing += SetGameObjectList; //������ �񵿱�� ó���� ������ ȭ�鸮�� �ڵ����� ���ֱ����� �߰�

        InitSaveObjects(); //����ȭ�� �ʱⰪ����
        SetLastPageIndex(); //����¡�� ���� �ʱⰪ����
        SaveLoadManager.Instance.isDoneDataLoaing += SetGameObjectList;
    }
    
    /// <summary>
    /// Ȱ��ȭ�� Ǯ���� ������ ������ ������Ʈ�� ����±���߰�
    /// </summary>
    private void OnEnable()
    {
        if (saveWindowObject != null) { //��ó�� �ʱ�ȭ�Ҷ��� �ڵ����� ����Ǵ� ���� Ȱ��ȭ�ÿ��� üũ����.
            SetPoolBug(pageMaxSize,saveWindowObject.transform.childCount);
            if (saveWindowObject.activeSelf) {//������Ʈ�� Ȱ��ȭ�Ǿ� �����̰����ϱ⶧���� üũ����
                
                SetGameObjectList(SaveLoadManager.Instance.SaveDataList); //�ʱ�ȭ �۾��� �񵿱�� ���ϵ����͸� �о���⶧���� �����̾ȉ��������ִ� 
            }
        }
        
    }
    
    /// <summary>
    /// Ǯ���� 2�辿�ø��⶧���� �Ⱦ��������� ����� �װ͵��� ��Ȱ��ȭ �ϴ� �۾�.
    /// </summary>
    /// <param name="startIndex">���� �ε���</param>
    /// <param name="lastIndex">�� �ε���</param>
    private void SetPoolBug(int startIndex , int lastIndex)
    {
        for (int i = startIndex; i < lastIndex; i++)//�Ⱦ��� ���ϸ�ŭ�� ������.
        {
            saveWindowObject.transform.GetChild(i).gameObject.SetActive(false); //�Ⱦ������� ����� 
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
    /// ����ȭ�鿡 ���ϸ�ŭ ������ƮǮ�̿��Ͽ� �����Ϳ�����Ʈ �����Ͽ� �����ϴ� �Լ�
    /// </summary>
    private void InitSaveObjects() {

        int childCount = saveWindowObject.transform.childCount;
        //pageMaxSize ��ŭ ȭ�鿡 ���� ������Ʈ�� �����ؼ� �����´�.
        if (childCount> 0) 
        { //���������� �ʱ�ȭ������ Ǯ���� �����Ͽ� 0���̻��̵ȴ�.
            if (childCount < pageMaxSize)
            {// �ʱ�ȭ�� ������ ������Ʈ������ ������ �ִ���� ������ ũ�� 
                for (int i = childCount; i < pageMaxSize; i++) //�����Ѹ�ŭ �����´�.
                {
                    MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.SAVEDATAPOOL); // Ǯ�� �ø������� �ٻ��  �ڵ�Ǯ����
                }

            }
        }

        SetPoolBug(pageMaxSize , saveWindowObject.transform.childCount);//2�辿�þ�°��߾Ⱦ��°� ��Ȱ��ȭ
        
        for (int i = 0; i < pageMaxSize; i++)//����������ŭ�� ������
        {
            saveWindowObject.transform.GetChild(i).localPosition = new Vector3(0, -(saveDataHeight * i), 0);// â��ġ ����ֱ� 
            SetGameObject(null, i);//�ʱⵥ���� ���� (*�ʱⰪ �����̶� ������ 0���������� �����ϱ⶧���� ��������.*)
        }
        
        //Ʈ�������� �����غ����� �����Ÿ���� ���������ιٲ��� �����Ÿ�� �����Ͽ���.
        saveWindowObject.GetComponent<RectTransform>().sizeDelta = // �����Ÿ�� Vector2�� ���� ������ ����� �����ȴ�. 
                new Vector2(saveWindowObject.GetComponent<RectTransform>().rect.width, //�⺻������ 
                saveDataHeight * pageMaxSize); //�������� �� ���� ������ũ�� ���ϱ�
    }

    /// <summary>
    /// �����ϰų� �����ϰų� �����ϰų� �Ҷ� Ư��������Ʈ ���ε� ��� 
    /// �����Ǵ� �ε������� �־��ָ�ȴ�
    /// </summary>
    /// <param name="saveData">������ ���嵥����</param>
    /// <param name="index">������ �ε���</param>
    public void SetGameObject(JsonGameData saveData, int fileIndex) {

        int viewObjectNumber = fileIndex - (pageIndex * pageMaxSize); //�������� ������Ʈ ��ġã��
        //���������Ʈ �ѹ�(0 ~ pageMaxSize) =  �������Ϲ�ȣ(0 ~ �������ִ밪) - (�������ѹ�(������) 0�����ͽ��� * ���������� ���̴� ���� ) 

        SaveDataObject sd = saveWindowObject.transform.GetChild(viewObjectNumber).GetComponent<SaveDataObject>(); //������ ������Ʈ �����´�.

        sd.ObjectIndex = viewObjectNumber; //������Ʈ �ѹ����� ���ش� 

        if (saveData != null) { //���嵥���Ͱ� �ִ��� üũ
            
            //�����δ� ������ ���� ������Ƽ Set �Լ��� ȭ�鿡 �����ִ� ��ɳ־����.
            //���̴±�ɼ����� SaveDataObject Ŭ��������.
            sd.FileIndex = saveData.DataIndex; 
            sd.name = saveData.CharcterInfo[0].CharcterName; //������Ʈ�� �ٲ���� Ȯ�ο�
            sd.CreateTime = saveData.SaveTime;
            sd.Money = saveData.CharcterInfo[0].Money;
            sd.SceanName = saveData.SceanName;
        }   
        else
        {
            //�⺻�� ����
            sd.FileIndex = fileIndex; //�⺻���� ���� �ѹ���
            sd.name = "";
            sd.CreateTime = "";
            sd.Money = 0;
            sd.SceanName = EnumList.SceanName.NONE;
        }
    }


    /// <summary>
    /// ���������� ���̴� ������Ʈ���� �����͸� �ٽü����Ѵ�.
    /// <param name="saveDataList">ȭ�鿡 �Ѹ� �����͸���Ʈ</param>
    /// </summary>
    public void SetGameObjectList(JsonGameData[] saveDataList)
    {
        if (saveDataList == null)
        { // �о�� �������������°�� ����
            Debug.Log("�ʱ�ȭ?");
            return;
        }
        int startIndex = pageIndex * pageMaxSize; //���������ۿ�����Ʈ��ġ�� ��������

        int lastIndex = (pageIndex + 1) * pageMaxSize > SaveLoadManager.Instance.MaxSaveDataLength ? //���ϸ���Ʈ �ִ밪 < ��������¡�� * ȭ�鿡���������ִ������Ʈ��
                            SaveLoadManager.Instance.MaxSaveDataLength : //�������������� ���������� ����
                            (pageIndex + 1) * pageMaxSize; //�ƴϸ� �Ϲ����� ����¡ 

        for (int i = startIndex; i < lastIndex; i++)
        { //�����͸� ����������ŭ�� Ȯ���Ѵ�.
            SetGameObject(saveDataList[i], i); // �����͸� �������� 
        }
        int visibleEndIndex = lastIndex - startIndex; //�������� ������ �ε������� �ش�.
        SetPoolBug(visibleEndIndex, saveWindowObject.transform.childCount);//Ǯ�� ������Ʈ�� 2�辿�ø��µ� �����ϴ°͵��� ��Ȱ��ȭ�۾����ʿ��ؼ� �߰��ߴ�.

    }

}
