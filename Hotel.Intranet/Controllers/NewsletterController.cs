using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Newsletter;
using System.Net.Mail;
using System.Net;

namespace Hotel.Intranet.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;

        public NewsletterController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Newsletter
        public async Task<IActionResult> Index()
        {
            ViewBag.NewsletterConfigOptions = _context.NewsletterConfig.Select(nc => new {
                nc.Id,
                nc.Title
            }).ToList();

            return _context.Newsletter != null ? 
                          View(await _context.Newsletter.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Newsletter'  is null.");
        }

        // GET: Newsletter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Newsletter == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletter
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsletter == null)
            {
                return NotFound();
            }

            return View(newsletter);
        }

        // GET: Newsletter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Newsletter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,SubscribedOn,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Newsletter newsletter)
        {
                newsletter.SubscribedOn = DateTime.Now;
                newsletter.IsActive = true;
                newsletter.AddedBy = "Admin";
                _context.Add(newsletter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Newsletter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Newsletter == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletter.FindAsync(id);
            if (newsletter == null)
            {
                return NotFound();
            }
            return View(newsletter);
        }

        // POST: Newsletter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,SubscribedOn,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Newsletter newsletter)
        {
            if (id != newsletter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsletter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsletterExists(newsletter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newsletter);
        }

        // GET: Newsletter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Newsletter == null)
            {
                return Problem("Entity set 'HotelContext.Newsletter'  is null.");
            }
            var newsletter = await _context.Newsletter.FindAsync(id);
            if (newsletter != null)
            {
                _context.Newsletter.Remove(newsletter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Newsletter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Newsletter == null)
            {
                return Problem("Entity set 'HotelContext.Newsletter'  is null.");
            }
            var newsletter = await _context.Newsletter.FindAsync(id);
            if (newsletter != null)
            {
                _context.Newsletter.Remove(newsletter);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsletterExists(int id)
        {
          return (_context.Newsletter?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region Email
        [HttpPost]
        public IActionResult SendNewsletterEmail(int versionId)
        {
            try
            {
                SendEmail(versionId); 

                return Ok();
            }
            catch (Exception ex)
            {
                // Log an exception or return the appropriate error status
                return StatusCode(500, "Error sending newsletter: " + ex.Message);
            }
        }
        private async Task SendEmail(int versionId)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var smtpClient = new SmtpClient(smtpSettings["SmtpServer"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
            };

            var emails = _context.Newsletter.Select(n => n.Email).ToList();
            var content = _context.NewsletterConfig.FirstOrDefault(nc => nc.Id == versionId)?.Content;

            foreach (var email in emails)
            {
                var message = new MailMessage
                {
                    From = new MailAddress(smtpSettings["Username"]),
                    Subject = "Newsletter",
                    IsBodyHtml = true,
                    Body = content,
                };

                message.To.Add(email);

                await smtpClient.SendMailAsync(message);
            }
        }

        #endregion
    }
}
