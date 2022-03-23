using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    public int danmuCount;
    public InputField InputField;
    public Live live;

    public void AddDanmu() 
    {
        string  name = "";
        danmuCount = int.Parse(InputField.text);


        for (int i = 0; i < danmuCount; i++)
        {
            live.DanmakuQueue.Enqueue(new Danmaku
            {
                name ="火水" + i,
                text = "test一二三四一二三四一二三四一二三四一二三四一二三四",
                userID = (uint)i,
                GuardLv = (byte)1,
                color = "1",
                MedalLv = 1,
                ULLv = 1,
                MedalName = "hs",
                imgAddress = "",
            });
        }

    }
}
