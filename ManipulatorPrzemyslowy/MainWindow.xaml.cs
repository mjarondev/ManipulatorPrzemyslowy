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


namespace ManipulatorPrzemyslowy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //events

        //other windows
        CommunicationPort comPort;

        //dane połączenia z portem COM
        SendData data;

        //Serial port
        SerialPort serialPort;

        public MainWindow()
        {
            //inicjalizacja z domyslnymi ustawieniami portu
            //TODO: dodać obsługę wywalenia wyjątku gdy nie ma aktywnych portów COM
            data = new SendData();

            serialPort = new SerialPort();

            InitializeComponent();
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
            data = e.data;
            testLbl.Content = e.data.BaudRate;
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
            comPort?.Close();
        }

        //Ustawia i otwiera/zapyka serial port
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if(!serialPort.IsOpen)
            {
                serialPort.PortName = data.PortName;
                serialPort.BaudRate = data.BaudRate;
                serialPort.Parity = data.PortParity;
                serialPort.DataBits = data.DataBits;
                serialPort.StopBits = data.PortStopBits;
                serialPort.Handshake = data.PortHandshake;
                serialPort.Open();

                ConnectButton.Content = "Disconnect";
            }
            else
            {
                serialPort.Close();
                ConnectButton.Content = "Connect";

            }
        }

        //Wysyła PING na sztywno, do zmiany
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(serialPort.IsOpen)
            {
                serialPort.Write("PING");
            }
        }
    }

    


}



