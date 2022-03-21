using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class PrizeDrawPanel : MonoBehaviour
{
    public Text countInPool;
    public Text prizeText;
    public Text errorText;
    public InputField customInputField;
    [Header("奖池")]
    public GameObject poolPanel;
    public Pool pool;
    [Header("CSGORoll")]
    public GameObject cSGORollPanel;

    [Header("名单预制物")]
    public GameObject userInWinner;
    [Header("名单生成地")]
    public Transform parent;


    public GameObject mainPanel;
    public GameObject countPanel;
    public GameObject prizeDrawAnimPanel;

    public GameObject mainCamera;

    public GameObject prizeDrawAnim;
    public GameObject animCamera;

    public List<string> winnerNotSure = new List<string>();

    public void OpenAnim()
    {
        mainPanel.SetActive(false);
        countPanel.SetActive(false);
        mainCamera.SetActive(false);

        prizeDrawAnimPanel.SetActive(true);
        prizeDrawAnim.SetActive(true);
        animCamera.SetActive(true);

        prizeDrawAnimPanel.GetComponent<PrizeDrawAnimPanel>().winnerName.gameObject.SetActive(false);
        prizeDrawAnimPanel.GetComponent<PrizeDrawAnimPanel>().DestroyAnimObj();
        prizeDrawAnimPanel.GetComponent<PrizeDrawAnimPanel>().button.SetActive(false);

        //prizeDrawAnimPanel.GetComponent<PrizeDrawAnimPanel>().isShow = false;

        this.gameObject.SetActive(false);

    }
    System.Random random;
    #region 抽奖
    public void EnterPrizeDraw(int number)
    {
        if (ListOfUserController.controller.listOfPrizePool.Count <= 0)
        {
            return;
        }
        if (number <= 0)
        {
            return;
        }
        random = new System.Random((int)DateTime.Now.Ticks);
        if (number == 1)
        {
            SingleDraw();
        }
        if (number > 1)
        {
            MultipleDraws(number);
        }
        OpenAnim();
    }

    //单抽
    public void SingleDraw()
    {
        if (ListOfUserController.controller.listOfPrizePool.Count <= 0)
        {
            return;
        }


        string winner = ListOfUserController.controller.listOfPrizePool.OrderBy(s => random.Next(0, 100)).First();

        ListOfUserController.controller.RemoveListOfPrizePool(winner);
        CreateUserInWinner(winner);
        ListOfUserController.controller.listOfWinnerAnim.Add(winner);

        foreach (string winUser in winnerNotSure)
        {
            if (winUser == winner)
            {
                return;
            }
        }
        winnerNotSure.Add(winner);
        //Debug.Log("jiangchi: "+ ListOfUserController.controller.ListOfPrizePool.Count);
        //Debug.Log("mingdan: "+ ListOfUserController.controller.listOfUser.Count);

    }
    //多抽
    public void MultipleDraws(int number)
    {
        for (int i = 0; i < number; i++)
        {
            SingleDraw();
        }
    }
    //自定义
    public void CustonmisedDraw()
    {
        EnterPrizeDraw(int.Parse(customInputField.text));
    }

    public void CheckCustonmisedDraw()
    {
        if (int.Parse(customInputField.text) > ListOfUserController.controller.listOfPrizePool.Count)
        {
            customInputField.text = ListOfUserController.controller.listOfPrizePool.Count.ToString();
        }
    }

    //重置
    public void ResetPool()
    {
        ListOfUserController.controller.ResetListOfPrizePool();
        winnerNotSure = new List<string>();
        DeleteUserInWinner();
    }

    public void CreateUserInWinner(string user)
    {
        //创造
        GameObject userInPoolObj = Instantiate(userInWinner);
        userInPoolObj.GetComponent<UserInPool>().SetUserName(user);
        userInPoolObj.transform.SetParent(parent);
    }

    public void DeleteUserInWinner()
    {
        //删除
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    #endregion


    public void OpenPoolPanel()
    {
        pool.CreateUserInPool();
        poolPanel.SetActive(true);
    }

    public void OpenCSGORollPanel()
    {
        cSGORollPanel.SetActive(true);
    }

    public void CloseChoujiangPanel()
    {
        try
        {
            AddListOfWinner();
            CreateThisRecord();
            winnerNotSure = new List<string>();
            countPanel.GetComponent<CountPanel>().ResetCountPanel();
            this.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            errorText.text = e.ToString();
        }

    }

    public void AddListOfWinner()
    {
        if (winnerNotSure.Count == 0)
        {
            return;
        }
        string Time = GetTimeStampMs().ToString();
        SqliteController.Instance.OpenSqlite();
        foreach (string winner in winnerNotSure)
        {
            SqliteController.Instance.InsertWinner(winner, ListOfUserController.controller.prize, Time);
        }
        SqliteController.Instance.Release();
        if (ListOfUserController.controller.listOfWinnerExcluded.Count == 0)
        {
            foreach (var winner in winnerNotSure)
            {
                ListOfUserController.controller.listOfWinnerExcluded.Add(winner);
            }
            return;
        }
        foreach (string winner in winnerNotSure)
        {
            int i = 0;
            foreach (string winUser in ListOfUserController.controller.listOfWinnerExcluded)
            {
                if (winner == winUser)
                {
                    i++;
                    break;
                }
            }
            if (i == 0)
            {
                ListOfUserController.controller.listOfWinnerExcluded.Add(winner);
            }
        }


    }

    public void CreateThisRecord()
    {
        if (winnerNotSure.Count == 0)
        {
            return;
        }
        WinnersInfoItem winnersInfoItem = new WinnersInfoItem
        {
            prize = ListOfUserController.controller.prize,
            winners = winnerNotSure
        };
        WinnerInfoController.controller.CreateWinnerRecordNew(winnersInfoItem);
    }

    private void Update()
    {
        countInPool.text = "当前奖池还有" + ListOfUserController.controller.listOfPrizePool.Count.ToString() + "人";
        prizeText.text = "奖品：" + ListOfUserController.controller.prize;
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    private double GetTimeStampMs()
    {
        double timeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000) * 0.001;
        return timeStamp;
    }
}
