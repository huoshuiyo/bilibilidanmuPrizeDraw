using System;
using System.Collections;
using System.Collections.Generic;

using SatorImaging.AppWindowUtility;

using UnityEngine;
using UnityEngine.UI;

public class DanmuSettingPanel : MonoBehaviour
{
    public GameObject guardGiftPart;
    public GameObject goldGiftPart;
    public GameObject freeGiftPart;

    public Toggle guardGiftToggle;
    public Toggle goldGiftToggle;
    public Toggle freeGiftToggle;

    public InputField danmuLevelInput;

    public GameObject openEButton;
    public GameObject openLvButton;
    public GameObject closeEButton;
    public GameObject closeLvButton;

    public GameObject eButton;
    public GameObject lvMu;

    public Text errorText;

    public void ChangeView(int a)
    {
        switch (a)
        {
            case 0:
                Screen.SetResolution(500, 750, false);
                break;
            case 1:
                Screen.SetResolution(200, 750, false);
                break;
            default:
                Screen.SetResolution(1200, 750, false);
                break;
        }

    }

    public void OpenE()
    {
        openEButton.SetActive(false);
        closeEButton.SetActive(true);
        eButton.SetActive(true);
    }

    public void CloseE()
    {
        openEButton.SetActive(true);
        closeEButton.SetActive(false);
        eButton.SetActive(false);
    }

    public void OpenLv()
    {
#if UNITY_STANDALONE_WIN
        AppWindowUtility.Transparent = true;
        AppWindowUtility.AlwaysOnTop = true;
#endif

        openLvButton.SetActive(false);
        closeLvButton.SetActive(true);
        lvMu.SetActive(false);

    }
    public void CloseLv()
    {
#if UNITY_STANDALONE_WIN
        AppWindowUtility.Transparent = false;
        AppWindowUtility.AlwaysOnTop = false;
#endif
        openLvButton.SetActive(true);
        closeLvButton.SetActive(false);
        lvMu.SetActive(true);

    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("DanmuLevelInput"))
        {
            danmuLevelInput.text = PlayerPrefs.GetString("DanmuLevelInput");
        }
        else
        {
            danmuLevelInput.text = "0";
        }
    }
    public void FinishSetting()
    {

        if (danmuLevelInput.text == "")
        {
            Debug.Log("请输入弹幕限制等级");
            return;
        }
        GiftShowControl();
        Live.danmakuMinULLevel = int.Parse(danmuLevelInput.text);
        PlayerPrefs.SetString("DanmuLevelInput", danmuLevelInput.text);
        this.gameObject.SetActive(false);
    }

    public void GiftShowControl()
    {
        if (guardGiftToggle.isOn)
        {
            guardGiftPart.SetActive(true);
        }
        else
        {
            guardGiftPart.SetActive(false);
        }
        if (goldGiftToggle.isOn)
        {
            goldGiftPart.SetActive(true);
        }
        else
        {
            goldGiftPart.SetActive(false);
        }
        if (freeGiftToggle.isOn)
        {
            freeGiftPart.SetActive(true);
        }
        else
        {
            freeGiftPart.SetActive(false);
        }
    }




}
