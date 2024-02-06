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
    public class RepairTasksController : Controller
    {
        private readonly HotelContext _context;

        public RepairTasksController(HotelContext context)
        {
            _context = context;
        }

        // GET: RepairTasks
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.RepairTask.Include(r => r.Employee).Include(r => r.RepairStatus).Include(r => r.Room);

            ViewBag.Employee = _context.Employee.ToList();

            return View(await hotelContext.ToListAsync());
        }

        // GET: RepairTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RepairTask == null)
            {
                return NotFound();
            }

            var repairTask = await _context.RepairTask
                .Include(r => r.Employee)
                .Include(r => r.RepairStatus)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repairTask == null)
            {
                return NotFound();
            }

            return View(repairTask);
        }

        // GET: RepairTasks/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "FirstName");
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName");
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "IdRoom");
            return View();
        }

        // POST: RepairTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScheduledDate,Status,RoomId,EmployeeId,Note,StatusId")] RepairTask repairTask)
        {
            //if (ModelState.IsValid)
            //{
            repairTask.StatusId = 12;
                _context.Room.Find(repairTask.RoomId).StatusId = 11;
                _context.Add(repairTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "FirstName", repairTask.EmployeeId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", repairTask.StatusId);
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "IdRoom", repairTask.RoomId);
            return View(repairTask);
        }

        // GET: RepairTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RepairTask == null)
            {
                return NotFound();
            }

            var repairTask = await _context.RepairTask.FindAsync(id);
            if (repairTask == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "FirstName", repairTask.EmployeeId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", repairTask.StatusId);
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "IdRoom", repairTask.RoomId);
            return View(repairTask);
        }

        // POST: RepairTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScheduledDate,Status,RoomId,EmployeeId,Note,StatusId")] RepairTask repairTask)
        {
            if (id != repairTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Znajdź pokój o danym RoomId
                    var room = _context.Room.Find(repairTask.RoomId);
                    if (repairTask.StatusId == 9)
                    {
                        room.StatusId = 9;
                    }

                    _context.Update(repairTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairTaskExists(repairTask.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "FirstName", repairTask.EmployeeId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", repairTask.StatusId);
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "IdRoom", repairTask.RoomId);
            return View(repairTask);
        }

        // GET: RepairTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RepairTask == null)
            {
                return NotFound();
            }

            var repairTask = await _context.RepairTask
                .Include(r => r.Employee)
                .Include(r => r.RepairStatus)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repairTask == null)
            {
                return NotFound();
            }

            return View(repairTask);
        }

        // POST: RepairTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RepairTask == null)
            {
                return Problem("Entity set 'HotelContext.RepairTask'  is null.");
            }
            var repairTask = await _context.RepairTask.FindAsync(id);
            if (repairTask != null)
            {
                _context.RepairTask.Remove(repairTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepairTaskExists(int id)
        {
          return (_context.RepairTask?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: RepairTask/ChangeStatus/5
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairTask = await _context.RepairTask.FindAsync(id);
            if (repairTask == null)
            {
                return NotFound();
            }

            // Pobierz dostępne statusy z bazy danych i przekaż do widoku
            ViewBag.Statuses = await _context.Status.Where(s => s.StatusId == 12 || s.StatusId == 9).ToListAsync();

            return View(repairTask);
        }

        // POST: RepairTask/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int newStatusId)
        {
            var repairTask = await _context.RepairTask.FindAsync(id);
            if (repairTask == null)
            {
                return NotFound();
            }

            // Zmień status rezerwacji na nowy
            repairTask.StatusId = newStatusId;
            if (newStatusId == 12)
            {
                _context.Room.Find(repairTask.RoomId).StatusId = 12;
            }
            else if (newStatusId == 9)
            {
                _context.Room.Find(repairTask.RoomId).StatusId = 9;
            }

            // Zapisz zmiany
            _context.Update(repairTask);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
