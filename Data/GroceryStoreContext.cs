// Date: 2024-06-12
// Author: Narendra Kumar Jonnakuti
// Description: Dbcontext to coonect with the database.

using Microsoft.EntityFrameworkCore;
using NK.Models;

namespace NK.Data
{
    public class GroceryStoreContext : DbContext
    {
        public GroceryStoreContext(DbContextOptions<GroceryStoreContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }

    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public string ItemImageUrl { get; set; }
    }


}
