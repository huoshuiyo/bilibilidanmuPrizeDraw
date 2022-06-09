using System;

using UnityEngine;

using UnityEngine.UI;

public class timeNow : MonoBehaviour
{
    public Text timeText;
    private void Awake()
    {
        timeText = GetComponent<Text>();
    }
    void Update()
    {
        DateTime dateTime = DateTime.Now;
        timeText.text = string.Format("{0:D}-{1:D}-{2:D} {3:D}:{4:D}:{5:D}", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }

}
