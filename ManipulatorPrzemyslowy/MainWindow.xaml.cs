using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;



namespace ManipulatorPrzemyslowy
{

    public partial class MainWindow : Window
    {

        //log (może do zmiany?)
        BindingList<string> log = new BindingList<string>();


        //other windows
        CommunicationPort comPort;
        CommandTool comTool;
        JogOperator jogOp;

        //dane połączenia z portem COM
        SendData data;

        //dane otrzymane z robota
        StringBuilder receivedData = new StringBuilder();
        RobotData robotData;

        //dane wyslane do robota
        string lastSendCommand;

        //Serial port
        SerialPort serialPort;

        public MainWindow()
        {
            //zmiana cultureinfo wątku, aby separator dziesiętny był kropką a nie przecinkiem
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //inicjalizacja z domyslnymi ustawieniami portu oraz uaktualnienie wyswietlanych danych
            //gdy dane domyślne nie mogą zostać załadowane ustawia dane puste
            InitializeComponent();

            data = new SendData();
            serialPort = new SerialPort();
            robotData = new RobotData();

            LogList.ItemsSource = log;
            
            try
            {
                data.SetToDefault();
                UpdateVisibleData();
            }
            catch(ComPortNotActiveException ex)
            {
                SetEmptyVisibleData();
                MessageBox.Show(ex.Message);
            }


        }

        //w momencie otrzymania informacji z portu pobiera i uaktualnia je
        // serialPort.DataReceived jest uruchamiany w pobocznym wątku, dlatego został użyty Dispatcher do uaktualnienia
        // danych w wątku głównym obsługującym główne okno
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => 
            {
                receivedData.Clear();
                while (serialPort.BytesToRead >= 1) 
                {
                    char c = Convert.ToChar(serialPort.ReadByte());
                    receivedData.Append(c);
                    if (c == '\r')
                        break;
                }
                string str = receivedData.ToString().Trim('\n', '\r');

                if (!(comTool is null))
                    comTool.RobotInfoTxtBlock.Text = str;

                if (lastSendCommand == "WH")
                {
                    robotData.DecodeFrame(str);
                }

                AddToLog("Received: " + str);
                
            }));
            
        }

        //Uruchamia okno Communication Port lub jeżeli jest ono uruchomione aktywuje je
        private void CommunicationPortButton_Click(object sender, RoutedEventArgs e)
        {
            if (comPort is null)
            {
                comPort = new CommunicationPort(data);
                comPort.WindowClosed += ComPortWindowClosed;
                comPort.UpdateData += DataUpdated;
                comPort.Show();
            }
            else
            {
                comPort.Activate();
            }
        }

        //W przypadku gdy w oknie ComPort dane zostały przepisuje te dane do okna głównego
        private void DataUpdated(object sender, UpdateDataEventArgs e)
        {
            ShowSendControls();
            data = e.data;
            UpdateVisibleData();
            serialPort.Close();
            ConnectButton.Content = "Connect";
        }

        //W przypadku gdy okno ComPort zostało zamknięte kasuje odniesienie do niego w głównym oknie
        private void ComPortWindowClosed(object sender, WindowClosedEventArgs e)
        {
            if (comPort != null)
            {
                comPort.WindowClosed -= ComPortWindowClosed;
                comPort = null;
            }
        }


        //Zamyka pozostałe okna gdy główne zostało wyłączone
        private void Window_Closed(object sender, EventArgs e)
        {
            serialPort?.Close();
            comPort?.Close();
            comTool?.Close();
            jogOp?.Close();
        }

        //Ustawia i otwiera/zamyka serial port
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if(!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = data.PortName;
                    serialPort.Parity = data.PortParity;
                    serialPort.DataBits = data.DataBits;
                    serialPort.StopBits = data.PortStopBits;
                    serialPort.Handshake = data.PortHandshake;
                    serialPort.BaudRate = data.BaudRate;

                    serialPort.DataReceived += DataReceived;

                    serialPort.Open();

                    if (!(comTool is null))
                        comTool.ConnectionInfoLbl.Content = "connected";
                    if (!(jogOp is null))
                        jogOp.ConnectionInfoLbl.Content = "connected";

                    ConnectButton.Content = "Disconnect";

                    AddToLog("Connected");
                    SendToRobot(null, new SendDataEventArgs("WH"));

                }
                catch(ComPortNotActiveException ex)
                {
                    serialPort.Close();
                    serialPort.DataReceived -= DataReceived;
                    data.SetToEmpty();
                    SetEmptyVisibleData();
                    MessageBox.Show(ex.Message);
                }
                catch(ComPortInvalidValueException ex)
                {
                    serialPort.Close();
                    serialPort.DataReceived -= DataReceived;
                    data.SetToEmpty();
                    SetEmptyVisibleData();
                    MessageBox.Show(ex.Message);
                }
                
            }
            else
            {
                serialPort.Close();
                serialPort.DataReceived -= DataReceived;
                if (!(comTool is null))
                    comTool.ConnectionInfoLbl.Content = "disconnected";
                if (!(jogOp is null))
                    jogOp.ConnectionInfoLbl.Content = "disconnected";
                ConnectButton.Content = "Connect";
            }
        }

        

        //Wysyła wiadomość do robota jeżeli port COM jest otwarty
        private void SendToRobot(object sender, SendDataEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                string s;
                if (e.args != null)
                    s = e.command + " " + e.args;
                else
                    s = e.command;

                serialPort.Write(s + "\r");
                lastSendCommand = e.command;
                AddToLog("Send: " + s);
                UpdateRobotData(e.command);
                //AddToLog("Last command: " + e.command);
                //AddToLog("Last args: " + e.args);
            }
        }

        //Sprawdza czy przeslano dane dotyczace informacji zawartych w RobotData
        private void UpdateRobotData(string s)
        {
            switch(s)
            {
                case "GO":
                    robotData.Grip = "O";
                    break;
                case "GC":
                    robotData.Grip = "C";
                    break;
            }
            if(jogOp != null)
            {
                jogOp.UpdateData();
            }
        }

        //Aktualizuje widoczne dane na temat portu w MainWindow
        private void UpdateVisibleData()
        {
            try
            {
                BaudRateLbl.Content = data.BaudRate.ToString();
                DataBitsLbl.Content = data.DataBits.ToString();
                ParityLbl.Content = data.PortParity.ToString();
                StopBitsLbl.Content = data.PortStopBits.ToString();
                HandshakeLbl.Content = data.PortHandshake.ToString();
                SendTimeoutLbl.Content = data.SendTimeout.ToString();
                ReceiveTimeoutLbl.Content = data.ReceiveTimeout.ToString();
                PortLbl.Content = data.PortName.ToString();
            }
            catch(ComPortNotActiveException ex)
            {
                data.SetToEmpty();
                SetEmptyVisibleData();
                MessageBox.Show(ex.Message);
            }


        }

        //Pokazuje schowane przyciski łączenia
        private void ShowSendControls()
        {
            ConnectButton.Visibility = Visibility.Visible;
        }

        //Chowa przyciski wysyłania oraz czyści widoczne dane na temat portu COM 
        private void SetEmptyVisibleData()
        {
            ConnectButton.Visibility = Visibility.Hidden;
            BaudRateLbl.Content = "";
            DataBitsLbl.Content = "";
            ParityLbl.Content = "";
            StopBitsLbl.Content = "";
            HandshakeLbl.Content = "";
            SendTimeoutLbl.Content = "";
            ReceiveTimeoutLbl.Content = "";
            PortLbl.Content = "";

            AddToLog("Nieprawidłowe ustawienia połączenia portu.\nNależy ponownie ustawić opcje w Communication Port.");
            
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowSendControls();
                data.SetToDefault();
                UpdateVisibleData();
            }
            catch (ComPortNotActiveException ex)
            {
                SetEmptyVisibleData();
                MessageBox.Show(ex.Message);
            }
        }

        //W przypadku gdy okno ComTool zostało zamknięte kasuje odniesienie do niego w głównym oknie
        private void ComToolWindowClosed(object sender, WindowClosedEventArgs e)
        {
            if (comTool != null)
            {
                comTool.WindowClosed -= ComToolWindowClosed;
                comTool = null;
            }
        }


        //Uruchamia okno Command Tool lub jeżeli jest ono uruchomione aktywuje je
        private void CommandToolButton_Click(object sender, RoutedEventArgs e)
        {
            if (comTool is null)
            {
                comTool = new CommandTool();
                comTool.WindowClosed += ComToolWindowClosed;
                comTool.DataSend += SendToRobot;
                if(serialPort.IsOpen)
                    comTool.ConnectionInfoLbl.Content = "connected";
                else
                    comTool.ConnectionInfoLbl.Content = "disconnected";
                comTool.Show();
            }
            else
            {
                comTool.Activate();
            }
        }

        //Zapisuje Log do pliku
        private void SaveLogButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "LogFile";
            saveFileDialog.DefaultExt = ".txt";
            if(saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, string.Join<string>("\n", log));
            }
        }

        //W przypadku gdy okno ComTool zostało zamknięte kasuje odniesienie do niego w głównym oknie
        private void JogOpWindowClosed(object sender, WindowClosedEventArgs e)
        {
            if (jogOp != null)
            {
                jogOp.WindowClosed -= JogOpWindowClosed;
                jogOp = null;
            }
        }
        //Uruchamia okno Jog Operator lub jeżeli jest ono uruchomione aktywuje je
        private void JogOperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (jogOp is null)
            {
                jogOp = new JogOperator(ref robotData);
                jogOp.WindowClosed += JogOpWindowClosed;
                jogOp.DataSend += SendToRobot;
                if (serialPort.IsOpen)
                    jogOp.ConnectionInfoLbl.Content = "connected";
                else
                    jogOp.ConnectionInfoLbl.Content = "disconnected";
                jogOp.Show();
            }
            else
            {
                jogOp.Activate();
            }
        }

        //zmienia rozmiar okna po rozwinięciu CurrentComPortData
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.MaxWidth = 570;
            this.Width = 570;
            Expander.Width = 300;
            ExpanderPanel.Width = 300;
        }

        //zmienia rozmiar okna po zwinięciu CurrentComPortData
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.MaxWidth = 350;
            this.Width = 350;
            Expander.Width = 50;
            ExpanderPanel.Width = 60;
        }

        //dodaje element do listy Log
        public void AddToLog(string s)
        {
            if (log.Count >= 60)
            {
                log.RemoveAt(log.Count - 1);
            }
            log.Insert(0, DateTime.Now.ToString() + ": " + s);
        }

        //kopiuje do schowka zaznaczone elementy z listy Log
        private void LogListCopy(object sender, RoutedEventArgs e)
        {
            StringBuilder str = new StringBuilder();
            foreach (string item in LogList.SelectedItems)
                str.Append(item+"\r\n");
            
            Clipboard.SetText(str.ToString());
        }

        //kopiuje do schowka zaznaczone elementy z listy Log
        private void LogList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                StringBuilder str = new StringBuilder();
                foreach (string item in LogList.SelectedItems)
                    str.Append(item + "\r\n");

                Clipboard.SetText(str.ToString());
            }
        }
    }

    


}



