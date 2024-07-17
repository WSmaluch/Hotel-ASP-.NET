using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Intranet.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelContext _context;

        public AccountController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            return View(); // Zwraca widok formularza logowania
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Employee.FirstOrDefault(e => e.FirstName == username && e.LastName == password);

            if (username == user.FirstName && password == user.LastName)
            {
                // Jeśli uwierzytelnienie się powiedzie, możesz przekierować użytkownika do innej strony
                return RedirectToAction("Index", "Home"); // Przekierowanie na stronę główną
            }
            else
            {
                // Jeśli uwierzytelnienie się nie powiedzie, możesz przekazać komunikat błędu z powrotem do widoku LoginPage
                ViewBag.ErrorMessage = "Nieprawidłowy login lub hasło.";
                return View("LoginPage"); // Zwraca widok formularza logowania z komunikatem błędu
            }
        }
    }
}
