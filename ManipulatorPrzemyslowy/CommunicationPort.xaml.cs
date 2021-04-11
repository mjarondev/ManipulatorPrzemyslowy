using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.Ports; //zainstalowany przez nuget

namespace ManipulatorPrzemyslowy
{
    /// <summary>
    /// Interaction logic for CommunicationPort.xaml
    /// </summary>
    /// 

    public partial class CommunicationPort : Window
    {
        //Dane do wybrania w combobox
        readonly string[] baudRates = new string[] {"110", "300", "600", "1200", "2400", "4800", "9600", "14400",
            "19200", "28800", "38400", "56000", "57600", "115200", "230400"};
        readonly string[] parity = new string[] {"None", "Odd", "Even", "Mark", "Space"};
        readonly string[] dataBits = new string[] {"5", "6", "7", "8"};
        readonly string[] stopBits = new string[] { "1", "1.5", "2" };
        readonly string[] handshake = new string[] { "None", "XOnXOff", "RTS", "RTSXOnXOff" }; //RTS-RequestToSend


        //events
        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }


        public CommunicationPort()
        {
            InitializeComponent();
            BaudRateCombo.ItemsSource = baudRates;
            ParityCombo.ItemsSource = parity;
            DataBitsCombo.ItemsSource = dataBits;
            StopBitsCombo.ItemsSource = stopBits;
            HandshakeComboBox.ItemsSource = handshake;
        }

        //Odświerza listę aktywnych portów COM
        public void RefreshState()
        {
            portBox.ItemsSource = SerialPort.GetPortNames();
        }

        //Przy zamknięciu okna PortCom wywołuje zdarzenie usuwające odniesienie do ComPort w oknie głównym
        private void Window_Closed(object sender, EventArgs e)
        {
            OnWindowClosed(new WindowClosedEventArgs());
        }

        //Odświerza listę aktywnych portów COM przy otwieraniu listy portów
        private void portBox_DropDownOpened(object sender, EventArgs e)
        {
            RefreshState();
        }
    }
}
