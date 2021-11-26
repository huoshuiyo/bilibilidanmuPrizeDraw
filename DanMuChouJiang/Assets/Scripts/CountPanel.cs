﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountPanel : MonoBehaviour
{
    public Text orderText;
    public Text countText;

    public GameObject beginCountBotton;
    public GameObject endCountBotton;
    public GameObject chouJiangBotton;

    public GameObject chouJiangPanel;
    public GameObject setOrderPanel;

    private void Start()
    {
        beginCountBotton.SetActive(true);
        endCountBotton.SetActive(false);
        chouJiangBotton.SetActive(false);
        countText.gameObject.SetActive(false);
    }
    //开始统计
    public void BeginToCount() 
    {
        beginCountBotton.SetActive(false);
        endCountBotton.SetActive(true);
        chouJiangBotton.SetActive(false);
        countText.gameObject.SetActive(true);
        Danmu.mingDan = new List<string>();
        Danmu.isBegin = true;

    }

    //结束统计
    public void EndToCount()
    {
        beginCountBotton.SetActive(true);
        endCountBotton.SetActive(false);
        chouJiangBotton.SetActive(true);
        Danmu.isBegin = false;

    }

    public void OpenChouJiangPanel() 
    {
        chouJiangPanel.SetActive(true);
        ChouJiangPanel.jiangChi = Danmu.mingDan;
    }

    public void OpenSetOrderPanel()
    {
        setOrderPanel.SetActive(true);
    }

    public void CloseCountPanel() 
    {
        this.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        orderText.text = "抽奖口令："+Danmu.order;
        countText.text = "现在已经有" + Danmu.mingDan.Count.ToString() + "人参加了抽奖";
    }
}
