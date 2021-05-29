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
            if (check.CheckValuesCorrectness(arr))
            {
                OnDataSend(new SendDataEventArgs("PD " + string.Join<string>(",", arr)));
            }
            else
            {
                throw new ArgumentException();
            }
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
            if (arr.Length != 11)
                throw new ArgumentException();

            bool ret = true;
            for (int i = 0; i < 7; i++)
                ret &= CheckNumberCorrectness(arr[i]);
            for (int i = 7; i < 11; i++)
                ret &= CheckCharCorrectness(arr[i], i);

            return ret;

        }

        public bool CheckNumberCorrectness(string s)
        {
            double i;
            return double.TryParse(s, out i);
        }

        public bool CheckCharCorrectness(string s, int position)
        {
            bool CheckChar(string s, int position, string firstLetter, string secondLetter, int checkPosition)
            {
                if (position == checkPosition && (s == firstLetter || s == secondLetter))
                    return true;
                return false;
            }

            return CheckChar(s, position, "R", "L", 7) ||
                CheckChar(s, position, "B", "A", 8) ||
                CheckChar(s, position, "F", "N", 9) ||
                CheckChar(s, position, "O", "C", 10);
        }
    }
}
