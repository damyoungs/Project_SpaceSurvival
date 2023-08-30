using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 오브젝트마다 고유 id(int), isNpc(Bool)를 가지며 이 두가지로 토크마스터(대화), 퀘스트마스터에 있는 대화내용을 매칭하여 화면 출력함
public class GameMaster : MonoBehaviour
{
    public QuestMaster questMaster;
    public TalkMaster talkMaster;
    public int talkIndex;

    /// <summary>
    /// 대화창 Panel
    /// </summary>
    public GameObject TalkPanel;

    /// <summary>
    /// 이름창.Text
    /// </summary>
    public TextMeshProUGUI NameBox;
    
    /// <summary>
    /// 대화창.Text
    /// </summary>
    public TextMeshProUGUI TalkBox;

    /// <summary>
    /// 퀘스트창.Text
    /// </summary>
    public TextMeshProUGUI QuestBox;

    /// <summary>
    /// 대화를 진행할 오브젝트
    /// </summary>
    public GameObject ObjectName;

    /// <summary>
    /// 대화창이 실행되는지 확인
    /// </summary>
    public bool isAction;

    /// <summary>
    /// 이름을 저장하는 임시 변수
    /// </summary>
    private string NameTag;

    /// <summary>
    /// 대화를 진행시키는 함수
    /// </summary>
    /// <param name="objName">입력받은 오브젝트</param>
    public void Action(GameObject objName)  // 입력받은 오브젝트를 화면에 출력
    {
        ObjectName = objName;
        NameTag = objName.name;
        ObjData objData = ObjectName.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        TalkPanel.SetActive(isAction);
    }

    /// <summary>
    /// 데이터를 받아서 Panel에 보내주는 함수
    /// </summary>
    /// <param name="id">대화 번호</param>
    /// <param name="isNpc">Npc 여부</param>
    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = questMaster.GetQuestTalkIndex(id); // 입력받은 오브젝트의 id와 퀘스트 마스터의 id로 반환
        string talkData = talkMaster.GetTalk(id+questTalkIndex, talkIndex); // 오브젝트 id에 퀘스트 id를 더하여 오브젝트가 가진 퀘스트를 반환

        if (talkData == null)   // 진행할 대화가 없으면 초기화
        {
            isAction = false;
            talkIndex = 0;
            QuestBox.text = questMaster.CheckQuest(id);
            NameBox.text = null;
            TalkBox.text = null;
            return;
        }
        if (isNpc)  // Npc일 경우
        {
            NameBox.text = NameTag;
            StartCoroutine(Typing(talkData));
        }
        else  // 사물일 경우
        {
            NameBox.text = NameTag;
            StartCoroutine(Typing(talkData));
            //TalkBox.text = talkData;
        }

        isAction = true;
        talkIndex++;
    }

    /// <summary>
    /// 타이핑 효과(0.05초마다 텍스트 박스에 한글자씩 추가해주는 코루틴)
    /// </summary>
    /// <param name="text">대화내용</param>
    /// <returns>코루틴 사용</returns>
    IEnumerator Typing(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            TalkBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
