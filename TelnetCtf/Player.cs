using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelnetCtf
{
    public class Player
    {
        public Position Position { get; set; }

        public double Angle { get; set; }

        public double GunAngle { get; set; }

        public double FacePosition(Position targetPosition)
        {
            var angle = Math.Atan((targetPosition.Y - Position.Y) / (targetPosition.X - Position.X)) * (180 / Math.PI);
            angle = Math.Round(angle);
            var oldAngle = Angle;
            Angle = angle;
            return Angle - oldAngle;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
