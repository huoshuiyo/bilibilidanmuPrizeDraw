using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOrderPanel : MonoBehaviour
{
    public InputField orderInput;
    public InputField prizeInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Order"))
        {
            orderInput.text = PlayerPrefs.GetString("Order");
        }
        if (PlayerPrefs.HasKey("Prize"))
        {
            prizeInput.text = PlayerPrefs.GetString("Prize");
        }
    }
    public void SetOrder() 
    {
        MingDanController.controller.order = orderInput.text;
        PlayerPrefs.SetString("Order", orderInput.text);
        SetPrize();
    }

    public void SetPrize()
    {
        MingDanController.controller.SetPrize(prizeInput.text);
        PlayerPrefs.SetString("Prize", prizeInput.text);
        this.gameObject.SetActive(false);
    }
}
