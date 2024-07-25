// Date: 2024-07-22
// Author: Narendra Kumar Jonnakuti
// Description: Controller handling updates to stock details by the shopkeeper.

using Microsoft.AspNetCore.Mvc;
using NK.Data;
using NK.Models;

namespace NK.Controllers
{
    // This is the ItemController class, responsible for handling item-related actions.
    public class ItemController : Controller
    {
        // Private field to hold the database context.
        private readonly GroceryStoreContext _context;

        // Constructor that initializes the controller with the given database context.
        public ItemController(GroceryStoreContext context)
        {
            _context = context;
        }

        // GET: Item/Create
        // This method handles the GET request for the item creation page.
        public IActionResult Create()
        {
            // Return the item creation view when the create page is accessed.
            return View();
        }

        // POST: Item/Create
        // This method handles the POST request for the item creation form submission.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemViewModel model)
        {
            // Check if the model state is valid.
            if (ModelState.IsValid)
            {
                // Create a new item entity from the view model.
                var item = new Item
                {
                    ItemName = model.ItemName,
                    ItemPrice = model.ItemPrice,
                    ItemQuantity = model.ItemQuantity,
                    ItemImageUrl = model.ItemImageUrl
                };

                // Add the new item to the database context and save changes.
                _context.Items.Add(item);
                _context.SaveChanges();

                // Redirect to the create page after successful item creation.
                return RedirectToAction("Create", "Item");
            }
            // Return the create view with the model to display validation errors.
            return View(model);
        }
    }
}
