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
        //events
        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }


        public CommunicationPort()
        {
            InitializeComponent();
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
