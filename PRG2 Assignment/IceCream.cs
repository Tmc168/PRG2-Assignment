using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PRG2_Assignment
{
    internal abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; }
        public List<Topping> Toppings { get; set; }

        public IceCream() { }

        public IceCream(string o, int s, List<Flavour> f, List<Topping> t)
        {
            Option = o;
            Scoops = s;
            Flavours = f;
            Toppings = t;
        }

        public abstract double CalculatePrice(List<List<string>> flavourList, List<List<string>> toppingList, List<List<string>> optionList);

        public override string ToString()
        {
            string flavours = "";
            foreach (var flavour in Flavours)
            {
                flavours += flavour.ToString();
            }

            string toppings = "";
            foreach (var topping in Toppings)
            {
                toppings += topping.ToString();
            }
            return "Option: " + Option + " | Scoops: " + Scoops + " | Flavours: " + flavours + " | Toppings: " + toppings;
        }
    }
}
