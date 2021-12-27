using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnersRecordsPanel : MonoBehaviour
{
    public void CloseWinnerRecordsPanel() 
    {
        this.gameObject.SetActive(false);
    }

    public void ClearDB() 
    {
        SqliteController.Instance.OpenSqlite();
        SqliteController.Instance.ClearDB();
        SqliteController.Instance.Release();
    }
}
