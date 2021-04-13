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
        readonly string[] parity = Enum.GetNames(typeof(Parity));
        readonly string[] dataBits = new string[] {"5", "6", "7", "8"};
        readonly string[] stopBits = Enum.GetNames(typeof(StopBits));
        readonly string[] handshake = Enum.GetNames(typeof(Handshake));

        //events
        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }


        public event EventHandler<UpdateDataEventArgs> UpdateData;

        protected virtual void OnUpdateData(UpdateDataEventArgs e)
        {
            UpdateData?.Invoke(this, e);
        }

        //dane połączenia z portem COM
        SendData data;

        public CommunicationPort(SendData d)
        {
            InitializeComponent();
            RefreshState();

            BaudRateCombo.ItemsSource = baudRates;
            ParityCombo.ItemsSource = parity;
            DataBitsCombo.ItemsSource = dataBits;
            StopBitsCombo.ItemsSource = stopBits;
            HandshakeComboBox.ItemsSource = handshake;

            SetData(d);

            PortCombo.SelectedItem = data.PortName.ToString();
            BaudRateCombo.SelectedItem = data.BaudRate.ToString();
            ParityCombo.SelectedItem = data.PortParity.ToString();
            DataBitsCombo.SelectedItem = data.DataBits.ToString();
            StopBitsCombo.SelectedItem = data.PortStopBits.ToString();
            HandshakeComboBox.SelectedItem = data.PortHandshake.ToString();
 

        }

        public void SetData(SendData d)
        {
            data = d;
        }

        //Odświerza listę aktywnych portów COM
        public void RefreshState()
        {
            PortCombo.ItemsSource = SerialPort.GetPortNames();
        }

        //Przy zamknięciu okna PortCom wywołuje zdarzenie usuwające odniesienie do ComPort w oknie głównym
        private void Window_Closed(object sender, EventArgs e)
        {
            OnWindowClosed(new WindowClosedEventArgs());
        }

        //Odświerza listę aktywnych portów COM przy otwieraniu listy portów
        private void PortCombo_DropDownOpened(object sender, EventArgs e)
        {
            RefreshState();
        }

        //Przy kliknięciu SaveButton wywołuje zdarzenie zapisujące dane w oknie głównym
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                data.PortName = PortCombo.Text.ToString();
                data.BaudRate = Int32.Parse(BaudRateCombo.Text);
                data.DataBits = Int32.Parse(DataBitsCombo.Text);
                data.SendTimeout = Int32.Parse(SendTimeoutBox.Text);
                data.ReceiveTimeout = Int32.Parse(ReceiveTimeoutBox.Text);
                data.PortParity = (Parity)Enum.Parse(typeof(Parity), ParityCombo.Text, true);
                data.PortStopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBitsCombo.Text, true);
                data.PortHandshake = (Handshake)Enum.Parse(typeof(Handshake), HandshakeComboBox.Text, true);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                //obsługa błędu związana ze zbyt dużymi lub zbyt małymi wartościami w SendTImeout oraz ReceiveTimeout
            }
            catch (ArgumentException ex)
            {

            }
            catch(FormatException ex)
            {
                //obsługa błędu związana z niewłaściwymi wartościami w SendTImeout oraz ReceiveTimeout
            }


            OnUpdateData(new UpdateDataEventArgs(data));
        }
    }
}
