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
    /// <summary>
    /// Interaction logic for PositionAdd.xaml
    /// </summary>
    public partial class PositionAdd : Window
    {
        public PositionAddCommunication posAdd;

        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }

        public PositionAdd()
        {
            posAdd = new PositionAddCommunication(new CheckValues());
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string LR, AB, FN, OC;
            if (LeftRadio.IsChecked == true)
                LR = "L";
            else if (RightRadio.IsChecked == true)
                LR = "R";
            else
                LR = "";

            if (AboveRadio.IsChecked == true)
                AB = "A";
            else if (BelowRadio.IsChecked == true)
                AB = "B";
            else
                AB = "";

            if (FlipRadio.IsChecked == true)
                FN = "F";
            else if (NoFlipRadio.IsChecked == true)
                FN = "N";
            else
                FN = "";
            
            if (OpenRadio.IsChecked == true)
                OC = "O";
            else if (CloseRadio.IsChecked == true)
                OC = "C";
            else
                OC = "";

            try
            {
                posAdd.SendPosition(new string[] {NameTxt.Text, XTxt.Text, YTxt.Text, ZTxt.Text,
                ATxt.Text, BTxt.Text, CTxt.Text, LR, AB, FN, OC});
            }
            catch(ArgumentException)
            {
                MessageBox.Show("Incorrect values.");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnWindowClosed(new WindowClosedEventArgs());
        }
    }
}
