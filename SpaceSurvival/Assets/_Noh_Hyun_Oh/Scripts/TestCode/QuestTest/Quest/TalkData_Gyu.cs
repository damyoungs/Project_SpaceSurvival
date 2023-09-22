using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// NPC 대화 목록을 관리할 클래스
/// </summary>
public class TalkData_Gyu : MonoBehaviour
{
    /// <summary>
    /// 퀘스트 대화전용
    /// 한줄에 꺼낼 배열 갯수
    /// 몇줄 작성할지에대한 값
    /// </summary>
    int talkQuestArrayLength = 3;

    string filePath = string.Empty;
    /// <summary>
    /// 대화 내용 저장된 파일이름
    /// </summary>
    const string fileFirstName = "NPC_";
    const string fileType = ".txt";
    /// <summary>
    /// 한줄을 구별할 기준 패턴단어 
    /// 파일에서 한줄한줄을 임의로 구분하기위해 필요하다.
    /// </summary>
    string lineCheckChar = "//";


    /// <summary>
    /// 파일에서 읽어온 데이터 저장해둘 이중배열
    /// 해시테이블보다는 이중배열이 더효율적일거같아서 사용했다. 
    /// </summary>
    string[][] talkData;

    private void Awake()
    {
        filePath = $"{Application.dataPath}/__CommonAssets/TextFile/NPC_TalkData/";
        talkData = new string[Enum.GetValues(typeof(TalkType)).Length][]; //이넘크기만큼 배열 잡아두고 
        FileRead_And_SplitFileData(); //외부 파일읽어서 대화목록 불러오기 
    }

    /// <summary>
    /// 외부 텍스트 파일 읽어서 스트링 데이터로 만들기 
    /// </summary>
    /// <param name="typeName">읽어올 파일이름(확장자포함)</param>
    /// <returns>읽어온 내용</returns>
    private string File_IO_TalkData_Read(string typeName)
    {
        string fullFilePath = $"{filePath}{fileFirstName}{typeName}{fileType}";
        try
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                Debug.Log("폴더생성");
            }
            if (!File.Exists(fullFilePath))
            {
                File.Create(fullFilePath);
                Debug.Log("파일 생성");

            }
            string result = File.ReadAllText(fullFilePath);
            return result;
            //Debug.Log($"파일 있어 읽었어 내용은  = {readFile}");
        }
        catch (Exception e)
        {
           Debug.LogWarning($"TalkType 의 {typeName} 값에 해당하는 대화Txt파일이 없습니다 \r\n파일이 필요한위치 :{fullFilePath}");
           return null;
        }
    }



    /// <summary>
    /// 문자열 특정 패턴으로 잘라서 저장하기 
    /// </summary>
    /// <param name="fileReadData">파일읽어온 문자열</param>
    /// <returns></returns>
    private void FileRead_And_SplitFileData()
    {
        Array enumArray = Enum.GetValues(typeof(TalkType));
        string temp;
        foreach (var enumValue in enumArray)
        {
            //이넘추가되면 값과 파일 종류를 맞춰야한다.
            //__CommonAssets/TextFile/NPC_TalkData/NPC_이넘이름.txt 파일이 생겨야 
            temp = File_IO_TalkData_Read(enumValue.ToString()); //파일읽어와서 담아두고 
            if (temp != null) //읽어온 값이 존재할경우 셋팅한다. 
            {
                talkData[(int)enumValue] = temp.Replace("\n", "").Split(lineCheckChar);
            } 
        }
    }

    /// <summary>
    /// 저장된 대화목록 가져오기
    /// </summary>
    /// <param name="type">대화 종류</param>
    /// <param name="talkIndex">대화 순번</param>
    /// <returns>대화 순번에 대한 대화목록 반환</returns>
    public string[] GetTalk(TalkType type , int talkIndex)
    {
        string[] result = new string[talkQuestArrayLength];      // 반환할 대화목록 크기정하고
        int startIndex = talkQuestArrayLength * talkIndex;       // talkIndex 를가지고 시작배열 위치잡고
        int endIndex = startIndex + talkQuestArrayLength;        // 종료위치도 구하고 
        int textIndex = 0;                                       // 배열 순차적으로담기위해 임시변수선언하고 
        int typeIndex = (int)type;
        for (int i = startIndex; i < endIndex; i++)              // 포문돌면서 
        {
            result[textIndex] = talkData[typeIndex][i];          // 임시변수로 반환할 목록에 순차적으로 배열위치 만큼 가져온다
            textIndex++;                                         // 순차적으로 넣기위해 갱신
        }

        return result;
    }
}
