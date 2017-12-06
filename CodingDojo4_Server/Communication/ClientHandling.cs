
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CodingDojo4_Server.Communication
{
    class ClientHandling
    {
        private Socket socket;
        byte[] buffer = new byte[1024];
       
        public ClientHandling(Socket socket)
        {
            this.socket = socket;
            //neuen Task ausführen
            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            int length;
            length = socket.Receive(buffer);
            Encoding.UTF8.GetString(buffer, 0, length);
        }
    }
}
