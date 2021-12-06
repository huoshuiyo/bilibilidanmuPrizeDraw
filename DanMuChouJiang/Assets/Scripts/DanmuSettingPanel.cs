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
