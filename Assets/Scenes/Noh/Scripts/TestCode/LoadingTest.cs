using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ε��� �ڵ����� �ߵ��ϴ��� �׽�Ʈ�ϴ� Ŭ���� 
/// </summary>
public class LoadingTest : MonoBehaviour
{
    private void Start() //�ε��̵Ǿ� �ش�â���� �̵��� 
    {
        StartCoroutine(returnTitle());
    }
    IEnumerator returnTitle() { 
        yield return new WaitForSeconds(1.0f);
        LoadingScean.SceanLoading(); //�ٽ� Ÿ��Ʋ�� �̵���Ų��.
    }
}
