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
}
