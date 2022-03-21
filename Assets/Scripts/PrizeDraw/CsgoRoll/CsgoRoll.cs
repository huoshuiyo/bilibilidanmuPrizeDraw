using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class CsgoRoll : MonoBehaviour
{

    [Header("速度")]
    public float speed = 0;

    [Header("速度范围")]
    public float speed1 = 0;
    public float speed2 = 0;

    [Header("加速度")]
    public float speedDown = 1;

    public GameObject parent;

    public PrizeDrawPanel PrizeDrawPanel;

    public GameObject beginButton;
    public GameObject backButton;

    private float parentX = 0;

    private bool isBegin =false;

    private bool isWin = false;

    public CsgoItem[] itemArray;

    public void BeginToCSGORoll() 
    {
        GiveUserItemName();
        speed = UnityEngine.Random.Range(speed1, speed2);
        parentX = 8704.5f;
        isBegin = true;
        isWin = false;
        beginButton.SetActive(false);
        backButton.SetActive(false);
    }

    public void GiveUserItemName() 
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            string user = ListOfUserController.controller.listOfPrizePool.OrderBy(s => UnityEngine.Random.Range(0, 100)).First();
            itemArray[i].userName.text = user;
        }
    }

    public void CloseCSGORollPanel() 
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (speed <= 0)
        {
            if (!isWin)
            {
                //如果还没有赢家
                string winner = itemArray[(int)Math.Round(61 - (parentX / 150))].userName.text;
                PrizeDrawPanel.CreateUserInWinner(winner);
                ListOfUserController.controller.RemoveListOfPrizePool(winner);
                isWin = true;
                beginButton.SetActive(true);
                backButton.SetActive(true);
            }
            isBegin = false;
            return;
        }
        if (!isBegin)
        {
            return;
        }
        parentX -= speed * Time.deltaTime;

        speed -= speedDown * Time.deltaTime;

        parent.transform.localPosition = new Vector3(parentX, 0f, 0);
    }
}
