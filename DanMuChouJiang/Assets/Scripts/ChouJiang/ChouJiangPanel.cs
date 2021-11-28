using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class ChouJiangPanel : MonoBehaviour
{
    public Text countInPool;
    public Text prizeText;

    public InputField ziDingYiChouJiang;

    [Header("奖池")]
    public GameObject poolPanel;

    public Pool pool;

    [Header("名单预制物")]
    public GameObject userInWinner;
    [Header("名单生成地")]
    public Transform parent;

    #region 抽奖
    //单抽
    public void ChouJiang()
    {
        if (MingDanController.controller.jiangChi.Count <= 0)
        {
            return;
        }
        string winner = MingDanController.controller.jiangChi.OrderBy(u => Guid.NewGuid()).First();
        MingDanController.controller.RemoveJiangChi(winner);
        CreateUserInWinner(winner);
        Debug.Log("jiangchi: "+ MingDanController.controller.jiangChi.Count);
        Debug.Log("mingdan: "+ MingDanController.controller.mingDan.Count);

    }
    //多抽
    public void DuoChou(int number)
    {
        for (int i = 0; i < number; i++)
        {
            ChouJiang();
        }
    }
    //自定义
    public void ZiDingYiDuoChou()
    {
        DuoChou(int.Parse(ziDingYiChouJiang.text));
    }

    public void CheckZiDingYiChouJiang()
    {
        if (int.Parse(ziDingYiChouJiang.text) > MingDanController.controller.jiangChi.Count)
        {
            ziDingYiChouJiang.text = MingDanController.controller.jiangChi.Count.ToString();
        }
    }

    #endregion

    //重置
    public void ResetPool()
    {
        MingDanController.controller.ResetJiangChi();
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

    public void OpenPoolPanel()
    {
        pool.CreateUserInPool();
        poolPanel.SetActive(true);
    }

    public void CloseChoujiangPanel()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        countInPool.text = "当前奖池还有" + MingDanController.controller.jiangChi.Count.ToString() + "人";
        prizeText.text = "奖品：" + MingDanController.controller.prize;
    }
}
