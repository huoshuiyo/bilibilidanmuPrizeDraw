using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleBilibiliDanmakuClient.Clients;
using SimpleBilibiliDanmakuClient.Models;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Live : MonoBehaviour
{
    public static int roomID = 0;
    public Queue<Danmaku> DanmakuQueue = new Queue<Danmaku>();
    public Queue<Gift> GiftQueue = new Queue<Gift>();
    public Queue<Guard> GuardQueue = new Queue<Guard>();
    public Queue<SuperChat> SCQueue = new Queue<SuperChat>();

    public Dictionary<int, string> imgdic;

    HttpRequestHelp h = new HttpRequestHelp();
    [Header("接收弹幕的最低UL等级")]
    public static int danmakuMinULLevel = 0;


    async void Start()
    {
        if (roomID<=0)
        {
            return;
        }
        try
        {
            
            TcpDanmakuClientV2 client = new TcpDanmakuClientV2();
            imgdic = new Dictionary<int, string>();
            await client.ConnectAsync(roomID);
            client.ReceivedMessageHandlerEvt += Client_ReceivedMessageHandlerEvt;
            client.ReceivedPopularityEvt += Client_ReceivedPopularityEvt;
            await Task.Delay(-1);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    #region 人气
    private Task Client_ReceivedPopularityEvt(IDanmakuClient client, ReceivedPopularityEventArgs e)
    {
        //Debug.Log("当前直播间人气数：" + e.Popularity);
        ListOfUserController.controller.SetPopularity(e.Popularity.ToString());
        return Task.CompletedTask;
    }
    #endregion


    private Task Client_ReceivedMessageHandlerEvt(IDanmakuClient client, ReceivedMessageEventArgs e)
    {
        try
        {
            string m = e.Message.ToString();
            //Debug.Log(m);
            JObject obj = (JObject)JsonConvert.DeserializeObject(m);
            string type = obj["cmd"].ToString();
            switch (type)
            {
                #region 进入直播间
                case "INTERACT_WORD":
                    
                    int id = obj["data"]["uid"].ToObject<int>();
                    string name = obj["data"]["uname"].ToString();
                    byte msg_type = obj["data"]["msg_type"].ToObject<byte>();
                    //Debug.Log("进入提示"+ msg_type + "：" + name + "： " +obj.ToString());
                    break;
                #endregion
                #region 弹幕
                case "DANMU_MSG":
                    uint userId = obj["info"][2][0].ToObject<uint>();
                    string userName = obj["info"][2][1].ToString();
                    string content = obj["info"][1].ToString();
                    int mark = obj["info"][0][9].ToObject<int>();
                    string color = obj["info"][0][7].ToString();
                    byte guardLv = obj["info"][7].ToObject<byte>();
                    byte ulLv = obj["info"][4][0].ToObject<byte>();
                    string medelName = "";
                    byte medelLv = 0;
                    if (obj["info"][3].ToString().Length > 5)
                    {
                        medelName = obj["info"][3][1].ToString();
                        medelLv = obj["info"][3][0].ToObject<byte>();
                    }



                    //Debug.Log(string.Format("[弹幕{6}]{0}：{1}   [舰长等级:{2}，勋章名:{7},勋章等级:{3}，UL:{4}，?:{5}]", userName, content, guardLv, medelLv, ulLv, color, userId, medelName));

                  
                    string imgAddress = "";
                    //爬头像未解决 问题:请求过多会被B站Ban了                
                    //try
                    //{
                    //    HttpRequestHelp.userId = (int)userId;
                    //    if (!imgdic.ContainsKey(HttpRequestHelp.userId))
                    //    {
                    //        string json = h.GetMsg();
                    //        var obj2 = JObject.Parse(json);
                    //        imgAddress = obj2["data"]["face"].ToString();
                    //        Debug.Log(imgAddress);
                    //        imgdic.Add(HttpRequestHelp.userId, imgAddress);
                    //    }
                    //    else
                    //    {
                    //        imgAddress = imgdic[HttpRequestHelp.userId];
                    //    }
                    //    Debug.Log(imgAddress);
                    //}
                    //catch (Exception)
                    //{
                    //    throw;
                    //}

                    //Debug.Log(string.Format("{0},{1}", obj["info"][3][1].ToString(), obj["info"][3][0].ToObject<byte>()));
                    if (ulLv >= danmakuMinULLevel)
                        DanmakuQueue.Enqueue(new Danmaku
                        {
                            name = userName,
                            text = content,
                            userID = userId,
                            GuardLv = guardLv,
                            color = color,
                            MedalLv = medelLv,
                            ULLv = ulLv,
                            MedalName = medelName,
                            imgAddress = imgAddress,
                        });
                    break;
                #endregion
                #region 礼物
                case "SEND_GIFT":
                    userId = obj["data"]["uid"].ToObject<uint>();
                    userName = obj["data"]["uname"].ToString();
                    uint totalPrice = obj["data"]["total_coin"].ToObject<uint>();
                    string action = obj["data"]["action"].ToString();
                    string giftName = obj["data"]["giftName"].ToString();
                    uint giftCount = obj["data"]["num"].ToObject<uint>();
                    string coinType = obj["data"]["coin_type"].ToString();
                    //Debug.Log(string.Format("[礼物信息]赠送者:{0}，礼物名：{1} {2}个，" +
                    //    "方式：{3}，价值：{4}，类型：{5}", userName, giftName,
                    //    giftCount, action, totalPrice, coinType));

                    GiftQueue.Enqueue(new Gift
                    {
                        Action = action,
                        GiftName = giftName,
                        GiftCount = giftCount,
                        TotalPrice = totalPrice,
                        UserID = (uint)userId,
                        UserName = userName,
                        CoinType = coinType
                    });
                    break;
                #endregion
                #region 舰长
                case "USER_TOAST_MSG"://舰长
                    userId = obj["data"]["uid"].ToObject<uint>();
                    userName = obj["data"]["username"].ToString();
                    giftName = obj["data"]["role_name"].ToString();
                    giftCount = obj["data"]["num"].ToObject<uint>();
                    byte guardLevel = obj["data"]["guard_level"].ToObject<byte>();
                    uint price = obj["data"]["price"].ToObject<uint>();
                    //Debug.Log(string.Format("[大航海]赠送者：{0}，礼物名：{1} {2}个，" +
                    //    "价值：{3}", userName, giftName, giftCount, price));

                    GuardQueue.Enqueue(new Guard
                    {
                        count = (int)giftCount,
                        GuardLevel = guardLevel,
                        GuardName = giftName,
                        userID = userId,
                        userName = userName,
                        Price = price
                    });
                    break;
                #endregion
                #region 超级留言
                case "SUPER_CHAT_MESSAGE"://SuperChat
                    uint SCID = obj["data"]["id"].ToObject<uint>();
                    userId = obj["data"]["uid"].ToObject<uint>();
                    price = obj["data"]["price"].ToObject<uint>();
                    string message = obj["data"]["message"].ToString();
                    userName = obj["data"]["user_info"]["uname"].ToString();
                    //Debug.Log(string.Format("[SuperChat]发送者：{0}，内容：{1}，价值：{2}",
                    //    userName, message, price));

                    SCQueue.Enqueue(new SuperChat
                    {
                        Message = message,
                        price = (int)price,
                        ScID = SCID,
                        UserID = userId,
                        userName = userName
                    });
                    break;
                #endregion
                #region 其他
                case "ONLINE_RANK_V2"://高能榜
                    break;
                case "ROOM_BLOCK_MSG"://黑名单
                    break;
                case "ROOM_REAL_TIME_MESSAGE_UPDATE"://粉丝数更新
                    uint fans = obj["data"]["fans"].ToObject<uint>();
                    Debug.Log(string.Format("[粉丝数发生变化]：{0}", fans));
                    break;
                    #endregion

            }

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }
}
