using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danmaku
{
    public string name;
    public string text;
    public Int64 userID;
    public byte GuardLv;
    public string MedalName;
    public int MedalLv;
    public string color;
    public int ULLv;
    public string imgAddress;
}


public class Gift
{
    public Int64 UserID;
    public string UserName;
    public uint TotalPrice;
    public string Action;
    public string GiftName;
    public uint GiftCount;
    public string CoinType;
}

public class Guard
{
    public string userName;
    public Int64 userID;
    public int GuardLevel;
    public int count;
    public string GuardName;
    public uint Price;
}
public class SuperChat
{
    public uint ScID;
    public string userName;
    public Int64 UserID;
    public int GuardLevel;
    public int price;
    public string Message;
}

public class DanmStamp
{
    public int count;
    public DateTime timeStamp;
}

public class GiftDetail
{
    /// <summary>
    /// 礼物名
    /// </summary>
    public string GiftName;
    /// <summary>
    /// 该用户送该礼物的总价值
    /// </summary>
    public int GiftTotalCost;
    /// <summary>
    /// 该用户送该礼物的总数量
    /// </summary>
    public int GiftTotalCount;
    /// <summary>
    /// 最后送该礼物的时间，超出5分钟清空
    /// </summary>
    public DateTime LastDateTime;
}
