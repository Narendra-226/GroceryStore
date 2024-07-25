// Date: 2024-06-24
// Author: Narendra Kumar Jonnakuti
// Description: Represents a view model for place order.

using NK.Data;

namespace NK.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
