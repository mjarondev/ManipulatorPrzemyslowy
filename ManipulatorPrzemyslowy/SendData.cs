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

        //TODO: Nie wiem czy te ify konwertujące wartości są potrzebne

        public int ReceiveTimeout
        {
            get
            {
                return receiveTimeout;
            }
            set
            {
                if (!Int32.TryParse(value.ToString(), out receiveTimeout))
                {
                    throw new ArgumentException("Invalid value, cannot convert to Int32");
                }

                if (value > 30 || value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value, should be in range 1-30");
                }

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
                if (!Int32.TryParse(value.ToString(), out sendTimeout))
                {
                    throw new ArgumentException("Invalid value, cannot convert to Int32");
                }

                if (value > 30 || value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid value, should be in range 1-30");
                }
            }
        }

        public string PortName
        {
            get
            {
                //TODO: dodac sprawdzanie czy dany port jest aktywny podczas odczytu
                return portName.ToString();
            }
            set
            {
                if (value.ToString().StartsWith("COM"))
                {
                    portName = value.ToString();
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
                object temp;
                if (Enum.TryParse(typeof(Parity), value.ToString(), true, out temp))
                {
                    parity = (Parity)temp;
                }
                else
                {
                    throw new ArgumentException("Invalid value, cannot convert to Parity");
                }
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
                if (!Int32.TryParse(value.ToString(), out dataBits))
                {
                    throw new ArgumentException("Invalid value, cannot convert to Int32");
                }
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
                object temp;
                if (Enum.TryParse(typeof(StopBits), value.ToString(), true, out temp))
                {
                    stopBits = (StopBits)temp;
                }
                else
                {
                    throw new ArgumentException("Invalid value, cannot convert to StopBits");
                }
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
                object temp;
                if (Enum.TryParse(typeof(Handshake), value.ToString(), true, out temp))
                {
                    handshake = (Handshake)temp;
                }
                else
                {
                    throw new ArgumentException("Invalid value, cannot convert to Handshake");
                }
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
