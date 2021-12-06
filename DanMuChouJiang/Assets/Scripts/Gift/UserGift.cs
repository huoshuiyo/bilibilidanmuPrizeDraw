using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserGift : MonoBehaviour
{
    public string username = "";
    public string gift = "";
    public string count = "";

    public string imgAddress = "";

    public Text _username;
    public Text _gift;
    public Text _count;

    private void Start()
    {

    }
    private void Update()
    {
        _username.text = username;
        _gift.text = gift;
        _count.text = count;
    }
}
