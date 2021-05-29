using System;
using System.Collections.Generic;
using System.Text;

namespace ManipulatorPrzemyslowy
{
    public class PositionAddCommunication
    {
        private ICheck check;

        public event EventHandler<SendDataEventArgs> DataSend;

        protected virtual void OnDataSend(SendDataEventArgs e)
        {
            DataSend?.Invoke(this, e);
        }

        public PositionAddCommunication(ICheck ch)
        {
            check = ch;
        }

        // wysyla pozycje do robota
        public void SendPosition(string[] arr)
        {
            throw new NotImplementedException();
        }


    }

    public interface ICheck
    {
        public bool CheckValuesCorrectness(string[] arr);
    }

    // sprawdza czy otrzymane dane są poprawne
    public class CheckValues : ICheck
    {
        public bool CheckValuesCorrectness(string[] arr)
        {
            throw new NotImplementedException();
        }

        public bool CheckNumberCorrectness(string s)
        {
            throw new NotImplementedException();
        }

        public bool CheckCharCorrectness(string c, int position)
        {
            throw new NotImplementedException();
        }
    }
}
