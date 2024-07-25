// Date: 2024-07-15
// Author: Likitha Ratna Chakka
// Description: Controller handling admin login.

using Microsoft.AspNetCore.Mvc;
using NK.Models;

namespace NK.Controllers
{
    // This is the AdminController class, responsible for handling admin login actions.
    public class AdminController : Controller
    {
        // This method handles the GET request for the login page.
        [HttpGet]
        public IActionResult Login()
        {
            // Return the login view when the login page is accessed.
            return View();
        }

        // This method handles the POST request for the login form submission.
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Validate the provided credentials.
            if (model.Username == "admin" && model.Password == "12345")
            {
                // If credentials are valid, redirect to the 'Create' action in the 'Item' controller.
                return RedirectToAction("Create", "Item");
            }

            // If credentials are invalid, add an error message to the ModelState.
            ModelState.AddModelError("", "Invalid username or password.");
            // Return the login view with the model to display the error message.
            return View(model);
        }
    }
}
