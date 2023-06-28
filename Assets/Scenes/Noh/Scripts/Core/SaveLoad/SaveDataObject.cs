using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// 저장화면에 보이는 파일 정보 
/// 이오브젝트 클릭시 파일인덱스를 넘겨야한다.
/// </summary>
public class SaveDataObject : SaveDataIsPool
{
  
    /// <summary>
    /// 클릭했을때 오브젝트 인덱스
    /// </summary>
    private int objectIndex = -1;
    public int ObjectIndex { 
        get => objectIndex; 
        set => objectIndex = value; 
    }
    /// <summary>
    /// 내부적인 인덱스값
    /// -1값이면 초기상태
    /// </summary>
    private int fileIndex = -1;
    public int FileIndex { 
        get => fileIndex;
        set {
            fileIndex = value;
            if (fileIndex > -1)
            {
                saveFileNumber.text = $"No.{fileIndex.ToString("D3")}";
            }
            else
            {
                saveFileNumber.text = $"No.{objectIndex.ToString("D3")}" ;
            }
        }
    }
    /// <summary>
    /// 저장화면에 보일 저장파일 생성날짜
    /// </summary>
    private string createTime ;
    public string CreateTime { 
        get => createTime;
        set { 
            createTime = value;
            createTimeObj.text = $"{createTime}";
        } 
    }
    /// <summary>
    /// 저장화면에 보일 캐릭터이름
    /// </summary>
    private int charcterLevel = -1;
    public int CharcterLevel { 
        get => charcterLevel;
        set { 
            charcterLevel = value;
            charcterLevelObj.text = $"Lv.{charcterLevel}";
        }
    }

    /// <summary>
    /// 저장화면에보일 캐릭터 소지금액
    /// </summary>
    private double money = 3333333333333333333;
    public double Money { 
        get => money;
        set { 
            money = value;
            charcterMoneyObj.text = $"$ {money}";
        }
    }
    /// <summary>
    /// 저장화면에 보일 씬정보
    /// </summary>
    private EnumList.SceanName sceanName;
    public EnumList.SceanName SceanName {
        get => sceanName;
        set { 
            sceanName = value;
            sceanNameObject.text = $" Map :{sceanName}";
        }
    }
    /// <summary>
    /// 오브젝트밑에 텍스트 오브젝트들 
    /// </summary>
    [SerializeField]
    TextMeshProUGUI saveFileNumber; // 파일이름? 
    [SerializeField]
    TextMeshProUGUI sceanNameObject; // 캐릭터이름 , 저장위치 , 돈 , 레벨 정도?
    [SerializeField]
    TextMeshProUGUI createTimeObj;   // 저장시간 보여주기
    [SerializeField]
    TextMeshProUGUI charcterLevelObj; // 파일이름? 
    [SerializeField]
    TextMeshProUGUI charcterMoneyObj; // 캐릭터이름 , 저장위치 , 돈 , 레벨 정도?
    [SerializeField]
    TextMeshProUGUI etcObj;   // 저장시간 보여주기
    [SerializeField]
    GameObject isFocus;

    InputKeyMouse inputSystem;

    SaveLoadPopupWindow proccessManager;

    /// <summary>
    /// 생성시 포지션리셋 여부 셋팅
    /// </summary>
    private void Awake()
    {
        inputSystem = new InputKeyMouse();
        //풀에서 처리시 로컬포지션 리셋하지않게 변수셋팅
        isPositionReset = false;
    }
    private void Start()
    {
        proccessManager = SaveLoadPopupWindow.Instance; //저장화면 처리하는클래스가져오기
    }

    public void InFocusObject() 
    {
        if (proccessManager.NewIndex != fileIndex) //같은 오브젝트 클릭했는지 체크
        {
            if (proccessManager.NewIndex > -1 && proccessManager.CopyCheck) // 카피버튼을 클릭했고 데이터가 없는것을 클릭안했는지 체크
            {

                proccessManager.OldIndex = proccessManager.NewIndex; // 카피할 데이터 번호 셋팅
                proccessManager.NewIndex = fileIndex; //카피될 데이터 번호 셋팅
                proccessManager.OpenPopupAction(EnumList.SaveLoadButtonList.COPY); //카피실행

            }
            else
            {
                SaveLoadPopupWindow.Instance.NewIndex = fileIndex; //새로클릭했으면 다시 셋팅 
            }
        }
        
    }

}
