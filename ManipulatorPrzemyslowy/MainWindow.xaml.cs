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

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (comPort is null)
            {
                comPort = new CommunicationPort();
                comPort.WindowClosed += ComPortWindowClosed;
                comPort.Show();
            }
            else
            {
                comPort.Activate();
            }
        }

        private void ComPortWindowClosed(object sender, WindowClosedEventArgs e)
        {
            comPort.WindowClosed -= ComPortWindowClosed;
            comPort = null;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            comPort?.Close();
        }
    }

    


}



