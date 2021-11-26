using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DanmuHelper
{
    public class HttpRequestHelp
    {
        public static int userId = 0;
        static string Url = "";
        //接受返回的数据
        string jsonData = "";

        public String GetMsg()
        {
            Url = "https://api.bilibili.com/x/space/acc/info?mid=" + userId.ToString() + "&jsonp=jsonp";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);  //创建request
            req.Timeout = 30 * 1000;
            req.Method = "GET";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6";
            req.Referer = "https://space.bilibili.com/484662152";
            //req.ContentType = "application/x-www-form-urlencoded";
            //byte[] btBodys = Encoding.UTF8.GetBytes(string.Format(body, roomId));
            //req.ContentLength = btBodys.Length;
            //req.GetRequestStream().Write(btBodys, 0, btBodys.Length);
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return "连接失败:" + response.StatusCode.ToString();
                }
                else
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        jsonData = sr.ReadToEnd();
                    }
                    UnityEngine.Debug.Log("爬取成功");
                    return jsonData;
                }
            }
        }
    }
}
