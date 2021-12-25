using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCamera : MonoBehaviour
{
    public PrizeDrawAnimPanel prizeDrawAnimPanel;
    public Transform gameLight;
    public void BeginCheck() 
    {
        prizeDrawAnimPanel.isShow = true;
        prizeDrawAnimPanel.CheckIsCreatePrizeDrawAnim();
        gameLight.localEulerAngles = new Vector3(-16, -20, 30);
    }
}
