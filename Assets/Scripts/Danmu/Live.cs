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
using BLiveAPI;

public class Live : MonoBehaviour
{
    public static int roomID = 0;

    public Queue<Danmaku> DanmakuQueue = new Queue<Danmaku>();
    public Queue<Gift> GiftQueue = new Queue<Gift>();
    public Queue<Guard> GuardQueue = new Queue<Guard>();
    public Queue<SuperChat> SCQueue = new Queue<SuperChat>();

    public Dictionary<int, string> imgdic;

    HttpRequestHelp h = new HttpRequestHelp();
    [Header("danmakuMinULLevel")] public static int danmakuMinULLevel = 0;

    public Text errorText;

    private BLiveApi api;
    

    public float UILockTime = 5f;

    public void ReConnectToDanmu()
    {
        if (UILockTime > 0)
        {
            return;
        }

       
        UILockTime = 5f;
        api.Close();
        Start();
    }

    async void Start()
    {

        if (roomID <= 0)
        {
            return;
        }
        api = new BLiveApi();
        api.DanmuMsg += DanmuMsgEvent;
        api.OpHeartbeatReply += OpHeartbeatReplyEvent;
        api.WebSocketClose += WebSocketCloseEvent;
        api.WebSocketError += WebSocketErrorEvent;
        api.DecodeError += DecodeErrorEvent;
        api.OpSendSmsReply += OnSendGiftEvent;
        api.OpSendSmsReply += OtherMessagesEvent;

        try
        {
            await api.Connect((ulong)roomID, 2);
        }
        catch (Exception e)
        {
            if (e is WebSocketCloseException)
            {
                Debug.Log(e);
            }
            else
            {
                Debug.LogError(e);
            }
        }
    }
    private void DanmuMsgEvent(object sender, (string msg, ulong userId, string userName, int guardLevel,string face, JObject rawData) e)
    {
        Client_ReceivedMessageEvt(e.rawData);
    }

//用于接收心跳消息的方法
    private void OpHeartbeatReplyEvent(object sender, (int heartbeatReply, byte[] rawData) e)
    {
    }
//用于接收主动关闭消息的方法(使用者主动调用api.Close()时),同时会触发异常
    private void WebSocketCloseEvent(object sender, (string message, int code) e)
    {
    }
//用于接收被动关闭消息的方法(一般是网络错误等原因),同时会触发异常
    private void WebSocketErrorEvent(object sender, (string message, int code) e)
    {
    }
//用于接收API内部解码错误,一般情况下不会触发,除非B站改逻辑或其他特殊情况,此消息触发时不会引起异常
//目前发现在不同的C#版本引入库时会出现不同的问题，所以暂时将此异常抛出并终止与直播间的连接
//Unity使用本库时Brotli库不可用,在使用API的Connect方法时请将第二个参数设置为2
//.NET项目使用本库时需要自己在NuGet安装 Newtonsoft.Json
//.NET Framework项目目前使用无问题
    private void DecodeErrorEvent(object sender, (string message, Exception e) e)
    {
    }
//用于接收API内部提供的一个简单处理过后的弹幕消息的方法

//当方法与OpSendSmsReply绑定时需要使用[TargetCmd("cmd1","cmd2"...)]设置方法想要接收的命令,建议每个方法只设置1个命令
//此方法是使用者自定义的用于接收OpSendSmsReply事件中SEND_GIFT命令对应的事件的方法
    [TargetCmd("SEND_GIFT")]
    private void OnSendGiftEvent(object sender, (string cmd, string hitCmd, JObject rawData) e)
    {
        Client_ReceivedMessageEvt(e.rawData);
    }
//TargetCmd支持填入ALL和OTHERS
//当携带ALL或者没有标注[TargetCmd("cmd1","cmd2"...)]时,该方法会无差别的接收所有SMS消息,但会首先命中TargetCmd参数列表中的其他命令
//当cmd只命中ALL或此方法未携带[TargetCmd("cmd1","cmd2"...)]时,不视作命令被命中,携带OTHERS的方法仍然会被Invoke
    [TargetCmd("ALL")]
    private void OnAllEvent(object sender, (string cmd, string hitCmd, JObject rawData) e)
    {
        Client_ReceivedMessageEvt(e.rawData);
    }
//当携带OTHERS时,该方法会接收未被其他方法命中的SMS消息,但TargetCmd参数列表中的其他命令被命中时不会被再次Invoke
    [TargetCmd("OTHERS")]
    private  void OtherMessagesEvent(object sender, (string cmd, string hitCmd, JObject rawData) e)
    {
        Client_ReceivedMessageEvt(e.rawData);
    }
    private void Update()
    {
        UILockTime -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        try
        {
            api.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    #region ÈËÆø

    private Task Client_ReceivedPopularityEvt(IDanmakuClient client, ReceivedPopularityEventArgs e)
    {
        //Debug.Log("µ±Ç°Ö±²¥¼äÈËÆøÊý£º" + e.Popularity);
        //ListOfUserController.controller.SetPopularity(e.Popularity.ToString());
        return Task.CompletedTask;
    }

    #endregion

    public void Client_ReceivedMessageEvt(JObject obj)
    {
        try
        {

            string type = obj["cmd"].ToString();
            switch (type)
            {
                #region INTERACT_WORD

                case "INTERACT_WORD":

                    Int64 id = obj["data"]["uid"].ToObject<Int64>();
                    string name = obj["data"]["uname"].ToString();
                    byte msg_type = obj["data"]["msg_type"].ToObject<byte>();
                    //Debug.Log("msg_type"+ msg_type + "name" + name + "obj.ToString()" +obj.ToString());
                    break;

                #endregion

                #region WATCHED_CHANGE

                case "WATCHED_CHANGE":

                    int num = obj["data"]["num"].ToObject<int>();
                    //string text_small = obj["data"]["uname"].ToString();
                    //byte msg_type = obj["data"]["msg_type"].ToObject<byte>();
                    ListOfUserController.controller.SetPopularity(num.ToString());
                    break;

                #endregion

                #region DANMU_MSG

                case "DANMU_MSG":
                    Int64 userId = obj["info"][2][0].ToObject<Int64>();
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
                    
                                                                     
           
                    if (userName.Length == 4)
                    {
                        // 判断后三个字符是否为***
                        if (userName[1] == '*' && userName[2] == '*' && userName[3] == '*' )
                        {
                            Debug.LogError("chong lian");
                            ReConnectToDanmu();
                            return;
                        }
                    }


                    if (guardLv > 0)
                    {
                        Debug.Log(string.Format(
                            "[userId{6}]{0} content:{1}   [guardLv:{2} medelName:{7}, medelLv:{3} ulLv:{4} color:{5}]",
                            userName, content, guardLv, medelLv, ulLv, color, userId, medelName));
                    }
                    
                    
                    string imgAddress = "";


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

                #region SEND_GIFT

                case "SEND_GIFT":
                    userId = obj["data"]["uid"].ToObject<Int64>();
                    userName = obj["data"]["uname"].ToString();
                    uint totalPrice = obj["data"]["total_coin"].ToObject<uint>();
                    string action = obj["data"]["action"].ToString();
                    string giftName = obj["data"]["giftName"].ToString();
                    uint giftCount = obj["data"]["num"].ToObject<uint>();
                    string coinType = obj["data"]["coin_type"].ToString();
                    //Debug.Log(string.Format("[ÀñÎïÐÅÏ¢]ÔùËÍÕß:{0}£¬ÀñÎïÃû£º{1} {2}¸ö£¬" +
                    //    "·½Ê½£º{3}£¬¼ÛÖµ£º{4}£¬ÀàÐÍ£º{5}", userName, giftName,
                    //    giftCount, action, totalPrice, coinType));

                    GiftQueue.Enqueue(new Gift
                    {
                        Action = action,
                        GiftName = giftName,
                        GiftCount = giftCount,
                        TotalPrice = totalPrice,
                        UserID = userId,
                        UserName = userName,
                        CoinType = coinType
                    });
                    break;

                #endregion

                #region USER_TOAST_MSG

                case "USER_TOAST_MSG": //½¢³¤
                    userId = obj["data"]["uid"].ToObject<Int64>();
                    userName = obj["data"]["username"].ToString();
                    giftName = obj["data"]["role_name"].ToString();
                    giftCount = obj["data"]["num"].ToObject<uint>();
                    byte guardLevel = obj["data"]["guard_level"].ToObject<byte>();
                    uint price = obj["data"]["price"].ToObject<uint>();
                    //Debug.Log(string.Format("[´óº½º£]ÔùËÍÕß£º{0}£¬ÀñÎïÃû£º{1} {2}¸ö£¬" +
                    //    "¼ÛÖµ£º{3}", userName, giftName, giftCount, price));

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

                #region SUPER_CHAT_MESSAGE

                case "SUPER_CHAT_MESSAGE": //SuperChat
                    uint SCID = obj["data"]["id"].ToObject<uint>();
                    userId = obj["data"]["uid"].ToObject<Int64>();
                    price = obj["data"]["price"].ToObject<uint>();
                    string message = obj["data"]["message"].ToString();
                    userName = obj["data"]["user_info"]["uname"].ToString();
                    //Debug.Log(string.Format("[SuperChat]·¢ËÍÕß£º{0}£¬ÄÚÈÝ£º{1}£¬¼ÛÖµ£º{2}",
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

                #region OTHER

                case "ONLINE_RANK_V2": //¸ßÄÜ°ñ
                    break;
                case "ROOM_BLOCK_MSG": //ºÚÃûµ¥
                    break;
                case "ROOM_REAL_TIME_MESSAGE_UPDATE": //·ÛË¿Êý¸üÐÂ
                    uint fans = obj["data"]["fans"].ToObject<uint>();
                    Debug.Log(string.Format("[·ÛË¿Êý·¢Éú±ä»¯]£º{0}", fans));
                    break;

                #endregion
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return;
        }
        
    }
    
    
    public Task Client_ReceivedMessageHandlerEvt(IDanmakuClient client, ReceivedMessageEventArgs e)
    {
        try
        {
            Debug.Log(e);
            string m = e.Message.ToString();
            Debug.Log(m);
            JObject obj = (JObject)JsonConvert.DeserializeObject(m);
            string type = obj["cmd"].ToString();
            switch (type)
            {
                #region INTERACT_WORD

                case "INTERACT_WORD":

                    Int64 id = obj["data"]["uid"].ToObject<Int64>();
                    string name = obj["data"]["uname"].ToString();
                    byte msg_type = obj["data"]["msg_type"].ToObject<byte>();
                    //Debug.Log("msg_type"+ msg_type + "name" + name + "obj.ToString()" +obj.ToString());
                    break;

                #endregion

                #region WATCHED_CHANGE

                case "WATCHED_CHANGE":

                    int num = obj["data"]["num"].ToObject<int>();
                    //string text_small = obj["data"]["uname"].ToString();
                    //byte msg_type = obj["data"]["msg_type"].ToObject<byte>();
                    ListOfUserController.controller.SetPopularity(num.ToString());
                    break;

                #endregion

                #region DANMU_MSG

                case "DANMU_MSG":
                    Int64 userId = obj["info"][2][0].ToObject<Int64>();
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
                    
                                                                     
           
                    if (userName.Length == 4)
                    {
                        // 判断后三个字符是否为***
                        if (userName[1] == '*' && userName[2] == '*' && userName[3] == '*' )
                        {
                            Debug.LogError("chong lian");
                            ReConnectToDanmu();
                            return Task.CompletedTask;
                        }
                    }


                    if (guardLv > 0)
                    {
                        Debug.Log(string.Format(
                            "[userId{6}]{0} content:{1}   [guardLv:{2} medelName:{7}, medelLv:{3} ulLv:{4} color:{5}]",
                            userName, content, guardLv, medelLv, ulLv, color, userId, medelName));
                    }
                    
                    
                    string imgAddress = "";


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

                #region SEND_GIFT

                case "SEND_GIFT":
                    userId = obj["data"]["uid"].ToObject<Int64>();
                    userName = obj["data"]["uname"].ToString();
                    uint totalPrice = obj["data"]["total_coin"].ToObject<uint>();
                    string action = obj["data"]["action"].ToString();
                    string giftName = obj["data"]["giftName"].ToString();
                    uint giftCount = obj["data"]["num"].ToObject<uint>();
                    string coinType = obj["data"]["coin_type"].ToString();
                    //Debug.Log(string.Format("[ÀñÎïÐÅÏ¢]ÔùËÍÕß:{0}£¬ÀñÎïÃû£º{1} {2}¸ö£¬" +
                    //    "·½Ê½£º{3}£¬¼ÛÖµ£º{4}£¬ÀàÐÍ£º{5}", userName, giftName,
                    //    giftCount, action, totalPrice, coinType));

                    GiftQueue.Enqueue(new Gift
                    {
                        Action = action,
                        GiftName = giftName,
                        GiftCount = giftCount,
                        TotalPrice = totalPrice,
                        UserID = userId,
                        UserName = userName,
                        CoinType = coinType
                    });
                    break;

                #endregion

                #region USER_TOAST_MSG

                case "USER_TOAST_MSG": //½¢³¤
                    userId = obj["data"]["uid"].ToObject<Int64>();
                    userName = obj["data"]["username"].ToString();
                    giftName = obj["data"]["role_name"].ToString();
                    giftCount = obj["data"]["num"].ToObject<uint>();
                    byte guardLevel = obj["data"]["guard_level"].ToObject<byte>();
                    uint price = obj["data"]["price"].ToObject<uint>();
                    //Debug.Log(string.Format("[´óº½º£]ÔùËÍÕß£º{0}£¬ÀñÎïÃû£º{1} {2}¸ö£¬" +
                    //    "¼ÛÖµ£º{3}", userName, giftName, giftCount, price));

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

                #region SUPER_CHAT_MESSAGE

                case "SUPER_CHAT_MESSAGE": //SuperChat
                    uint SCID = obj["data"]["id"].ToObject<uint>();
                    userId = obj["data"]["uid"].ToObject<Int64>();
                    price = obj["data"]["price"].ToObject<uint>();
                    string message = obj["data"]["message"].ToString();
                    userName = obj["data"]["user_info"]["uname"].ToString();
                    //Debug.Log(string.Format("[SuperChat]·¢ËÍÕß£º{0}£¬ÄÚÈÝ£º{1}£¬¼ÛÖµ£º{2}",
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

                #region OTHER

                case "ONLINE_RANK_V2": //¸ßÄÜ°ñ
                    break;
                case "ROOM_BLOCK_MSG": //ºÚÃûµ¥
                    break;
                case "ROOM_REAL_TIME_MESSAGE_UPDATE": //·ÛË¿Êý¸üÐÂ
                    uint fans = obj["data"]["fans"].ToObject<uint>();
                    Debug.Log(string.Format("[·ÛË¿Êý·¢Éú±ä»¯]£º{0}", fans));
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