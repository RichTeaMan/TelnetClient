using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelnetCtf
{
    public class Position
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public Position()
        {

        }

        public Position(string positionLine)
        {
            try
            {
                // You are at position: 0.0 , 0.0 , 2.0
                var numberPart = positionLine.Split(':')[1];
                var numbers = numberPart.Split(',');

                X = double.Parse(numbers[0].Trim());
                Y = double.Parse(numbers[1].Trim());
                Z = double.Parse(numbers[2].Trim());
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
