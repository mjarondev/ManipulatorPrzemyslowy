using System;
using System.Collections.Generic;
using System.Text;

namespace ManipulatorPrzemyslowy
{
    public class ComPortNotActiveException : ApplicationException
    {
        public ComPortNotActiveException()
        {

        }
        public ComPortNotActiveException(string msg) : base(msg)
        {

        }
    }

    public class ComPortInvalidValueException : ApplicationException
    {
        public ComPortInvalidValueException()
        {

        }
        public ComPortInvalidValueException(string msg) : base(msg)
        {

        }
    }

    public class InvalidValueException : ApplicationException
    {
        public InvalidValueException()
        {

        }
        public InvalidValueException(string msg) : base(msg)
        {

        }
    }

}
