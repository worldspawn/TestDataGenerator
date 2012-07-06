using System;
using System.Linq;
using System.Collections.Generic;

namespace TestData.Profiles.Tests.TestClasses
{
    public class Order : TestClass
    {
        public Order()
        {
        }

        public Order(User orderedBy) : this()
        {
            OrderedBy = orderedBy;
        }

        public List<OrderItem> Items { get; set; }
        public User OrderedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public decimal Amount()
        {
            return Items.Sum(o => o.Quantity * o.Product.Amount);
        }
    }

    public class Product : TestClass
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}