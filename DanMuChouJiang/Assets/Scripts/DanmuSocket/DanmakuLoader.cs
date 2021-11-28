using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BilibiliDM_PluginFramework;
using BitConverter;
using DanmuHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BiliDMLib
{
    public class DanmakuLoader
    {
        //大概是默认主机 有这两个服务器
        private string[] defaulthosts = new string[] { "livecmt-2.bilibili.com", "livecmt-1.bilibili.com" };
        //弹幕聊天的服务器地址
        private string ChatHost = "chat.bilibili.com";
        //端口号
        private int ChatPort = 2243; // TCP协议默认端口疑似修改到 2243
        //Tcp 客户端 socket
        private TcpClient Client;
        //流
        private Stream NetStream;
        //应该是 获取 房间ID 的地址
        private string CIDInfoUrl = "https://api.live.bilibili.com/room/v1/Danmu/getConf?room_id=";
        //判断是否连接
        private bool Connected = false;
        //异常声名
        public Exception Error;
        ////事件 大概是处理 接受弹幕后的回调
        //public event ReceivedDanmakuEvt ReceivedDanmaku;
        ////取消连接 时候的回调
        //public event DisconnectEvt Disconnected;
        ////接受 房间 个数？
        //public event ReceivedRoomCountEvt ReceivedRoomCount;
        public Action<string,string,string> OnDanmuCallBack;
        //打印信息回调？
        public event LogMessageEvt LogMessage;
        //日志 是否启动 应该是
        private bool debuglog = true;
        //协议转换 啥意思？
        private short protocolversion = 2;
        //上一个房间号
        private static int lastroomid;
        //上一个服务器
        private static string lastserver;
        //Http 对象
        private static HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) };
        //        private object shit_lock=new object();//ReceiveMessageLoop 似乎好像大概會同時運行兩個的bug, 但是不修了, 鎖上算了

        /// <summary>
        /// 申请异步连接  需要输入对应房间号
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(int roomId)
        {
            try
            {
                //如果已经连接 就直接抛出异常
                if (this.Connected) throw new InvalidOperationException();
                //接收一下 roomID
                var channelId = roomId;
                //
                //                var request = WebRequest.Create(RoomInfoUrl + roomId + ".json");
                //                var response = request.GetResponse();
                //
                //                int channelId;
                //                using (var stream = response.GetResponseStream())
                //                using (var sr = new StreamReader(stream))
                //                {
                //                    var json = await sr.ReadToEndAsync();
                //                    Debug.WriteLine(json);
                //                    dynamic jo = JObject.Parse(json);
                //                    channelId = (int) jo.list[0].cid;
                //                }
                var token = "";
                //判断 连接的房间号 不为上一个房间号 
                if (channelId != lastroomid)
                {
                    try
                    {
                        //发起一个Http请求  
                        //https://api.live.bilibili.com/room/v1/Danmu/getConf?room_id=channelId  这个网址
                        var req = await httpClient.GetStringAsync(CIDInfoUrl + channelId);
                        JObject roomobj = JObject.Parse(req);
                        token = roomobj["data"]["token"] + "";
                        ChatHost = roomobj["data"]["host"] + "";

                        ChatPort = roomobj["data"]["port"].Value<int>();
                        if (string.IsNullOrEmpty(ChatHost))
                        {
                            throw new Exception();
                        }

                    }
                    catch (WebException ex)
                    {
                        ChatHost = defaulthosts[new Random().Next(defaulthosts.Length)];

                        var errorResponse = ex.Response as HttpWebResponse;
                        if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                        {
                            // 直播间不存在（HTTP 404）
                            var msg = "该直播间疑似不存在，弹幕姬只支持使用原房间号连接";
                            LogMessage?.Invoke(this, new LogMessageArgs() { message = msg });
                        }
                        else
                        {
                            // B站服务器响应错误
                            var msg = "B站服务器响应弹幕服务器地址出错，尝试使用常见地址连接";
                            LogMessage?.Invoke(this, new LogMessageArgs() { message = msg });
                        }
                    }
                    catch (Exception)
                    {
                        // 其他错误（XML解析错误？）
                        ChatHost = defaulthosts[new Random().Next(defaulthosts.Length)];
                        var msg = "获取弹幕服务器地址时出现未知错误，尝试使用常见地址连接";
                        LogMessage?.Invoke(this, new LogMessageArgs() { message = msg });
                    }


                }
                else
                {
                    ChatHost = lastserver;
                }
                //创建 TCP对象
                Client = new TcpClient();
                //DNS解析域名 服务器IP地址
                var ipAddress = await System.Net.Dns.GetHostAddressesAsync(ChatHost);
                var random = new Random();
                var idx = random.Next(ipAddress.Length);
                await Client.ConnectAsync(ipAddress[idx], ChatPort);//随机选择一个进行连接

                NetStream = Stream.Synchronized(Client.GetStream());//获取 流数据

                //发送一个 加入频道的 消息
                Console.WriteLine("发送验证消息");
                if (await SendJoinChannel(channelId, token)) //token 感觉类似于 一个验证 标记 channelID就是 roomID
                {
                    Console.WriteLine("成功");
                    Connected = true;//返回成功后
                    _ = this.HeartbeatLoop();//循环发送心跳包
                    _ = this.ReceiveMessageLoop();//循环接收消息
                    lastserver = ChatHost;//设置最近的服务器
                    lastroomid = roomId;//设置最近的roomid
                    return true;
                }
                Console.WriteLine("消息失败");
                return false;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                return false;
            }
        }

        private async Task ReceiveMessageLoop()
        {

            try
            {
                //头部
                var stableBuffer = new byte[16];
                //信息
                var buffer = new byte[4096];
                while (this.Connected)
                {
                    //读取头部
                    await NetStream.ReadAsync(stableBuffer, 0, 16);
                    //反序列化 协议头部内容
                    var protocol = DanmakuProtocol.FromBuffer(stableBuffer);
                    if (protocol.PacketLength < 16)
                    {
                        throw new NotSupportedException("协议失败: (L:" + protocol.PacketLength + ")");
                    }
                    var payloadlength = protocol.PacketLength - 16;
                    if (payloadlength == 0)
                    {
                        continue; // 没有内容了
                    }

                    buffer = new byte[payloadlength];
                    //继续接受 协议总长度-协议头部 长度 的字节数据
                    await NetStream.ReadAsync(buffer, 0, payloadlength);
                    if (protocol.Version == 2 && protocol.Action == 5) // 这里处理 版本是2且 行为是5表示['cmd']命令
                    {
                        //跳过两个字节不知为何
                        //这种特殊消息应该是 用 两个头部包起来的
                        using (var ms = new MemoryStream(buffer, 2, payloadlength - 2)) // Skip 0x78 0xDA
                        using (var deflate = new DeflateStream(ms, CompressionMode.Decompress)) //提供压缩算法 
                        {
                            var headerbuffer = new byte[16];
                            try
                            {
                                //这里 应该是 版本号为2的协议被压缩了 所以需要 用压缩的流读取 然后循环
                                while (true)
                                {
                                    await deflate.ReadAsync(headerbuffer, 0, 16);
                                    var protocol_in = DanmakuProtocol.FromBuffer(headerbuffer);


                                    //Console.WriteLine("HeaderLength=" + protocol_in.HeaderLength);
                                    //Console.WriteLine("Version=" + protocol_in.Version);
                                    //Console.WriteLine("Action=" + protocol_in.Action);
                                    //Console.WriteLine("Parameter=" + protocol_in.Parameter);


                                    payloadlength = protocol_in.PacketLength - 16;
                                    if (payloadlength <= 0) break;
                                    var danmakubuffer = new byte[payloadlength];
                                    await deflate.ReadAsync(danmakubuffer, 0, payloadlength);
                                    int num = 0;
                                    for (int i = 0; i < danmakubuffer.Length; i++)
                                    {
                                        if (danmakubuffer[i] == 0)
                                            num++;
                                    }
                                    if (num == danmakubuffer.Length) break;
                                    ProcessDanmaku(protocol.Action, danmakubuffer);
                                    //Console.WriteLine("总长=" + protocol_in.PacketLength);
                                    //Console.WriteLine("解析完成");
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                    else
                    {
                        ProcessDanmaku(protocol.Action, buffer);
                    }
                }
            }
            //catch (NotSupportedException ex)
            //{
            //    this.Error = ex;
            //    _disconnect();
            //}
            catch (Exception ex)
            {
                this.Error = ex;
                _disconnect();

            }


        }

        private void ProcessDanmaku(int action, byte[] buffer)
        {
            switch (action)
            {
                case 3: // (OpHeartbeatReply) 人气
                    {
                        var viewer = EndianBitConverter.BigEndian.ToUInt32(buffer, 0); //观众人数
                        MingDanController.controller.SetRenQi(viewer.ToString());
                        UnityEngine.Debug.Log("接收到协议指令3 显示人数:" + viewer);
                        //ReceivedRoomCount?.Invoke(this, new ReceivedRoomCountArgs() { UserCount = viewer });
                        break;
                    }
                case 5://playerCommand (OpSendMsgReply)
                    {
                        //5需要命令
                        var json = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        if (debuglog)
                        {
                            var obj = JObject.Parse(json);
                            if (obj["cmd"].ToString() == "DANMU_MSG")
                            {
                                string username = obj["info"][2][1].ToString();
                                string content = obj["info"][1].ToString();
                                string userId = obj["info"][2][0].ToString();
                                UnityEngine.Debug.Log(userId + ":" + username +":" + content);
                                OnDanmuCallBack?.Invoke(username,content,userId);
                            }
                            //Console.WriteLine("显示json信息:" + json);
                        }
                        try
                        {
                            //var obj = JObject.Parse(json);
                            //if (obj["cmd"].ToString() == "DANMU_MSG")
                            //{
                            //    Console.WriteLine(obj["info"][1].ToString());
                            //}
                            //var dama = new DanmakuModel(json, 2);
                            //ReceivedDanmaku?.Invoke(this, new ReceivedDanmakuArgs() { Danmaku = dama });
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                        break;
                    }
                case 8: // (OpAuthReply)
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private async Task HeartbeatLoop()
        {

            try
            {
                while (this.Connected)
                {
                    //没30秒发送一次 心跳
                    await this.SendHeartbeatAsync();
                    await Task.Delay(30000);
                }
            }
            catch (Exception ex)
            {
                this.Error = ex;
                _disconnect();

            }
        }

        public void Disconnect()
        {

            Connected = false;
            try
            {
                Client.Close();
            }
            catch (Exception)
            {

            }


            NetStream = null;
        }

        private void _disconnect()
        {
            if (Connected)
            {
                Debug.WriteLine("Disconnected");

                Connected = false;

                Client.Close();

                NetStream = null;
            //    if (Disconnected != null)
            //    {
            //        //Disconnected(this, new DisconnectEvtArgs() { Error = Error });
            //    }
            }

        }

        private async Task SendHeartbeatAsync()
        {
            await SendSocketDataAsync(2);//行为2
            Debug.WriteLine("Message Sent: Heartbeat");
        }

        Task SendSocketDataAsync(int action, string body = "")
        {
            //应该是个重载 为了简化调用
            return SendSocketDataAsync(0, 16, protocolversion, action, 1, body);
        }
        async Task SendSocketDataAsync(int packetlength, short magic, short ver, int action, int param = 1, string body = "")
        {
            //将要传送的数据转换为Byte[]
            var playload = Encoding.UTF8.GetBytes(body);
            //计算数据长度  + 包头16字节
            if (packetlength == 0)
            {
                packetlength = playload.Length + 16;
            }
            //创建 buffer 存储协议
            var buffer = new byte[packetlength];
            //将数据写入 buffer
            using (var ms = new MemoryStream(buffer))
            {


                var b = EndianBitConverter.BigEndian.GetBytes(buffer.Length);

                await ms.WriteAsync(b, 0, 4);
                b = EndianBitConverter.BigEndian.GetBytes(magic);
                await ms.WriteAsync(b, 0, 2);
                b = EndianBitConverter.BigEndian.GetBytes(ver);
                await ms.WriteAsync(b, 0, 2);
                b = EndianBitConverter.BigEndian.GetBytes(action);
                await ms.WriteAsync(b, 0, 4);
                b = EndianBitConverter.BigEndian.GetBytes(param);
                await ms.WriteAsync(b, 0, 4);
                //先依次写入 16字节 头部 然后将后面的数据全部写入
                if (playload.Length > 0)
                {
                    await ms.WriteAsync(playload, 0, playload.Length);
                }
                //最后 将 buffer 送入 TCP流中
                await NetStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        private async Task<bool> SendJoinChannel(int channelId, string token)
        {
            //这应该发送的 是 第一次连接 要发送认证包  protover = 2 是心跳包的协议
            var packetModel = new { roomid = channelId, uid = 0, protover = 2, token = token, platform = "danmuji" };
            var playload = JsonConvert.SerializeObject(packetModel);
            await SendSocketDataAsync(7, playload);//这个7  代表认证并加入房间  发送这个行为 ID
            return true;
        }



        public DanmakuLoader()
        {
        }


    }

    public delegate void LogMessageEvt(object sender, LogMessageArgs e);
    public class LogMessageArgs
    {
        public string message = string.Empty;
    }


    public struct DanmakuProtocol
    {
        /// <summary>
        /// 消息总长度 (协议头 + 数据长度)
        /// </summary>
        public int PacketLength;
        /// <summary>
        /// 消息头长度 (固定为16[sizeof(DanmakuProtocol)])
        /// </summary>
        public short HeaderLength;
        /// <summary>
        /// 消息版本号
        /// </summary>
        public short Version;
        /// <summary>
        /// 消息类型
        /// </summary>
        public int Action;
        /// <summary>
        /// 参数, 固定为1
        /// </summary>
        public int Parameter;

        public static DanmakuProtocol FromBuffer(byte[] buffer)
        {
            if (buffer.Length < 16) { throw new ArgumentException(); }
            return new DanmakuProtocol()
            {
                PacketLength = EndianBitConverter.BigEndian.ToInt32(buffer, 0),
                HeaderLength = EndianBitConverter.BigEndian.ToInt16(buffer, 4),
                Version = EndianBitConverter.BigEndian.ToInt16(buffer, 6),
                Action = EndianBitConverter.BigEndian.ToInt32(buffer, 8),
                Parameter = EndianBitConverter.BigEndian.ToInt32(buffer, 12),
            };
        }
    }

}