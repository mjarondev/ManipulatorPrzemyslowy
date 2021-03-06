using System;
using System.Collections.Generic;
using System.Text;

namespace ManipulatorPrzemyslowy
{
    //Klasa zdarzenia wywoływanego przy zamknięciu okna
    public class WindowClosedEventArgs : EventArgs
    {
        public WindowClosedEventArgs()
        {

        }
    }

    //Klasa zdarzenia przekazującego dane portów COM
    public class UpdateDataEventArgs : EventArgs
    {
        public readonly SendData data;
        public UpdateDataEventArgs(SendData receivedData)
        {
            data = receivedData;
        }
    }

    //Klasa zdarzenia przekazująca komendę do wysłania
    public class SendDataEventArgs : EventArgs
    {
        public readonly string command;
        public readonly string args;

        public SendDataEventArgs(string sendCommand)
        {
            command = sendCommand;
            args = null;
        }

        public SendDataEventArgs(string sendCommand, string sendArgs)
        {
            command = sendCommand;
            args = sendArgs;
        }
        
    }

}
