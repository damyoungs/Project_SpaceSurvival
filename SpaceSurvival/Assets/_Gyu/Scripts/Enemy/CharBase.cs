using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBase : MonoBehaviour
{
    public static CharBase instance;

    public int questChapter = 0;

    Transform lockOnTarget;

    public Quest myquest;
    //public List<Quest> myquests;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

}
