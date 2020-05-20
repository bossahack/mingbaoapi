using Book.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Book.Watch
{
    public class OnlineUser
    {
        public static Socket server;
        int point = 1025;
        public void Listen()
        {
            //接收
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.EnableBroadcast = true;
            server.Bind(new IPEndPoint(IPAddress.Any, point));//绑定端口号和IP
            server.ReceiveBufferSize = 1024 * 1024;
            server.SendBufferSize = 1024 * 1024;

            Console.WriteLine("开始监听");
            Task.Run(() =>
            {
                try
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                    byte[] buffer = new byte[10];

                    while (true)
                    {
                        try
                        {
                            int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                            string message = Encoding.UTF8.GetString(buffer, 0, length);
                            if (message == "0") {
                                Console.WriteLine(DateTime.Now.ToShortTimeString());
                                server.SendTo(Encoding.Default.GetBytes("a"), point);
                                continue;
                            }

                            int shopId = 0;
                            int.TryParse(message, out shopId);
                            if (shopId == 0)
                                continue;

                            Console.WriteLine(point);
                            IPEndPoint ipPoint = ((System.Net.IPEndPoint)point);
                            ShopOnlineService.GetInstance().Save(shopId, ipPoint.Address.ToString(), ipPoint.Port);
                            //server.SendTo(Encoding.Default.GetBytes("a"), ipPoint);
                        }catch(Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
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
