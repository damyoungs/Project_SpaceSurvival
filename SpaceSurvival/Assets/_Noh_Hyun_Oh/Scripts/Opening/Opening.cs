using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �������� (0,20,-70)
/// �����̼� (45,0,0)
/// </summary>
public class Opening : MonoBehaviour
{

    /// <summary>
    /// �ؽ�Ʈ �⺻ ������ ������
    /// </summary>
    [SerializeField]
    TextMeshPro textPrefab;
    
    /// <summary>
    /// �ؽ�Ʈ �⺻ ������
    /// </summary>
    const float textWidth = 73.0f;
    
    /// <summary>
    /// �ؽ�Ʈ �⺻ ������
    /// </summary>
    const float textHeight = 5.0f;

    /// <summary>
    /// �ٰ��� + �� �Ʒ����� ����  - �� ������ �Ʒ��� ����
    /// </summary>
    [SerializeField]
    float textPadding = -10.0f;
    
    /// <summary>
    /// ī�޶� �⺻ ��ġ
    /// </summary>
    Vector3 cameraPosition = new Vector3(0.0f,20.0f,0.0f);

    /// <summary>
    /// ī�޶� �⺻ ����
    /// </summary>
    Vector3 cameraRotate = new Vector3(45.0f,0.0f,0.0f);

    [SerializeField]
    float cameraMoveSpeed = 3.0f;
    float elaspadSpeed = 0.0f;

    Camera mainCamera;
    /// <summary>
    /// �Ѷ��ο� ������ ���� ��
    /// </summary>
    [SerializeField]
    int fileLineSize = 20;

    int lineCount = 0;

    private void Awake()
    {

        lineCount = SetTextArray(TextFileRead()); //�����ϸ� �ٷ� ������ �ҷ��ͼ� ����

        OpeningButtonManager buttons = GameObject.FindObjectOfType<OpeningButtonManager>(); //��ư �������ִ� ���۳�Ʈ ã�� 

        buttons.speedUpButton += () => {
            if (elaspadSpeed < cameraMoveSpeed + 0.1f) //�̵��ӵ� 1��� üũ
            {
                elaspadSpeed = cameraMoveSpeed * 2; //�ι�� �ø���.
            }
            else if (elaspadSpeed < cameraMoveSpeed * 2 + 0.1f) //�̵��ӵ� 2��� üũ 
            {
                elaspadSpeed = cameraMoveSpeed * 3; //����� �ø���.
            }
            else 
            {
                elaspadSpeed = cameraMoveSpeed; // ���� �ӵ��� ������.
            }
        };

        buttons.skipButton += () => {
            LoadingScean.SceanLoading(EnumList.SceanName.TITLE); //��ŵ�̸� ���̵�
        };

        elaspadSpeed = cameraMoveSpeed; //�̵��ӵ� �⺻�ӵ��� ����
    }

    private void Start()
    {
        //ī�޶� �ҷ��ͼ� ��ġ���
        mainCamera = Camera.main;
        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.rotation = Quaternion.Euler(cameraRotate);
        //ī�޶� �̵�����
        StartCoroutine(OpeningCameraMove(lineCount));
    }

    /// <summary>
    /// ī�޶� �̵��Ͽ� ������������ ����.
    /// </summary>
    /// <param name="endValue">������ ��ġ��</param>
    IEnumerator OpeningCameraMove(int endValue) 
    {
        //���� ��� �߰� �ʿ� 

        float checkValue = mainCamera.transform.position.z;
        float checkEnd = endValue * textPadding - 100.0f;
        while (checkValue > checkEnd) 
        {
            checkValue -= Time.deltaTime * elaspadSpeed;
            mainCamera.transform.position = new Vector3(
                                                0,
                                                20,
                                                checkValue);
            yield return null;
        }
        //�����̴� ���� ȭ������ �̵�
        LoadingScean.SceanLoading(EnumList.SceanName.TITLE); 
    }


    /// <summary>
    /// ���ӵ� ������ �迭�� ������ TextMeshPro �� ����� ȭ�鿡�Ѹ��·���
    /// </summary>
    /// <param name="fileText">�о�� ������ ���ӵȱ����� �迭</param>
    /// <returns>������ ���μ�</returns>
    private int SetTextArray(string[] fileText) 
    {
        int textLength = fileText.Length; //���� ���� �Ѱ��� 
        RectTransform rt; //��ġ������ ��Ʈ ���� �����صΰ� 
        int textLineLength = 0;
        int lineCount = 1;      //����°���� üũ�� ������������ �ϳ������ϰ�����ϱ⶧���� 1�� �ʱⰪ

        TextMeshPro textObject = Instantiate(textPrefab,transform); //������ �����ؼ� ��� ����
        rt = textObject.GetComponent<RectTransform>(); //��ġ ������ ��Ʈ ã�ƿ��� 
        rt.anchoredPosition3D = new Vector3(0.0f, 0.0f, 0.0f); //ó�� ��ġ �����ϰ� 
        for (int i = 0; i < textLength ; i++) //���� �������� ��ŭ ������ 
        {
            if (textLineLength + fileText[i].Length > fileLineSize)  //���ٿ� ������ ���� �Ѿ�� 
            {
                textObject = Instantiate(textPrefab,transform); //���Ӱ� ������ ���� �����ϰ� 
                textLineLength = fileText[i].Length; //���� ���� ���� 
                textObject.text = fileText[i]; //���� ��� 
                rt = textObject.GetComponent<RectTransform>(); //��ġ ������ ��Ʈ ã�ƿ��� 
                rt.anchoredPosition3D = new Vector3(0, 0, lineCount * textPadding); //��ġ �����ϰ� 
                lineCount++;
            }
            else //���ٿ� ������ ���� �ȳѾ�� �ٽ� �߰��ϰ� 
            {
                textLineLength += fileText[i].Length+1; //���� ���� �߰��صΰ� 
                textObject.text += $" {fileText[i]}"; //���뵵 �߰� 
            }
      
        }

        return lineCount;
    }


    /// <summary>
    /// txt ���� �о string�� ��� ���� 
    /// </summary>
    /// <returns>���ӵ� ������ �迭</returns>
    private string[] TextFileRead() 
    {
        string filePath = $"{Application.dataPath}/__CommonAssets/TextFile/";
        string fileFullPath = filePath + "Synopsis.txt";

        string[] textArray = null; //��ȯ�Ұ� ����
        
        try
        {
            string readText = string.Empty;
            if (Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if( File.Exists(fileFullPath))
            {
                readText = File.ReadAllText(fileFullPath);
            }
            textArray = readText.Split(' ','\r','\n'); //�������� ������ �迭�� ��´�.
            //foreach (string text in textArray) 
            //{
            //    Debug.Log($"�ؽ�Ʈ :{text}  ==  ���� :{text.Length} "); //üũ�� ��� 
            //}
        }
        catch (Exception e)
        {
            Debug.LogWarning($"���� ���� ����  \r\n {e.Message}");
        }
        return textArray;
    }

}
