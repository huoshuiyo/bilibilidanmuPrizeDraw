using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class Pool : MonoBehaviour
{
    [Header("名单预制物")]
    public GameObject userInPool;

    public Text countInPool;

    [Header("名单生成地")]
    public Transform parent;

    [Header("名单列表")]
    public List<GameObject> usersInPool = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        countInPool.text = "当前奖池还有" + ListOfUserController.controller.listOfPrizePool.Count.ToString() + "人";
    }
    //创造池
    public void CreateUserInPool()
    {
        //删除
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);//
        }
        //创造
        usersInPool = new List<GameObject>();
        foreach (var user in ListOfUserController.controller.listOfPrizePool)
        {
            GameObject userInPoolObj = Instantiate(userInPool);
            usersInPool.Add(userInPoolObj);
            userInPoolObj.GetComponent<UserInPool>().SetUserName(user);
            userInPoolObj.transform.SetParent(parent);
        }
    }

    //响指
    System.Random random;
    public void XiangZhi()
    {
        random = new System.Random((int)DateTime.Now.Ticks);
        int userInPoolCount = ListOfUserController.controller.listOfPrizePool.Count;
        for (int i = 0; i < (userInPoolCount / 2); i++)
        {
            DeleteUserFromPool();
        }
    }

    public void DeleteUserFromPool()
    {
        string loser = ListOfUserController.controller.listOfPrizePool.OrderBy(s => random.Next(0, 100)).First();
        foreach (GameObject user in usersInPool)
        {
            string userName = user.GetComponent<UserInPool>().userName.text;
            if (userName == loser)
            {
                user.GetComponent<Animator>().SetBool("isClose", true);
                usersInPool.Remove(user);
                break;
            }
        }
        ListOfUserController.controller.RemoveListOfPrizePool(loser);

    }


    //关闭
    public void ClosePool()
    {
        this.gameObject.SetActive(false);
    }
}
