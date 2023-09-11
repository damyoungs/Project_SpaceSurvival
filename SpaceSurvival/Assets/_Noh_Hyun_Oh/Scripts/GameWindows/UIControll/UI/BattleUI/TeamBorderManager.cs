using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// �������� ������
/// </summary>
public class TeamBorderManager : MonoBehaviour
{
    /// <summary>
    /// ��������ŭ ����â�� �����ش� 
    /// </summary>
    /// <param name="length">������</param>
    public void ViewTeamInfo(int length = 0) 
    {
        for (int i = 0; i < length; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// â�� �ݴ´� 
    /// </summary>
    public void UnView() 
    {
        int childLength = transform.childCount;
        for (int i = 0; i < childLength; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
