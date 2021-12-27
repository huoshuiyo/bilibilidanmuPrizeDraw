using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public InputField orderInput;
    public InputField prizeInput;

    public InputField fansMedalInput;
    public InputField fansMedalLevelInput;

    public Text winnerCount;

    public Toggle isOpenWinnerExcluded;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Order"))
        {
            orderInput.text = PlayerPrefs.GetString("Order");
        }
        if (PlayerPrefs.HasKey("Prize"))
        {
            prizeInput.text = PlayerPrefs.GetString("Prize");
        }
        if (PlayerPrefs.HasKey("FansMedal"))
        {
            fansMedalInput.text = PlayerPrefs.GetString("FansMedal");
        }
        if (PlayerPrefs.HasKey("FansMedalLevel"))
        {
            fansMedalLevelInput.text = PlayerPrefs.GetString("FansMedalLevel");
        }
        if (PlayerPrefs.HasKey("IsWinnerExcluded"))
        {
            string a = PlayerPrefs.GetString("IsWinnerExcluded");
            if (a == "true")
            {
                isOpenWinnerExcluded.isOn = true;
            }
            else
            {
                isOpenWinnerExcluded.isOn = false;
            }

        }
    }
    public void FinishSetting() 
    {
        SetOrder();
        SetPrize();
        FansMedal();
        FansMedalLevel();
        SetIsWinnerExcluded();
        this.gameObject.SetActive(false);
    }

    public void SetIsWinnerExcluded()
    {
        ListOfUserController.controller.isWinnerExcluded = isOpenWinnerExcluded.isOn;
        PlayerPrefs.SetString("IsWinnerExcluded", isOpenWinnerExcluded.isOn.ToString());
    }
    public void SetOrder() 
    {
        ListOfUserController.controller.SetOrder(orderInput.text) ;
        PlayerPrefs.SetString("Order", orderInput.text);
    }

    public void SetPrize()
    {
        ListOfUserController.controller.SetPrize(prizeInput.text);
        PlayerPrefs.SetString("Prize", prizeInput.text);
    }
    public void FansMedal()
    {
        ListOfUserController.controller.SetFansMedal(fansMedalInput.text);
        PlayerPrefs.SetString("FansMedal", fansMedalInput.text);
    }

    public void FansMedalLevel()
    {
        ListOfUserController.controller.SetFansMedalLevel(fansMedalLevelInput.text);
        PlayerPrefs.SetString("FansMedalLevel", fansMedalLevelInput.text);
    }

    public void ResetListOfWinner() 
    {
        ListOfUserController.controller.ResetListOfWinner();
        SqliteController.Instance.OpenSqlite();
        SqliteController.Instance.UpdateWinnerExcluded();
        SqliteController.Instance.Release();
    }

    private void Update()
    {
        winnerCount.text = "累计：" + ListOfUserController.controller.listOfWinnerExcluded.Count + "人";
    }
}
