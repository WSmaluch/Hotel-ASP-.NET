using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Booking.Extensions;

namespace Hotel.Intranet.Controllers
{
    public class TypesController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;


		public TypesController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

		}

        // GET: Types
        public async Task<IActionResult> Index()
        {
            var types = await _context.Types.Include(t => t.Rooms).Include(r => r.Facilities).ToListAsync();

            return View(types);
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                 .Include(t => t.Rooms)
                 .Include(r => r.Facilities)
                 .FirstOrDefaultAsync(m => m.IdType == id);
            if (types == null)
            {
                return NotFound();
            }

            return View(types);
        }

        // GET: Types/Create
        public IActionResult Create()
        {
            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");
            ViewData["Rooms"] = new SelectList(_context.Room, "IdRoom", "Number");
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdType,Name,Description,Size,MaxAmountOfPeople,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Types types, List<int> facilities, IFormFile photoFile)
		{
			if (photoFile != null && photoFile.Length > 0)
			{
					// Processing the uploaded file
					var imageService = new ImgurService(_configuration);
					var imageUrl = await imageService.UploadImageAsync(photoFile);

					// Saving a link to string
					types.PhotosURL = imageUrl;
			}
            else
            {
                types.PhotosURL = "No photo";
            }

			ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");

			types.AddedDate = DateTime.Now;
			types.AddedBy = "Admin";

				if (facilities != null)
				{
					foreach (var facilityId in facilities)
					{
						var facility = await _context.Facilities.FindAsync(facilityId);
						if (facility != null)
						{
							types.Facilities.Add(facility);
						}
					}
				}

				_context.Add(types);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));

		}

		// GET: Types/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types.Include(t => t.Facilities).FirstOrDefaultAsync(t => t.IdType == id);

            if (types == null)
            {
                return NotFound();
            }

            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");

            return View(types);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdType,Name,Description,PhotosURL,Size,MaxAmountOfPeople,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Types types, IFormFile photoFile)
        {
            if (id != types.IdType)
            {
                return NotFound();
            }

			var existingOption = await _context.Types.AsNoTracking().FirstOrDefaultAsync(o => o.IdType == id);

			if (existingOption == null)
			{
				return NotFound();
			}

			if (photoFile != null && photoFile.Length > 0)
			{
				// Processing the uploaded file
				var imageService = new ImgurService(_configuration);
				var imageUrl = await imageService.UploadImageAsync(photoFile);

				// Saving a link to string
				types.PhotosURL = imageUrl;
			}
			else
			{
				// Retention of existing PhotoUrl if no new file uploaded - it is the same
				types.PhotosURL = existingOption.PhotosURL;
			}

			try
                {
                    types.ModifiedDate = DateTime.Now;
                    _context.Update(types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypesExists(types.IdType))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");
                return RedirectToAction(nameof(Index));
        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Types == null)
            {
                return Problem("Entity set 'HotelContext.Types'  is null.");
            }
            var type = await _context.Types.FindAsync(id);
            if (type != null)
            {
                _context.Types.Remove(type);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Types == null)
            {
                return Problem("Entity set 'HotelContext.Types'  is null.");
            }
            var types = await _context.Types.FindAsync(id);
            if (types != null)
            {
                _context.Types.Remove(types);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypesExists(int id)
        {
          return (_context.Types?.Any(e => e.IdType == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> EditFacilities(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                .Include(t => t.Facilities)
                .FirstOrDefaultAsync(t => t.IdType == id);

            if (types == null)
            {
                return NotFound();
            }

            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");

            return View(types);
        }

        [HttpPost]
        public IActionResult AddFacilities(int typeId, List<int> facilityIds)
        {
            var types = _context.Types.Include(r => r.Facilities).FirstOrDefault(r => r.IdType == typeId);

            types.Facilities.Clear();

            if (types != null)
            {
                foreach (var facilityId in facilityIds)
                {
                    var facilityToAdd = _context.Facilities.FirstOrDefault(f => f.IdFacility == facilityId);
                    if (facilityToAdd != null && !types.Facilities.Contains(facilityToAdd))
                    {
                        types.Facilities.Add(facilityToAdd);
                    }
                }

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Edit), new { id = typeId });
        }

    }
}
