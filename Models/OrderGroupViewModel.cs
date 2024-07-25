// Date: 2024-07-13
// Author: Likitha Ratna Chakka
// Description: Represents a view Orders view model.

namespace NK.Models
{
    public class OrderGroupViewModel
    {
        public string CustomerName { get; set; }
        public DateTime PickupTime { get; set; }
        public List<OrderDetails> Items { get; set; }
    }
}
