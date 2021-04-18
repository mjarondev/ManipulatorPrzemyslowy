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

namespace ManipulatorPrzemyslowy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //other windows
        CommunicationPort comPort;
        CommandTool comTool;

        //dane połączenia z portem COM
        SendData data;

        //Serial port
        SerialPort serialPort;

        public MainWindow()
        {

            //inicjalizacja z domyslnymi ustawieniami portu oraz uaktualnienie wyswietlanych danych
            InitializeComponent();
            
            data = new SendData();
            serialPort = new SerialPort();

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

        //w momencie otrzymania informacji z portu pobiera je i uaktualnia dane w głównym oknie
        // serialPort.DataReceived jest uruchamiany w pobocznym wątku, dlatego został użyty Dispatcher do uaktualnienia
        // danych w wątku głównym obsługującym główne okno
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => 
            { while (serialPort.BytesToRead >= 1) 
                { 
                    InfoLbl.Content = serialPort.ReadLine().ToString() + serialPort.BytesToRead.ToString(); 
                }
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
        }

        //Ustawia i otwiera/zapyka serial port
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if(!serialPort.IsOpen)
            {
                try
                {
                    serialPort.BaudRate = data.BaudRate;
                    serialPort.Parity = data.PortParity;
                    serialPort.DataBits = data.DataBits;
                    serialPort.StopBits = data.PortStopBits;
                    serialPort.Handshake = data.PortHandshake;
                    serialPort.PortName = data.PortName;

                    serialPort.DataReceived += DataReceived;

                    serialPort.Open();

                    ConnectButton.Content = "Disconnect";

                    serialPort.Write("WH\r");


                }
                catch(ComPortNotActiveException ex)
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
                ConnectButton.Content = "Connect";
            }
        }

        //Wysyła wiadomość, do zmiany
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(serialPort.IsOpen)
            {
                serialPort.Write(SendTxtBox.Text);
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

        private void ShowSendControls()
        {
            SendButton.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Visible;
            SendTxtBox.Visibility = Visibility.Visible;
            SendLbl.Visibility = Visibility.Visible;
            InfoLbl.Content = "";
        }

        private void SetEmptyVisibleData()
        {
            SendButton.Visibility = Visibility.Hidden;
            ConnectButton.Visibility = Visibility.Hidden;
            SendTxtBox.Visibility = Visibility.Hidden;
            SendLbl.Visibility = Visibility.Hidden;
            BaudRateLbl.Content = "";
            DataBitsLbl.Content = "";
            ParityLbl.Content = "";
            StopBitsLbl.Content = "";
            HandshakeLbl.Content = "";
            SendTimeoutLbl.Content = "";
            ReceiveTimeoutLbl.Content = "";
            PortLbl.Content = "";

            InfoLbl.Content = "Nieprawidłowe ustawienia połączenia portu.\nNależy ponownie ustawić opcje w Communication Port.";
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

        private void ComToolWindowClosed(object sender, WindowClosedEventArgs e)
        {
            if (comTool != null)
            {
                comTool.WindowClosed -= ComToolWindowClosed;
                comTool = null;
            }
        }

        private void CommandToolButton_Click(object sender, RoutedEventArgs e)
        {
            if (comTool is null)
            {
                comTool = new CommandTool();
                comTool.WindowClosed += ComToolWindowClosed;
                comTool.Show();
            }
            else
            {
                comTool.Activate();
            }
        }
    }

    


}



