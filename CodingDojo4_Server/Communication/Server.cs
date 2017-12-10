
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
        //Socket und Liste für client erstellen
        Socket serverSocket;
        List<ClientHandling> clients = new List<ClientHandling>();
        //gui updaten
        Action<string> GuiUpdate;
        //thread zum akzeptieren der clients
        Thread acceptingThread;


        public Server(string ip, int port, Action<string> guiUpdate)
        {
            //updaten der gui
            GuiUpdate = guiUpdate;
            //serverSocket initialisieren
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            //anzahl der max clients definieren
            serverSocket.Listen(5);
        }

        public void StartAccepting()
        {
            //thread
            acceptingThread = new Thread(new ThreadStart(Accept));
            //?
            acceptingThread.IsBackground = true;
            //starten
            acceptingThread.Start();
        }

        private void Accept()
        {
            while (acceptingThread.IsAlive)
            {

                try
                {
                    clients.Add(new ClientHandling(serverSocket.Accept(), new Action<string, Socket>(NewMessageReceived)));
                }
                catch (Exception e)
                {

                    //wenn serversocket.close
                }

            }
        }

        public void StopAccepting()
        {
            //socket und thread stoppen
            serverSocket.Close();
            acceptingThread.Abort();
            //alle client verbindungen schließen
            foreach (var item in clients)
            {
                item.Close();
            }
            //Liste leeren
            clients.Clear();

        }

        private void NewMessageReceived(string m, Socket senderSocket)
        {
            //bei message gui updaten
            GuiUpdate(m);
            //message an die clients ausgeben
            foreach (var item in clients)
            {
                if(item.ClientSocket != senderSocket)
                {
                    item.Send(m);
                }
            }
        }

        public void DisconnectClient(string name)
        {
            //wenn client sich disconnected aus liste raus und connection schließen
            foreach (var item in clients)
            {
                if (item.Name.Equals(name))
                {                
                    item.Close();
                    clients.Remove(item);
                    break;
                }
            }
        }
    }
}
