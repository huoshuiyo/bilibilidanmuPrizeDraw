using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfUserController : MonoBehaviour
{
    
    #region 单例模式
    public static ListOfUserController controller;
    private void Awake()
    {
        controller = this;
    }
    #endregion

    public List<string> listOfUser = new List<string>();

    public List<string> ListOfPrizePool = new List<string>();

    public string prize="";

    public string popularity = "";

    public string order = "";

    public string fansMedal = "";

    public string fansMedalLevel = "";


    private void Start()
    {
        if (PlayerPrefs.HasKey("Order"))
        {
            order = PlayerPrefs.GetString("Order");
        }
        if (PlayerPrefs.HasKey("Prize"))
        {
            prize = PlayerPrefs.GetString("Prize");
        }
        if (PlayerPrefs.HasKey("FansMedal"))
        {
            fansMedal = PlayerPrefs.GetString("FansMedal");
        }
        if (PlayerPrefs.HasKey("FansMedalLevel"))
        {
            fansMedalLevel = PlayerPrefs.GetString("FansMedalLevel");
        }
    }

    public void AddListOfUser(string username)
    {
        listOfUser.Add(username);
    }

    public void SetPopularity(string popularity)
    {
        this.popularity = popularity;

    }

    public void SetOrder(string order)
    {
        this.order = order;
    }

    public void SetPrize(string prize)
    {
        this.prize = prize;
    }

    public void SetFansMedal(string fansMedal)
    {
        this.fansMedal = fansMedal;
    }

    public void SetFansMedalLevel(string fansMedalLevel)
    {
        this.fansMedalLevel = fansMedalLevel;

    }

    public void ResetListOfUser()
    {
        listOfUser= new List<string>();
    }

    public void ResetListOfPrizePool()
    {
        ListOfPrizePool = new List<string>();
        foreach (var item in listOfUser)
        {
            ListOfPrizePool.Add(item);
        }
    }

    public void RemoveListOfPrizePool(string username)
    {
        ListOfPrizePool.Remove(username);
    }
}
