﻿using System;
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
    public partial class CommunicationPort : Window
    {
        public CommunicationPort()
        {
            InitializeComponent();
            RefreshState();
        }

        public void RefreshState()
        {
            portBox.ItemsSource = SerialPort.GetPortNames();
        }

    }
}