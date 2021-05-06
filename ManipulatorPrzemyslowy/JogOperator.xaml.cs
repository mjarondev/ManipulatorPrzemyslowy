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

        enum robotArm
        {
            Waist = 1,
            Shoulder = 2,
            Elbow = 3,
            Twist = 4,
            Pitch = 5,
            Roll = 6
        }

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

        //Wysyła komendę poruszającą wybraną część robota o wybraną ilość stopni
        private void MoveArm(robotArm armNumber, bool rightSide)
        {
            double value;
            if (double.TryParse(JogIncrementTxt.Text, out value) && value >= 0 && value <= maxIncrement)
            {
                if (!rightSide)
                    value = -value;
                OnDataSend(new SendDataEventArgs("DJ " + (int)armNumber + "," + Math.Round(value, 2).ToString()));
            }
        }

        private void WaistRightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Waist, true);
        }

        private void WaistLeftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Waist, false);
        }

        private void ShoulderRightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Shoulder, true);
        }

        private void ShoulderLeftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Shoulder, false);
        }

        private void ElbowRightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Elbow, true);
        }

        private void ElbowLeftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Elbow, false);
        }

        private void TwistRightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Twist, true);
        }

        private void TwistLeftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Twist, false);
        }

        private void PitchRightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Pitch, true);
        }

        private void PitchLeftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Pitch, false);
        }

        private void RollRightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Roll, true);
        }

        private void RollLeftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArm(robotArm.Roll, false);
        }
    }
}
