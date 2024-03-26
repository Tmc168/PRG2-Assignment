
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    internal class Order
    {
        public int ID { get; set; }  
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList;

        public Order() { }

        public Order(int i, DateTime t)
        {
            ID = i;
            TimeReceived = t;
            IceCreamList = new();
        }

        public void ModifyIceCream(int index, string option, int scoops, bool dipped, string waffleFlavour, List<Flavour> flavours, List<Topping> toppings)
        {   
            // Ice cream to modify

            if (option.ToLower() == "waffle")
            {
                IceCreamList[index] = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
            } else if (option.ToLower() == "cone")
            {
                IceCreamList[index] = new Cone(option, scoops, flavours, toppings, dipped);
            } else if (option.ToLower() == "cup")
            {
                IceCreamList[index] = new Cup(option, scoops, flavours, toppings);
            }

            
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int index)
        {
            IceCreamList.RemoveAt(index);
        }

        public double CalculateTotal(List<List<string>> flavourList, List<List<string>> toppingList, List<List<string>> optionList)
        {
            double total = 0;
            foreach(IceCream iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice(flavourList,toppingList, optionList);
            }
            return total;
        }

        public override string ToString()
        {
            string iceCreams = "\t";
            foreach(IceCream ic in IceCreamList)
            {
                iceCreams += ic + "\n\t";
            }
            return "[ID: " + ID + " Time Received: " + TimeReceived + "\n" + iceCreams + "]";
        }
    }

}
