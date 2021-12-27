using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerInfoController : MonoBehaviour
{

    #region SingletonMode
    public static WinnerInfoController controller;

    private void Awake()
    {
        controller = this;
    }
    #endregion

    public List<WinnerInfo> winnerInfos;
    public List<WinnersInfoItem> winnersInfoItems;

    public GameObject winnersInfoItemPerfabs;

    public Transform winnersInfoItemParent;

    public void ProcessingWinnerData()
    {
        SqliteController.Instance.OpenSqlite();

        IEnumerable<WinnerInfo> enumerable = SqliteController.Instance.SelectDatas<WinnerInfo>();
        List<WinnerInfo> winnerInfos = new List<WinnerInfo>(enumerable);

        SqliteController.Instance.Release();

        WinnersInfoItem winnersInfoItemTemp = new WinnersInfoItem();
        foreach (WinnerInfo info in winnerInfos)
        {
            if (info.IsExcluded == 1)
            {
                int i = 0;
                foreach (string winUser in ListOfUserController.controller.listOfWinnerExcluded)
                {
                    if (info.Uid == winUser)
                    {
                        i++;
                        break;
                    }
                }
                if (i == 0)
                {
                    ListOfUserController.controller.listOfWinnerExcluded.Add(info.Uid);
                }
            }

            if (info.Prize == winnersInfoItemTemp.prize)
            {
                winnersInfoItemTemp.winners.Add(info.Uid);
                continue;
            }

            if (winnersInfoItemTemp.prize != null)
            {
                winnersInfoItems.Add(winnersInfoItemTemp);
            }
            winnersInfoItemTemp = new WinnersInfoItem
            {
                prize = info.Prize,
                winners = new List<string>()
            };
            winnersInfoItemTemp.winners.Add(info.Uid);
        }
        if (winnersInfoItemTemp.prize != null)
        {
            winnersInfoItems.Add(winnersInfoItemTemp);
        }


        foreach (WinnersInfoItem infoItem in winnersInfoItems)
        {
            CreateWinnerRecord(infoItem);
        }

    }

    public void CreateWinnerRecord(WinnersInfoItem item)
    {
        GameObject winnersInfoItemObj = Instantiate(winnersInfoItemPerfabs);
        winnersInfoItemObj.GetComponent<WinnersInfoItem>().prize = item.prize;
        winnersInfoItemObj.GetComponent<WinnersInfoItem>().winners = item.winners;
        winnersInfoItemObj.GetComponent<WinnersInfoItem>().Show();
        winnersInfoItemObj.transform.SetParent(winnersInfoItemParent);     
    }

    public void CreateWinnerRecordNew(WinnersInfoItem item)
    {
        GameObject winnersInfoItemObj = Instantiate(winnersInfoItemPerfabs);
        winnersInfoItemObj.GetComponent<WinnersInfoItem>().prize = item.prize;
        winnersInfoItemObj.GetComponent<WinnersInfoItem>().winners = item.winners;
        winnersInfoItemObj.GetComponent<WinnersInfoItem>().Show();
        winnersInfoItemObj.transform.SetParent(winnersInfoItemParent);

        winnersInfoItemObj.transform.SetSiblingIndex(0);
    }

}
