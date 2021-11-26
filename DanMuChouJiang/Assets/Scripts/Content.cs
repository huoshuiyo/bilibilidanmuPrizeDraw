using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public string username = "";
    public string content = "";
    public Image img;

    public string imgAddress = "";

    public Text _username;
    public Text _content;

    private void Start()
    {
        if (!string.IsNullOrEmpty(imgAddress))
        {
            StartCoroutine(GetImage(imgAddress));
        }
    }
    private void Update()
    {
        _username.text = username;
        _content.text = content;
    }

    IEnumerator GetImage(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D tex = www.texture;
            Sprite temp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
            this.img.sprite = temp; //设置的图片，显示从URL图片
        }
    }
}
