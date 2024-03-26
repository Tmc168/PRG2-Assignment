using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    internal class Topping
    {
        public string Type { get; set; }
        
        public Topping() { }

        public Topping(string t) 
        {
            Type = t;
        }

        public override string ToString()
        {
            return Type + " + ";
        }
    }
}
