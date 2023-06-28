using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWindowPageIn : MonoBehaviour
{
    SaveDataSort proccessClass;
    private void Awake()
    {
        proccessClass = GetComponent<SaveDataSort>();
    }
    /// <summary>
    /// 페이지 버튼 클릭 이벤트
    /// </summary>
    public void OnPageDownButton()
    {
        

    }
}
