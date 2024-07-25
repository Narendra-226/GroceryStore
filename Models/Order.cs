// Date: 2024-06-29
// Author: Sruthi Mullaguri
// Description: Represents a view model for Orders to cart.

using NK.Data;

namespace NK.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
