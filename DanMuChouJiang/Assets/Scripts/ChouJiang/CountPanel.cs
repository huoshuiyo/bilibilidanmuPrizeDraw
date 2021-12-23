using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountPanel : MonoBehaviour
{
    public Text orderText;
    public Text prizeText;
    public Text countText;
    public Text fansLevelText;

    public GameObject beginCountBotton;
    public GameObject endCountBotton;
    public GameObject prizeDrawBotton;
    public GameObject settingBotton;

    public GameObject prizeDrawPanel;
    public GameObject settingPanel;

    private void Start()
    {
        ResetCountPanel();
    }
    public void ResetCountPanel() 
    {
        beginCountBotton.SetActive(true);
        endCountBotton.SetActive(false);
        prizeDrawBotton.SetActive(false);
        countText.gameObject.SetActive(false);
        settingBotton.SetActive(true);
        fansLevelText.gameObject.SetActive(false);
    }

    public void BeginToCount() 
    {
        beginCountBotton.SetActive(false);
        endCountBotton.SetActive(true);
        prizeDrawBotton.SetActive(false);
        countText.gameObject.SetActive(true);
        settingBotton.SetActive(false);
        if (int.Parse(ListOfUserController.controller.fansMedalLevel)>0)
        {
            fansLevelText.gameObject.SetActive(true);
            fansLevelText.text = "粉丝牌等级>="+ ListOfUserController.controller.fansMedalLevel;
        }       
        ListOfUserController.controller.listOfUser = new List<string>();
        Danmu.isBegin = true;

    }

    //结束统计
    public void EndToCount()
    {
        beginCountBotton.SetActive(true);
        endCountBotton.SetActive(false);
        prizeDrawBotton.SetActive(true);
        settingBotton.SetActive(true);
        fansLevelText.gameObject.SetActive(false);
        Danmu.isBegin = false;
    }

    public void OpenChouJiangPanel() 
    {
        prizeDrawPanel.SetActive(true);
        ListOfUserController.controller.ResetListOfPrizePool();
        prizeDrawPanel.GetComponent<PrizeDrawPanel>().DeleteUserInWinner();
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseCountPanel() 
    {
        this.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        orderText.text = "抽奖口令："+ ListOfUserController.controller.order;
        countText.text = "现在已经有" + ListOfUserController.controller.listOfUser.Count.ToString() + "人参加了抽奖";
        prizeText.text = "奖品：" + ListOfUserController.controller.prize;
    }
}
