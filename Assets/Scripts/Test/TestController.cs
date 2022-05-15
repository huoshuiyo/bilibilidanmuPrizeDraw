using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    public int danmuCount;
    public InputField InputField;
    public Live live;
    public string testString;
    public int guardLevel;

    public void AddDanmu() 
    {
        string  name = "";
        danmuCount = int.Parse(InputField.text);


        for (int i = 0; i < danmuCount; i++)
        {
            live.DanmakuQueue.Enqueue(new Danmaku
            {
                name ="火水" + i,
                text = testString,
                userID = (uint)i,
                GuardLv = (byte)guardLevel,
                color = "1",
                MedalLv = 1,
                ULLv = 1,
                MedalName = "hs",
                imgAddress = "",
            });
        }

    }
}
