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

    public static bool isPrizeDrawBegin = false;

    #region DanmuCreate

    public GameObject newUserItem;
    public GameObject userItem;
    public Transform parent;
    public Transform enterThePrizeDrawParent;

    public GameObject[] danmuArray = new GameObject[17];
    public int danmuCount = 0;

    public GameObject[] danmuDrawPrizeArray = new GameObject[10];
    public int danmuDrawPrizeCount = 0;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject gameObjectDraw = Instantiate(userItem);
            gameObjectDraw.transform.SetParent(enterThePrizeDrawParent);
            gameObjectDraw.GetComponent<Content>().PlayClose();
            danmuDrawPrizeArray[i] = gameObjectDraw;
        }
    }

    public void CreateDanmu(Danmaku danmaku)
    {
        if (isPrizeDrawBegin) DanmuDrawPrizeBeginToCount(danmaku);

        GameObject bulletChatObj;

        if (danmuArray[danmuCount] != null)
        {
            bulletChatObj = danmuArray[danmuCount];
        }
        else
        {
            bulletChatObj = Instantiate(newUserItem);
        }
        NewContent newContent = bulletChatObj.GetComponent<NewContent>();
        newContent.imgAddress = danmaku.imgAddress;
        newContent.username = danmaku.name;
        newContent.content = danmaku.text;
        newContent._MedalName = danmaku.MedalName;
        newContent._MedalLV = danmaku.MedalLv.ToString();
        newContent._GuardLv = danmaku.GuardLv;
        newContent.usernameShadow.effectColor = RandomColor();
        newContent.ReStart();
        newContent.PlayCloseLater(30f);

        bulletChatObj.transform.SetParent(parent);
        danmuArray[danmuCount] = bulletChatObj;
        danmuCount = danmuCount + 1;
        if (danmuCount > danmuArray.Length-1)
        {
            danmuCount = 0;
        }

        bulletChatObj.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 开启弹幕抽奖
    /// </summary>
    /// <param name="danmaku"></param>
    public void DanmuDrawPrizeBeginToCount(Danmaku danmaku)
    {
        if (danmaku.text != ListOfUserController.controller.order) return;
        if (ListOfUserController.controller.fansMedalLevel != "")
        {
            if (int.Parse(ListOfUserController.controller.fansMedalLevel) > 0)
            {
                if (danmaku.MedalName != ListOfUserController.controller.fansMedal) return;
                if (danmaku.MedalLv < int.Parse(ListOfUserController.controller.fansMedalLevel)) return;
            }
        }
        if (ListOfUserController.controller.guardLevel != 0)
        {
            if (ListOfUserController.controller.guardLevel == 1)
            {
                if (danmaku.GuardLv != 1) return;
            }
            if (ListOfUserController.controller.guardLevel == 2)
            {
                if (danmaku.GuardLv == 0) return;
                if (danmaku.GuardLv > 2) return;
            }
            if (ListOfUserController.controller.guardLevel == 3)
            {
                if (danmaku.GuardLv == 0) return;
            }
        }
        if (ListOfUserController.controller.danmuHS.Contains(danmaku.name)) return;

        //foreach (var ID in ListOfUserController.controller.listOfUser)
        //{
        //    if (danmaku.name == ID) return;
        //}

        ListOfUserController.controller.danmuHS.Add(danmaku.name, 1);

        ListOfUserController.controller.AddListOfUser(danmaku.name);

        GameObject enterThePrizeDrawObj;
        if (danmuDrawPrizeArray[danmuDrawPrizeCount] != null)
        {
            enterThePrizeDrawObj = danmuDrawPrizeArray[danmuDrawPrizeCount];
        }
        else
        {
            enterThePrizeDrawObj = Instantiate(userItem);
        }

        Content content = enterThePrizeDrawObj.GetComponent<Content>();
        content.imgAddress = danmaku.imgAddress;
        content.username = danmaku.name;
        content.content = "参与了抽奖";
        content.usernameShadow.effectColor = RandomColor();

        content.PlayCloseLater(20f);

        enterThePrizeDrawObj.transform.SetParent(enterThePrizeDrawParent);
        danmuDrawPrizeArray[danmuDrawPrizeCount] = enterThePrizeDrawObj;
        danmuDrawPrizeCount = danmuDrawPrizeCount + 1;
        if (danmuDrawPrizeCount > 9)
        {
            danmuDrawPrizeCount = 0;
        }

        enterThePrizeDrawObj.transform.SetAsLastSibling();

    }


    #endregion

    #region GiftCreate
    public GameObject guardItem;
    public GameObject giftItem;

    public Transform guardParent;
    public Transform goldGiftParent;
    public Transform freeGiftParent;

    public GameObject[] guardArray = new GameObject[7];
    public GameObject[] freeGiftArray = new GameObject[6];
    public GameObject[] goldGiftArray = new GameObject[10];


    public int guardCount = 0;
    public int freeGiftCount = 0;
    public int goldGiftCount = 0;

    public void CreateGift(Gift gift)
    {
        GameObject giftObj;
        if (gift.CoinType == "gold")
        {
            if (goldGiftArray[goldGiftCount] != null)
            {
                giftObj = goldGiftArray[goldGiftCount];
            }
            else
            {
                giftObj = Instantiate(giftItem);
            }

            giftObj.GetComponent<UserGift>().username = gift.UserName;
            giftObj.GetComponent<UserGift>().gift = gift.GiftName;
            giftObj.GetComponent<UserGift>().count = "X" + gift.GiftCount;

            giftObj.transform.SetParent(goldGiftParent);
            goldGiftArray[goldGiftCount] = giftObj;
            goldGiftCount = goldGiftCount + 1;
            if (goldGiftCount > 9)
            {
                goldGiftCount = 0;
            }
            giftObj.transform.SetAsLastSibling();
        }
        if (gift.CoinType == "silver")
        {
            if (freeGiftArray[freeGiftCount] != null)
            {
                giftObj = freeGiftArray[freeGiftCount];
            }
            else
            {
                giftObj = Instantiate(giftItem);
            }

            giftObj.GetComponent<UserGift>().username = gift.UserName;
            giftObj.GetComponent<UserGift>().gift = gift.GiftName;
            giftObj.GetComponent<UserGift>().count = "X" + gift.GiftCount;

            giftObj.transform.SetParent(freeGiftParent);
            freeGiftArray[freeGiftCount] = giftObj;
            freeGiftCount = freeGiftCount + 1;
            if (freeGiftCount > 5)
            {
                freeGiftCount = 0;
            }
            giftObj.transform.SetAsLastSibling();
        }

    }

    public void CreateGuard(Guard guard)
    {
        GameObject guardObj;

        if (guardArray[guardCount] != null)
        {
            guardObj = guardArray[guardCount];
        }
        else
        {
            guardObj = Instantiate(guardItem);
        }

        guardObj.GetComponent<UserGuard>().username = guard.userName;
        guardObj.GetComponent<UserGuard>().guard = guard.GuardName;
        guardObj.GetComponent<UserGuard>().count = guard.count + "个月";

        guardObj.transform.SetParent(guardParent);
        guardArray[guardCount] = guardObj;
        guardCount = guardCount + 1;
        if (guardCount > 6)
        {
            guardCount = 0;
        }
        guardObj.transform.SetAsLastSibling();
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

    public void SetClose()
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
                Content content = danmuDrawPrizeArray[i].GetComponent<Content>();
                content.PlayClose();
            }
            catch (Exception)
            {
                throw;
            }      
        }

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
            //GetGift
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
