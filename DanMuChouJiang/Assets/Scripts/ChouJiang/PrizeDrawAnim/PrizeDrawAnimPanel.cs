using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PrizeDrawAnimPanel : MonoBehaviour
{
    public Text winnerName;

    public List<Transform> prizeDrawAnims;

    public Transform prizeDrawAnimParent;

    public GameObject mainPanel;
    public GameObject countPanel;
    public GameObject PrizeDrawPanel;
    public GameObject mainCamera;
    public GameObject button;

    public GameObject prizeDrawAnim;
    public GameObject animCamera;

    public Transform prizeDrawAnimObj = null;

    public Transform gameLight;

    public bool isShow = false;

    public string winnerInUser;


    public void CreatePrizeDrawAnim()
    {
        prizeDrawAnimObj = Instantiate(prizeDrawAnims.OrderBy(u => Guid.NewGuid()).First());
        prizeDrawAnimObj.position = new Vector3(0.33f, 3.81f, 10.6f);
        prizeDrawAnimObj.localEulerAngles = new Vector3(-6.74f, 0, 0);
        string winner = ListOfUserController.controller.listOfWinnerAnim.OrderBy(u => Guid.NewGuid()).First();
        winnerInUser = winner;
        //prizeDrawAnim.gameObject.GetComponent<PrizeDrawAnim>().winnerName = winner;
        //prizeDrawAnim.gameObject.GetComponent<PrizeDrawAnim>().prizeDrawAnimPanel = gameObject.GetComponent<PrizeDrawAnimPanel>();
        ListOfUserController.controller.RemoveListOfWinnerAnim(winner);
        prizeDrawAnimObj.parent = prizeDrawAnimParent;
        Invoke("ShowWinnerName", 1.9f);
    }

    public void ShowWinnerName()
    {
        isShow = true;
        winnerName.text = winnerInUser;
        winnerName.gameObject.SetActive(true);
        button.SetActive(true);
    }

    public void CheckIsCreatePrizeDrawAnim()
    {
        if (!isShow) return;
        button.SetActive(false);
        isShow = false;
        DestroyAnimObj();
        winnerName.gameObject.SetActive(false);
        if (ListOfUserController.controller.listOfWinnerAnim.Count > 0)
        {
            CreatePrizeDrawAnim();
        }
        else
        {
            JumpPrizeDrawAnim();
        }

    }

    public void JumpPrizeDrawAnim()
    {
        DestroyAnimObj();
        gameLight.localEulerAngles = new Vector3(-21, -160, -163);
        ListOfUserController.controller.listOfWinnerAnim = new List<string>();
        mainPanel.SetActive(true);
        countPanel.SetActive(true);
        PrizeDrawPanel.SetActive(true);
        mainCamera.SetActive(true);

        prizeDrawAnim.SetActive(false);
        animCamera.SetActive(false);

        this.gameObject.SetActive(false);

    }

    public void DestroyAnimObj() 
    {
        if (prizeDrawAnimObj != null) Destroy(prizeDrawAnimObj.gameObject);
    }
}
