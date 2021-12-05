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

    #region 抽奖
    //单抽
    public void SingleDraw()
    {
        if (ListOfUserController.controller.ListOfPrizePool.Count <= 0)
        {
            return;
        }
        string winner = ListOfUserController.controller.ListOfPrizePool.OrderBy(u => Guid.NewGuid()).First();
        ListOfUserController.controller.RemoveListOfPrizePool(winner);
        CreateUserInWinner(winner);
        Debug.Log("jiangchi: "+ ListOfUserController.controller.ListOfPrizePool.Count);
        Debug.Log("mingdan: "+ ListOfUserController.controller.listOfUser.Count);

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
        MultipleDraws(int.Parse(ziDingYiChouJiang.text));
    }

    public void CheckCustonmisedDraw()
    {
        if (int.Parse(ziDingYiChouJiang.text) > ListOfUserController.controller.ListOfPrizePool.Count)
        {
            ziDingYiChouJiang.text = ListOfUserController.controller.ListOfPrizePool.Count.ToString();
        }
    }

    //重置
    public void ResetPool()
    {
        ListOfUserController.controller.ResetListOfPrizePool();
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
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        countInPool.text = "当前奖池还有" + ListOfUserController.controller.ListOfPrizePool.Count.ToString() + "人";
        prizeText.text = "奖品：" + ListOfUserController.controller.prize;
    }
}
