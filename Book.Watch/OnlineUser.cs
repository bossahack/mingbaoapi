using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Book.Watch
{
    public class OnlineUser
    {
        static Socket server;
        int point = 1025;
        public void Listen()
        {
            //接收
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Any, point));//绑定端口号和IP

            while (true)
            {
                Console.WriteLine("开始监听");

                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                int shopId = 0;
                int.TryParse(message,out shopId);
                if (shopId == 0)
                    continue;

                
            }
        }
    }
}
