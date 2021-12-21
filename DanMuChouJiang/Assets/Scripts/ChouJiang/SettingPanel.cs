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

    public Toggle isOpenWinner;

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

            }

        }
    }
    public void FinishSetting() 
    {
        SetOrder();
        SetPrize();
        FansMedal();
        FansMedalLevel();
        this.gameObject.SetActive(false);
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
}
