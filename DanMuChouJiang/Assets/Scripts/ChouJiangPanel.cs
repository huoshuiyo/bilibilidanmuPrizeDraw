using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChouJiangPanel : MonoBehaviour
{
    public Text winners;
    public static List<string> jiangChi;

    public void ChouJiang()
    {
        if (jiangChi.Count==0)
        {
            return;
        }
        string winner = jiangChi.OrderBy(u => Guid.NewGuid()).First();
        Debug.Log(winner);
        winners.text += winner + ";";
        jiangChi.Remove(winner);
    }

    public void CloseChoujiangPanel() 
    {
        this.gameObject.SetActive(false);
    }

}
