using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkMaster : MonoBehaviour
{
    /// <summary>
    /// 매칭할 대화 배열 <고유id, 대화 내용>
    /// </summary>
    Dictionary<int, string[]> talkData;


    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    // 대화 내용 추가
    void GenerateData()     // talkData.Add(고유 id, new string[]{"대화 내용", "그 다음 대화 내용"});
    {
        // talkData.Add(고유 id, new string[]{"대화 내용", "그 다음 대화 내용"});
        talkData.Add(1000, new string[] { "오! 새로운 여행자인가", "\n만나서 반갑네!! 난 외계인이라고 하네" });
        talkData.Add(2000, new string[] { "배고파", " 먹을게 필요해..." });
        talkData.Add(100, new string[] { "이건 네모난 상자", "\n그저 네모난 평범한 상자"});


        // talkData.Add(고유 id + 퀘스트 id + 다음 퀘스트를 가져올 index, new string[]{"퀘스트 내용"});
        talkData.Add(1000 + 10, new string[] { "저기 미안한데.. 내가 찾고있는게 있거든..", "\n혹시 도와줄수 있을가?" });
        talkData.Add(2000 + 10 + 1, new string[] { "상자?", "\n상자라면 저기 옆에있는걸 말하는거같은데?" });
    }

    /// <summary>
    /// 대화 내용이 끝날때까지 내용을 반환 함
    /// </summary>
    /// <param name="id">고유 id</param>
    /// <param name="talkIndex">대화의 끝을 찾는 인덱스</param>
    /// <returns>반환할 내용</returns>
    public string GetTalk(int id, int talkIndex)
    {
        if(talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
