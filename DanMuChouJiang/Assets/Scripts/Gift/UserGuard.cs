using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserGuard : MonoBehaviour
{
    public string username = "";
    public string guard = "";
    public string count = "";

    public string imgAddress = "";

    public Text _username;
    public Text _guard;
    public Text _count;

    private void Start()
    {

    }
    private void Update()
    {
        _username.text = username;
        _guard.text = guard;
        _count.text = count;
    }
}
