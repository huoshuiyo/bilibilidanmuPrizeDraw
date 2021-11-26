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
        jiangChi = new List<string>();
        foreach (var item in mingDan)
        {
            jiangChi.Add(item);
        }
    }

    public void RemoveJiangChi(string username)
    {
        jiangChi.Remove(username);
    }
}
