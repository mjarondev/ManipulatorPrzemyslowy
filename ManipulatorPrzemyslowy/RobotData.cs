using System;
using System.Collections.Generic;
using System.Text;

namespace ManipulatorPrzemyslowy
{
    public class RobotData
    {
        //dane na temat robota z ramki otrzymanej po wysłaniu WH
        private string xCoord, yCoord, zCoord, aAngle, bAngle, cAngle, leftRight, aboveBelow, nonflipFlip, grip;

        public RobotData()
        {
            EmptyFrame();
        }

        //funkcja ustawiająca puste stringi
        public void EmptyFrame()
        {
            xCoord = "0.0";
            yCoord = "0.0";
            zCoord = "0.0";
            aAngle = "0.0";
            bAngle = "0.0";
            cAngle = "0.0";
            leftRight = "R";
            aboveBelow = "A";
            nonflipFlip = "N";
            grip = "O";
        }

        //funkcja dekodująca ramkę otrzymaną od robota po wysłaniu WH
        public void DecodeFrame(string frame)
        {
            string[] data = frame.Split(",");

            if (data.Length != 10)
                throw new InvalidValueException("Incorrect data format.");

            xCoord = data[0];
            yCoord = data[1];
            zCoord = data[2];
            aAngle = data[3];
            bAngle = data[4];
            cAngle = data[5];
            LeftRight = data[6];
            AboveBelow = data[7];
            NonflipFlip = data[8];
            Grip = data[9];
        }

        public string LeftRight
        {
            get
            {
                return leftRight;
            }
            set
            {
                if (!(value == "R" || value == "L"))
                    throw new InvalidValueException("Incorrect data format.");
                leftRight = value;
            }
        }

        public string AboveBelow
        {
            get
            {
                return aboveBelow;
            }
            set
            {
                if (!(value == "A" || value == "B"))
                    throw new InvalidValueException("Incorrect data format.");
                aboveBelow = value;
            }
        }

        public string NonflipFlip
        {
            get
            {
                return nonflipFlip;
            }
            set
            {
                if (!(value == "N" || value == "F"))
                    throw new InvalidValueException("Incorrect data format.");
                nonflipFlip = value;
            }
        }
    

        public string Grip
        {
            get
            {
                return grip;
            }
            set
            {
                if (!(value == "O" || value == "C"))
                    throw new InvalidValueException("Incorrect data format.");
                grip = value;
            }
        }

    }
}
