// Date: 2024-06-28
// Author: Sruthi Mullaguri
// Description: Represents a view model for saving items to cart.

using NK.Data;

namespace NK.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public string SessionId { get; set; }
    }
}
