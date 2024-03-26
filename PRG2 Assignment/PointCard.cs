using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    internal class PointCard
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }

        public PointCard() { }

        public PointCard(int p, int pc)
        {
            Points = p;
            PunchCard = pc;
        }

        public void AddPoints(int points)
        {
            Points += points;
        }

        public void RedeemPoints(int points) 
        { 
            Points -= points;
        }

        public void Punch()
        {
            PunchCard++;
            if (PunchCard >= 11)
            {
                PunchCard = 10;
            }
        }

        public override string ToString()
        {
            return "";
        }
    }
}
