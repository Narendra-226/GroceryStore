// Date: 2024-07-02
// Author: Likitha Ratna Chakka
// Description: Represents a view Orders view model.

namespace NK.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime PickupTime { get; set; }
        public string ItemName { get; set; }
        public int ItemCount { get; set; }
        public decimal ItemCost { get; set; }
    }
}
