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


    // Update is called once per frame
    void Update()
    {
        countInPool.text = "当前奖池还有"+ MingDanController.controller.jiangChi.Count.ToString() + "人";
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
        foreach (var user in MingDanController.controller.jiangChi)
        {
            GameObject userInPoolObj = Instantiate(userInPool);
            userInPoolObj.GetComponent<UserInPool>().SetUserName(user);
            userInPoolObj.transform.SetParent(parent);
        }
    }
    //响指
    public void XiangZhi()
    {
        int jiangChiCount = MingDanController.controller.jiangChi.Count;
        for(int i = 0;i < (jiangChiCount/2);i++) 
        {
            string loser = MingDanController.controller.jiangChi.OrderBy(u => Guid.NewGuid()).First();
            MingDanController.controller.RemoveJiangChi(loser);
        }
        CreateUserInPool();
    }

    //关闭
    public void ClosePool()
    {
        this.gameObject.SetActive(false);
    }
}
