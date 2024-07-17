using Hotel.Data;
using Hotel.Intranet.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
            return View(); 
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Employee.FirstOrDefault(e => e.Login == username);
            
            var unhashedPasswordPassed = PasswordHasher.VerifyPassword(password, user.PasswordHash);


            if (username == user.Login && unhashedPasswordPassed)
            {
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login or password.";
                return View("LoginPage");
            }
        }
    }
}
