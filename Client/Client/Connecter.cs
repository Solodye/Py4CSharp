using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class Connecter
    {
        internal static Socket Connect(IPEndPoint port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //目标端口已经在cfger中绑定好了
            try
            {
                socket.Connect(port);
                Console.WriteLine(port + "连接成功!");
            }
            catch (Exception e)
            {
                Console.WriteLine("连接失败：" + e.Message);
                return null;
            }
            return socket;
        }
    }
}
