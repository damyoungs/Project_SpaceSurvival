using System;
using System.Collections.Generic;
using UnityEngine;
/*
 JsonUtility.ToJson  다중배열 지원안됨 . 
 
 테스트용 코드 이파일에 대해서는 참고만 하시면됩니다.
 
 
 
 */
/// <summary>
/// 테스트 클래스 참고용
/// </summary>
[Serializable]
public struct dumyData
{
    public string[][] charcterName;//다중배열 저장안됨
    public int age;
    public float height;
    public double weight;
    public short min;
    public long money;
    public ulong score;
    public string[] toStirngs;
    public ulong[] ulongs;
    public long[][] longs;//저장안됨

    public dumyData(
        string[][] charcterName ,
        int age                 ,
        float height            ,
        double weight           , 
        short min               ,
        long money              ,
        ulong score             ,
        string[] toStirngs      ,
        ulong[] ulongs          ,
        long[][] longs          
        ) { 
        this.charcterName   = charcterName;
        this.toStirngs      = toStirngs;
        this.age            = age;
        this.height         = height;
        this.weight         = weight;
        this.min            = min;
        this.money          = money;
        this.score          = score;
        this.longs          = longs;
        this.ulongs         = ulongs;
    }

}
[Serializable] //필수로 해야 직렬화작업을거쳐서 json파싱이된다. 클래스나 구조체의 선언부 위에속성을 붙혀주면된다.
public struct stringArray
{
    [SerializeField] //private 선언시 이속성이있으면 json 파싱이된다
    string[] values ;
    public string[] Values { 
        get => values ;
        set => values = value;
    }
    public int[] tempInt; // 속성추가안할시 public 으로해도된다
    public stringArray(string[] values, int[] tempInt) { 
        this.values = values;
        this.tempInt = tempInt;
    }
}
[Serializable]
public struct stringDoubleArray {
    [SerializeField]
    stringArray[] values;
    public stringArray[] Values { 
        get => values;
        set => values = value;  
    }
    public float[] yami;
    public stringDoubleArray(stringArray[] values, float[] yami) {
        this.values = values;
        this.yami = yami;
    }
}
[Serializable]
public struct stringTripleArray
{
    [SerializeField]
    stringDoubleArray[] values;
    
    public stringTripleArray(stringDoubleArray[] values) {
        this.values = values;
    }
}

[Serializable]
public class TestSaveData<T> : JsonGameData  // 상속받은 것도 같이 json으로 파싱이된다. 제네릭 도 같이 파싱이 된다.
{
    public TestSaveData() {
        CharcterInfo = new StructList.CharcterInfo[1];
        CharcterInfo[0].Level = 98;
        CharcterInfo[0].CharcterName = "아주그냥끝장을보는놈";
        CharcterInfo[0].CharcterPosition = new Vector3(0,0,9999.0f);
        CharcterInfo[0].EXP = 990930930;
        CharcterInfo[0].Money = 19191919191919191;
    }   
    [SerializeField]stringTripleArray stringTripleArray = new stringTripleArray();
    [SerializeField]dumyData dumyData = new dumyData();
    [SerializeField]dumyData dumyEmpty;
    [SerializeField]Vector3 vector3Test = new Vector3(55.5f, 22.3f, 44.5f);
    [SerializeField]Vector2 vector2Test = new Vector2(12.4f, 11.3f);
    [SerializeField]int[] invenIndex = { 75444545, 12213234, 1234556123, 234234211, 352112345, 1262343673, 1231215161 };
    [SerializeField]List<T> test = new List<T>();
    public List<T> Test {
        get => test;
        set => test = value;
    }
    [SerializeField]List<T> test1;
    [SerializeField]int[][] test3 = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 } }; ///다중배열은 안된다.
    [SerializeField]string[] tempString = {"이","게","저","장","되","네" };
    [SerializeField]stringArray[] tempArray = new stringArray[3];
    [SerializeField]stringDoubleArray[] tempDoubleArray = new stringDoubleArray[3];
    [SerializeField]float[] yamiTest = { 10.5f, 20.6f, 33.41f,555.3f};
    public void TestFunc() {

        //다중배열 처리 방식 데이터입력 확인  json파싱도 확인완료  정상적으로 저장되고 불러옴 
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i].Values = new string[3]; //새로만들어서 
            tempArray[i].tempInt = new int[3];
            for (int j = 0; j < tempArray[i].Values.Length; j++)
            {
                tempArray[i].Values[j] = tempString[j]; //하나씩 넣어도되고 
            }
            for (int j = 0; j < tempArray[i].tempInt.Length; j++) {
                tempArray[i].tempInt[j] = invenIndex[j];
            }
        }
        for (int i = 0; i < tempDoubleArray.Length; i++)
        {
            tempDoubleArray[i].Values = tempArray;// 밖에서 만들어진것을 집어넣어도된다.
            tempDoubleArray[i].yami = yamiTest;
        }
        stringTripleArray  = new stringTripleArray(tempDoubleArray);


        //foreach는 읽기 전용으로 만들어져있어서 value 변수는 기본적으로 readonly 값을 가진다.
        //foreach (stringArray value in tempArray) { 
        //    value.values = tempString;
        //}

    }
    public JsonGameData SetSaveData() {
        TestSaveData<T> sd = new();
        sd.TestFunc();
        sd.SkillList = new StructList.CharcterSkills[2];
        sd.SkillList[0].SkillIndex = 0;
        sd.SkillList[1].SkillIndex = 1;
        sd.SkillList[0].Values = 94;
        sd.SkillList[1].Values = 922;
        sd.ItemList = new StructList.CharcterItems[2];
        sd.ItemList[0].ItemIndex = 923;
        sd.ItemList[1].ItemIndex = 9;
        sd.ItemList[0].Values = 212;
        sd.ItemList[1].Values = 22;
        sd.CharcterInfo = new StructList.CharcterInfo[1];
        sd.CharcterInfo[0].EXP = 59458.2332f;
        sd.CharcterInfo[0].CharcterName = "장발산";
        sd.CharcterInfo[0].CharcterPosition = Vector3.zero;
        sd.CharcterInfo[0].Level = 57;
        sd.QuestList = new StructList.CharcterQuest[98];
        for (int i= 0; i< sd.QuestList.Length; i++) {
            sd.QuestList[i].QuestIndex = i;
            sd.QuestList[i].QuestIProgress = i * i;
        }
        return sd;
    }

    public void SaveDataParsing(JsonGameData OriginData) {
        int a =  OriginData.DataIndex;
        EnumList.SceanName o = OriginData.SceanName;
        string time = OriginData.SaveTime;
        StructList.CharcterSkills[] b = OriginData.SkillList;
        StructList.CharcterInfo[] c = OriginData.CharcterInfo;
        StructList.CharcterItems[] d = OriginData.ItemList;
        StructList.CharcterQuest[] f = OriginData.QuestList;

    }
}
