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

namespace ManipulatorPrzemyslowy
{
    /// <summary>
    /// Interaction logic for PositionAdd.xaml
    /// </summary>
    public partial class PositionAdd : Window
    {
        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }

        public PositionAdd()
        {
            InitializeComponent();
        }







    }
}