using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 팀관리용 제작중
/// </summary>
public class TeamBorderManager : MonoBehaviour
{
    GameObject[] teamList;

    private void Awake()
    {
        teamList = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            teamList[i] = transform.GetChild(i).gameObject;
        }

    }
}
