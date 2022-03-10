using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EButton : MonoBehaviour
{
    public GameObject EPanel;

    public void OpenEPanel() 
    {
        EPanel.SetActive(true);
    }

    public void CloseEPanel()
    {
        EPanel.SetActive(false);
    }
}
