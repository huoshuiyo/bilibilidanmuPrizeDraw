using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Danmu : MonoBehaviour
{
    public Live live;

    public static bool isBegin = false;

    #region DanmuCreate

    public GameObject userItem;
    public Transform parent;
    public Transform enterThePrizeDrawParent;

    public void CreateDanmu(Danmaku danmaku)
    {
        GameObject bulletChatObj = Instantiate(userItem);
        bulletChatObj.GetComponent<Content>().imgAddress = danmaku.imgAddress;
        bulletChatObj.GetComponent<Content>().username = danmaku.name;
        bulletChatObj.GetComponent<Content>().content = danmaku.text;
        bulletChatObj.GetComponent<Content>().usernameShadow.effectColor = RandomColor();
        bulletChatObj.transform.SetParent(parent);
        Destroy(bulletChatObj, 30f);

        #region DanmuDrawPrizeBeginToCount
        if (isBegin)
        {
            if (danmaku.text == ListOfUserController.controller.order)
            {
                if (ListOfUserController.controller.fansMedalLevel != "")
                {
                    if (int.Parse(ListOfUserController.controller.fansMedalLevel) > 0)
                    {
                        if (danmaku.MedalName != ListOfUserController.controller.fansMedal) return;
                        if (danmaku.MedalLv < int.Parse(ListOfUserController.controller.fansMedalLevel)) return;
                    }
                }

                foreach (var ID in ListOfUserController.controller.listOfUser)
                {
                    if (danmaku.name == ID) return;
                }
                ListOfUserController.controller.AddListOfUser(danmaku.name);
                GameObject enterThePrizeDrawObj = Instantiate(userItem);
                enterThePrizeDrawObj.GetComponent<Content>().imgAddress = danmaku.imgAddress;
                enterThePrizeDrawObj.GetComponent<Content>().username = danmaku.name;
                enterThePrizeDrawObj.GetComponent<Content>().content = "参与了抽奖";
                enterThePrizeDrawObj.GetComponent<Content>().usernameShadow.effectColor = RandomColor();
                enterThePrizeDrawObj.transform.SetParent(enterThePrizeDrawParent);
                Destroy(enterThePrizeDrawObj, 20f);

            }
        }
        #endregion

    }
    #endregion

    #region GiftCreate
    public GameObject guardItem;
    public GameObject giftItem;

    public Transform guardParent;
    public Transform goldGiftParent;
    public Transform freeGiftParent;
    public void CreateGift(Gift gift)
    {
        GameObject giftObj = Instantiate(giftItem);
        giftObj.GetComponent<UserGift>().username = gift.UserName;
        giftObj.GetComponent<UserGift>().gift = gift.GiftName;
        giftObj.GetComponent<UserGift>().count = "X" + gift.GiftCount;
        if (gift.CoinType == "gold")
        {
            giftObj.transform.SetParent(goldGiftParent);
        }
        if (gift.CoinType == "silver")
        {
            giftObj.transform.SetParent(freeGiftParent);
            Destroy(giftObj, 30f);
        }

    }

    public void CreateGuard(Guard guard)
    {
        GameObject guardObj = Instantiate(giftItem);
        guardObj.GetComponent<UserGuard>().username = guard.userName;
        guardObj.GetComponent<UserGuard>().guard = guard.GuardName;
        guardObj.GetComponent<UserGuard>().count = guard.count + "个月";
        guardObj.transform.SetParent(guardParent);
    }
    #endregion


    Color RandomColor()
    {
        //RandomColorRGB
        float r = UnityEngine.Random.Range(0f, 1f);
        float g = UnityEngine.Random.Range(0f, 1f);
        float b = UnityEngine.Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        return color;
    }

    private void Update()
    {
        if (live.DanmakuQueue.Count > 0)
        {
            //GetDanmu
            Danmaku danmaku = live.DanmakuQueue.Dequeue();
            CreateDanmu(danmaku);
        }
        if (live.GiftQueue.Count > 0)
        {
            //GetDanmu
            Gift gift = live.GiftQueue.Dequeue();
            CreateGift(gift);
        }
        if (live.GuardQueue.Count > 0)
        {
            //GetDanmu
            Guard guard = live.GuardQueue.Dequeue();
            CreateGuard(guard);
        }

    }
}
