using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDataSingleton : Singleton<BaseDataSingleton>
    //�̷������� ������Ʈ�� �������� ���⼭ �����ؼ� Instantiate�Լ��� �����ؼ� ����ϴ� ��� 
    //prefab ���� ���� �͵��� �̱��� �ϳ��� �����ϸ� ���Ұ� �����ϴ�. 
    [SerializeField]
    GameObject item1;
    [SerializeField]
    GameObject item2;
    [SerializeField]
    GameObject item3;
    [SerializeField]
    GameObject item4;
    [SerializeField]
    GameObject item5;
    [SerializeField]
    GameObject item6;
    [SerializeField]
    GameObject item7;
    [SerializeField]
    GameObject item8;
    [SerializeField]
    GameObject item9;

    GameObject[] itemList;

    public GameObject[] ItemList
    {
        get {
            if (ItemList == null) {
                itemList = new GameObject[9];
                itemList[0] = item1;
                itemList[1] = item2;
                itemList[2] = item3;
                itemList[3] = item4;
                itemList[4] = item5;
                itemList[5] = item6;
                itemList[6] = item7;
                itemList[7] = item8;
                itemList[8] = item9;

            }
            return itemList;
        }
        private set
            {
            itemList = value; 
            }

    }
// ĳ���� ������ �̱��� ��ӹ޾Ƽ� �ְ� �׾ȿ� �����͸� �־ �����ϴ� ������� 
// ���� �ʿ��� ������Ʈ ���⿡�� �߰��Ͽ� �����ϴ� �ڵ�λ�� �ϴ¹��


//����Ƽ ���鼭 ���ӿ�����Ʈ�� ������ �ϵ��ڵ����� ����Ʈ�ι޾ƿü��ִ¹���������� ���ӿ�����Ʈ �����ϰ� �����鿡�� �ְ� �ϴ� �۾������� �ڵ�����ϱ� ���Ұ� �����ϴ�. 


}
