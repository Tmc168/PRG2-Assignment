using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    internal class Customer
    {
        public string Name { get; set; }
        public int MemberID { get; set; }
        public DateTime DOB { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory;
        public PointCard Rewards { get; set; }

        public Customer() { }

        public Customer(string n, int m, DateTime d) 
        { 
            Name = n;
            MemberID = m;
            DOB = d;
            OrderHistory = new();
        }

        public Order MakeOrder()
        {
            CurrentOrder = new Order();
            OrderHistory.Add(CurrentOrder);
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            return DOB == DateTime.Today;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
