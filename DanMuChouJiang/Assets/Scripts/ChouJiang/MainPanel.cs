using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPanel : MonoBehaviour
{

    public Animator buttonBGAnimator;

    public GameObject countPanel;
    public GameObject danmuSettingPanel;

    public Text popularityText;

    public bool isStart = true;

    public void BackToBegin() 
    {
        SceneManager.LoadScene(0);
    }

    public void OpenCountPanel() 
    {
        countPanel.SetActive(true);
        countPanel.GetComponent<CountPanel>().ResetCountPanel();
    }

    public void OpenDanMuSettingPanel()
    {
        danmuSettingPanel.SetActive(true);
    }

    public void ButtonBGControl()
    {
       // Debug.Log("1");
        if (isStart)
        {
            buttonBGAnimator.SetBool("isStart",true);
            buttonBGAnimator.SetBool("isClose", false);
        }
        else
        {
            buttonBGAnimator.SetBool("isClose", true);
            buttonBGAnimator.SetBool("isStart", false);
        }

        isStart = !isStart;
    }

    private void Update()
    {
        popularityText.text = "人气：" + ListOfUserController.controller.popularity;
    }

}
