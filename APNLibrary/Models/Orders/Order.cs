namespace APNLibrary.Models.Orders
{
    public class Order
    {
        public string? OrderId { get; set; }
        public IEnumerable<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
