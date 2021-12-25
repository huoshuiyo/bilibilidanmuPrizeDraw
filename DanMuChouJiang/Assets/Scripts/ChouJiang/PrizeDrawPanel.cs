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

    public InputField ziDingYiChouJiang;

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
        string winner = ListOfUserController.controller.listOfPrizePool.OrderBy(u => Guid.NewGuid()).First();
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
        EnterPrizeDraw(int.Parse(ziDingYiChouJiang.text));
    }

    public void CheckCustonmisedDraw()
    {
        if (int.Parse(ziDingYiChouJiang.text) > ListOfUserController.controller.listOfPrizePool.Count)
        {
            ziDingYiChouJiang.text = ListOfUserController.controller.listOfPrizePool.Count.ToString();
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
        AddListOfWinner();
        this.gameObject.SetActive(false);
    }

    public void AddListOfWinner() 
    {
        if (winnerNotSure.Count == 0)
        {
            return;
        }
        if (ListOfUserController.controller.listOfWinner.Count == 0)
        {
            foreach (var winner in winnerNotSure)
            {
                ListOfUserController.controller.listOfWinner.Add(winner);
            }
            return;
        }
        foreach (string winner in winnerNotSure)
        {
            int i = 0;
            foreach (string winUser in ListOfUserController.controller.listOfWinner)
            {
                if (winner == winUser)
                {
                    i++;
                    break;
                }
            }
            if (i == 0)
            {
                ListOfUserController.controller.listOfWinner.Add(winner);
            }

        }
    }

    private void Update()
    {
        countInPool.text = "当前奖池还有" + ListOfUserController.controller.listOfPrizePool.Count.ToString() + "人";
        prizeText.text = "奖品：" + ListOfUserController.controller.prize;
    }
}
