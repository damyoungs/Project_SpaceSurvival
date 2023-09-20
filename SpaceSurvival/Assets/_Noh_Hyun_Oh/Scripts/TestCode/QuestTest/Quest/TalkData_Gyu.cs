using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData_Gyu : MonoBehaviour
{
    /// <summary>
    /// 딕셔너리 int 키값이 유니크해야되면  string이중배열로 처리 하자 
    /// 인덱스가 0~ 순차적이지만 유니크하게 셋팅되니 해당방법이낫다.
    /// 데이터를 만드는용도면 대충 이런식으로 한다.
    /// 아니면 외부 텍스트 읽어서 파싱작업해야함.
    /// 유니티에서 scriptable 이있으니 그것 사용해도되고.
    /// </summary>
    string[][] talkData = new string[][]
    {
        new string[]{"1000Npc 답변1","1000Npc 답변1" },
        new string[]{"2000NPC 답변2","2000NPC 답변2" },
        new string[]{"3000NPC 답변3","3000NPC 답변3" }
    };
    //List<string[]> talkData = new List<string[]>(); // 수정이많으면 리스트를 사용한다 위에 구조랑 비슷하다.
    //Dictionary<int, string[]> talkData = new Dictionary<int, string[]>(); // 대화데이터가 많이 들어갈경우 해시테이블(딕셔너리) 가좋다.

    /// <summary>
    /// 저장된 대화목록 가져오기
    /// </summary>
    /// <param name="id">엔피씨 id </param>
    /// <returns>인덱스 위치에 저장된 대화목록의 String배열</returns>
    public string[] GetTalk(int id)
    {
        return talkData[id];
    }
}
