using EnumList;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateObjectIsPool : ObjectIsPool, IStateData
{
    /// <summary>
    /// ���̵� �Ǵ� �����̻��� ��������� Ǯ�� �������ֵ��� ��ġ�� ����
    /// </summary>
    Transform poolTransform;
    public Transform PoolTransform 
    {
        protected get => poolTransform;
        set 
        {
            if (poolTransform == null) //Ǯ��ġ�� ó�� �����ɶ� �ѹ��� ����  
            {
                poolTransform = value;
            }
        }
    }
    /// <summary>
    /// �����̻��� ����
    /// </summary>
    StateType stateType;
    public StateType Type 
    {
        get => stateType;
        set 
        {
            stateType = value;
        }
    }


    /// <summary>
    /// �����̻��� ������ �̹���
    /// </summary>
    private Image icon; 
    public Image Icon => icon;

    /// <summary>
    /// ���ϴ� ���ҵǴ� ��ġ 
    /// </summary>
    private float reducedDuration = 0.0f;
    public float ReducedDuration { get => reducedDuration; set => reducedDuration = value; }
    /// <summary>
    /// �����̻��� �ִ� ���ӽð�
    /// </summary>
    private float maxDuation = 0.0f;
    public float MaxDuration { get => maxDuation; set => maxDuation =value; }
    /// <summary>
    /// ���� ���ӽð�
    /// </summary>
    private float currentDuration = 0.0f;
    public float CurrentDuration { 
        get => currentDuration;

        set 
        {
            currentDuration = value; 
            if (value < 0.0f) //���ӽð��� �ٵǸ�
            {
                InitValue();//�ʱ�ȭ

            }
        }
    }


    /// <summary>
    /// �ʱ�ȭ �۾�
    /// </summary>
    public void InitValue() 
    {
        //���°� �ʱ�ȭ�ϰ� 
        Type = StateType.None;
        ReducedDuration = -1.0f;
        maxDuation = -1.0f;
        currentDuration = -1.0f;
        gameObject.SetActive(false); //ť�� �ٽ� �ֱ����� ��Ȱ��ȭ 
        transform.SetParent(poolTransform); //Ǯ�� ������.
        /*
         transform.SetParent �Լ��� ��ü�� Ȱ��ȭ ��Ȱ��ȭ ��ü�Ǵ½������� ���ɰ�� ����������.
         �׷��� Ȱ��ȭüũ���Ͽ� SetParent �� �����Ѵ�.
         ����Ȯ�� : ������Ʈ�� Ȱ��ȭ �����϶� gameobject.SetActive(false)������ �ٷ�  transform.SetParent(�θ�) �� �����ϴ� ������.
         */
    }

   
}
