using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EButton : MonoBehaviour
{
    public CanvasGroup EPanelCanvasGroup;

    public void OpenEPanel() 
    {
        EPanelCanvasGroup.alpha = 1;
        EPanelCanvasGroup.blocksRaycasts = true;
        EPanelCanvasGroup.interactable = true;
    }

    public void CloseEPanel()
    {
        EPanelCanvasGroup.alpha = 0;
        EPanelCanvasGroup.blocksRaycasts = false;
        EPanelCanvasGroup.interactable = false;
    }
}
