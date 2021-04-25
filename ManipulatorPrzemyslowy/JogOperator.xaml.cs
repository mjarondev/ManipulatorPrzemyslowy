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


namespace ManipulatorPrzemyslowy
{

    public partial class JogOperator : Window
    {
        //dane o robocie
        RobotData robotData;

        //events
        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }

        public event EventHandler<SendDataEventArgs> DataSend;

        protected virtual void OnDataSend(SendDataEventArgs e)
        {
            DataSend?.Invoke(this, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnWindowClosed(new WindowClosedEventArgs());
        }

        //konstruktor
        public JogOperator()
        {
            InitializeComponent();
            ChangeGripIcon();
        }

        public JogOperator(ref RobotData robot)
        {
            InitializeComponent();
            robotData = robot;
            ChangeGripIcon();
        }


        //wysyła komendy do zamknięcia/otwarcia chwytaka zmieniając ikonę przycisku na owtartą/zamkniętą
        private void GripButton_Click(object sender, RoutedEventArgs e)
        {
            if(robotData.Grip == "O")
            {
                robotData.Grip = "C";
                OnDataSend(new SendDataEventArgs("GC"));
            }
            else
            {
                robotData.Grip = "O";
                OnDataSend(new SendDataEventArgs("GO"));
            }
            ChangeGripIcon();
        }

        public void UpdateData()
        {
            ChangeGripIcon();
        }

        //zmienia ikonę przycisku na owtartą/zamkniętą
        private void ChangeGripIcon()
        {
            if (robotData.Grip == "C")
            {
                im.Source = new BitmapImage(new Uri("icons/chwytakOtwarcie.ico", UriKind.Relative));
                GripButton.ToolTip = "Grip is closed.\nClick to open grip.";
            }
            else
            {
                im.Source = new BitmapImage(new Uri("icons/chwytakZamkniecie.ico", UriKind.Relative));
                GripButton.ToolTip = "Grip is open.\nClick to close grip.";
            }
        }


        



    }
}
