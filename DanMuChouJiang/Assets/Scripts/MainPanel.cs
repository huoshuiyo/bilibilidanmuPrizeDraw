using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPanel : MonoBehaviour
{
    public GameObject countPanel;
    public void BackToBegin() 
    {
        SceneManager.LoadScene(0);
    }

    public void OpenCountPanel() 
    {
        countPanel.SetActive(true);
    }

}
