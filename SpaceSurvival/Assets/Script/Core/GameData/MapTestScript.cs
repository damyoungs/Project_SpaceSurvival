using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapTestScript : SingletonBase<MapTestScript>
{
    [SerializeField]
    private GameObject[] cellList;
    //������â���� �巡�׾� ������� ����Ʈ�� 0��°���� ������� �������ִµ� 
    //������ �˼������ ������ Ȯ���ϱ����� ������ �ʿ���
    //���� �� ���鶧 ����Ʈ�� ��Ƽ� ����ϸ� ���Ұ� ����.
    public void TestC() {
        //Debug.Log("start");
        foreach (var a in cellList) { 
            Debug.Log(a.name);
        }
        //Debug.Log("end");
    }

}
