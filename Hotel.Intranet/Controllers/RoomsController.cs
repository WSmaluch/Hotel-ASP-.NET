using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Booking;

namespace Hotel.Intranet.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelContext _context;

        public RoomsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Room.Include(r => r.Type).Include(r => r.RoomStatus).Include(r => r.Facilities).ToListAsync();

            var d = await _context.Types.Include(t => t.Rooms).Include(r => r.Facilities).ToListAsync();


            var f = _context.Types;
            ViewBag.Types = d;

            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Room == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.Type)
                .Include(r => r.Facilities)
                .FirstOrDefaultAsync(m => m.IdRoom == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name");
            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRoom,TypeId,Number,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Room room, List<int> facilities)
        {
            //if (ModelState.IsValid)
            //{
            if (facilities != null)
            {
                // Dodaj wybrane udogodnienia do pokoju
                foreach (var facilityId in facilities)
                {
                    var facility = await _context.Facilities.FindAsync(facilityId);
                    if (facility != null)
                    {
                        room.Facilities.Add(facility);
                    }
                }
                //}
                room.StatusId = 9;
                room.AddedDate = DateTime.Now;
                room.IsActive = true;
                room.AddedBy = "Admin";
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name", room.TypeId);
            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");

            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Room == null)
            {
                return NotFound();
            }

            var room = await _context.Room
           .Include(r => r.Facilities)
           .FirstOrDefaultAsync(r => r.IdRoom == id);

            if (room == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name", room.TypeId);
            ViewBag.Statuses = new SelectList(_context.Status, "StatusId", "StatusName", room.StatusId);

            // Pobierz wszystkie dostępne udogodnienia
            var allFacilities = await _context.Facilities.ToListAsync();

            // Ustal, które z udogodnień są przypisane do danego pokoju
            var selectedFacilities = room.Facilities.ToList();

            // Przypisz wszystkie udogodnienia do ViewBag.Facilities z zaznaczonymi udogodnieniami dla danego pokoju
            ViewBag.Facilities = selectedFacilities;

            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRoom,TypeId,Number,StatusId,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Room room)
        {
            if (id != room.IdRoom)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.IdRoom))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Description", room.TypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Room == null)
            {
                return Problem("Entity set 'HotelContext.Room'  is null.");
            }
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                _context.Room.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Room == null)
            {
                return Problem("Entity set 'HotelContext.Room'  is null.");
            }
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                _context.Room.Remove(room);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
          return (_context.Room?.Any(e => e.IdRoom == id)).GetValueOrDefault();
        }

        // GET: Rooms/ChangeStatus/5
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Pobierz dostępne statusy z bazy danych i przekaż do widoku
            ViewBag.Statuses = await _context.Status.ToListAsync();

            return View(room);
        }

        // POST: Rooms/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int newStatusId)
        {
            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            room.StatusId = newStatusId;
            // Zapisz zmiany
            _context.Update(room);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditFacilities(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(t => t.Facilities)
                .FirstOrDefaultAsync(t => t.IdRoom == id);

            if (room == null)
            {
                return NotFound();
            }

            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");

            return View(room);
        }

		[HttpPost]
		public IActionResult AddFacilities(int roomId, List<int> facilityIds)
		{
			// Tutaj dodaj logikę dodawania udogodnień do pokoju (roomId)
			// Użyj facilityIds do określenia, które udogodnienia zostały wybrane

			// Przykładowa logika:
			var room = _context.Room.Include(r => r.Facilities).FirstOrDefault(r => r.IdRoom == roomId);

            room.Facilities.Clear();

			if (room != null)
			{
				foreach (var facilityId in facilityIds)
				{
					var facilityToAdd = _context.Facilities.FirstOrDefault(f => f.IdFacility == facilityId);
					if (facilityToAdd != null && !room.Facilities.Contains(facilityToAdd))
					{
						room.Facilities.Add(facilityToAdd);
					}
				}

				_context.SaveChanges();
			}

			// Przekieruj użytkownika z powrotem do widoku edycji pokoju lub innego, w zależności od potrzeb
			return RedirectToAction(nameof(Edit), new { id = roomId });
		}
	}
}
