using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 팀관리용 제작중
/// </summary>
public class TeamBorderManager : MonoBehaviour
{
    TeamBorderStateUI[] teamState;
    public TeamBorderStateUI[] TeamStateUIs => teamState;
    private void Awake()
    {
        teamState = transform.GetComponentsInChildren<TeamBorderStateUI>(true);
    }
    /// <summary>
    /// 팀원수만큼 상태창을 보여준다 
    /// </summary>
    /// <param name="length">팀원수</param>
    public void ViewTeamInfo(int length = 0) 
    {
        for (int i = 0; i < length; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 창을 닫는다 
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
