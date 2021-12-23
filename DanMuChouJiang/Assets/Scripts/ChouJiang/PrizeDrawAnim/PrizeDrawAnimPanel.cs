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

    public GameObject prizeDrawAnim;
    public GameObject animCamera;

    public Transform prizeDrawAnimObj = null;

    public bool isShow = false;

    public string winnerInUser;


    public void CreatePrizeDrawAnim()
    {
        prizeDrawAnimObj = Instantiate(prizeDrawAnims.OrderBy(u => Guid.NewGuid()).First());
        prizeDrawAnimObj.localPosition = new Vector3(2.11f, 3.52f, 9.74f);
        string winner = ListOfUserController.controller.listOfWinnerAnim.OrderBy(u => Guid.NewGuid()).First();
        winnerInUser = winner;
        //prizeDrawAnim.gameObject.GetComponent<PrizeDrawAnim>().winnerName = winner;
        //prizeDrawAnim.gameObject.GetComponent<PrizeDrawAnim>().prizeDrawAnimPanel = gameObject.GetComponent<PrizeDrawAnimPanel>();
        ListOfUserController.controller.RemoveListOfWinnerAnim(winner);
        prizeDrawAnimObj.parent = prizeDrawAnimParent;
        Invoke("ShowWinnerName", 2);
    }

    public void ShowWinnerName()
    {
        isShow = true;
        winnerName.text = winnerInUser;
        winnerName.gameObject.SetActive(true);
    }

    public void CheckIsCreatePrizeDrawAnim()
    {
        if (!isShow) return;
        isShow = false;
        if (prizeDrawAnimObj != null) Destroy(prizeDrawAnimObj.gameObject);
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
        if (prizeDrawAnimObj != null) Destroy(prizeDrawAnimObj.gameObject);
        mainPanel.SetActive(true);
        countPanel.SetActive(true);
        PrizeDrawPanel.SetActive(true);
        mainCamera.SetActive(true);

        prizeDrawAnim.SetActive(false);
        animCamera.SetActive(false);

        this.gameObject.SetActive(false);

        ListOfUserController.controller.listOfWinnerAnim = new List<string>();
    }
}
