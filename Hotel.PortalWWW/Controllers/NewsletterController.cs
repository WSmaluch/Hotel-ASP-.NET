using Hotel.Data;
using Hotel.Data.Data.CMS.Newsletter;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.PortalWWW.Controllers
{
    public class NewsletterController : BaseController
    {

        public NewsletterController(HotelContext context) : base(context)
        {
        }

        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            try
            {
                // Validate and process the email subscription
                if (string.IsNullOrEmpty(email))
                {
                    ViewBag.Message = "Please provide an email address.";
                }
                else if (IsEmailExist(email))
                {
                    ViewBag.Message = "This email address is already subscribed.";
                }
                else
                {
                    SaveEmailToNewsletter(email);
                    ViewBag.Message = "Thank you for subscribing!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Index", "Home");
        }

        private void SaveEmailToNewsletter(string email)
        {
            var newsletterSubscription = new Newsletter
            {
                Email = email,
                SubscribedOn = DateTime.Now
            };

            _context.Newsletter.Add(newsletterSubscription);
            _context.SaveChanges();
        }

        private bool IsEmailExist(string email)
        {
            return _context.Newsletter.Any(n => n.Email == email);
        }

    }

}
