using Microsoft.AspNetCore.Mvc;
using WebApplication2.ModelsEAD;

namespace WebApplication2.Controllers
{
    public class MenuController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name, int price, string description, IFormFile image, int quantity)
        {
            // Manual validation logic
            if (string.IsNullOrWhiteSpace(name) || price <= 0 || string.IsNullOrWhiteSpace(description) || quantity <= 0 || image == null)
            {
                ViewBag.ErrorMessage = "All fields are required and must be valid.";
                return View();
            }

            byte[] imageBytes = null;

            // Convert the uploaded image to a byte array
            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Create a new Cake object
            var newCake = new Cake
            {
                Name = name,
                Price = price,
                Description = description,
                Image = imageBytes,
                Quantity = quantity
            };

            // Create an instance of DbContext (Bakery2Context) manually and save the cake
            using (var dbContext = new Bakery2Context())
            {
                dbContext.Cakes.Add(newCake);
                dbContext.SaveChanges();
            }

            // Redirect to the menu index or another page after successful addition
            return View();
        }

        public IActionResult displaycake()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {

                return RedirectToAction("login","Login");
            }
            else
            {
                // Create an instance of Bakery2Context manually
                using (var dbContext = new Bakery2Context())
                {
                    // Retrieve all cakes from the database
                    var cakes = dbContext.Cakes.ToList();

                    // Return the DisplayCake view with cakes data
                    return View(cakes);
                }
            }
            
        }

       
        public IActionResult Cart()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {

                return RedirectToAction("login", "Login");
            }
            else
            {
                return View();
            }
           
        }
        public IActionResult Bill()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {

                return RedirectToAction("login", "Login");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Orders()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                // If not logged in, redirect to the login page
                return RedirectToAction("Login","login");
            }
            else
            {
                // If logged in, redirect to the cart page
                return RedirectToAction("Cart");
            }
        }
    }
}
