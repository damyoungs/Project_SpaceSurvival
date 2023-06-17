using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���嵥����ȭ�鿡 ������ ������Ʈ ����Ŭ����
/// </summary>
public class SaveDataPool : MultipleObjectPool<ObjectIsPool>
{
    /// <summary>
    /// ����ȭ�� ������Ʈ������ ������
    /// </summary>
    GameObject saveDataWindow;
    private void Awake()
    {
        //saveDataWindow = GameObject.FindGameObjectWithTag("SaveList"); //���̺� ���ϸ���Ʈ�� ���������� ������ ��ġ
        if (GameObject.FindGameObjectWithTag("WindowList")) { 
            saveDataWindow = GameObject.FindGameObjectWithTag("WindowList").transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        }
        
        

        
    }
    /// <summary>
    /// Ǯ�� ������ �θ���ġ�� �ٲٱ����� �߰���
    /// �Լ��� Ǯ�� �ʱ�ȭ �ϰ����� �����۾����ʿ��Ұ�� ����ϸ�ȴ�.
    /// </summary>
    protected override void SettingFuntion()
    {
        if (saveDataWindow != null) { 
            setPosition = saveDataWindow.transform; //�⺻������ Ǯ�Ʒ��� ���������� ���ϴ¿�����Ʈ�Ʒ��� �����ǰ� �����Ͽ���.
        }
    }
   
}
