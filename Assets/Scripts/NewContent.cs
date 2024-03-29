﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewContent : MonoBehaviour
{
    public string username = "";
    public string content = "";
    public Image img;

    public string imgAddress = "";
    public Shadow usernameShadow;

    public Text _username;
    public Text _content;

    public Animator _animator;

    public string _MedalName;
    public string _MedalLV;
    public Image _MedalImg;

    public Text _MedalNameText;

    public int _GuardLv;

    public Image _GuardImg;

    private void Start()
    {
        if (!string.IsNullOrEmpty(imgAddress))
        {
            StartCoroutine(GetImage(imgAddress));
        }
    }
    private void Update()
    {

    }

    public void ReStart()
    {

        _username.text = username;
        _content.text = "\n" + content;
        _GuardImg.enabled = false;
        if (_GuardLv > 0)
        {
            //Debug.Log(_GuardLv);
            switch (_GuardLv)
            {
                case 1:
                    _GuardImg.color = new Color(241 / 255f, 236 / 255f, 88 / 255f);
                    break;
                case 2:
                    _GuardImg.color = new Color(203 / 255f, 119 / 255f, 255 / 255f);
                    break;
                case 3:
                    _GuardImg.color = new Color(17 / 255f, 175 / 255f, 243 / 255f, 163 / 255f);
                    break;
            }
            _GuardImg.enabled = true;
        }

        if (!string.IsNullOrEmpty(_MedalName))
        {
            _MedalImg.enabled = true;
            _MedalNameText.text = " " + _MedalName + " " + _MedalLV + " ";
        }
        else
        {
            _MedalImg.enabled = false;
            _MedalNameText.text = "";
        }
    }

    public void PlayCloseLater(float sec)
    {
        CancelInvoke();

        _animator.SetBool("isClose", false);
        _animator.Play("_Start");
        Invoke("PlayClose", sec);
    }

    public void PlayClose()
    {
        _MedalImg.enabled = false;
        _GuardImg.enabled = false;
        _animator.SetBool("isClose", true);
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
