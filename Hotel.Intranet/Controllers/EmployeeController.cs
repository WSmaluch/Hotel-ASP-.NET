using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Employess;
using Hotel.Intranet.Helpers;

namespace Hotel.Intranet.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HotelContext _context;

        public EmployeeController(HotelContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Employee.Include(e => e.Contact).Include(e => e.Department).Include(e => e.Qualification).Include(e => e.Salary);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Contact)
                .Include(e => e.Department)
                .Include(e => e.Qualification)
                .Include(e => e.Salary)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactID", "Address");
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName");
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "QualificationID", "QualificationName");
            ViewData["SalaryId"] = new SelectList(_context.Salary, "SalaryID", "SalaryDetails");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,FirstName,LastName,HiringDate,ContactId,Login,PasswordHash,DepartmentId,QualificationId,SalaryId")] Employee employee)
        {
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactID", "Address", employee.ContactId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName", employee.DepartmentId);
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "QualificationID", "QualificationName", employee.QualificationId);
            ViewData["SalaryId"] = new SelectList(_context.Salary, "SalaryID", "SalaryDetails", employee.SalaryId);

            employee.PasswordHash = PasswordHasher.HashPassword(employee.PasswordHash);
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactID", "Address", employee.ContactId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName", employee.DepartmentId);
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "QualificationID", "QualificationName", employee.QualificationId);
            ViewData["SalaryId"] = new SelectList(_context.Salary, "SalaryID", "SalaryDetails", employee.SalaryId);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,FirstName,LastName,HiringDate,ContactId,Login,PasswordHash,DepartmentId,QualificationId,SalaryId")] Employee employee, string newPassword)
        {
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactID", "Address", employee.ContactId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName", employee.DepartmentId);
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "QualificationID", "QualificationName", employee.QualificationId);
            ViewData["SalaryId"] = new SelectList(_context.Salary, "SalaryID", "SalaryDetails", employee.SalaryId);

            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            try
            {
                if (!string.IsNullOrEmpty(newPassword))
                {
                    employee.PasswordHash = PasswordHasher.HashPassword(newPassword);
                }

                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.EmployeeID))
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

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'HotelContext.Employee'  is null.");
            }
            var empl = await _context.Employee.FindAsync(id);
            if (empl != null)
            {
                _context.Employee.Remove(empl);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'HotelContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employee?.Any(e => e.EmployeeID == id)).GetValueOrDefault();
        }

    }
}
