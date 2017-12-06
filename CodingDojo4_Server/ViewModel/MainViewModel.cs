using System;
using CodingDojo4_Server.Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CodingDojo4_Server.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        //SErver vorbereiten, Serverklasse importieren
        Server server;
        public Boolean RunServer;
        public RelayCommand StartBtnClicked { get; set; }
        public MainViewModel()
        {
            StartBtnClicked = new RelayCommand(ShowStartBtnClicked, IsShowStartBtnClicked);
        }

        private bool IsShowStartBtnClicked()
        {
            return !RunServer;
        }

        private void ShowStartBtnClicked()
        {
            server = new Server();
            RunServer = true;
        }

       
    }
}