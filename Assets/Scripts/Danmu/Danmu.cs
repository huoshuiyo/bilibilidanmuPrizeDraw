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

    public GameObject[] danmuArray = new GameObject[14];
    public int danmuCount = 0;

    public GameObject[] danmuDrawPrizeArray = new GameObject[8];
    public int danmuDrawPrizeCount = 0;

    public void CreateDanmu(Danmaku danmaku)
    {
        GameObject bulletChatObj = Instantiate(userItem);
        bulletChatObj.GetComponent<Content>().imgAddress = danmaku.imgAddress;
        bulletChatObj.GetComponent<Content>().username = danmaku.name;
        bulletChatObj.GetComponent<Content>().content = danmaku.text;
        bulletChatObj.GetComponent<Content>().usernameShadow.effectColor = RandomColor();



        if (danmuArray[danmuCount] != null)
        {
            Destroy(danmuArray[danmuCount]);
        }

        bulletChatObj.transform.SetParent(parent);
        danmuArray[danmuCount] = bulletChatObj;
        danmuCount = danmuCount + 1;
        if (danmuCount > 13)
        {
            danmuCount = 0;
        }
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


                if (danmuDrawPrizeArray[danmuDrawPrizeCount] != null)
                {
                    Destroy(danmuDrawPrizeArray[danmuDrawPrizeCount]);
                }
                enterThePrizeDrawObj.transform.SetParent(enterThePrizeDrawParent);
                danmuDrawPrizeArray[danmuDrawPrizeCount] = enterThePrizeDrawObj;
                danmuDrawPrizeCount = danmuDrawPrizeCount + 1;
                if (danmuDrawPrizeCount > 7)
                {
                    danmuDrawPrizeCount = 0;
                }
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

    public GameObject[] guardArray = new GameObject[4];
    public GameObject[] freeGiftArray = new GameObject[4];
    public GameObject[] goldGiftArray = new GameObject[9];


    public int guardCount = 0;
    public int freeGiftCount = 0;
    public int goldGiftCount = 0;

    public void CreateGift(Gift gift)
    {
        GameObject giftObj = Instantiate(giftItem);
        giftObj.GetComponent<UserGift>().username = gift.UserName;
        giftObj.GetComponent<UserGift>().gift = gift.GiftName;
        giftObj.GetComponent<UserGift>().count = "X" + gift.GiftCount;
        if (gift.CoinType == "gold")
        {
            if (goldGiftArray[goldGiftCount]!=null)
            {
                Destroy(goldGiftArray[goldGiftCount]);
            }
            giftObj.transform.SetParent(goldGiftParent);
            goldGiftArray[goldGiftCount] = giftObj;
            goldGiftCount = goldGiftCount + 1;
            if (goldGiftCount > 8)
            {
                goldGiftCount = 0;
            }
            Destroy(giftObj, 300f);
        }
        if (gift.CoinType == "silver")
        {
            if (freeGiftArray[freeGiftCount] != null)
            {
                Destroy(freeGiftArray[freeGiftCount]);
            }
            giftObj.transform.SetParent(freeGiftParent);
            freeGiftArray[freeGiftCount] = giftObj;
            freeGiftCount = freeGiftCount + 1;
            if (freeGiftCount > 3)
            {
                freeGiftCount = 0;
            }
            Destroy(giftObj, 30f);
        }

    }

    public void CreateGuard(Guard guard)
    {
        GameObject guardObj = Instantiate(guardItem);
        guardObj.GetComponent<UserGuard>().username = guard.userName;
        guardObj.GetComponent<UserGuard>().guard = guard.GuardName;
        guardObj.GetComponent<UserGuard>().count = guard.count + "个月";
        if (guardArray[guardCount] != null)
        {
            Destroy(guardArray[guardCount]);
        }
        guardObj.transform.SetParent(guardParent);
        guardArray[guardCount] = guardObj;
        guardCount = guardCount + 1;
        if (guardCount > 3)
        {
            guardCount = 0;
        }
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
            //GetGiftFree
            Gift gift = live.GiftQueue.Dequeue();

            CreateGift(gift);
        }
        if (live.GuardQueue.Count > 0)
        {
            //GetGuard
            Guard guard = live.GuardQueue.Dequeue();
            CreateGuard(guard);
        }

    }
}
