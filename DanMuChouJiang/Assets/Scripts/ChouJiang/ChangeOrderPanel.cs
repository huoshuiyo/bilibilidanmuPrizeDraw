using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOrderPanel : MonoBehaviour
{
    public InputField order;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Order"))
        {
            order.text = PlayerPrefs.GetString("Order");
        }
    }
    public void SetOrder() 
    {
        Danmu.order = order.text;
        PlayerPrefs.SetString("Order", order.text);
        this.gameObject.SetActive(false);
    }
}
