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

namespace ManipulatorPrzemyslowy
{
    /// <summary>
    /// Interaction logic for CommandTool.xaml
    /// </summary>
    
    
    public partial class CommandTool : Window
    {
        //dane o komendach
        string[] commands = new string[] {
            "AN", "CF", "CL", "CP", "CR", "DA", "DC", "DJ", "DL", "DP",
            "DR", "DS", "DW", "EA", "ED", "EQ", "ER", "GC", "GF", "GO",
            "GP", "GS", "GT", "HE", "HLT", "HO", "IC", "ID", "INP", "IP",
            "JRC", "LG", "LR", "MA", "MC", "MJ", "MO", "MP", "MPB", "MPC",
            "MR", "MRA", "MS", "MT", "MTS", "N", "NE", "NT", "NW", "NX",
            "OB", "OC", "OD", "OG", "OPN", "OR", "OVR", "PA", "PC", "PD",
            "PL", "PMR", "PMW", "PR", "PRN", "PT", "PW", "PX", "QN", "RC",
            "RN", "RS", "RT", "SC", "SD", "SF", "SM", "SP", "STR", "TB",
            "TBD", "TI", "TL", "VR", "WH", "WT", "XO"};
        
        string[] syntax = new string[] {
            "AN - And\nSyntax: AN <operation data>",
            "CF - Change Figure\nSyntax: CF <position number> [, [<R/L>] [, [<A/B>] [, [<N/F>]]]]",
            "CL - Counter Load\nSyntax: CL <counter number/character string number>",
            "CP - Compare Counter\nSyntax: CP <counter number/character string number>",
            "CR - Counter Read\nSyntax: CR <counter number/character string number>",
            "DA - Disable Act\nSyntax: DA <input bit number>",
            "DC - Decrement Counter\nSyntax: DC <counter number>",
            "DJ - Draw Joint\nSyntax: DJ <joint number>, <turning angle>",
            "DL - DeleteLine\nSyntax: DL <line number (a)> [, [<line number (b)> [, [<step (a)>\n" +
            "[, [<step (b)>]]]]]]",
            "DP - Decrement Position\nSyntax: DP",
            "DR - Data Read\nSyntax: DR [<output bit number>]",
            "DS - Draw Straight\nSyntax: DS [<travel distance in X>], [<travel distance in Y>],\n" +
            "[<travel distance in Z>]",
            "DW - Draw\nSyntax: DW [<travel distance in X>], [<travel distance in Y>],\n" +
            "[<travel distance in Z>]",
            "EA - Enable Act\nSyntax: EA [<+/->] <input bit number>, <line number>\n" +
            "[, [<branching approach>]]",
            "ED - End\nSyntax: ED",
            "EQ - Equal\nSyntax: EQ <compare value>, <branching line number>",
            "ER - Error Read\nSyntax: ER [<alarm history number>]",
            "GC - Grip Close\nSyntax: GC [<hand number>]",
            "GF - Grip Flag\nSyntax: GF <switch>",
            "GO - Grip Open\nSyntax: GO [<hand number>]",
            "GP - Grip Pressure\nSyntax: GP <starting gripping force>, <retained gripping force>,\n" +
            "<starting gripping force retention rime>",
            "GS - Go Sub\nSyntax: [<line number>] [, [<program name>]]",
            "GT - Go To\nSyntax: GT <line number>",
            "HE - Here\nSyntax: HE <position number>",
            "HLT - Halt\nSyntax: HLT",
            "HO - Home\nSyntax: HO [<origin setting approach>]",
            "IC - Increment Counter\nSyntax: IC <counter number>",
            "ID - Input Direct\nSyntax: ID [<input bit number>]",
            "INP - Input\nSyntax: INP <channel number>,\n<counter number/position number/character string number>\n" +
            "[, [<contents selection>]]",
            "IP - Increment Position\nSyntax: IP",
            "JRC - Joint Roll Change\nSyntax: JRC <[+]1/-1>",
            "LG - If Larger\nSyntax: LG <compared value/character string number>,\n<branching line number>",
            "LR - Line Read\nSyntax: LR [<line number>]",
            "MA - Move Approach\nSyntax: MA <position number (a)>, <position number (b)>\n" +
            "[, [<O/C>]]",
            "MC - Moce Continuous\nSyntax: MC <position number (a)>, <position number (b)>\n" +
            "[, [<O/C>]]",
            "MJ - Move Joint\nSyntax: MJ [<waist joint angle>], [<shoulder joint angle>],\n" +
            "[<elbow joint angle>], [<twist joint angle>],\n" +
            "[<pitch joint angle>], [<roll joint angle>]",
            "MO - Move\nSyntax: MO <position number> [, [<O/C>]]",
            "MP - Move Position\nSyntax: MP [<X coordinate value>], [<Y coordinate value>],\n" +
            "[<Z coordinate value>], [<A turn angle>], [<B turn angle>],\n" +
            "[<C turn angle>] [, [<R/L>] [, [<A/B>] [, [<N/F>]]]]",
            "MPB - Move Playback\nSyntax: MPB [<speed>], [<timer>], [<output ON>], [<Output OFF>],\n" +
            "[<input ON>], [<input OFF>], [, [<interpolation>, [<X coordinate>],\n" +
            "[<Y coordinate>], [<Z coordinate>], [<A turning angle>], [<B turning angle>],\n" +
            "[<C turning angle>] [, [<R/L>] [, [<A/B>] [, [<N/F>]]]] [, [<O/C>]]]",
            "MPC - Move Playback Continuous\nSyntax: MPC [<interpolation>], [<X coordinate>], [<Y coordinate>],\n" +
            "[<Z coordinate>], [<A turning angle>], [<B turning angle>],\n" +
            "[<C turning angle>] [, [<R/L>] [, [<A/B>] [, [<N/F>]]]] [, [<O/C>]]]",
            "MR - Move R\nSyntax: MR <position number (a)>, <position number (b)>,\n<position number (c)> [, [<O/C>]]",
            "MRA - Move R A\nSyntax: MRA <position number> [, [<O/C>]]",
            "MS - Move Straight\nSyntax: MS <position number> [, [<O/C>]]",
            "MT - Move Tool\nSyntax: MT <position number>, [<travel distance>] [, [<O/C>]]",
            "MTS - Move Tool Straight\nSyntax: MTS <position number>, [<travel distance>] [, [<O/C>]]",
            "N - Number\nSyntax: N <program name>",
            "NE - If Not Equal\nSyntax: NE <compared value/character string number>,\n" +
            "<branching line number>",
            "NT - Nest\nSyntax: NT",
            "NW - New\nSyntax: NW",
            "NX - Next\nSyntax: NX",
            "OB - Output Bit\nSyntax: OB [<+/->] <bit number>",
            "OC - Output Counter\nSyntax: OC <counter number> [, [<output bit>] [, [<bit width>]]]",
            "OD - Output Direct\nSyntax: OD <output data> [, [<output bit number>] [, [<bit width>]]]",
            "OG - Origin\nSyntax: OG",
            "OPN - Open\nSyntax: OPN <channel number>, <device number>",
            "OR - Or\nSyntax: OR <operation data>",
            "OVR - Override\nSyntax: OVR <specified override>",
            "PA - Pallet Assign\nSyntax: PA <pallet number>, <number of column grid points>,\n" +
            "<number of row grid points>",
            "PC - Position Clear\nSyntax: PC <position number (a)> [, [<position number (b)>]]",
            "PD - Position Define\nSyntax: PD <position number>, [<X coordinate>], [<Y coordinate>],\n" +
            "[<Z coordinate>], [<A turning angle>], [<B turning angle>], [<C turning angle>]\n" +
            "[, [<R/L>] [, [<A/B>]] [, [<N/F>]]] [, [<O/C>]]",
            "PL - Position Load\nSyntax: PL <position number (a)>, <position number (b)>",
            "PMR - Parameter Read\nSyntax: PMR [\"<parameter name>\"]",
            "PMW - Parameter Writing\nSyntax: PMW \"<parameter name>\", \"<parameter contents>\"",
            "PR - Position Read\nSyntax: PR [<position number>]",
            "PRN - Print\nSyntax: PRN <counter value> | <position coordinates> | \"<character string data>\"",
            "PT - Pallet\nSyntax: PT <pallet number>",
            "PW - Pulse Wait\nSyntax: PW <positioning pulse>",
            "PX - Position Exchange\nSyntax: PX <position number (a)>, <position number (b)>",
            "QN - Questio Number\nSyntax: QN [<program name>]",
            "RC - Repeat Cycle\nSyntax: RC <number of repeated cycles>",
            "RN - Run\nSyntax: RN [<start line number> [, <end line number> [, <program name>]]]",
            "RS - Reset\nSyntax: RS [<reset number>]",
            "RT - Return\nSyntax: RT [<line number>]",
            "SC - Set Counter\nSyntax: SC <counter number/character string number>,\n" +
            "[<counter set value/charaster string set value>]",
            "SD - Speed Define\nSyntax: <moving speed> [, <first order time constant>\n" +
            "[, <acceleration time>, <deceleration time> [, <CNT setting>]]]",
            "SF - Shift\nSyntax: SG <position number (a)>, <position number (b)>",
            "SM - If Smaller\nSyntax: SM <compared value/character string number>,\n" +
            "<branching line number>",
            "SP - Speed\nSyntax: SP <speed level> [, <H/L> [, <CNT setting>]]",
            "STR - Step Read\nSyntax: STR [<step number>]",
            "TB - Test Bit\nSyntax: TB [<+/->] <bit number>, <branching line number>",
            "TBD - Test Bit Direct\nSyntax: TBD [<+/->] <input bit number>, <branching line number>",
            "TI - Timer\nSyntax: TI <timer counter>",
            "TL - Tool\nSyntax: TL [<tool length>]",
            "VR - Version Read\nSyntax: VR",
            "WH - Where\nSyntax: WH",
            "WT - What Tool\nSyntax: WT",
            "XO - Exclusive Or\nSyntax: XO <operation data>"
        };


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
            CommandList.ItemsSource = commands;
            commandSyntax = new Dictionary<string, string>();
            for(int i = 0; i < commands.Length; i++)
            {
                commandSyntax.Add(commands[i], syntax[i]);
            }
            
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
                OnDataSend(new SendDataEventArgs(CommandTxtBox.Text));
        }

        //po wpisaniu części lub całości komendy w oknie edycji pokazuje ją na liście komend i wyświetla jej składnie 
        private void CommandTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CommandTxtBox.Text.Length <= 3)
            {
                string s = CommandTxtBox.Text.Split(" ")[0];
                if(s.All<Char>(Char.IsUpper))
                {
                    if(commandSyntax.ContainsKey(s))
                    {
                        CommandList.SelectedItem = s;
                        CommandList.ScrollIntoView(s);
                    }
                    else
                    {
                        foreach(string str in commandSyntax.Keys)
                        {
                            if(str.StartsWith(s))
                            {
                                CommandList.SelectedItem = str;
                                CommandList.ScrollIntoView(str);
                                break;
                            }
                        }

                    }

                }
                else
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
            }
        }
    }
}
