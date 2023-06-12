using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectedPool : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public Action onDisable;

    protected virtual void OnEnable()
    {
        //��ġ�� �ʱ�ȭ �⺻������ ������Ʈ�����Ҷ� Ʈ�������� 0,0,0 ���� �����ؾ��Ѵ�.
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    protected virtual void OnDisable() 
    {
        
        //��Ȱ��ȭ ó�������� �ѹ����ϱ����� onDisable ���������Ϳ� �ְ� �����Ѵ�.
        onDisable?.Invoke();
    }
    /// <summary>
    /// ���� �ð� �Ŀ� �� ���ӿ�����Ʈ�� ��Ȱ��ȭ ��Ű�� �ڷ�ƾ
    /// </summary>
    /// <param name="delay">��Ȱ��ȭ�� �ɶ����� �ɸ��� �ð�(�⺻ = 0.0f)</param>
    /// <returns></returns>
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay��ŭ ����ϰ�
        gameObject.SetActive(false);            // ���� ������Ʈ ��Ȱ��ȭ
    }

}
