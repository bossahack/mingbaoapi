using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Book.Utils
{
    public class UdpSendHelper
    {
        static Socket sender= new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public static void Send(string ip,int port,string msg)
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            Byte[] sendByte = Encoding.Default.GetBytes(msg);
            sender.SendTo(sendByte, sendByte.Length, SocketFlags.None, point);
        }

    }
}
