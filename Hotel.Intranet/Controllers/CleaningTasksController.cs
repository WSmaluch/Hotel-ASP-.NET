using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Desktop;

namespace Hotel.Intranet.Controllers
{
    public class CleaningTasksController : Controller
    {
        private readonly HotelContext _context;

        public CleaningTasksController(HotelContext context)
        {
            _context = context;
        }

        // GET: CleaningTasks
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.CleaningTask.Include(c => c.ReservationStatus).Include(c => c.Room);
            ViewBag.Status = _context.Status.ToList();

            return View(await hotelContext.ToListAsync());
        }

        // GET: CleaningTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CleaningTask == null)
            {
                return NotFound();
            }

            var cleaningTask = await _context.CleaningTask
                .Include(c => c.ReservationStatus)
                .Include(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);

            
            if (cleaningTask == null)
            {
                return NotFound();
            }

            return View(cleaningTask);
        }

        // GET: CleaningTasks/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName");
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "IdRoom");
            return View();
        }

        // POST: CleaningTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScheduledDate,RoomId,StatusId")] CleaningTask cleaningTask)
        {
            //if (ModelState.IsValid)
            //{
                cleaningTask.StatusId =8;
                _context.Room.Find(cleaningTask.RoomId).StatusId = 8;
                _context.Add(cleaningTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", cleaningTask.StatusId);
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "IdRoom", cleaningTask.RoomId);
            return View(cleaningTask);
        }

        // GET: CleaningTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CleaningTask == null)
            {
                return NotFound();
            }

            var cleaningTask = await _context.CleaningTask.FindAsync(id);
            if (cleaningTask == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", cleaningTask.StatusId);
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "IdRoom", cleaningTask.RoomId);
            return View(cleaningTask);
        }

        // POST: CleaningTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScheduledDate,RoomId,StatusId")] CleaningTask cleaningTask)
        {
            if (id != cleaningTask.Id)
            {
                return NotFound();
            }

            try
            {
                // Sprawdź, czy cleaning task ma StatusId równy 7
                if (cleaningTask.StatusId == 7)
                {
                    // Znajdź pokój o danym RoomId
                    var room = _context.Room.Find(cleaningTask.RoomId);
                    if (room != null)
                    {
                        // Sprawdź, czy istnieje rezerwacja dla tego pokoju
                        var reservationExists = _context.Reservations.Any(r =>
                        r.RoomId == room.IdRoom &&
                        r.IsActive &&
                        DateTime.Today >= r.CheckIn.Date &&
                        DateTime.Today <= r.CheckOut.Date);
                        if (reservationExists)
                        {
                            // Jeżeli istnieje rezerwacja, zmień StatusId pokoju na 3
                            room.StatusId = 3;
                        }
                        else
                        {
                            // Jeżeli nie istnieje rezerwacja, zmień StatusId pokoju na 7
                            room.StatusId = 7;
                        }
                    }
                }

                _context.Update(cleaningTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CleaningTaskExists(cleaningTask.Id))
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



        // GET: CleaningTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CleaningTask == null)
            {
                return NotFound();
            }

            var cleaningTask = await _context.CleaningTask
                .Include(c => c.ReservationStatus)
                .Include(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cleaningTask == null)
            {
                return NotFound();
            }

            return View(cleaningTask);
        }

        // POST: CleaningTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CleaningTask == null)
            {
                return Problem("Entity set 'HotelContext.CleaningTask'  is null.");
            }
            var cleaningTask = await _context.CleaningTask.FindAsync(id);
            if (cleaningTask != null)
            {
                _context.CleaningTask.Remove(cleaningTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CleaningTaskExists(int id)
        {
          return (_context.CleaningTask?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: CleaningTask/ChangeStatus/5
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cleaningTask = await _context.CleaningTask.FindAsync(id);
            if (cleaningTask == null)
            {
                return NotFound();
            }

            // Pobierz dostępne statusy z bazy danych i przekaż do widoku
            ViewBag.Statuses = await _context.Status.Where(s => s.StatusId == 8 || s.StatusId == 7).ToListAsync();

            return View(cleaningTask);
        }

        // POST: CleaningTask/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int newStatusId)
        {
            var cleaningTask = await _context.CleaningTask.FindAsync(id);
            if (cleaningTask == null)
            {
                return NotFound();
            }

            // Zmień status rezerwacji na nowy
            cleaningTask.StatusId = newStatusId;
            if (newStatusId == 7)
            {
                _context.Room.Find(cleaningTask.RoomId).StatusId = 9;
            }
            else if (newStatusId == 8)
            {
                _context.Room.Find(cleaningTask.RoomId).StatusId = 8;
            }

            // Zapisz zmiany
            _context.Update(cleaningTask);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
