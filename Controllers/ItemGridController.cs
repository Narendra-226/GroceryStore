// Date: 2024-07-22
// Authors: Narendra Kumar Jonnakuti, ShyamRahul Chennupati, Likitha Ratna Chakka, Sruthi Mullaguri
// Description: Controller handling all functions related to the shopping page, placing orders, and order view page.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NK.Data;
using NK.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NK.Controllers
{
    // This is the ItemGridController class, responsible for handling item-related actions in the shopping page.
    public class ItemGridController : Controller
    {
        // Private field to hold the database context.
        private readonly GroceryStoreContext _context;

        // Constructor that initializes the controller with the given database context.
        public ItemGridController(GroceryStoreContext context)
        {
            _context = context;
        }

        // GET: ItemGrid/Shop
        // This method handles the GET request for the shop page, including item search functionality.
        public async Task<IActionResult> Shop(string searchString)
        {
            // Ensure a consistent session ID by setting a session variable if it does not already exist.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionId")))
            {
                HttpContext.Session.SetString("SessionId", Guid.NewGuid().ToString());
            }

            // Query to retrieve items from the database.
            var items = from i in _context.Items select i;

            // Filter items based on the search string if it is not null or empty.
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ItemName.Contains(searchString));
            }

            // Project items into ItemViewModel objects and convert to a list asynchronously.
            var itemViewModels = await items.Select(item => new ItemViewModel
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemPrice = item.ItemPrice,
                ItemQuantity = item.ItemQuantity,
                ItemImageUrl = item.ItemImageUrl
            }).ToListAsync();

            // Return the shop view with the item view models.
            return View(itemViewModels);
        }

        // POST: ItemGrid/AddToCart
        // This method handles the POST request to add an item to the cart.
        [HttpPost]
        public async Task<IActionResult> AddToCart(int itemId, int quantity)
        {
            // Retrieve the item from the database by its ID.
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                // Return a 404 Not Found response if the item does not exist.
                return NotFound();
            }

            // Retrieve the session ID from the session or set it if it does not already exist.
            var sessionId = HttpContext.Session.GetString("SessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = HttpContext.Session.Id;
                HttpContext.Session.SetString("SessionId", sessionId);
            }

            // Find an existing cart item for the current session and item ID.
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ItemId == itemId && ci.SessionId == sessionId);

            if (cartItem == null)
            {
                // If the cart item does not exist, create a new one and add it to the context.
                cartItem = new CartItem
                {
                    ItemId = itemId,
                    Quantity = quantity,
                    SessionId = sessionId
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                // If the cart item exists, update its quantity.
                cartItem.Quantity += quantity;
            }

            // Save changes to the database.
            await _context.SaveChangesAsync();

            // Redirect to the shop page after adding the item to the cart.
            return RedirectToAction("Shop");
        }

        // GET: ItemGrid/Cart
        // This method handles the GET request for the cart page.
        public async Task<IActionResult> Cart()
        {
            // Retrieve the session ID from the session or set it if it does not already exist.
            var sessionId = HttpContext.Session.GetString("SessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = HttpContext.Session.Id;
                HttpContext.Session.SetString("SessionId", sessionId);
            }

            // Query to retrieve cart items for the current session and include related item details.
            var cartItems = await _context.CartItems
                .Include(ci => ci.Item)
                .Where(ci => ci.SessionId == sessionId)
                .ToListAsync();

            // Return the cart view with the cart items.
            return View(cartItems);
        }

        // POST: ItemGrid/Checkout
        // This method handles the POST request to checkout the items in the cart.
        [HttpPost]
        public async Task<IActionResult> Checkout(DateTime pickupTime)
        {
            // Retrieve the session ID from the session or set it if it does not already exist.
            var sessionId = HttpContext.Session.GetString("SessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = HttpContext.Session.Id;
                HttpContext.Session.SetString("SessionId", sessionId);
            }

            // Query to retrieve cart items for the current session and include related item details.
            var cartItems = await _context.CartItems
                .Include(ci => ci.Item)
                .Where(ci => ci.SessionId == sessionId)
                .ToListAsync();

            // Retrieve the customer name from the session.
            var customerName = HttpContext.Session.GetString("CustomerName");

            // Redirect to the cart page if there are no items in the cart.
            if (!cartItems.Any())
            {
                return RedirectToAction("Cart");
            }

            // Loop through each cart item and create corresponding order details.
            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    CustomerName = customerName,
                    PickupTime = pickupTime,
                    ItemName = cartItem.Item.ItemName,
                    ItemCount = cartItem.Quantity,
                    ItemCost = cartItem.Item.ItemPrice
                };

                // Add the order detail to the context.
                _context.OrderDetails.Add(orderDetail);
            }

            // Remove the cart items from the context.
            _context.CartItems.RemoveRange(cartItems);

            // Save changes to the database.
            await _context.SaveChangesAsync();

            // Return the checkout view after successful checkout.
            return View();
        }

        // GET: ItemGrid/Orders
        // This method handles the GET request for the orders page.
        public async Task<IActionResult> Orders()
        {
            // Query to retrieve order details from the database, ordered by pickup time and customer name.
            var orders = await _context.OrderDetails
                .OrderBy(od => od.PickupTime)
                .ThenBy(od => od.CustomerName)
                .ToListAsync();

            // Group the orders by customer name and pickup time, and project into OrderGroupViewModel objects.
            var groupedOrders = orders
                .GroupBy(o => new { o.CustomerName, o.PickupTime })
                .Select(g => new OrderGroupViewModel
                {
                    CustomerName = g.Key.CustomerName,
                    PickupTime = g.Key.PickupTime,
                    Items = g.ToList()
                })
                .ToList();

            // Return the orders view with the grouped orders.
            return View(groupedOrders);
        }
    }
}
