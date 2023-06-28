using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 기본적으로 복수로 생성되는 객체 정의
/// EnumList.MultipleFactoryObjectList 이곳에 객체추가될내용 같이추가하시면됩니다.
/// </summary>
public class MultipleObjectsFactory : Singleton<MultipleObjectsFactory>
{
    /// <summary>
    /// 저장화면에 보여질 오브젝트풀
    /// </summary>
    SaveDataPool saveDataPool;

    /// <summary>
    /// 저장화면리스트 버튼들 
    /// </summary>
    SavePageButtonPool savePageButtonPool;

    /// <summary>
    /// 팩토리 생성시 초기화 함수
    /// </summary>
    /// <param name="scene">씬정보 딱히필요없음</param>
    /// <param name="mode">모드정보 딱히필요없음</param>
    protected override void Init(Scene scene, LoadSceneMode mode)
    {
#if UNITY_EDITOR
        Debug.LogWarning("가끔씩 순번꼬일때가있어서 체크 2번");
#endif
        saveDataPool = GetComponentInChildren<SaveDataPool>(true);
        savePageButtonPool = GetComponentInChildren<SavePageButtonPool>(true);
        base.Init(scene, mode);
        saveDataPool.Initialize();
        savePageButtonPool.Initialize();
    }

    /// <summary>
    /// 객체 생성하기
    /// </summary>
    /// <param name="type">객체종류</param>
    /// <returns>생성된 객체</returns>
    public GameObject GetObject(EnumList.MultipleFactoryObjectList type)
    {
        GameObject obj = null;
        switch (type)
        {
            case EnumList.MultipleFactoryObjectList.SAVEDATAPOOL:
                obj = saveDataPool?.GetObject()?.gameObject;
                break;
            case EnumList.MultipleFactoryObjectList.SAVEPAGEBUTTONPOOL:
                obj = savePageButtonPool?.GetObject()?.gameObject;
                break;
            default:

                break;
        }
        return obj;
    }
}


