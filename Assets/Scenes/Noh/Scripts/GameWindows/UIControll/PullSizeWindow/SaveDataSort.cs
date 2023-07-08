
using System;
using UnityEngine;
using UnityEngine.UI;

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
            if (pageIndex < 0) //���������� 0�� ù��°�̴� �׺�������������
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
    /// ���������� ���� �ִ� �������� ������Ʈ �ִ� ����
    /// </summary>
    [SerializeField]
    int pageViewSaveObjectMaxSize = 8;
    public int PageViewSaveObjectMaxSize => pageViewSaveObjectMaxSize;

    /// <summary>
    /// ������ ������ �� 
    /// </summary>
    int lastPageIndex = -1;
    public int LastPageIndex => lastPageIndex;

    /// <summary>
    /// �������������� ���� �������� ������Ʈ ����
    /// </summary>
    int lastPageObjectLength = -1;

    /// <summary>
    /// ���������Ʈ ����ũ��
    /// </summary>
    float saveObjectHeight = 150.0f;

    /// <summary>
    /// ������ ������Ʈ ���� ����
    /// </summary>
    float pageObjectWidthPadding = 95.0f;

    /// <summary>
    /// ���������� ������Ʈ ��ġ
    /// </summary>
    private GameObject saveWindowObject;

    /// <summary>
    /// ���������� ������Ʈ ��ġ
    /// ����¡ ó���Ұ��� ������Ʈ ��ġ
    /// </summary>
    private GameObject saveWindowPageObject;

    /// <summary>
    /// ������������ �˾������� ��ġ
    /// </summary>
    private SaveLoadPopupWindow saveLoadPopupWindow;

    /// <summary>
    /// ȭ����ȯ�� �ٽùߵ��ȵ� Ȯ�οϷ�.
    /// OnEnable ���� �ʰ� ����ȴ�.
    /// </summary>
    private void Start()
    {

        //�̱��� �������� Awake ���� ��ã�´�. 
        saveWindowObject = SaveLoadManager.Instance.SaveLoadWindow;
        saveWindowPageObject = SaveLoadManager.Instance.SaveLoadPagingWindow;
        saveLoadPopupWindow = WindowList.Instance.IOPopupWindow;

        //��������Ʈ ���� 
        SaveLoadManager.Instance.saveObjectReflash += SetGameObject; //��ɽ���� ������Ʈ�� �ٽñ׸���.
        SaveLoadManager.Instance.isDoneDataLoaing += SetGameObjectList; //������ �񵿱�� ó���� ������ ȭ�鸮�� �ڵ����� ���ֱ����� �߰�
        saveLoadPopupWindow.focusInChangeFunction += SetFocusView; //�����ưŬ���� ó���ϴ� �Լ�����

        InitLastPageIndex(); //����¡�� ���� �ʱⰪ����
        InitSaveObjects(); //����ȭ�� �ʱⰪ����
    }

    /// <summary>
    /// Ȱ��ȭ�� Ǯ���� ������ ������ ������Ʈ�� ����±���߰�
    /// Start�Լ����� ��������ȴ�.
    /// </summary>
    private void OnEnable()
    {

        if (saveWindowObject != null) { //��ŸƮ�Լ����� ��������Ǽ� ó������ ������ �߻��Ѵ�.
            SetGameObjectList(SaveLoadManager.Instance.SaveDataList); //�ʱ�ȭ �۾��� �񵿱�� ���ϵ����͸� �о���⶧���� �����̾ȉ��������ִ� 
        }
    }

    /// <summary>
    /// ���� ���������� ȭ�鿡 ������ �������� ������Ʈ ������ ��ȯ�Ѵ�.
    /// </summary>
    /// <returns>������������ ���������Ʈ����</returns>
    private int GetGameObjectLength()
    {
        return pageIndex > lastPageIndex ? lastPageObjectLength : pageViewSaveObjectMaxSize;
    }

   
    /// <summary>
    /// ���� �ε����� ���� ȭ�鿡 ���������Ʈ�� ������ߵǴ��� ������Ʈ ��ġ�� ��ȯ�Ѵ�.
    /// ���������Ʈ �ѹ�(0 ~ pageMaxSize) =  �������Ϲ�ȣ(0 ~ �������ִ밪) - (�������ѹ�(������) 0�����ͽ��� * ���������� ���̴� ���� ) 
    /// </summary>
    /// <param name="fileIndex">�����ε���</param>
    /// <returns>���� �������� ������Ʈ �ε���</returns>
    private int GetGameObjectIndex(int fileIndex)
    {
        return fileIndex - (pageIndex * pageViewSaveObjectMaxSize);
    }


    /// <summary>
    /// �����ִ밹�� �� ���� �������� ���̴� ������Ʈ���� �� ������ �������������� �������������� ������ ������Ʈ������ �����´�.
    /// </summary>
    private void InitLastPageIndex()
    {
        lastPageIndex = (SaveLoadManager.Instance.MaxSaveDataLength / pageViewSaveObjectMaxSize);  //������ ���� ��������
        lastPageObjectLength = (SaveLoadManager.Instance.MaxSaveDataLength & pageViewSaveObjectMaxSize); //�������������� ������ ������Ʈ ����
    }

    /// <summary>
    /// 1. Ǯ���� ������ ������Ʈ�� ������ ���缭 ���������ʱ⶧��(Ǯ���� �ι�� �ø����۾�)�� �۾��� �߰� - ���߿� Ǯ�� �����ؾߵɵ��ϴ�.
    /// 2. Ǯ���� ������ ������Ʈ�߿� �Ⱦ��� �κ��� �����ִ� �۾�
    /// </summary>
    private void InitSaveObjects()
    {
        //����¡
        int childCount = saveWindowPageObject.transform.childCount; //���� Ǯ���� ������ ������Ʈ ���� �� �����´�. (����¡)
        int proccessLength = GetGameObjectLength(); //������������ ����¡������Ʈ ������ �����´�.
        if (childCount < proccessLength)//������ ������Ʈ�� ȭ�鿡 ������ �������� ������� 
        {
            PoolBugFunction(saveWindowPageObject.transform, childCount, proccessLength, EnumList.MultipleFactoryObjectList.SAVEPAGEBUTTONPOOL);//�����Ѻκа����ͼ� �ʿ���ºκа��߱�
        }

        childCount = saveWindowObject.transform.childCount; //���� Ǯ���� ������ ������Ʈ ���� �� �����´�. (���������Ʈ)
        proccessLength = GetGameObjectLength(); //������������ ���������Ʈ ������ �����´�.
        if (childCount < proccessLength)//������ ������Ʈ�� ȭ�鿡 ������ �������� ������� 
        {
            PoolBugFunction(saveWindowObject.transform, childCount, proccessLength, EnumList.MultipleFactoryObjectList.SAVEDATAPOOL);//�����Ѻκа����ͼ� �ʿ���ºκа��߱�
        }

        for (int i = 0; i < proccessLength; i++)//����������ŭ�� ������
        {
            saveWindowObject.transform.GetChild(i).localPosition = new Vector3(0, -(saveObjectHeight * i), 0);// â��ġ ����ֱ� 
        }
        SetListWindowSize(proccessLength);//����ȭ�� ũ������

        SetPageList();//����¡ ������ ȭ�鿡�Ѹ���
    }

    /// <summary>
    /// Ǯ���� ������ ������Ʈ ���� �����Ѻκ� �߰��ϰ� �ʿ���� �͵� ��Ȱ��ȭ�ϴ� �Լ� ȣ��
    /// </summary>
    /// <param name="position">Ǯ�� ������ġ�� ���� ������Ʈ</param>
    /// <param name="childCount">Ǯ���� ������ ������Ʈ ����</param>
    /// <param name="proccessLength">�ʿ��� ������Ʈ ����</param>
    /// <param name="type">������ ������Ʈ Ÿ��</param>
    private void PoolBugFunction(Transform position, int childCount, int proccessLength, EnumList.MultipleFactoryObjectList type) {

        for (int i = childCount; i < proccessLength; i++) //�ʿ��Ѹ�ŭ �߰��� �����Ѵ� 
        {
            MultipleObjectsFactory.Instance.GetObject(type);//������Ʈ �߰��ؼ� ������ Ǯ�ǻ�����ø���.
        }
        SetPoolBug(position, proccessLength);//�ʿ���� ������Ʈ�� ��Ȱ��ȭ �ϴ� �Լ�
    }

    /// <summary>
    /// Ǯ���� 2�辿�ø��⶧���� �Ⱦ��������� ����� �װ͵��� ��Ȱ��ȭ �ϴ� �۾�.
    /// </summary>
    /// <param name="position">�ʱ�ȭ�� ��ġ</param>
    /// <param name="startIndex">���� �ε���</param>
    private void SetPoolBug(Transform position, int startIndex)
    {
        int lastIndex = position.childCount;// �������Ŀ� �߰��� ������ �ٽóѱ��.
        for (int i = 0; i < lastIndex; i++) {
            position.GetChild(i).gameObject.SetActive(true); //������Ʈ Ȱ��ȭ�ؼ� �ϴ� ���κ����ص�
        }
        for (int i = startIndex; i < lastIndex; i++)//�Ⱦ��� ���ϸ�ŭ�� ������.
        {
            position.GetChild(i).gameObject.SetActive(false); //�Ⱦ������� ����� 
        }
        int pageLength = lastIndex - startIndex < 1 ? lastIndex : lastIndex - startIndex; // �������� ��Ʈ �������� �ƴϳĿ����� ���̴޶������ؼ� ����ó��
        SetListWindowSize(pageLength); //������ ũ������ 
    }


    /// <summary>
    /// ���嵥���� ����Ʈ�� ������ ũ�⸦ �����Ѵ�.
    /// </summary>
    private void SetListWindowSize(int fileLength) {
        //Ʈ�������� �����غ����� �����Ÿ���� ���������ιٲ��� �����Ÿ�� �����Ͽ���.
        saveWindowObject.GetComponent<RectTransform>().sizeDelta = // �����Ÿ�� Vector2�� ���� ������ ����� �����ȴ�. 
                new Vector2(saveWindowObject.GetComponent<RectTransform>().rect.width, //�⺻������ 
                saveObjectHeight * fileLength); //�������� �� ���� ������ũ�� ���ϱ�
    }

    /// <summary>
    /// ���������� ���̴� ������Ʈ���� �����͸� �ٽü����Ѵ�.
    /// <param name="saveDataList">ȭ�鿡 �Ѹ� �����͸���Ʈ</param>
    /// </summary>
    private void SetGameObjectList(JsonGameData[] saveDataList)
    {
        if (saveDataList == null)
        { // �о�� �������������°�� ����
            Debug.Log("�ʱ�ȭ?");
            return;
        }
        int startIndex = pageIndex * pageViewSaveObjectMaxSize; //���������ۿ�����Ʈ��ġ�� ��������

        int lastIndex = (pageIndex + 1) * pageViewSaveObjectMaxSize > SaveLoadManager.Instance.MaxSaveDataLength ? //���ϸ���Ʈ �ִ밪 < ��������¡�� * ȭ�鿡���������ִ������Ʈ��
                            SaveLoadManager.Instance.MaxSaveDataLength : //�������������� ���������� ����
                            (pageIndex + 1) * pageViewSaveObjectMaxSize; //�ƴϸ� �Ϲ����� ����¡ 

        for (int i = startIndex; i < lastIndex; i++)
        { //�����͸� ����������ŭ�� Ȯ���Ѵ�.
            SetGameObject(saveDataList[i], i); // �����͸� �������� 
        }
        int visibleEndIndex = lastIndex - startIndex; //�������� ������ �ε������� �ش�.
        SetPoolBug(saveWindowObject.transform, visibleEndIndex);//Ǯ�� ������Ʈ�� 2�辿�ø��µ� �����ϴ°͵��� ��Ȱ��ȭ�۾����ʿ��ؼ� �߰��ߴ�.

    }

    /// <summary>
    /// ==�̿ϼ�==
    /// ���� ���õǾ��Ѵ� 
    /// lastPageIndex = ������ �Ѱ���-1�� 
    /// pageMaxSize = ���������� ���̴°���
    /// �ΰ��� �����̵Ǹ� ���� ������ �������� ȭ�鿡 ��ó������ ��ȯ�Ѵ�.
    /// �ش��Լ��� pageMaxSize < lastPageIndex // �϶��� ���ȴ� �ϴ� �⺻�� 8�� �����ϸ� ��������.
    /// ��ɴ��߰��ҷ��� ��������ؾ���.
    /// </summary>
    /// <param name="viewLength">ȭ�鿡 ���� ����������</param>
    /// <returns>for���� 0��°�� ���õ� ��</returns>
    private int GetStartIndex(int viewLength)
    {
        int returnNum = 0; //��ȯ�� ���� �ʱⰪ �����̻��̾ȳѾ�� 0����ȯ�Ѵ�.
        int halfPageNum = viewLength / 2; //�������� ����
        int addPage = viewLength % 2; //�̰����� Ȧ����¦���� ��������
        if (pageIndex >= halfPageNum + addPage) // ������������ �������ǹ�����(�������������̴� ������ Ȧ���� +1) ���� ���ų� Ŭ�� 
        { // �������� �߰����� �Ѿ��                      
            if (pageIndex + halfPageNum >= lastPageIndex) //����������Ʈ(����������+ �������ǹ�����)���� ������ �ִ밪���� ���ų� Ŭ�� 
            {
                returnNum = lastPageIndex - viewLength + 1; //�������������� �����Ҷ� ó���ɰ�
            }
            else
            {
                returnNum = pageIndex - halfPageNum + (1 - addPage); // �߰������� �϶� ó���ɰ�
            }

        }
        return returnNum + 1; //�������� 0���� ���ƴ϶� 1���� �������ϱ⶧���� +1
    }

    /// <summary>
    /// �����ε����� ������ �ش������Ʈ�� �ٽñ׸���.

    /// </summary>
    /// <param name="saveData">������ ���嵥����</param>
    /// <param name="index">������ �ε���</param>
    public void SetGameObject(JsonGameData saveData, int fileIndex) {

        int viewObjectNumber = GetGameObjectIndex(fileIndex); //�������� ������Ʈ ��ġã��

        SaveDataObject sd = saveWindowObject.transform.GetChild(viewObjectNumber).GetComponent<SaveDataObject>(); //������ ������Ʈ �����´�.

        sd.ObjectIndex = viewObjectNumber; //������Ʈ �ѹ����� ���ش� 

        if (saveData != null) { //���嵥���Ͱ� �ִ��� üũ

            //�����δ� ������ ���� ������Ƽ Set �Լ��� ȭ�鿡 �����ִ� ��ɳ־����.
            //���̴±�ɼ����� SaveDataObject Ŭ��������.
            sd.FileIndex = saveData.DataIndex;
            sd.CreateTime = saveData.SaveTime;
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
    /// ������ ���ڸ� �糪���Ѵ�.
    /// </summary>
    public void SetPageList(int index = -1) {
        if (index > -1) PageIndex = index;
        SetGameObjectList(SaveLoadManager.Instance.SaveDataList); // ���ϸ���Ʈ �����ֱ�

        int viewLength = GetGameObjectLength(); //��ȭ�鿡 ������ ���������� 

        int startIndex = GetStartIndex(viewLength); // ����¡ó���� ó�� ǥ���Ұ��� �����´� 

        for (int i = 0; i < viewLength; i++) { //�������� �ٽõ��鼭 �����Ѵ�
            RectTransform tempRect = saveWindowPageObject.transform.GetChild(i).GetComponent<RectTransform>(); // ������Ʈ�� ��ġ��������������
            Vector3 tempX = tempRect.anchoredPosition; //anchoredPosition ���� �����ؾ� ��ġ���� �ٲ��.
            tempX.x = pageObjectWidthPadding * (i + 1);  //�е����ݸ�ŭ �յ��ϰ� ����
            tempRect.anchoredPosition = tempX; // ��ġ ���� 
            saveWindowPageObject.transform.GetChild(i).GetComponent<SavePageButtonIsPool>().PageIndex = startIndex + i; //������ �ε����� ǥ��
        }
        ResetSaveFocusing();//�������̵��� �ʱ�ȭ
    }

    /// <summary>
    /// ���̺����� ��Ŀ�� �����ϱ� 
    /// </summary>
    public void ResetSaveFocusing() {
        int initLength = GetGameObjectLength();
        for (int i = 0; i < initLength; i++)
        {
            saveWindowObject.transform.GetChild(i).GetComponent<Image>().color = Color.black;
        }
    }

    /// <summary>
    /// ���̺� ���� ��Ŀ�� �ϱ�
    /// </summary>
    /// <param name="fileIndex">������ ���� �ε���</param>
    /// <param name="isCopy">���� �����������Ȯ��</param>
    public void SetFocusView(int fileIndex,bool isCopy)
    {
        if (fileIndex > -1) //���������� ����ε� �����Ͱ��ִ°�� 
        {
            int pageIndexObejct = GetGameObjectIndex(fileIndex); //���ӿ�����Ʈ��ġ��������
            if (!isCopy) //���簡 �ƴѰ�� 
            {
                ResetSaveFocusing(); //���� ��Ŀ�� �����ϰ�
                saveWindowObject.transform.GetChild(pageIndexObejct).GetComponent<Image>().color = Color.white; //���� ��Ŀ��
            }
            else // ��������ΰ��
            {
                if (saveLoadPopupWindow.OldIndex == saveLoadPopupWindow.NewIndex)
                { //�����ư �������ÿ� �������� ������ �̸��´�.
                    saveWindowObject.transform.GetChild(GetGameObjectIndex(saveLoadPopupWindow.OldIndex)).GetComponent<Image>().color = Color.red;
                }
                else //�����ư������ ������ �����ҽ�  
                {
                    saveWindowObject.transform.GetChild(GetGameObjectIndex(saveLoadPopupWindow.NewIndex)).GetComponent<Image>().color = Color.blue;
                }
            }
        } 
    }
}
