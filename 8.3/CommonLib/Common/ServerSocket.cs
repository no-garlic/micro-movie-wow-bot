using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace CommonLib.Common
{
    public class ServerSocket
    {
        private TcpListener m_ServerSocket;

        public ServerSocket(int port)
        {
            IPAddress localAddress;
            localAddress.Address
            
            m_ServerSocket = new TcpListener(port);

       
        }

    }
}
