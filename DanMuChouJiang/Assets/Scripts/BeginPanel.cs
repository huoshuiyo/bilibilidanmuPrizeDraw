using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginPanel : MonoBehaviour
{
    public InputField roomInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("RoomID"))
        {
            roomInput.text = PlayerPrefs.GetString("RoomID");
        }
    }

    public void EnterRoom() 
    {
        if (roomInput.text=="")
        {
            Debug.Log("请输入房间号");
            return;
        }
        Danmu.roomID = int.Parse(roomInput.text);
        PlayerPrefs.SetString("RoomID", roomInput.text);
        SceneManager.LoadScene(1);
    }
}
