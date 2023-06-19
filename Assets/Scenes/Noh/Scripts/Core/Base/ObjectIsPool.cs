using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ǯ���� �⺻ Ŭ����
/// </summary>
public class ObjectIsPool : MonoBehaviour
{
    /// <summary>
    /// ��Ȱ��ȭ�� Queue�� ��ȯ ó���� �̷�������Ѵ�.
    /// ������Ʈ Ǯ�� �� ������Ʈ���� ��ӹ��� Ŭ���� 
    /// </summary>
    public Action onDisable;
    /// <summary>
    /// ��ġ�ʱ�ȭ ���������� �����ϴ� ���� 
    /// </summary>
    protected bool isPositionReset = true; 
    protected virtual void OnEnable()
    {
        if (isPositionReset) 
        {
            //��ġ�� �ʱ�ȭ �⺻������ ������Ʈ�����Ҷ� Ʈ�������� 0,0,0 ���� �����ؾ��Ѵ�.
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
    /// <summary>
    /// ��Ȱ��ȭ�� �θ� ����� ���������Ͽ� ť�� �ʱ�ȭ�Ҽ�����.
    /// </summary>
    protected virtual void OnDisable() 
    {
        onDisable?.Invoke(); //Queue �ʱ�ȭ�Ѵ�.
    }

    /// <summary>
    /// ���� �ð� �Ŀ� �� ���ӿ�����Ʈ�� ��Ȱ��ȭ ��Ű�� �ڷ�ƾ
    /// </summary>
    /// <param name="delay">��Ȱ��ȭ�� �ɶ����� �ɸ��� �ð�(�⺻ = 0.0f)</param>
    /// <returns></returns>
    protected virtual IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay��ŭ ����ϰ�
        gameObject.SetActive(false);            // ���� ������Ʈ ��Ȱ��ȭ
    }

}
