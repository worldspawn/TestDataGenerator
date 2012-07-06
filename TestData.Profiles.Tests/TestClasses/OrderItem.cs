namespace TestData.Profiles.Tests.TestClasses
{
    public class OrderItem : TestClass
    {
        public OrderItem()
        {
        }

        public OrderItem(Order order): this()
        {
            Order = order;
        }

        public Order Order { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}