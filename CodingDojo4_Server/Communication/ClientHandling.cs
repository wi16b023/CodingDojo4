
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodingDojo4_Server.Communication
{
    class ClientHandling
    {
        //vieles automatisch aus server.cs erstellt!
        
        private Action<string, Socket> action;
        private byte[] buffer = new byte[512];
        private Thread clientReceiveThread;
        //zum beenden
        const string endMessage = "@quit";

        public Socket ClientSocket { get; private set; }
        public string Name { get; private set; }

        public ClientHandling(Socket socket, Action<string, Socket> action)
        {
            this.ClientSocket = socket;
            this.action = action;
            //neuer thread zum message receiving
            clientReceiveThread = new Thread(Receive);
            clientReceiveThread.Start();
        }

        //methode für messages
        private void Receive()
        {
            string message = "";
            while (!message.Equals(endMessage))
            {
                int length = ClientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                //property name zuweisen
                if (Name == null && message.Contains(":"))
                {
                    Name = message.Split(':')[0];
                }
                //delegate für gui
                action(message, ClientSocket);
            }
            Close();
        }

        public void Close()
        {
            //schließen
            Send(endMessage);
            ClientSocket.Close(1);
            clientReceiveThread.Abort();
        }

        public void Send(string m)
        {
            ClientSocket.Send(Encoding.UTF8.GetBytes(m));
        }
    }
}
