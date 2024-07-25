// Date: 2024-06-19
// Author: Narendra Kumar Jonnakuti
// Description: Controller handling customer-related actions such as signup and login.

using Microsoft.AspNetCore.Mvc;
using NK.Data;
using NK.Models;

namespace NK.Controllers
{
    // This is the CustomerController class, responsible for handling customer signup and login actions.
    public class CustomerController : Controller
    {
        // Private field to hold the database context.
        private readonly GroceryStoreContext _context;

        // Constructor that initializes the controller with the given database context.
        public CustomerController(GroceryStoreContext context)
        {
            _context = context;
        }

        // GET: Customer/Signup
        // This method handles the GET request for the signup page.
        public IActionResult Signup()
        {
            // Return the signup view when the signup page is accessed.
            return View();
        }

        // POST: Customer/Signup
        // This method handles the POST request for the signup form submission.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(CustomerViewModel model)
        {
            // Check if the model state is valid.
            if (ModelState.IsValid)
            {
                // Create a new customer entity from the view model.
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone
                };
                // Here you would hash the password before storing it.
                _context.Customers.Add(customer);
                _context.SaveChanges();
                // Redirect to the login page after successful signup.
                return RedirectToAction("Login");
            }
            // Return the signup view with the model to display validation errors.
            return View(model);
        }

        // GET: Customer/Login
        // This method handles the GET request for the login page.
        public IActionResult Login()
        {
            // Return the login view when the login page is accessed.
            return View();
        }

        // POST: Customer/Login
        // This method handles the POST request for the login form submission.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            // Check if the model state is valid.
            if (ModelState.IsValid)
            {
                // Find the customer by email.
                var customer = _context.Customers.SingleOrDefault(c => c.CustomerEmail == email);
                // If the customer exists and the password is verified, proceed with login logic.
                if (customer != null && VerifyPassword(customer, password)) // Implement VerifyPassword method
                {
                    // Implement login logic (e.g., setting session variables, redirecting to another page).
                    return RedirectToAction("Shop", "ItemGrid");
                }
                // If login fails, add an error message to the ModelState.
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            // Return the login view to display validation errors.
            return View();
        }

        // Method to verify the customer's password.
        private bool VerifyPassword(Customer customer, string password)
        {
            // Set the customer's name in the session (this should ideally be done after successful password verification).
            HttpContext.Session.SetString("CustomerName", customer.CustomerName.ToString());
            // Implement password verification logic here (e.g., comparing hashed passwords).
            return true; // Placeholder return value, replace with actual logic.
        }
    }
}
