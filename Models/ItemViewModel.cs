// Date: 2024-06-12
// Author: Narendra Kumar Jonnakuti
// Description: Represents a view model for storing Item data.

namespace NK.Models
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public string ItemImageUrl { get; set; }
    }
}
