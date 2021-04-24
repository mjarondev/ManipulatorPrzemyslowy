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
        string grip;
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
            changeGripIcon();
        }



        //wysyła komendy do zamknięcia/otwarcia chwytaka zmieniając ikonę przycisku na owtartą/zamkniętą
        private void GripButton_Click(object sender, RoutedEventArgs e)
        {
            if(grip == "O")
            {
                grip = "C";
                OnDataSend(new SendDataEventArgs("GC"));
            }
            else
            {
                grip = "O";
                OnDataSend(new SendDataEventArgs("GO"));
            }
            changeGripIcon();
        }

        //zmienia ikonę przycisku na owtartą/zamkniętą
        private void changeGripIcon()
        {
            if (grip == "C")
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
