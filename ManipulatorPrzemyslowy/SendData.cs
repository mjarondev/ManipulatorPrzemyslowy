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
                if (receiveTimeout < 1)
                {
                    throw new ComPortInvalidValueException("Invalid receive timeout value.Check input data or COM port connection.");
                }
                return receiveTimeout;
            }
            set
            {
                if (value > 30 || value < 1)
                {
                    throw new ComPortInvalidValueException("Invalid receive timeout value, should be in range 1-30.");
                }
                receiveTimeout = value;

            }
        }

        public int SendTimeout
        {
            get
            {
                if (sendTimeout < 1)
                {
                    throw new ComPortInvalidValueException("Invalid send timeout value. Check input data or COM port connection.");
                }
                return sendTimeout;
            }
            set
            {
                if (value > 30 || value < 1)
                {
                    throw new ComPortInvalidValueException("Invalid send timeout value, should be in range 1-30.");
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
                throw new ComPortNotActiveException("Selected COM port is not active.");
            }
            set
            {
                if (value.ToString().StartsWith("COM"))
                {
                    portName = value.ToString();
                }
                else
                {
                    throw new ArgumentException("Invalid value, required a COM port name.");
                }
            }
        }

        public int BaudRate
        {
            get
            {
                if(baudRate <= 0)
                {
                    throw new ComPortInvalidValueException("Invalid baud rate value. Check input data or COM port connection.");
                }
                return baudRate;
            }
            set
            {
                if (!Int32.TryParse(value.ToString(), out baudRate))
                {
                    throw new ArgumentException("Invalid baud rate value, cannot convert to Int32.");
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
                if (dataBits <= 0)
                {
                    throw new ComPortInvalidValueException("Invalid data bits value. Check input data or COM port connection.");
                }
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
            SetToEmpty();
        }

        //ustawia domyślne ustawienia komunikacji portu COM
        public void SetToDefault()
        {
            if (SerialPort.GetPortNames().Length < 1)
            {
                SetToEmpty();
                throw new ComPortNotActiveException("No active port COM found.");
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

        public void SetToEmpty()
        {
            portName = "";
            baudRate = 0;
            dataBits = 0;
            sendTimeout = 0;
            receiveTimeout = 0;
            parity = Parity.None;
            stopBits = StopBits.None;
            handshake = Handshake.None;
        }


    }
}
