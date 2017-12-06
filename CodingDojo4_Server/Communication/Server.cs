
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodingDojo4_Server.Communication
{
    class Server
    {
        private TcpListener tcpServer;
        private List<ClientHandling> clients = new List<ClientHandling>();
        //Action<string> GuiUpdate;
        Thread t;
        public Server()
        {            
            tcpServer = new TcpListener(IPAddress.Loopback, 8090);
            tcpServer.Start();
            t = new Thread(new ThreadStart(AcceptClient));
            t.IsBackground = true;
            t.Start();
        }

        private void AcceptClient()
        {
            while (true)
            {
                clients.Add(new ClientHandling(tcpServer.AcceptSocket()));
            }
        }
    }
}
