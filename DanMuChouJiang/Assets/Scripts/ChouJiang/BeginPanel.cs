using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginPanel : MonoBehaviour
{
    public InputField roomInput;
    public InputField danmuLevelInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("RoomID"))
        {
            roomInput.text = PlayerPrefs.GetString("RoomID");
        }
        if (PlayerPrefs.HasKey("DanmuLevelInput"))
        {
            danmuLevelInput.text = PlayerPrefs.GetString("DanmuLevelInput");
        }
        else
        {
            danmuLevelInput.text = "0";
        }
    }

    public void EnterRoom() 
    {
        if (roomInput.text=="")
        {
            Debug.Log("请输入房间号");
            return;
        }
        if (danmuLevelInput.text == "")
        {
            Debug.Log("请输入弹幕限制等级");
            return;
        }
        Live.roomID = int.Parse(roomInput.text);
        PlayerPrefs.SetString("RoomID", roomInput.text);

        Live.danmakuMinULLevel = int.Parse(danmuLevelInput.text);
        PlayerPrefs.SetString("DanmuLevelInput", danmuLevelInput.text);
        SceneManager.LoadScene(1);
    }
}
