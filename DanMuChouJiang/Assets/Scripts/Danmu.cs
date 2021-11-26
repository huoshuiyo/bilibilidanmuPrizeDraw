using BiliDMLib;
using DanmuHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Danmu : MonoBehaviour
{
    public static int roomID;
    public GameObject userItem;

    public Transform parent;
    DanmakuLoader ld;


    public Dictionary<int, string> imgdic;

    HttpRequestHelp h = new HttpRequestHelp();

    public static List<string> mingDan = new List<string>();
    public static bool isBegin = false;
    public static string order = "";

    public Transform chouJiangCanYu;

    private async void Start()
    {
        if (PlayerPrefs.HasKey("Order"))
        {
            order = PlayerPrefs.GetString("Order");
        }

        imgdic = new Dictionary<int, string>();
        ld = new DanmakuLoader();
        bool isConnect = await ld.ConnectAsync(roomID);
        Debug.Log(isConnect);
        ld.OnDanmuCallBack = (a, b, c) => {
            HttpRequestHelp.userId = int.Parse(c);
            string imgAddress = "";
            if (!imgdic.ContainsKey(HttpRequestHelp.userId))
            {
                string json = h.GetMsg();
                var obj = JObject.Parse(json);
                imgAddress = obj["data"]["face"].ToString();
                imgdic.Add(HttpRequestHelp.userId, imgAddress);
            }
            else
            {
                imgAddress = imgdic[HttpRequestHelp.userId];
            }
            Generate(a, b , imgAddress); 
        };
    }
    public void Generate(string username,string content,string imgAddress)
    {
        GameObject danMuObj = Instantiate(userItem);
        danMuObj.GetComponent<Content>().imgAddress = imgAddress;
        danMuObj.GetComponent<Content>().username = username;
        danMuObj.GetComponent<Content>().content = content;
        danMuObj.GetComponent<Content>()._username.color = RandomColor();
        danMuObj.transform.SetParent(parent);
        Destroy(danMuObj, 15f);
        if (isBegin)
        {
            //如果弹幕等于口令
            if (content == order)
            {
                int checkID = 0;
                foreach (var ID in mingDan) 
                {
                    if (username == ID)
                    {
                        checkID++;
                        break;
                    }
                }
                if (checkID == 0)
                {
                    mingDan.Add(username);
                    GameObject chouJiangObj = Instantiate(userItem);
                    chouJiangObj.GetComponent<Content>().imgAddress = imgAddress;
                    chouJiangObj.GetComponent<Content>().username = username;
                    chouJiangObj.GetComponent<Content>().content = "参与了抽奖";
                    chouJiangObj.GetComponent<Content>()._username.color = RandomColor();
                    chouJiangObj.transform.SetParent(chouJiangCanYu);
                    Destroy(chouJiangObj, 15f);
                }
            }
        }   
    }

    private void OnApplicationQuit()
    {
        if (ld != null)
        {
            ld.OnDanmuCallBack = null;
            ld.Disconnect();
        }
    }

    Color RandomColor()
    {
        //随机颜色的RGB值。即刻得到一个随机的颜色
        float r = UnityEngine.Random.Range(0f, 1f);
        float g = UnityEngine.Random.Range(0f, 1f);
        float b = UnityEngine.Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        return color;
    }
}
