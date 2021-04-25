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

        //funkcja dekodująca ramkę otrzymaną od robota po wysłaniu WH na dane
        public void DecodeFrame(string frame)
        {
            //dodac wyjatek
            string[] data = frame.Split(",");
            xCoord = data[0];
            yCoord = data[1];
            zCoord = data[2];
            aAngle = data[3];
            bAngle = data[4];
            cAngle = data[5];
            leftRight = data[6];
            aboveBelow = data[7];
            nonflipFlip = data[8];
            grip = data[9];
        }

    
        public string Grip
        {
            get
            {
                return grip;
            }
            set
            {
                //dodac wyjatek gdy wartosc bedzie nna niz C lub O
                if (value == "O" || value == "C")
                    grip = value;
            }
        }

    }
}
