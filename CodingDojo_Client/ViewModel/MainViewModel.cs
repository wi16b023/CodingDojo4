using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Net.Sockets;

namespace CodingDojo_Client.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        public string ChatName { get; set; }
        public string Message { get; set; }
        public bool Connected { get; set; }
        public RelayCommand ConnectBtnClicked { get; set; }
        public MainViewModel()
        {
            ConnectBtnClicked = new RelayCommand(showConnectBtnClicked, IsShowConnectBtnEnabled);
        }

        private bool IsShowConnectBtnEnabled()
        {
            return !Connected;
        }

        private void showConnectBtnClicked()
        {
            //man kann sich nicht verbinden wenn kein Name eingegeben ist
            if (ChatName != null && !ChatName.Equals(""))
            {
                Connected = false;
                TcpClient client = new TcpClient();
                client.Connect("localhost", 8090);
                Connected = true;
            }
        }
    }
}