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
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ManipulatorPrzemyslowy
{
    /// <summary>
    /// Interaction logic for CommandTool.xaml
    /// </summary>
    
    
    public partial class CommandTool : Window
    {
        Dictionary<string, string> commandSyntax;

        //events
        public event EventHandler<WindowClosedEventArgs> WindowClosed;

        protected virtual void OnWindowClosed(WindowClosedEventArgs e)
        {
            WindowClosed?.Invoke(this, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnWindowClosed(new WindowClosedEventArgs());
        }

        public event EventHandler<SendDataEventArgs> DataSend;

        protected virtual void OnDataSend(SendDataEventArgs e)
        {
            DataSend?.Invoke(this, e);
        }

        public CommandTool()
        {
            InitializeComponent();
            commandSyntax = new Dictionary<string, string>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.CloseInput = true;

            XElement xElement;
            //zrobić to asynchronicznie?
            using (XmlReader reader = XmlReader.Create(Environment.CurrentDirectory+"\\data\\Commands.xml", settings))
            {
                xElement = XElement.Load(reader);
            }

            foreach(XElement el in xElement.Elements())
            {
                commandSyntax.Add(el.Name.LocalName, el.Value);
            }

            CommandList.ItemsSource = commandSyntax.Keys;
            CommandList.SelectedIndex = 0;
            SyntaxLbl.Content = "";
        }

        //po dwukrotnym naciśnięciu komendy w liście komend wstawia wybraną komendę do okna edycji komend
        private void CommandList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CommandTxtBox.Text = CommandList.SelectedItem.ToString();
        }

        //pokazuje składnie zaznaczonego polecenia
        private void CommandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SyntaxLbl.Content = commandSyntax[CommandList.SelectedItem.ToString()];
        }

        //po naciśnięciu enter w liście komend wstawia wybraną komendę do okna edycji komend
        private void CommandList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
                CommandTxtBox.Text = CommandList.SelectedItem.ToString();
        }

        //uruchamia zdarzenie wysłania informacji jeżeli port jest otwarty
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ConnectionInfoLbl.Content.ToString() == "connected")
            {
                string[] s = CommandTxtBox.Text.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries);
                if (commandSyntax.ContainsKey(s[0]))
                {
                    if (s.Length > 1)
                        OnDataSend(new SendDataEventArgs(s[0], s[1]));
                    else
                        OnDataSend(new SendDataEventArgs(s[0]));
                }
                else
                {
                    RobotInfoTxtBlock.Text = "Nie można wysłać.\nNie ma takiego polecenia.";
                }

            }
        }

        //po wpisaniu części lub całości komendy w oknie edycji pokazuje ją na liście komend i wyświetla jej składnie 
        private void CommandTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            
            if (CommandTxtBox.Text.Length == 0)
            {
                SyntaxLbl.Content = "";
            }
            else if (CommandTxtBox.Text.Length <= 3)
            {
                string s = CommandTxtBox.Text.Split(" ")[0];

                bool found = false;
                foreach (string str in commandSyntax.Keys)
                {
                    if (str.StartsWith(s))
                    {
                        found = true;
                        if (str == CommandList.SelectedItem.ToString())
                        {
                            //w przypadku gdy ta sama komenda jest wpisana wielokronie nie aktywuje się selectionchanged event
                            //więc jest ono wywoływane ręcznie
                            CommandList.RaiseEvent(new SelectionChangedEventArgs(
                                ListBox.SelectionChangedEvent,
                                new List<ListBoxItem> { CommandList.Items[0] as ListBoxItem },
                                new List<ListBoxItem> { CommandList.SelectedItem as ListBoxItem }
                                ));
                        }
                        else
                        {
                            CommandList.SelectedItem = str;
                            CommandList.ScrollIntoView(str);
                        }
                        break;
                    }
                }

                if (!found)
                {
                    SyntaxLbl.Content = "Nie ma takiej komendy";
                }
                
            }

        }

        //pozwala na wybieranie komendy z listy komend z poziomu pola edycji komend
        private void CommandTxtBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Down && CommandList.SelectedIndex != CommandList.Items.Count)
            {
                CommandList.SelectedIndex = CommandList.SelectedIndex + 1;
                CommandList.ScrollIntoView(CommandList.SelectedItem);
            }
            else if (e.Key == Key.Up && CommandList.SelectedIndex != 0)
            {
                CommandList.SelectedIndex = CommandList.SelectedIndex -1;
                CommandList.ScrollIntoView(CommandList.SelectedItem);
            }
            else if(e.Key == Key.Return)
            {
                CommandTxtBox.Text = CommandList.SelectedItem.ToString();
                CommandTxtBox.CaretIndex = CommandTxtBox.Text.Length;
            }

        }

        private void RobotInfoCopy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(RobotInfoTxtBlock.Text);
        }
    }
}
