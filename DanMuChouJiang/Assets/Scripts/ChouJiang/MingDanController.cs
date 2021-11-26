using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MingDanController : MonoBehaviour
{

    #region 单例模式
    public static MingDanController controller;
    private void Awake()
    {
        controller = this;
    }
    #endregion

    public List<string> mingDan = new List<string>();

    public List<string> jiangChi = new List<string>();


    public void AddMingDan(string username)
    {
        mingDan.Add(username);
    }

    public void ResetMingDan()
    {
        mingDan= new List<string>();
    }

    public void ResetJiangChi()
    {
        jiangChi = mingDan;
    }

    public void RemoveJiangChi(string username)
    {
        jiangChi.Remove(username);
    }
}
