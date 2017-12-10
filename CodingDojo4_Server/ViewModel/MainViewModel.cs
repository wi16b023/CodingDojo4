using System;
using CodingDojo4_Server.Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CodingDojo4_DataHandling;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodingDojo4_Server.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private Server server;
        //port und ipaddress festlegen
        private const int port = 10100;
        private const string ip = "127.0.0.1";
        private bool isConnected = false;
        //add references!
        private DataHandler Handler;

        //relaycommands
        public RelayCommand StartBtnClickCmd { get; set; }
        public RelayCommand StopBtnClickCmd { get; set; }
        public RelayCommand DropClientBtnClickCmd { get; set; }
        public RelayCommand SaveToLogBtnClickCmd { get; set; }

        //collections needed
        public ObservableCollection<string> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }

        public string SelectedUser { get; set; }
        public int NoOfReceivedMessages{ get { return Messages.Count; } }      

        public ObservableCollection<string> LogFiles
        {
            get
            {
                return new ObservableCollection<string>(Handler.QueryFilesFromFolder());
            }
        }

        public ObservableCollection<string> LogMessages { get; set; }
        public string SelectedLogFile { get; set; }

        //relaycommands
        public RelayCommand OpenLogFileBtnClickCmd { get; set; }
        public RelayCommand DropLogFileBtnClickCmd { get; set; }
        
        public MainViewModel()
        {
            Handler = new DataHandler();
            Messages = new ObservableCollection<string>();
            Users = new ObservableCollection<string>();
            LogMessages = new ObservableCollection<string>();

            //set command for start button
            StartBtnClickCmd = new RelayCommand(
                () =>
                {
                    server = new Server(ip, port, UpdateGuiWithNewMessage);
                    server.StartAccepting();
                    isConnected = true;
                },
                () => { return !isConnected; });

            //set command for stop button
            StopBtnClickCmd = new RelayCommand(
                //action for execute
                () =>
                {
                    server.StopAccepting();
                    isConnected = false;
                },
                //can execute
                () => { return isConnected; });

            //init Command for Drop button with CanExecute statement
            DropClientBtnClickCmd = new RelayCommand(() =>
            {
                server.DisconnectClient(SelectedUser);
                Users.Remove(SelectedUser); // remove from GUI listbox
            },
                () => { return (SelectedUser != null); });

            //init Command for SaveToLogFIle button with CanExecute statement
            SaveToLogBtnClickCmd = new RelayCommand(
                () =>
                {
                    Handler.Save(Messages.ToList());
                    RaisePropertyChanged("LogFiles"); //to update the list in the log section
                },
                () => { return Messages.Count >= 1; }
                );

            //init Command for OpenLogFile button with CanExecute statement
            OpenLogFileBtnClickCmd = new RelayCommand(
                () =>
                {
                    LogMessages = new ObservableCollection<string>(Handler.Load(SelectedLogFile));
                    RaisePropertyChanged("LogMessages");
                },
                () => { return SelectedLogFile != null; }
                );
            //init Command for Drop Log files button with CanExecute statement
            DropLogFileBtnClickCmd = new RelayCommand(
                () =>
                {
                    Handler.Delete(SelectedLogFile);
                    RaisePropertyChanged("LogFiles"); //to update the list in the log section},
                },
                () => { return SelectedLogFile != null; });
        }

        private void UpdateGuiWithNewMessage(string m)
        {
            //switch thread to GUI thread to write to GUI
            App.Current.Dispatcher.Invoke(() =>
            {
                string name = m.Split(':')[0];
                if (!Users.Contains(name))
                {//not in list => add it
                    Users.Add(name);
                }
                //write message
                Messages.Add(m);
                //do this to inform the GUI about the update of the received message counter!
                RaisePropertyChanged("NoOfReceivedMessages");
            });
        }
    }
}