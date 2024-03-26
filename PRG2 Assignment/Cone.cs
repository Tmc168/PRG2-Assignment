using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace PRG2_Assignment
{
    internal class Cone:IceCream 
    {
        public bool Dipped { get; set; }
        public Cone() { }
        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool d):base(o,s,f,t)
        {
            Dipped = d;
        }
        public override double CalculatePrice(List<List<string>> flavourList, List<List<string>> toppingList, List<List<string>> optionList)
        {
            double totalPrice = 0.0;

            foreach (var flavourInfo in flavourList)
            {
                string flavourName = flavourInfo[0];
                double flavourPrice = Convert.ToDouble(flavourInfo[1]);

                foreach (var selectedFlavour in Flavours)
                    if (flavourName == selectedFlavour.Type)
                    {
                        totalPrice += flavourPrice * selectedFlavour.Quantity;
                    }
            }

            // Calculate price based on selected toppings
            foreach (var toppingInfo in toppingList)
            {
                string toppingName = toppingInfo[0];
                double toppingPrice = Convert.ToDouble(toppingInfo[1]);

                foreach (var selectedTopping in Toppings)
                {
                    if (toppingName == selectedTopping.Type)
                    {
                        totalPrice += toppingPrice;
                    }
                }
            }

            // Calculate price based on selected options
            foreach (var option in optionList)
            {
                //The option price is in the fifth column of optionList
                string optionType = option[0];
                int optionScoops = Convert.ToInt32(option[1]);
                bool optionDipped = !string.IsNullOrWhiteSpace(option[2]) && Convert.ToBoolean(option[2]);
                double optionPrice = Convert.ToDouble(option[4]);
                if (optionType == Option && optionScoops == Scoops && optionDipped == Dipped)
                {
                    totalPrice += optionPrice;
                }

            }

            return totalPrice;
        }
        
    
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
            return base.ToString() + " | Dipped: " + Dipped;
        }
    } 
}
