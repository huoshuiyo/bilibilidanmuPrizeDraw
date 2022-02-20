using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfUserController : MonoBehaviour
{

    #region SingletonMode
    public static ListOfUserController controller;
    private void Awake()
    {
        controller = this;
    }
    #endregion

    public Hashtable danmuHS = new Hashtable();
    public List<string> listOfUser = new List<string>();
    public List<string> listOfPrizePool = new List<string>();
    public List<string> listOfWinnerExcluded = new List<string>();
    public List<string> listOfWinnerAnim = new List<string>();


    public string prize="";
    public string popularity = "";
    public string order = "";
    public string fansMedal = "";
    public string fansMedalLevel = "";

    public bool isWinnerExcluded = false;

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

    public void ResetListOfWinner()
    {
        listOfWinnerExcluded = new List<string>();
    }

    public void ResetListOfPrizePool()
    {
        listOfPrizePool = new List<string>();
        if (isWinnerExcluded)
        {
            RemoveWinnerInListOfUser();
        }
        foreach (var item in listOfUser)
        {
            listOfPrizePool.Add(item);
        }
    }

    public void RemoveListOfPrizePool(string username)
    {
        listOfPrizePool.Remove(username);
    }

    public void RemoveListOfWinnerAnim(string username)
    {
        listOfWinnerAnim.Remove(username);
    }

    public void RemoveWinnerInListOfUser()
    {
        foreach (var winner in listOfWinnerExcluded)
        {
            try
            {
                listOfUser.Remove(winner);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
