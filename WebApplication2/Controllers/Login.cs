using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.ModelsEAD;

namespace WebApplication2.Controllers
{
    public class Login : Controller
    {
        public IActionResult Signup()
        {
            return View("Signup");
        }

        [HttpPost]
        public IActionResult SaveData(string username, string password, string email, string confirm_password)
        {
            using (Bakery2Context db = new Bakery2Context())
            {
                // Check if the email already exists
                var existingUser = db.Signups.FirstOrDefault(u => u.Email == email);

                if (password != confirm_password)
                {
                    ViewBag.Message = "Please Confirm your Password!";
                    return View("Signup"); // Reload the signup view with the alert
                }

                if (existingUser != null)
                {
                    // If email exists, return an alert message
                    ViewBag.Message = "User already exists!";
                    return View("Signup"); // Reload the signup view with the alert
                }
                else
                {
                    // Add the new user to the database
                    Signup newUser = new Signup { Name = username, Email = email, Password = password };
                    db.Signups.Add(newUser);
                    db.SaveChanges();

                    // Show a success message
                    ViewBag.Message = "Signup successful!";
                    return View("Login"); // Redirect to the login view
                }
            }
        }
        public IActionResult login()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Authenticate(string username, string password)
        {
            using (Bakery2Context db = new Bakery2Context())
            {
                // Find the user by username
                var user = db.Signups.FirstOrDefault(u => u.Name == username);
                if (user == null)
                {
                    // User not found
                    ViewBag.Message = "Username not found!";
                    return View("Login"); // Reload the login page with the message
                }

                if (user.Password != password)
                {
                    // Incorrect password
                    ViewBag.Message = "Incorrect password!";
                    return View("Login"); // Reload the login page with the message
                }

                // Successful login
                HttpContext.Session.SetString("Username", username); // Store username in session
                HttpContext.Session.SetString("Name", user.Name); // Optional: Store user's full name in session

                // Redirect to the displaycake action in the Menu controller
                return RedirectToAction("DisplayCake", "Menu");
            }
        }


        public IActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verify2(string email)
        {
            using (Bakery2Context db = new Bakery2Context())
            {
                // Check if the email exists in the database
                var user = db.Signups.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    ViewBag.Message = "Email does not exist!";
                    return View("Verify");
                }

                // Pass the email to the ChangePassword view
                var model = new { Email = email };
                return View("ChangePassword", model);
            }
        }

        [HttpPost]
        public IActionResult Changing(string email, string password, string confirm_password)
        {
            if (password != confirm_password)
            {
                ViewBag.Message = "Please Confirm your Password!";
                return View("ChangePassword", new { Email = email }); // Reload the change password view with the alert
            }

            using (Bakery2Context db = new Bakery2Context())
            {
                var user = db.Signups.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    user.Password = password; // Update the password
                    db.SaveChanges();

                    ViewBag.Message = "Your password has been successfully changed.";
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewBag.Message = "User not found.";
                    return View("ChangePassword", new { Email = email });
                }
            }
        }
        public IActionResult Profile()
        {
            // Get the username from the session
            var username = HttpContext.Session.GetString("Username");

            // If no username exists in the session, redirect to the login page
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("login", "Login");
            }

            // Using the Bakery2Context to query the database
            using (Bakery2Context db = new Bakery2Context())
            {
                // Find the user by username
                var user = db.Signups.FirstOrDefault(u => u.Name == username);

                // If user is not found, return a custom error view
                if (user == null)
                {
                    // You can create a custom error page for the NotFound scenario
                    return View("Error", new ErrorViewModel { RequestId = "User not found!" });
                }

                // Return the profile view with the user information
                return View(user);
            }
        }


        // Action to edit the profile
        [HttpGet]
    public IActionResult EditProfile()
    {
        // Get the username from the session
        var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("login");
            }
            else
            {

                using (Bakery2Context db = new Bakery2Context())
                {
                    var user = db.Signups.FirstOrDefault(u => u.Name == username);

                    if (user == null)
                    {
                        return NotFound();
                    }


                    return View(user); // Pass user info to the profile view
                }
            }
        }

        // Action to handle editing and saving customer information
        [HttpPost]
       
        public IActionResult UpdateProfile(Signup updatedUser)
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            using (Bakery2Context db = new Bakery2Context())
            {
                var user = db.Signups.FirstOrDefault(u => u.Name == username);

                if (user == null)
                {
                    return NotFound();
                }

                // Update user data
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                user.Password = updatedUser.Password;

                db.SaveChanges(); // Save changes to the database
            }

            // Redirect to Profile page or Show Success message
            return RedirectToAction("Profile");
        }


        // Action to log out
        public IActionResult Logout()
    {
        // Remove the session data
        HttpContext.Session.Clear();

        // Redirect to login page
        return RedirectToAction("login", "Login");
    }
    }
}
