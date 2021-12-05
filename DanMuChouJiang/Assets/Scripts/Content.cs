using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public string username = "";
    public string content = "";
    public Image img;

    public string imgAddress = "";
    public Shadow usernameShadow;

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
        UnityWebRequest myTex = UnityWebRequestTexture.GetTexture(url);
        yield return myTex.SendWebRequest();
        if (!myTex.isNetworkError)
        {
            Texture2D tex = ((DownloadHandlerTexture)myTex.downloadHandler).texture;
            Sprite temp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
            this.img.sprite = temp; //设置的图片，显示从URL图片
        }
    }
}
