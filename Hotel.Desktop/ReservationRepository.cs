using Hotel.Data.Data.Booking;
using Hotel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Data.Data.Desktop;
using System.Data.Entity;
using Hotel.Data.Data.Employess;
using Hotel.Data.Data.Booking.Extensions;

namespace Hotel.Desktop
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelContext _hotelContext;

        public ReservationRepository(HotelContext hotelContext)
        {
            
            _hotelContext = hotelContext;
        }

        public List<Cleaner> GetCleaners()
        {
            return _hotelContext.Cleaner.ToList();
        }

        public List<CleaningTask> GetCleaningTasks()
        {
            DateTime oneWeekAgo = DateTime.Now.AddDays(-7);


            var tasks = _hotelContext.CleaningTask
              .Include(r => r.Room)
              .Where(t => (t.ScheduledDate >= oneWeekAgo && t.ScheduledDate <= DateTime.Now) || t.StatusId == 8)
              .ToList();


            var statuses = _hotelContext.Status;

            foreach (var task in tasks)
            {
                task.StatusName = statuses.Find(task.StatusId).StatusName;

            }

            return tasks;
        }

        public List<Employee>? GetEmployees()
        {
            return _hotelContext.Employee.ToList();
        }

        public List<RepairTask>? GetReapairTasks()
        {
            DateTime oneWeekAgo = DateTime.Now.AddDays(-7);

            return _hotelContext.RepairTask
                .Where(r => r.ScheduledDate <= oneWeekAgo || r.StatusId == 12)
                .ToList();
        }

        public List<RepairTask> GetRepairsTasksWithEmployees()
        {
            var tasksWithEmployees = _hotelContext.RepairTask
                .Include(rt => rt.Employee)
                .Include(r => r.Room)
                .ToList();

            var statuses = _hotelContext.Status;

            foreach (var task in tasksWithEmployees)
            {
                task.StatusName = statuses.Find(task.StatusId).StatusName;

                task.Employee = _hotelContext.Employee.Find(task.EmployeeId);
            }

            return tasksWithEmployees;
        }



        public List<Reservation> GetReservations()
        {
            DateTime today = DateTime.Now.Date;
            DayOfWeek firstDayOfWeek = DayOfWeek.Monday;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)firstDayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

            return _hotelContext.Reservations.Where(r => r.CheckIn.Date <= endOfWeek && r.CheckOut.Date >= startOfWeek || r.StatusId == 3).ToList();
        }

        public List<Room> GetRooms()
        {
            return _hotelContext.Room.ToList();
        }

    }
}
