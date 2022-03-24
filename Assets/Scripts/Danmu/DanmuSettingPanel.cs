using System.Collections;
using System.Collections.Generic;

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
        openLvButton.SetActive(false);
        closeLvButton.SetActive(true);
        lvMu.SetActive(true);
    }
    public void CloseLv()
    {
        openLvButton.SetActive(true);
        closeLvButton.SetActive(false);
        lvMu.SetActive(false);
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
