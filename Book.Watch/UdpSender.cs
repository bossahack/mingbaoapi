using Book.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Book.Watch
{
    public class UdpSender
    {

        static Socket Wlanserver;
        int point = 1025;
        public void Listen()
        {
            //接收
            Wlanserver = new Socket(SocketType.Dgram, ProtocolType.Udp);
            //server.EnableBroadcast = true;
            Wlanserver.Bind(new IPEndPoint(IPAddress.Loopback, point));//绑定端口号和IP

            Console.WriteLine("开始内部广播监听");
            Task.Run(() =>
            {
                try
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                    byte[] buffer = new byte[1024];

                    while (true)
                    {
                        int length = Wlanserver.ReceiveFrom(buffer, ref point);//接收数据报
                        string message = Encoding.UTF8.GetString(buffer, 0, length);
                        Console.WriteLine(message);
                        var arr = message.Split('&');
                        if (arr.Length != 2)
                            continue;

                        int shopId = 0;
                        int.TryParse(arr[0], out shopId);
                        if (shopId == 0)
                            continue;

                        var online = ShopOnLineDal.GetInstance().Get(shopId);
                        if (online != null && (DateTime.Now - online.LastKeepTime).TotalMinutes < 5)
                        {
                            EndPoint shopPoint = new IPEndPoint(IPAddress.Parse(online.Ip), online.Port);
                            OnlineUser.server.SendTo(Encoding.Default.GetBytes(arr[1]), shopPoint);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            });

        }
    }
}
