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
        const double maxIncrement = 10;

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

        //konstruktory
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

        //aktualizuje dane
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

        //wysyła wartość z w JogSpeedTxt do robota po naciśnięciu enter 
        private void JogSpeedTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                double value;
                if(double.TryParse(JogSpeedTxt.Text, out value) && value >= 0 && value <= 10)
                {
                    JogSpeedSlider.Value = value;
                    OnDataSend(new SendDataEventArgs("SP " + Math.Round(value, 2).ToString()));
                }
            }
        }

        //pokazuje aktualną wartość JogSpeedSlider w JogSpeedTxt
        private void JogSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            JogSpeedTxt.Text = Math.Round(JogSpeedSlider.Value, 2).ToString();
        }

        //po puszczeniu myszy nad sliderem wysyła wartość z JogSpeedSlider
        private void JogSpeedSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double value = Math.Round(JogSpeedSlider.Value, 2);
            JogSpeedTxt.Text = value.ToString();
            OnDataSend(new SendDataEventArgs("SP " + value.ToString()));
        }

        //wysyła wartość z w JogSpeedTxt do robota po opuszczeniu JogSpeedTxt
        private void JogSpeedTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogSpeedTxt.Text, out value) && value >= 0 && value <= 10)
            {
                JogSpeedSlider.Value = value;
                OnDataSend(new SendDataEventArgs("SP " + Math.Round(value, 2).ToString()));
            }
        }

        //pokazuje aktualną wartość JogIncrementSlider w JogIncrementTxt
        private void JogIncrementSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            JogIncrementTxt.Text = Math.Round(JogIncrementSlider.Value, 2).ToString();
        }

        //pokazuje aktualną wartość JogIncrementTxt w JogIncrementSlider po naciśnięciu enter
        private void JogIncrementTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                double value;
                if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
                {
                    JogIncrementSlider.Value = value;
                }
            }
            
        }

        //pokazuje aktualną wartość JogIncrementTxt w JogIncrementSlider po naciśnięciu przycisku
        private void JogIncrementTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                JogIncrementSlider.Value = value;
            }
        }

        private void WaistRightButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 1," + Math.Round(value, 2).ToString()));
            }
        }

        private void WaistLeftButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 1," + Math.Round(-value, 2).ToString()));
            }
        }

        private void ShoulderRightButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 2," + Math.Round(value, 2).ToString()));
            }
        }

        private void ShoulderLeftButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 2," + Math.Round(-value, 2).ToString()));
            }
        }

        private void ElbowRightButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 3," + Math.Round(value, 2).ToString()));
            }
        }

        private void ElbowLeftButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 3," + Math.Round(-value, 2).ToString()));
            }
        }

        private void TwistRightButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 4," + Math.Round(value, 2).ToString()));
            }
        }

        private void TwistLeftButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 4," + Math.Round(-value, 2).ToString()));
            }
        }

        private void PitchRightButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 5," + Math.Round(value, 2).ToString()));
            }
        }

        private void PitchLeftButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 5," + Math.Round(-value, 2).ToString()));
            }
        }

        private void RollRightButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 6," + Math.Round(value, 2).ToString()));
            }
        }

        private void RollLeftButton_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                OnDataSend(new SendDataEventArgs("DJ 6," + Math.Round(-value, 2).ToString()));
            }
        }
    }
}
