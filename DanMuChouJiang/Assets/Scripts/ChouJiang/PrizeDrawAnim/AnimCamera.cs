using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCamera : MonoBehaviour
{
    public PrizeDrawAnimPanel prizeDrawAnimPanel;

    public void BeginCheck() 
    {
        prizeDrawAnimPanel.isShow = true;
        prizeDrawAnimPanel.CheckIsCreatePrizeDrawAnim();
    }
}
