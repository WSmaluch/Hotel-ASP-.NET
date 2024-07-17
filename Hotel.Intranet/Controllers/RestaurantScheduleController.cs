using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Restaurant;

namespace Hotel.Intranet.Controllers
{
    public class RestaurantScheduleController : Controller
    {
        private readonly HotelContext _context;

        public RestaurantScheduleController(HotelContext context)
        {
            _context = context;
        }

        // GET: RestaurantSchedule
        public async Task<IActionResult> Index()
        {
              return _context.RestaurantSchedule != null ? 
                          View(await _context.RestaurantSchedule.ToListAsync()) :
                          Problem("Entity set 'HotelContext.RestaurantSchedule'  is null.");
        }

        // GET: RestaurantSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RestaurantSchedule == null)
            {
                return NotFound();
            }

            var restaurantSchedule = await _context.RestaurantSchedule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantSchedule == null)
            {
                return NotFound();
            }

            return View(restaurantSchedule);
        }

        // GET: RestaurantSchedule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayOfWeek,OpenTime,CloseTime,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RestaurantSchedule restaurantSchedule)
        {
            var overlappingSchedule = await _context.RestaurantSchedule
        .FirstOrDefaultAsync(rs => rs.DayOfWeek == restaurantSchedule.DayOfWeek &&
                                   ((restaurantSchedule.OpenTime >= rs.OpenTime && restaurantSchedule.OpenTime < rs.CloseTime) ||
                                    (restaurantSchedule.CloseTime > rs.OpenTime && restaurantSchedule.CloseTime <= rs.CloseTime) ||
                                    (restaurantSchedule.OpenTime <= rs.OpenTime && restaurantSchedule.CloseTime >= rs.CloseTime)));

            if (overlappingSchedule != null)
            {
                ModelState.AddModelError("", "The schedule overlaps with an existing schedule.");
                return View(restaurantSchedule);
            }

            restaurantSchedule.AddedDate = DateTime.Today;
            restaurantSchedule.AddedBy = "Admin";
            restaurantSchedule.IsActive = true;
            _context.Add(restaurantSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: RestaurantSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RestaurantSchedule == null)
            {
                return NotFound();
            }

            var restaurantSchedule = await _context.RestaurantSchedule.FindAsync(id);
            if (restaurantSchedule == null)
            {
                return NotFound();
            }
            return View(restaurantSchedule);
        }

        // POST: RestaurantSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayOfWeek,OpenTime,CloseTime,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RestaurantSchedule restaurantSchedule)
        {
            if (id != restaurantSchedule.Id)
            {
                return NotFound();
            }

            var overlappingSchedule = await _context.RestaurantSchedule
        .FirstOrDefaultAsync(rs => rs.DayOfWeek == restaurantSchedule.DayOfWeek &&
                                   ((restaurantSchedule.OpenTime >= rs.OpenTime && restaurantSchedule.OpenTime < rs.CloseTime) ||
                                    (restaurantSchedule.CloseTime > rs.OpenTime && restaurantSchedule.CloseTime <= rs.CloseTime) ||
                                    (restaurantSchedule.OpenTime <= rs.OpenTime && restaurantSchedule.CloseTime >= rs.CloseTime)));

            if (overlappingSchedule != null)
            {
                ModelState.AddModelError("", "The schedule overlaps with an existing schedule.");
                return View(restaurantSchedule);
            }

            try
                {
                    _context.Update(restaurantSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantScheduleExists(restaurantSchedule.Id))
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


        // GET: RestaurantSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.RestaurantSchedule == null)
            {
                return Problem("Entity set 'HotelContext.RestaurantSchedule'  is null.");
            }
            var restaurantSchedule = await _context.RestaurantSchedule.FindAsync(id);
            if (restaurantSchedule != null)
            {
                _context.RestaurantSchedule.Remove(restaurantSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: RestaurantSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RestaurantSchedule == null)
            {
                return Problem("Entity set 'HotelContext.RestaurantSchedule'  is null.");
            }
            var restaurantSchedule = await _context.RestaurantSchedule.FindAsync(id);
            if (restaurantSchedule != null)
            {
                _context.RestaurantSchedule.Remove(restaurantSchedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantScheduleExists(int id)
        {
            return _context.RestaurantSchedule.Any(e => e.Id == id);
        }
    }
}
