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
        //event
        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }


        public CommunicationPort()
        {
            InitializeComponent();
            portBox.ItemsSource = SerialPort.GetPortNames();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnWindowClosed(new WindowClosedEventArgs());
        }
    }
}
