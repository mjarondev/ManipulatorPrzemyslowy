using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace ManipulatorPrzemyslowy
{
    //klasa posiadająca dane konieczne do rozpoczęcia komunikacji portem COM
    public class SendData
    {
        private string portName;
        private int baudRate, dataBits, sendTimeout, receiveTimeout;
        private Parity parity;
        private StopBits stopBits;
        private Handshake handshake;


        public int ReceiveTimeout
        {
            get
            {
                return receiveTimeout;
            }
            set
            {
                if (value > 30 || value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value, should be in range 1-30");
                }
                receiveTimeout = value;

            }
        }

        public int SendTimeout
        {
            get
            {
                return sendTimeout;
            }
            set
            {
                if (value > 30 || value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value, should be in range 1-30");
                }
                sendTimeout = value;
            }
        }

        public string PortName
        {
            get
            {
                foreach(string port in SerialPort.GetPortNames())
                {
                    if (port.Equals(portName))
                    {
                        return portName.ToString();
                    }
                }
                throw new ComPortNotActiveException("Selected COM port is not active");
            }
            set
            {
                if (value.ToString().StartsWith("COM"))
                {
                    portName = value.ToString();
                }
                else
                {
                    throw new ArgumentException("Invalid value, required a COM port name");
                }
            }
        }

        public int BaudRate
        {
            get
            {
                return baudRate;
            }
            set
            {
                if (!Int32.TryParse(value.ToString(), out baudRate))
                {
                    throw new ArgumentException("Invalid value, cannot convert to Int32");
                }

            }
        }

        public Parity PortParity
        {
            get
            {
                return parity;
            }
            set
            {
                parity = value;
            }
        }

        public int DataBits
        {
            get
            {
                return dataBits;
            }
            set
            {
                dataBits = value;
            }
        }

        public StopBits PortStopBits
        {
            get
            {
                return stopBits;
            }
            set
            {
                stopBits = value;
            }
        }

        public Handshake PortHandshake
        {
            get
            {
                return handshake;
            }
            set
            {
                handshake = value;
            }
        }

        public SendData()
        {
            setToDefault();
        }

        //ustawia domyślne ustawienia komunikacji portu COM
        public void setToDefault()
        {
            if (SerialPort.GetPortNames().Length < 1)
            {
                throw new InvalidOperationException("Nie znaleziono aktywnego portu COM");
            }
            portName = SerialPort.GetPortNames()[0];
            baudRate = 9600;
            dataBits = 8;
            sendTimeout = 5;
            receiveTimeout = 5;
            parity = Parity.None;
            stopBits = StopBits.One;
            handshake = Handshake.None;
        }


    }
}
