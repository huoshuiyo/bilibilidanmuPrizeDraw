using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnersInfoItem:MonoBehaviour
{

    public string prize;

    public string time;

    public List<string> winners;

    public Text prizeText;
    public Text winnersText;

    public void Show() 
    {
        prizeText.text = prize;
        foreach (string win in winners) 
        {
            winnersText.text += win +", ";
        }
    }
}
