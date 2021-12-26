using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInWinner : MonoBehaviour
{
    public Text userName;

    public void SetUserName(string user)
    {
        userName.text = user;
    }
}
